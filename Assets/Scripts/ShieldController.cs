using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class ShieldController : MonoBehaviour
{
    public float manaPerSecond = 2f;
    public float manaPerDamage = 3f;
    public ParticleSystem shield;
    public GameObject shieldContactPrefab;
    public bool shieldState;
    public bool shieldWantedState;

    private ManaController _manaController;
    private PlayerInputs _inputs;
    private EmissionModule _emissionModule;


    // Start is called before the first frame update
    void Start()
    {
        _inputs = GetComponent<PlayerInputs>();
        _manaController = GetComponent<ManaController>();
        _emissionModule = shield.emission;
        //ActionAssignmentController.Attach(2, OnShieldActivated, OnShieldDesactivated);
    }

    // Update is called once per frame
    void Update()
    {
        var manaCost = manaPerSecond * Time.deltaTime;
        if (shieldWantedState)
        {
            shieldState = _manaController.TryCastSpell(manaCost);
        }
        else
        {
            shieldState = false;
        }
        _emissionModule.enabled = shieldState;
    }

    public bool AbsorbHit(float damage, out float damageNotAbsorbed)
    {
        if (!shieldState)
        {
            damageNotAbsorbed = damage;
            return false;
        }
        if (!_manaController.TrySpendMana(damage * manaPerDamage, out var manaNotSpent))
        {
            damageNotAbsorbed = manaNotSpent / manaPerDamage;
            return false;
        }
        damageNotAbsorbed = 0;
        return true;
    }

    public void ShowContact(Vector2 point){
        if (!shieldState) return;
        var shieldContactInstance = GameObject.Instantiate<GameObject>(shieldContactPrefab, new Vector3(point.x, point.y), Quaternion.identity);
        shieldContactInstance.transform.parent = transform;
        GameObject.Destroy(shieldContactInstance, 1f);
    }
}
