using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformerController : PhysicsObject
{

    public static PlayerPlatformerController Instance;
    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 3;
    public AudioClip audioClipJump;
    public AudioClip audioClipLanding;
    public AudioClip audioClipAttack;
    public bool frozen;
    public bool isGrounded;
    public bool doubleJumpUnlocked;
    public bool flipX => _spriteRenderer.flipX;

    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private AudioSource _audioSource;
    private bool _doubleJumpPossible;

    // Use this for initialization
    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        Instance = this;
    }

    protected override void ComputeVelocity()
    {
        if (grounded)
        {
            _doubleJumpPossible = true;
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