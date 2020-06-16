using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerPlatformerController : PhysicsObject
{

    public static PlayerPlatformerController Instance;
    public event Action<int> HpChanged;
    public float maxSpeed = 7f;
    public float jumpTakeOffSpeed = 3f;
    public AudioClip audioClipJump;
    public AudioClip audioClipLanding;
    public AudioClip audioClipAttack;
    public bool isGrounded;
    public bool flipX => transform.localScale.x < 0f;
    public Vector3 campsiteLocation;
    public int maxHp;
    public bool hasDoubleJump;
    public int hp
    {
        get => _hp;
        set
        {
            _hp = value;
            HpChanged?.Invoke(value);
        }
    }

    private SpriteRenderer[] _spriteRenderers;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private AudioSource _audioSource;
    private bool _doubleJumpPossible;
    private GameObject _idlePose;
    private GameObject _runPose;
    private int _hp;


    private struct ChamberPosition{
        internal float value;
        internal Func<Vector2> getPoint;
    }

    // Use this for initialization
    void Awake()
    {
        Instance = this;
        if (SaveSystem.latestSaveData != null)
        {
            Debug.Log($"Saved max hp: {SaveSystem.latestSaveData.MaxHp}");
            Debug.Log($"Saved hp: {SaveSystem.latestSaveData.Hp}");
            Debug.Log($"Saved has double jump : {SaveSystem.latestSaveData.HasDoubleJump}");
            Debug.Log($"Saved campsite location : {SaveSystem.latestSaveData.CampsiteLocation}");
            maxHp = SaveSystem.latestSaveData.MaxHp;
            hp = SaveSystem.latestSaveData.Hp;
            hasDoubleJump = SaveSystem.latestSaveData.HasDoubleJump;
            campsiteLocation = SaveSystem.latestSaveData.CampsiteLocation != null && SaveSystem.latestSaveData.CampsiteLocation.Length == 3 ? new Vector3(SaveSystem.latestSaveData.CampsiteLocation[0], SaveSystem.latestSaveData.CampsiteLocation[1], SaveSystem.latestSaveData.CampsiteLocation[2]) : GameController.Instance.campsiteLocations[0];
        }
        else
        {
            maxHp = 3;
            hp = 3;
            hasDoubleJump = false;
            campsiteLocation = GameController.Instance.campsiteLocations[0];
            SaveSystem.Save();
        }
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _idlePose = transform.Find("idle").gameObject;
        _runPose = transform.Find("run").gameObject;
        GameController.ChamberChanged += OnChamberChanged;
    }
    void Start()
    {
        Debug.Log($"Campsite count: {GameController.Instance.campsiteLocations.Length}");
        transform.position = campsiteLocation;
    }

    protected override void ComputeVelocity()
    {
        if (grounded)
        {
            _doubleJumpPossible = hasDoubleJump;
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
        _spriteRenderer.enabled = false;
        _runPose.SetActive(true);
        _idlePose.SetActive(false);
    }

    public void PlayJump()
    {
        _audioSource.PlayOneShot(audioClipJump);
        _spriteRenderer.enabled = false;
        _runPose.SetActive(true);
        _idlePose.SetActive(false);
    }

    public void PlayAttack()
    {
        _audioSource.PlayOneShot(audioClipAttack);
        _spriteRenderer.enabled = true;
        _runPose.SetActive(false);
        _idlePose.SetActive(false);
    }

    public void AckJump()
    {
        _animator.SetBool("jump", false);
    }

    public void SetIdlePose()
    {
        _spriteRenderer.enabled = false;
        _idlePose.SetActive(true);
        _runPose.SetActive(false);
    }

    public void SetRunPose()
    {
        _spriteRenderer.enabled = false;
        _runPose.SetActive(true);
        _idlePose.SetActive(false);
    }

    public void SetTakeOffPose()
    {
        _spriteRenderer.enabled = false;
        _runPose.SetActive(true);
        _idlePose.SetActive(false);
    }
}