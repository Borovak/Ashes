using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerLifeController : LifeController
{
    public static event Action<float> HpChanged;
    public static event Action<float> MaxHpChanged;
    private int _hp;
    private int _maxHp;
    public AudioClip deathMusic;

    protected override void AfterStart()
    {
        _isPlayer = true;
        SaveSystem.GameSaved += OnGameSaved;
        SetHp(SaveSystem.LastLoadedSave.Hp);
        SetMaxHp(SaveSystem.LastLoadedSave.MaxHp);
    }

    // Update is called once per frame
    protected override void AfterUpdate()
    {
    }

    protected override void SetMaxHp(int value)
    {
        _maxHp = value;
        MaxHpChanged?.Invoke(_maxHp);
    }

    public override int GetMaxHp()
    {
        return _maxHp;
    }

    protected override void SetHp(int value)
    {
        _hp = value;
        HpChanged?.Invoke(_hp);
    }

    public override int GetHp()
    {
        return _hp;
    }

    protected override void OnDeath()
    {
        DeathScreenController.Show();
    }

    private void OnGameSaved(bool healOnSave)
    {
        if (!healOnSave) return;
        SetHp(SaveSystem.LastLoadedSave.MaxHp);
    }
}
