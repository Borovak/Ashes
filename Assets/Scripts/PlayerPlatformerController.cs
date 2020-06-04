using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformerController : PhysicsObject
{

    public static PlayerPlatformerController Instance;
    public static PlayerData playerData;
    public float maxSpeed = 7f;
    public float jumpTakeOffSpeed = 3f;
    public AudioClip audioClipJump;
    public AudioClip audioClipLanding;
    public AudioClip audioClipAttack;
    public bool isGrounded;
    public bool flipX => transform.localScale.x < 0f;

    private SpriteRenderer[] _spriteRenderers;
    private Animator _animator;
    private AudioSource _audioSource;
    private bool _doubleJumpPossible;
    private GameObject _idlePose;
    private GameObject _runPose;

    // Use this for initialization
    void Awake()
    {
        playerData = new PlayerData(3, 3, true, 0);
        //_spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _idlePose = transform.Find("idle").gameObject;
        _runPose = transform.Find("run").gameObject;
        Instance = this;
        GameController.ChamberChanged += OnChamberChanged;
    }

    protected override void ComputeVelocity()
    {
        if (grounded)
        {
            _doubleJumpPossible = playerData.HasDoubleJump;
        }

        Vector2 move = Vector2.zero;

        move.x = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            if (grounded)
            {
                _animator.SetTrigger("jump");
                velocity.y = jumpTakeOffSpeed;
            }
            else if (_doubleJumpPossible)
            {
                velocity.y = jumpTakeOffSpeed;
                _doubleJumpPossible = false;
            }
        }
        else if (Input.GetButtonUp("Jump"))
        {
            if (velocity.y > 0)
            {
                velocity.y = velocity.y * 0.5f;
            }
            _animator.SetBool("jump", false);
        }

        bool flipSprite = (transform.localScale.x == -1f ? (move.x > 0.01f) : (move.x < -0.01f));
        if (flipSprite)
        {
            var scale = transform.localScale;
            scale.x = -scale.x;
            transform.localScale = scale;
        }
        // bool flipSprite = (!_spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < -0.01f));
        // if (flipSprite)
        // {
        //     _spriteRenderer.flipX = !_spriteRenderer.flipX;
        // }
        _animator.SetBool("grounded", grounded);
        _animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);
        _animator.SetFloat("velocityY", velocity.y);

        targetVelocity = move * maxSpeed;
        isGrounded = grounded;
    }

    private void OnChamberChanged(ChamberController chamber)
    {
        Vector2 point;
        var relativeX = transform.position.x - chamber.transform.position.x;
        var relativeY = chamber.transform.position.y - transform.position.y;
        var maxX = chamber.transform.position.x + chamber.w * ChamberController.unitSize;
        var maxY = chamber.transform.position.y + chamber.h * ChamberController.unitSize;
        var spawnOffset = 4f;
        if (relativeX <= 2f)
        {
            point = new Vector2(chamber.transform.position.x + spawnOffset, transform.position.y);
        }
        else if (relativeX >= chamber.w * ChamberController.unitSize - 2f)
        {
            point = new Vector2(maxX - spawnOffset, transform.position.y);
        }
        else if (relativeY <= 2f)
        {
            point = new Vector2(transform.position.x, chamber.transform.position.y + spawnOffset);
        }
        else if (relativeY >= chamber.y * ChamberController.unitSize - 2f)
        {
            point = new Vector2(transform.position.x, maxY - spawnOffset);
        }
        else { return; }
        Debug.Log($"Forced destination => rX: {relativeX}, rY: {relativeY}, maxX: {maxX}, maxY: {maxY}, point: {point}");
        forcedDestinationEnabled = true;
        forcedDestinationPoint = point;
        forcedDestinationSpeed = maxSpeed;
    }

    public void PlayLanding()
    {
        _audioSource.PlayOneShot(audioClipLanding);
        _runPose.SetActive(true);
        _idlePose.SetActive(false);
    }

    public void PlayJump()
    {
        _audioSource.PlayOneShot(audioClipJump);
        _runPose.SetActive(true);
        _idlePose.SetActive(false);
    }

    public void PlayAttack()
    {
        _audioSource.PlayOneShot(audioClipAttack);
        _runPose.SetActive(true);
        _idlePose.SetActive(false);
    }

    public void AckJump()
    {
        _animator.SetBool("jump", false);
    }

    public void SetIdlePose()
    {
        _idlePose.SetActive(true);
        _runPose.SetActive(false);
    }

    public void SetRunPose()
    {
        _runPose.SetActive(true);
        _idlePose.SetActive(false);
    }

    public void SetTakeOffPose()
    {
        _runPose.SetActive(true);
        _idlePose.SetActive(false);
    }
}