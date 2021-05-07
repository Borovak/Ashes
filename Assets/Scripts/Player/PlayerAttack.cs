using System.Collections.Generic;
using Classes;
using Static;
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
        public GameObject energySwipePrefab;
        public GameObject energyBallPrefab;
        public float chargingDuration = 0.05f;
        public float attackDuration = 0.05f;
        public Light2D attackLight;

        private float _attackCooldown;
        private Animator _animator;
        private PlayerInputs _inputs;
        private ManaController _manaController;
        private PlayerLifeController _lifeController;
        private AttackStates _attackState;
        private float _attackStateTimeRemaining;
        private Constants.SpellElements _nextSpellElement;


        // Start is called before the first frame update
        void Start()
        {
            _animator = GetComponent<Animator>();
            _inputs = GetComponent<PlayerInputs>();
            _manaController = GetComponent<ManaController>();
            _lifeController = GetComponent<PlayerLifeController>();
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
            if (_attackState == AttackStates.Idle) return;
            _attackStateTimeRemaining -= Time.deltaTime;
            if (!(_attackStateTimeRemaining <= 0)) return;
            switch (_attackState)
            {
                case AttackStates.Charging:
                    _attackState = AttackStates.Attack;
                    _attackStateTimeRemaining = attackDuration;
                    var energySwipeObject = Instantiate(energySwipePrefab, attackLight.transform.position, Quaternion.identity, attackLight.transform);
                    var energySwipeSpriteRenderer = energySwipeObject.GetComponent<SpriteRenderer>();
                    energySwipeSpriteRenderer.color = SpellElementManager.GetColorFromSpellElement(_nextSpellElement);
                    break;
                case AttackStates.Attack:
                    _attackState = AttackStates.Idle;
                    break;
            }
        }

        public void EnergySwipe(Constants.SpellElements spellElement)
        {
            if (!_lifeController.IsAlive || _attackCooldown > 0 || MenuController.CanvasMode != MenuController.CanvasModes.Game) return;
            _nextSpellElement = spellElement;
            _animator.SetTrigger("energySwipe");
            MeleeAttack(spellElement);
            _attackCooldown = 1f / attackRate;
            _attackState = AttackStates.Charging;
            _attackStateTimeRemaining = chargingDuration;
        }

        public void EnergyBall(Constants.SpellElements spellElement)
        {
            if (!_lifeController.IsAlive || _attackCooldown > 0 || MenuController.CanvasMode != MenuController.CanvasModes.Game) return;
            if (!_manaController.TryCastSpell(3f)) return;
            _nextSpellElement = spellElement;
            _animator.SetTrigger("energyBall");
            _attackCooldown = 1f / attackRate;
        }

        public void Heal()
        {
            if (!_lifeController.IsAlive || _attackCooldown > 0 || MenuController.CanvasMode != MenuController.CanvasModes.Game) return;
            if (!_manaController.TryCastSpell(5f)) return;
            _animator.SetTrigger("heal");
            _lifeController.Heal(10f);
            _attackCooldown = 1f / attackRate;
        }

        private void GroundBreak()
        {
            if (!_lifeController.IsAlive || _attackCooldown > 0 || MenuController.CanvasMode != MenuController.CanvasModes.Game) return;
            if (!_manaController.TryCastSpell(5f)) return;
            _animator.SetTrigger("groundBreak");
            _attackCooldown = 1f / attackRate;
        }

        private void MeleeAttack(Constants.SpellElements spellElement)
        {
            var enemiesHit = new HashSet<GameObject>();
            var hits = Physics2D.CircleCastAll(attackLight.transform.position, attackRange, Vector2.zero, 0f, whatIsEnemies);
            for (int i = 0; i < hits.Length; i++)
            {
                var hit = hits[i];            
                if (!hit.collider.gameObject.TryGetComponent<EnemyLifeController>(out var enemy) || enemiesHit.Contains(hit.collider.gameObject)) continue;
                enemiesHit.Add(hit.collider.gameObject);
                enemy.TakeDamage(attackDamage, gameObject.name, hit.point, false, spellElement);
            }
        }

        void InstantiateEnergyBall()
        {
            var energyBallObject = Instantiate(energyBallPrefab, attackLight.transform.position, Quaternion.identity);
            var controller = energyBallObject.GetComponent<EnergyBallController>();
            controller.speed = 20f;
            controller.damage = 3f;
            controller.diameter = 0.5f;
            controller.spellElement = _nextSpellElement;
            controller.destination = attackLight.transform.transform.position.x > transform.position.x ? attackLight.transform.transform.position + Vector3.right * 1000f : attackLight.transform.transform.position + Vector3.left * 1000f;
            controller.emitFromPlayer = true;
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackLight.transform.position, attackRange);
        }
    }
}
