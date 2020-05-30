using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformerController : PhysicsObject
{

    public static PlayerPlatformerController Instance;
    public static PlayerData playerData;
    public float maxSpeed = 7f;
    public float jumpTakeOffSpeed = 3f;
    public float invinsibilityPeriodAfterDamage = 2f;
    public AudioClip audioClipJump;
    public AudioClip audioClipLanding;
    public AudioClip audioClipAttack;
    public bool frozen;
    public bool isGrounded;
    public bool flipX => _spriteRenderer.flipX;
    public Color damageColor;
    public float invinsibilityFlashRate;

    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private AudioSource _audioSource;
    private bool _doubleJumpPossible;
    private float _invinsibleFor;
    private float _invinsibilityFlash = 0f;

    // Use this for initialization
    void Awake()
    {
        playerData = new PlayerData(3, 3, true);
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        Instance = this;
    }

    protected override void ComputeVelocity()
    {
        if (grounded)
        {
            _doubleJumpPossible = playerData.HasDoubleJump;
        }

        ManageInvinsibility();
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
        }

        bool flipSprite = (!_spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < -0.01f));
        if (flipSprite)
        {
            _spriteRenderer.flipX = !_spriteRenderer.flipX;
        }
        _animator.SetBool("grounded", grounded);
        _animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);
        _animator.SetFloat("velocityY", velocity.y);

        targetVelocity = move * maxSpeed;
        isGrounded = grounded;
    }

    private void ManageInvinsibility()
    {
        if (_invinsibleFor > 0)
        {
            _invinsibleFor -= Time.deltaTime;
            if (_invinsibilityFlash == 0f)
            {
                _invinsibilityFlash = 2f;
            }
        }
        _invinsibilityFlash = Mathf.Max(_invinsibilityFlash - invinsibilityFlashRate * Time.deltaTime, 0f);
        if (_invinsibilityFlash > 1f)
        {
            _spriteRenderer.color = Color.Lerp(damageColor, Color.white, _invinsibilityFlash % 1f);
        }
        else
        {
            _spriteRenderer.color = Color.Lerp(Color.white, damageColor, _invinsibilityFlash % 1f);
        }
    }

    public void TakeDamage()
    {
        if (_invinsibleFor <= 0)
        {
            _invinsibleFor = invinsibilityPeriodAfterDamage;
            playerData.Hp -= 1;
        }
    }

    public void PlayLanding()
    {
        _audioSource.PlayOneShot(audioClipLanding);
    }

    public void PlayJump()
    {
        _audioSource.PlayOneShot(audioClipJump);
    }

    public void PlayAttack()
    {
        _audioSource.PlayOneShot(audioClipAttack);
    }

    public void AckJump()
    {
        _animator.SetBool("jump", false);
    }
}