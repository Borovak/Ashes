using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaController : MonoBehaviour
{
    public static event Action<float> MpChanged;
    public static event Action<float> MaxMpChanged;
    public float mpRegenPerSec;

    private float _maxMp;
    private float _mp;

    // Start is called before the first frame update
    void Start()
    {
        SetMp(SaveSystem.LastLoadedSave.Mp);
        SetMaxMp(SaveSystem.LastLoadedSave.MaxMp);
        mpRegenPerSec = SaveSystem.LastLoadedSave.MpRegenPerSec;
        SaveSystem.GameSaved += OnGameSaved;
    }

    // Update is called once per frame
    void Update()
    {
        if (GetMp() < _maxMp)
        {            
            SetMp(System.Math.Min(_maxMp, GetMp() + mpRegenPerSec * Time.deltaTime));
        }
    }

    public bool TryCastSpell(float cost)
    {
        if (GetMp() > cost)
        {
            SetMp(GetMp() - cost);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool TrySpendMana(float cost, out float manaNotSpent)
    {
        if (GetMp() >= cost)
        {
            SetMp(GetMp() - cost);
            manaNotSpent = 0;
            return true;
        }
        else
        {
            manaNotSpent = cost - GetMp();
            SetMp(0f);
            return false;
        }
    }

    public float GetMp()
    {
        return _mp;
    }

    public float GetMaxMp()
    {
        return _maxMp;
    }

    public void SetMp(float value)
    {
        _mp = Math.Min(value, _maxMp);
        MpChanged?.Invoke(value);
    }

    public void SetMaxMp(float maxValue)
    {
        _maxMp = maxValue;
        MaxMpChanged?.Invoke(maxValue);
    }

    public void Gain(float value)
    {
        SetMp(_mp + value);
    }

    private void OnGameSaved(bool healOnSave)
    {
        if (!healOnSave) return;
        SetMp(SaveSystem.LastLoadedSave.MaxMp);
    }
}
