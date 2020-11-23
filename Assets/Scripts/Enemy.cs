using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : PhysicsObject
{

    [SerializeField] public enum EnemyType { Bug, Zombie };
    public float maxSpeed;
    public float direction = 1f;
    [SerializeField] public float changeDirectionEase = 1f;
    public float knockbackForce;
    public float visionRange;
    public EnemyType enemyType;
    public bool followPlayer;
    public float followRange;
    public Vector2 raycastOffset;
    public bool seesLeftWall;
    public bool seesRightWall;
    public float wallCheckLength = 1f;

    protected abstract void AfterStart();
    protected abstract void CheckForLedges();
    protected abstract float jumpTakeOffSpeed { get; }
    protected Animator _animator;
    private float _staggeredFor;
    private RaycastHit2D _ground;
    private RaycastHit2D _leftWall;
    private RaycastHit2D _rightWall;
    private SpriteRenderer _spriteRenderer;
    private float _launch = 1f;
    private float _playerDifference;
    private float _scale;
    private float _directionSmooth = 1f;
    [SerializeField] private GameObject _graphic;


    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        //_animator.SetBool("isRunning", true);
        _scale = transform.localScale.x;
        if (_graphic == null)
        {
            _graphic = gameObject;
        }
        AfterStart();
    }

    // Update is called once per frame
    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;
        _playerDifference = PlayerPlatformerController.Instance.transform.position.x - transform.position.x;
        _directionSmooth += (direction - _directionSmooth) * Time.deltaTime * changeDirectionEase;
        if (_staggeredFor > 0)
        {
            WhenStaggered(ref move);
        }
        else
        {
            WhenNormal(ref move);
        }
        //_animator.SetBool ("grounded", grounded);
        //_animator.SetFloat ("velocityX", Mathf.Abs (velocity.x) / maxSpeed);
        targetVelocity = move * maxSpeed;
    }

    private void WhenNormal(ref Vector2 move)
    {
        move.x = 1 * _directionSmooth;

        //Flip the graphic depending on the speed
        if (move.x > 0.01f)
        {
            if (_graphic.transform.localScale.x < 0)
            {
                _graphic.transform.localScale = new Vector3(_scale, _scale, transform.localScale.z);
            }
        }
        else if (move.x < -0.01f)
        {
            if (_graphic.transform.localScale.x > 0)
            {
                _graphic.transform.localScale = new Vector3(-_scale, _scale, transform.localScale.z);
            }
        }

        //Check floor type
        _ground = Physics2D.Raycast(transform.position, -Vector2.up);
        Debug.DrawRay(transform.position, -Vector2.up, Color.magenta);
        _directionSmooth = direction;

        //Check for walls
        _rightWall = Physics2D.Raycast(new Vector2(transform.position.x + raycastOffset.x, transform.position.y + raycastOffset.y), Vector2.right, wallCheckLength, LayerManagement.Layout);
        _leftWall = Physics2D.Raycast(new Vector2(transform.position.x - raycastOffset.x, transform.position.y + raycastOffset.y), Vector2.left, wallCheckLength, LayerManagement.Layout);
        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + raycastOffset.y), Vector2.right, Color.yellow);
        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + raycastOffset.y), Vector2.left, Color.blue);

        if (_rightWall.collider != null)
        {
            direction = -1;
        }
        else if (_leftWall.collider != null)
        {
            direction = 1;
        }
        CheckForLedges();
    }

    private void WhenStaggered(ref Vector2 move)
    {
        _staggeredFor -= Time.deltaTime;
        move.x = _launch;
        _launch += (0 - _launch) * Time.deltaTime;
    }

    // public void Jump()
    // {
    //     Debug.Log($"{gameObject.name} jumps");
    //     if (!grounded) return;
    //     velocity.y = jumpTakeOffSpeed;
    //     PlayJumpSound();
    //     PlayStepSound();
    // }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, visionRange);
    }

    public void PlayJumpSound()
    {

    }

    public void PlayStepSound()
    {

    }
}
