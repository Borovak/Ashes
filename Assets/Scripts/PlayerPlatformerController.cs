using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerPlatformerController : PhysicsObject
{

    public static PlayerPlatformerController Instance;
    public float maxSpeed = 7f;
    public float jumpTakeOffSpeed = 3f;
    public float rollSpeed = 1f;
    public float transitionTime = 1f;
    public bool isGrounded;
    public bool hasDoubleJump => SaveData.workingData.HasDoubleJump;
    public bool freezeMovement;
    public bool isRolling;
    public GameObject landingPuffPrefab;
    public static Vector3 transitionMovement;

    private SpriteRenderer[] _spriteRenderers;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private AudioSource _audioSource;
    private bool _doubleJumpPossible;
    private bool _previousGrounded;
    private Animator _mainCameraAnimator;
    private PlayerInputs _inputs;
    private LifeController _lifeController;
    private float _timeSinceGrounded;

    // Use this for initialization
    void Awake()
    {
        Instance = this;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _inputs = GetComponent<PlayerInputs>();
        _lifeController = GetComponent<LifeController>();
        _mainCameraAnimator = Camera.main.transform.GetChild(0).GetComponent<Animator>();
        _inputs.Jump += Jump;
        _inputs.JumpRelease += JumpRelease;
        _inputs.Roll += Roll;
    }

    void OnDisable()
    {
        _inputs.Jump -= Jump;
        _inputs.JumpRelease -= JumpRelease;
        _inputs.Roll -= Roll;
    }

    void Start()
    {
    }

    protected override void ComputeVelocity()
    {
        CheckIfLanding();

        Vector2 move = Vector2.zero;
        if (_gameController.gameState == GameController.GameStates.TransitionIn || _gameController.gameState == GameController.GameStates.TransitionOut)
        {
            transitionTime -= Time.deltaTime;
            move.x = transitionMovement.x;
            SpriteFlipping(ref move);
        }
        else if (!freezeMovement)
        {
            move.x = _inputs.movement.x;
            SpriteFlipping(ref move);
        }
        else if (isRolling)
        {
            move.x = transform.localScale.x * rollSpeed;
        }

        _animator.SetBool("horizontalMoveDesired", Mathf.Abs(move.x) > 0.1);
        _animator.SetBool("grounded", grounded);
        _animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);
        _animator.SetFloat("velocityY", velocity.y);

        targetVelocity = move * maxSpeed;
        isGrounded = grounded;
        _timeSinceGrounded = isGrounded ? 0f : _timeSinceGrounded + Time.deltaTime;
        _animator.SetFloat("timeSinceGrounded", _timeSinceGrounded);
    }

    private void SpriteFlipping(ref Vector2 move)
    {
        bool flipSprite = (transform.localScale.x == -1f ? (move.x > 0.01f) : (move.x < -0.01f));
        if (flipSprite)
        {
            var scale = transform.localScale;
            scale.x = -scale.x;
            transform.localScale = scale;
        }
    }

    private void CheckIfLanding()
    {
        if (grounded)
        {
            if (!_previousGrounded)
            {
                var landingPuff = GameObject.Instantiate(landingPuffPrefab);
                landingPuff.transform.position = transform.position;
                _mainCameraAnimator.SetTrigger("Shake");
            }
            _doubleJumpPossible = hasDoubleJump;
        }
        _previousGrounded = grounded;
    }

    private void Jump()
    {
        if (!_lifeController.IsAlive) return;
        if (grounded || (velocity.y <= 0.1f && _timeSinceGrounded <= 0.2f))
        {
            _animator.SetBool("jump", true);
            velocity.y = jumpTakeOffSpeed;
        }
        else if (_doubleJumpPossible)
        {
            velocity.y = jumpTakeOffSpeed;
            _doubleJumpPossible = false;
        }
    }

    private void JumpRelease()
    {
        if (velocity.y > 0)
        {
            velocity.y = velocity.y * 0.5f;
        }
    }

    private void Roll()
    {
        if (!_lifeController.IsAlive) return;
        _animator.SetTrigger("roll");
    }
}