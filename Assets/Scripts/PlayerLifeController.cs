using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerLifeController : LifeController
{    
    
    protected override void AfterStart(){
        _isPlayer = true;
        SetHp(SaveData.workingData.Hp);
        SetMaxHp(SaveData.workingData.MaxHp);
    }

    // Update is called once per frame
    protected override void AfterUpdate()
    {
    }

    protected override void SetMaxHp(int value)
    {
        SaveData.workingData.MaxHp = value;
    }

    protected override int GetMaxHp()
    {
        return SaveData.workingData.MaxHp;
    }

    protected override void SetHp(int value)
    {
        SaveData.workingData.Hp = value;
    }

    protected override int GetHp()
    {
        return SaveData.workingData.Hp;
    }

    protected override void OnDeath()
    {
    }
}
