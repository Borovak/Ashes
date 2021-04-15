using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Player
{
    public class PlayerAttack : MonoBehaviour
    {
        private enum AttackStates
        {
            Idle,
            Charging,
            Attack
        }

        public float attackRange;
        public int attackDamage;
        public float attackRate;
        public LayerMask whatIsEnemies;
        public GameObject fireballPrefab;
        public float chargingDuration = 0.05f;
        public float attackDuration = 0.05f;
        public Light2D attackLight;
        public GameObject attackPrefab;

        private float _attackCooldown;
        private Animator _animator;
        private AudioSource _audioSource;
        private PlayerInputs _inputs;
        private ManaController _manaController;
        private PlayerLifeController _lifeController;
        private AttackStates _attackState;
        private float _attackStateTimeRemaining;


        // Start is called before the first frame update
        void Start()
        {
            _animator = GetComponent<Animator>();
            _audioSource = GetComponent<AudioSource>();
            _inputs = GetComponent<PlayerInputs>();
            _manaController = GetComponent<ManaController>();
            _lifeController = GetComponent<PlayerLifeController>();
            ActionAssignmentController.Attach(1, Attack);
            ActionAssignmentController.Attach(3, SelfSpell);
            ActionAssignmentController.Attach(4, AttackSpell);
            _inputs.GroundBreak += GroundBreak;
        }

        // Update is called once per frame
        void Update()
        {
            if (_attackCooldown > 0)
            {
                _attackCooldown -= Time.deltaTime;
            }
            attackLight.enabled = _attackState == AttackStates.Attack;
            if (_attackState != AttackStates.Idle)
            {
                _attackStateTimeRemaining -= Time.deltaTime;
                if (_attackStateTimeRemaining <= 0)
                {
                    switch (_attackState)
                    {
                        case AttackStates.Charging:
                            _attackState = AttackStates.Attack;
                            _attackStateTimeRemaining = attackDuration;
                            GameObject.Instantiate(attackPrefab, attackLight.transform.position, Quaternion.identity, attackLight.transform);
                            break;
                        case AttackStates.Attack:
                            _attackState = AttackStates.Idle;
                            break;
                    }
                }
            }
        }

        private void Attack()
        {
            if (!_lifeController.IsAlive || _attackCooldown > 0 || MenuController.CanvasMode != MenuController.CanvasModes.Game) return;
            _animator.SetTrigger("attack");
            MeleeAttack();
            _attackCooldown = 1f / attackRate;
            _attackState = AttackStates.Charging;
            _attackStateTimeRemaining = chargingDuration;
        }

        private void AttackSpell()
        {
            if (!_lifeController.IsAlive || _attackCooldown > 0 || MenuController.CanvasMode != MenuController.CanvasModes.Game) return;
            if (!_manaController.TryCastSpell(3f)) return;
            _animator.SetTrigger("fireball");
            _attackCooldown = 1f / attackRate;
        }

        private void SelfSpell()
        {
            if (!_lifeController.IsAlive || _attackCooldown > 0 || MenuController.CanvasMode != MenuController.CanvasModes.Game) return;
            if (!_manaController.TryCastSpell(5f)) return;
            _animator.SetTrigger("heal");
            _lifeController.Heal(1);
            _attackCooldown = 1f / attackRate;
        }

        private void GroundBreak()
        {
            if (!_lifeController.IsAlive || _attackCooldown > 0 || MenuController.CanvasMode != MenuController.CanvasModes.Game) return;
            if (!_manaController.TryCastSpell(5f)) return;
            _animator.SetTrigger("groundBreak");
            _attackCooldown = 1f / attackRate;
        }

        private void MeleeAttack()
        {
            var enemiesHit = new HashSet<GameObject>();
            var hits = Physics2D.CircleCastAll(attackLight.transform.position, attackRange, Vector2.zero, 0f, whatIsEnemies);
            for (int i = 0; i < hits.Length; i++)
            {
                var hit = hits[i];            
                if (!hit.collider.gameObject.TryGetComponent<EnemyLifeController>(out var enemy) || enemiesHit.Contains(hit.collider.gameObject)) continue;
                enemiesHit.Add(hit.collider.gameObject);
                enemy.TakeDamage(attackDamage, gameObject.name, hit.point);
            }
        }

        void CastFireball()
        {
            var fireballObject = Instantiate(fireballPrefab, attackLight.transform.position, Quaternion.identity);
            var fireball = fireballObject.GetComponent<DirectionalFireball>();
            fireball.speed = 20f;
            fireball.damage = 3;
            fireball.diameter = 0.3f;
            fireball.destination = attackLight.transform.transform.position.x > transform.position.x ? attackLight.transform.transform.position + Vector3.right * 1000f : attackLight.transform.transform.position + Vector3.left * 1000f;
            fireball.emitFromPlayer = true;
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackLight.transform.position, attackRange);
        }
    }
}
