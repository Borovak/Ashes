using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InvinsibilityController : MonoBehaviour
{

    public float invinsibilityPeriodAfterDamage = 2f;

    private float _invinsibleFor;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (_invinsibleFor > 0)
        {
            _invinsibleFor -= Time.deltaTime;
        }
    }

    public bool TryTakeDamage()
    {
        if (_invinsibleFor > 0) return false;
        _invinsibleFor = invinsibilityPeriodAfterDamage;
        return true;
    }

}
