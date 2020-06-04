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
    public bool isGrounded;
    public bool flipX => transform.localScale.x < 0f;
    public Color damageColor;
    public float invinsibilityFlashRate;

    private SpriteRenderer[] _spriteRenderers;
    private Animator _animator;
    private AudioSource _audioSource;
    private bool _doubleJumpPossible;
    private float _invinsibleFor;
    private float _invinsibilityFlash = 0f;
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
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
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
        var c1 = _invinsibilityFlash > 1f ? damageColor : Color.white;
        var c2 = _invinsibilityFlash > 1f ? Color.white : damageColor;
        var c = Color.Lerp(c1, c2, _invinsibilityFlash % 1f);
        foreach (var spriteRenderer in _spriteRenderers)
        {
            spriteRenderer.color = c;
        }
    }

    public void TakeDamage()
    {
        if (_invinsibleFor <= 0)
        {
            _invinsibleFor = invinsibilityPeriodAfterDamage;
            playerData.Hp -= 1;
            if (playerData.Hp <= 0)
            {
                if (CampsiteController.campsites.TryGetValue(playerData.CampsiteId, out var campsite))
                {
                    transform.position = campsite.transform.position;
                    playerData.Hp = playerData.MaxHp;
                }
            }
        }
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