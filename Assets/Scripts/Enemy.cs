using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : PhysicsObject 
{

	[SerializeField] public enum EnemyType {Bug, Zombie};
    public float health;
    public float maxSpeed;
    public float direction;
	public float _directionSmooth = 1f;
	[SerializeField] public float changeDirectionEase = 1f;
    public float staggeredTime;
    public float knockbackForce;
    public float visionRange;
    public EnemyType enemyType;
    public bool followPlayer;
	public float followRange;
    public float jumpTakeOffSpeed;
    public Vector2 raycastOffset;
    public LayerMask layerMask;
    public bool seesLeftWall;
    public bool seesRightWall;
    public LayerMask whatIsPlayer;
    public float groundCheckLength = 1f;
    public float wallCheckLength = 1f;

    protected Animator _animator;
    private float _staggeredFor;
	private RaycastHit2D _ground;
    private RaycastHit2D _leftWall;
    private RaycastHit2D _rightWall;
    private RaycastHit2D _leftLedge;
    private RaycastHit2D _rightLedge;
    private SpriteRenderer _spriteRenderer;
	private float _launch = 1f;
	private float _playerDifference;
	private float _scale;
	[SerializeField] private GameObject _graphic;

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        //_animator.SetBool("isRunning", true);
		_scale = transform.localScale.x;
        if (_graphic == null){
            _graphic = gameObject;
        }
    }

    // Update is called once per frame
    protected override void ComputeVelocity()
    {       
		Vector2 move = Vector2.zero;
		_playerDifference = PlayerPlatformerController.Instance.transform.position.x - transform.position.x;
		_directionSmooth += (direction - _directionSmooth) * Time.deltaTime * changeDirectionEase;
		if (_staggeredFor > 0){
			WhenStaggered(ref move);
		} else {			
			WhenNormal(ref move);
		}			
		//_animator.SetBool ("grounded", grounded);
        //_animator.SetFloat ("velocityX", Mathf.Abs (velocity.x) / maxSpeed);
        targetVelocity = move * maxSpeed;
    }

	private void WhenNormal(ref Vector2 move){
		move.x = 1 * _directionSmooth;

			//Flip the graphic depending on the speed
			if (move.x > 0.01f) {
				if (_graphic.transform.localScale.x < 0) {
					_graphic.transform.localScale = new Vector3 (_scale, _scale, transform.localScale.z);
				}
			} else if (move.x < -0.01f) {
				if (_graphic.transform.localScale.x > 0) {
					_graphic.transform.localScale = new Vector3 (-_scale, _scale, transform.localScale.z);
				}
			}

			//Check floor type
			_ground = Physics2D.Raycast (transform.position, -Vector2.up);
			Debug.DrawRay (transform.position, -Vector2.up, Color.magenta);

			//Check if player is within range to follow

			if (enemyType == EnemyType.Zombie) {
				if ((Mathf.Abs (_playerDifference) < followRange)) {
					followPlayer = true;
				} else {
					followPlayer = false;
				}
			}
				
			if (followPlayer) {
				if (_playerDifference < 0) {
					direction = -1;
				} else {
					direction = 1;
				}
			} else {
				//Allow enemy to instantly change direction when not following player
				_directionSmooth = direction;
			}



			//Check for walls
			_rightWall = Physics2D.Raycast (new Vector2 (transform.position.x + raycastOffset.x, transform.position.y + raycastOffset.y), Vector2.right, wallCheckLength, layerMask);
			Debug.DrawRay (new Vector2 (transform.position.x, transform.position.y + raycastOffset.y), Vector2.right, Color.yellow);

			if (_rightWall.collider != null) {
				if (!followPlayer) {
					direction = -1;
				} else {
					Jump ();
				}

			}

			_leftWall = Physics2D.Raycast (new Vector2 (transform.position.x - raycastOffset.x, transform.position.y + raycastOffset.y), Vector2.left, wallCheckLength, layerMask);
			Debug.DrawRay (new Vector2 (transform.position.x, transform.position.y + raycastOffset.y), Vector2.left, Color.blue);

			if (_leftWall.collider != null) {
				if (!followPlayer) {
					direction = 1;
				} else {
					Jump ();
				}
			}


			//Check for ledges
			if (!followPlayer) {
				_rightLedge = Physics2D.Raycast (new Vector2 (transform.position.x + raycastOffset.x, transform.position.y), Vector2.down, groundCheckLength);
				Debug.DrawRay (new Vector2 (transform.position.x + raycastOffset.x, transform.position.y), Vector2.down, Color.blue);
				if (_rightLedge.collider == null) {
					direction = -1;
				}

				_leftLedge = Physics2D.Raycast (new Vector2 (transform.position.x - raycastOffset.x, transform.position.y), Vector2.down, groundCheckLength);
				Debug.DrawRay (new Vector2 (transform.position.x - raycastOffset.x, transform.position.y), Vector2.down, Color.blue);

				if (_leftLedge.collider == null) {
					direction = 1;
				}
			}
	}

	private void WhenStaggered(ref Vector2 move){
		_staggeredFor -= Time.deltaTime;
		move.x = _launch;
		_launch += (0 - _launch) * Time.deltaTime;
	}

	public void Jump(){
		if (grounded) {
			velocity.y = jumpTakeOffSpeed;
			PlayJumpSound ();
			PlayStepSound ();
		}
	}

    void OnDrawGizmosSelected(){
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, visionRange);
    }

    public void PlayJumpSound (){

    }

    public void PlayStepSound (){
        
    }
}
