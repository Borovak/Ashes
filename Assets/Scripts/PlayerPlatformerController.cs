using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerPlatformerController : PhysicsObject
{
    public enum RollingStates
    {
        NotRolling,
        Rolling,
        RollingNoMotion
    }

    public static PlayerPlatformerController Instance;
    public float maxSpeed = 7f;
    public float jumpTakeOffSpeed = 3f;
    public float rollSpeed = 1f;
    public AudioClip audioClipJump;
    public AudioClip audioClipLanding;
    public AudioClip audioClipAttack;
    public bool isGrounded;
    public bool flipX => transform.localScale.x < 0f;
    public Vector3 campsiteLocation;
    public bool hasDoubleJump;
    public float gameTime;
    public GameObject landingPuffPrefab;
    public RollingStates rollingState;

    private SpriteRenderer[] _spriteRenderers;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private AudioSource _audioSource;
    private bool _doubleJumpPossible;
    private bool _previousGrounded;
    private Animator _mainCameraAnimator;
    private PlayerInputs _inputs;


    private struct ChamberPosition
    {
        internal float value;
        internal Func<Vector2> getPoint;
    }

    // Use this for initialization
    void Awake()
    {
        Instance = this;
        Debug.Log($"Save file present: {SaveSystem.latestSaveData != null}");
        if (SaveSystem.latestSaveData != null)
        {
            Debug.Log($"Saved has double jump : {SaveSystem.latestSaveData.HasDoubleJump}");
            Debug.Log($"Saved campsite location : {SaveSystem.latestSaveData.CampsiteLocation}");
            gameTime = SaveSystem.latestSaveData.GameTime;
            hasDoubleJump = SaveSystem.latestSaveData.HasDoubleJump;
            campsiteLocation = SaveSystem.latestSaveData.CampsiteLocation != null && SaveSystem.latestSaveData.CampsiteLocation.Length == 3 ? new Vector3(SaveSystem.latestSaveData.CampsiteLocation[0], SaveSystem.latestSaveData.CampsiteLocation[1], SaveSystem.latestSaveData.CampsiteLocation[2]) : GameController.Instance.campsiteLocations[0];
        }
        else
        {
            hasDoubleJump = false;
            campsiteLocation = new Vector3(-173f, 33f, 0f);
            SaveSystem.Save();
        }
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _inputs = GetComponent<PlayerInputs>();
        _mainCameraAnimator = Camera.main.transform.GetChild(0).GetComponent<Animator>();
        GameController.ChamberChanged += OnChamberChanged;
        _inputs.Jump += Jump;
        _inputs.JumpRelease += JumpRelease;
        _inputs.Roll += Roll;
    }
    void Start()
    {
        transform.position = campsiteLocation;
    }

    protected override void ComputeVelocity()
    {
        gameTime += Time.deltaTime;
        _animator.SetBool("horizontalMoveDesired", Mathf.Abs(_inputs.movement.x) > 0.1);
        CheckIfLanding();
        
        Vector2 move = Vector2.zero;
        if (rollingState == RollingStates.NotRolling)
        {
            move.x = _inputs.movement.x;

            bool flipSprite = (transform.localScale.x == -1f ? (move.x > 0.01f) : (move.x < -0.01f));
            if (flipSprite)
            {
                var scale = transform.localScale;
                scale.x = -scale.x;
                transform.localScale = scale;
            }
        }
        else if (rollingState == RollingStates.Rolling)
        {
            move.x = transform.localScale.x * rollSpeed;
        }

        _animator.SetBool("grounded", grounded);
        _animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);
        _animator.SetFloat("velocityY", velocity.y);

        targetVelocity = move * maxSpeed;
        isGrounded = grounded;
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
        if (grounded)
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
        _animator.SetTrigger("roll");
    }

    private void RollStopMotion()
    {
        rollingState = RollingStates.RollingNoMotion;
    }

    private void OnChamberChanged(ChamberController chamber)
    {
        const float spawnOffset = 3f;
        var minX = chamber.x * ChamberController.unitSize;
        var minY = chamber.y * ChamberController.unitSize;
        var maxX = minX + chamber.w * ChamberController.unitSize;
        var maxY = minY + chamber.h * ChamberController.unitSize;

        var actions = new List<ChamberPosition>{
            new ChamberPosition {value = transform.position.x - chamber.x * ChamberController.unitSize, getPoint = () => new Vector2(chamber.transform.position.x + spawnOffset, transform.position.y)},
            new ChamberPosition {value = transform.position.y - chamber.y * ChamberController.unitSize, getPoint = () => new Vector2(transform.position.x, chamber.transform.position.y + spawnOffset)},
            new ChamberPosition {value = transform.position.x - (minX + chamber.w * ChamberController.unitSize), getPoint = () => new Vector2(maxX - spawnOffset, transform.position.y)},
            new ChamberPosition {value = transform.position.y - (minY + chamber.h * ChamberController.unitSize), getPoint = () => new Vector2(transform.position.x, maxY - spawnOffset)}
        };
        var min = actions.Min(x => Mathf.Abs(x.value));
        if (min > 3f) return;
        var func = actions.First(x => Mathf.Abs(x.value) == min).getPoint;
        var point = func.Invoke();
        forcedDestinationEnabled = true;
        forcedDestinationPoint = point;
        forcedDestinationSpeed = maxSpeed;
    }

    public void PlayLanding()
    {
        _audioSource.PlayOneShot(audioClipLanding);
    }
}