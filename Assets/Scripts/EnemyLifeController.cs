using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyLifeController : LifeController
{    
    public int maxHp = 3;
    public int hp;

    protected override void AfterStart(){
        hp = maxHp;
        _isPlayer = false;
        SetHp(hp);
        SetMaxHp(maxHp);
    }

    // Update is called once per frame
    protected override void AfterUpdate()
    {
    }

    protected override void SetMaxHp(int value)
    {
        maxHp = value;
    }

    protected override int GetMaxHp()
    {
        return maxHp;
    }

    protected override void SetHp(int value)
    {
        hp = value;
    }

    protected override int GetHp()
    {
        return hp;
    }
}
