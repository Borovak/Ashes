using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Static;
using UnityEngine;

public class EnemyLifeController : LifeController
{    
    public int id;
    public float maxHp = 3;
    public float hp;
    public bool isBoss;
    public bool awake;

    protected override void AfterStart(){
        hp = maxHp;
        SetHp(hp);
        SetMaxHp(maxHp);
    }

    // Update is called once per frame
    protected override void AfterUpdate()
    {
    }

    protected override void SetMaxHp(float value)
    {
        maxHp = value;
    }

    public override float GetMaxHp()
    {
        return maxHp;
    }

    protected override void SetHp(float value)
    {
        hp = value;
    }

    public override float GetHp()
    {
        return hp;
    }

    protected override void OnDeath()
    {
        if (!DropController.GetDrops(transform.position, id, out var drops)) return;
        //Debug.Log($"Enemy {gameObject.name} drops {drops.Count} items: ({string.Join(",", drops.Select(x => x.name))})");
    }
}
