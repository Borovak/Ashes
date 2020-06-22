﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : PhysicsObject 
{

	[SerializeField] public enum EnemyType {Bug, Zombie};
    public float health;
    public float maxSpeed;
    public float direction;
	[SerializeField] public float changeDirectionEase = 1f;
    public float staggeredTime;
    public float knockbackForce;
    public float visionRange;
    public EnemyType enemyType;
    public bool followPlayer;
	public float followRange;
    public float jumpTakeOffSpeed;
    public ParticleSystem blood;
    public Vector2 bloodOffset;
    public Vector2 raycastOffset;
    public LayerMask layerMask;
    public bool seesLeftWall;
    public bool seesRightWall;
    public LayerMask whatIsPlayer;

    protected Animator _animator;
    private float _staggeredFor;
	private RaycastHit2D _ground;
    private RaycastHit2D _leftWall;
    private RaycastHit2D _rightWall;
    private RaycastHit2D _leftLedge;
    private RaycastHit2D _rightLedge;
    private SpriteRenderer _spriteRenderer;
	private float _launch = 1f;
	private float _directionSmooth = 1f;
	private float _playerDifference;
	[SerializeField] private GameObject _graphic;

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _animator.SetBool("isRunning", true);
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
		if (_staggeredFor <= 0) {
			move.x = 1 * _directionSmooth;

			//Flip the graphic depending on the speed
			if (move.x > 0.01f) {
				if (_graphic.transform.localScale.x == -1) {
					_graphic.transform.localScale = new Vector3 (1, transform.localScale.y, transform.localScale.z);
				}
			} else if (move.x < -0.01f) {
				if (_graphic.transform.localScale.x == 1) {
					_graphic.transform.localScale = new Vector3 (-1, transform.localScale.y, transform.localScale.z);
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
			_rightWall = Physics2D.Raycast (new Vector2 (transform.position.x + raycastOffset.x, transform.position.y + raycastOffset.y), Vector2.right, 1f, layerMask);
			Debug.DrawRay (new Vector2 (transform.position.x, transform.position.y + raycastOffset.y), Vector2.right, Color.yellow);

			if (_rightWall.collider != null) {
				if (!followPlayer) {
					direction = -1;
				} else {
					Jump ();
				}

			}

			_leftWall = Physics2D.Raycast (new Vector2 (transform.position.x - raycastOffset.x, transform.position.y + raycastOffset.y), Vector2.left, 1f, layerMask);
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
				_rightLedge = Physics2D.Raycast (new Vector2 (transform.position.x + raycastOffset.x, transform.position.y), Vector2.down, .5f);
				Debug.DrawRay (new Vector2 (transform.position.x + raycastOffset.x, transform.position.y), Vector2.down, Color.blue);
				if (_rightLedge.collider == null) {
					direction = -1;
				}

				_leftLedge = Physics2D.Raycast (new Vector2 (transform.position.x - raycastOffset.x, transform.position.y), Vector2.down, .5f);
				Debug.DrawRay (new Vector2 (transform.position.x - raycastOffset.x, transform.position.y), Vector2.down, Color.blue);

				if (_leftLedge.collider == null) {
					direction = 1;
				}
			}
		


		//Recovery after being hit
		} else if (_staggeredFor > 0) {
			_staggeredFor -= Time.deltaTime;
			move.x = _launch;
			_launch += (0 - _launch) * Time.deltaTime;
		}
			
		_animator.SetBool ("grounded", grounded);
        _animator.SetFloat ("velocityX", Mathf.Abs (velocity.x) / maxSpeed);
        targetVelocity = move * maxSpeed;
    }

	public void Jump(){
		if (grounded) {
			velocity.y = jumpTakeOffSpeed;
			PlayJumpSound ();
			PlayStepSound ();
		}
	}

    public void TakeDamage(float damage, Vector2 knockbackDirection){
		if (blood == null) return;
        Instantiate(blood, transform.position + new Vector3(bloodOffset.x, bloodOffset.y, 0f), Quaternion.identity);
        health -= damage;
        _staggeredFor = staggeredTime;
        var knockBackVector = knockbackDirection * knockbackForce;
        targetVelocity = knockBackVector;
        Debug.Log($"Enemy took {damage} damage, {health} hp remaining, knockback at {knockBackVector}");
            if (health <= 0){
            Destroy(gameObject);
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

    void OnTriggerEnter2D(Collider2D collider){
        if (!collider.gameObject.TryGetComponent<LifeController>(out var player)) return;
        Debug.Log($"Player took damage from {gameObject.name}");
        player.TakeDamage(1);
    }
}
