using UnityEngine;

namespace Player
{
    public class PlayerPlatformerController : PhysicsObject
    {

        public static PlayerPlatformerController Instance;
        public float maxSpeed = 7f;
        public float jumpTakeOffSpeed = 3f;
        public float dashSpeed = 1f;
        public float transitionTime = 1f;
        public bool isGrounded;
        public bool hasDoubleJump;
        public Transform invertOnFlipXGroup;

        public override bool canFly => false;

        public bool freezeMovement;
        public bool isDashing;
        public GameObject landingPuffPrefab;
        public static Vector3 transitionMovement;
        public float timeBetweenDashes = 1f;

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
        private bool _canDash;
        private float _dashTimer;
        private float _droppingThroughTime;
        private float _droppingThroughMaxTime = 0.1f;

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
            _inputs.Dash += Dash;
            _inputs.DropThrough += DropThrough;
            GameController.GameStateChanged += OnGameStateChanged;
        }

        void OnDisable()
        {
            _inputs.Jump -= Jump;
            _inputs.JumpRelease -= JumpRelease;
            _inputs.Dash -= Dash;
        }

        void Start()
        {
        }

        protected override void ComputeVelocity()
        {
            CheckIfLanding();

            //drop through logic
            if (!droppingThrough)
            {
                _droppingThroughTime = 0f;
            }
            else
            {

                _droppingThroughTime += Time.deltaTime;
                if (_droppingThroughTime >= _droppingThroughMaxTime)
                {
                    droppingThrough = false;
                }
            }

            Vector2 move = Vector2.zero;
            isGravityEnabled = !isDashing;
            if (GameController.gameState == GameController.GameStates.TransitionIn || GameController.gameState == GameController.GameStates.TransitionOut)
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
            else if (isDashing)
            {
                move.x = dashSpeed * (_spriteRenderer.flipX ? -1f : 1f);
            }
            if (grounded)
            {
                _canDash = true;
            }
            if (_dashTimer > 0)
            {
                _dashTimer -= Time.deltaTime;
            }
            _animator.SetBool("horizontalMoveDesired", Mathf.Abs(move.x) > 0.1);
            if (!grounded)
            {
                _animator.SetBool("skipLand", _timeSinceGrounded < 0.5f);
            }
            _animator.SetBool("grounded", grounded);
            _animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);
            _animator.SetFloat("velocityY", velocity.y);

            targetVelocity = move * maxSpeed;
            isGrounded = grounded;
            _timeSinceGrounded = isGrounded ? 0f : _timeSinceGrounded + Time.deltaTime;
        }

        private void SpriteFlipping(ref Vector2 move)
        {
            // bool flipSprite = (transform.localScale.x == -1f ? (move.x > 0.01f) : (move.x < -0.01f));
            // if (flipSprite)
            // {
            //     var scale = transform.localScale;
            //     scale.x = -scale.x;
            //     transform.localScale = scale;
            // }
            bool flipSprite = _spriteRenderer.flipX ? move.x > 0.01f : move.x < -0.01f;
            if (flipSprite)
            {
                _spriteRenderer.flipX = !_spriteRenderer.flipX;
            }
            var scale = invertOnFlipXGroup.localScale;
            scale.x = _spriteRenderer.flipX ? -1f : 1f;
            invertOnFlipXGroup.localScale = scale;
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
                    _animator.SetBool("dash", false);
                }
                _doubleJumpPossible = hasDoubleJump;
            }
            _previousGrounded = grounded;
        }

        private void Jump()
        {
            if (!_lifeController.IsAlive || MenuController.CanvasMode != MenuController.CanvasModes.Game) return;
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

        private void Dash()
        {
            if (!_lifeController.IsAlive || !_canDash || _dashTimer > 0 || MenuController.CanvasMode != MenuController.CanvasModes.Game) return;
            _animator.SetBool("dash", true);
            _canDash = false;
            _dashTimer = timeBetweenDashes;
        }

        private void DropThrough()
        {
            droppingThrough = true;
        }

        private void OnGameStateChanged(GameController.GameStates gameState)
        {
            if (_animator == null) return;
            switch (gameState)
            {
                case GameController.GameStates.Running:
                    _animator.enabled = true;
                    break;
                case GameController.GameStates.Paused:
                    _animator.enabled = false;
                    break;
            }
        }
    }
}