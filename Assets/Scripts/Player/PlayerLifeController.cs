using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerLifeController : LifeController
{
    public int hp;
    public int maxHp;

    protected override void AfterStart()
    {
        _isPlayer = true;
        SaveSystem.GameSaved += OnGameSaved;
        hp = SaveSystem.LastLoadedSave.Hp;
        maxHp = SaveSystem.LastLoadedSave.MaxHp;
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

    protected override void OnDeath()
    {
    }

    private void OnGameSaved(bool healOnSave)
    {
        if (!healOnSave) return;
        hp = SaveSystem.LastLoadedSave.MaxHp;
    }
}
