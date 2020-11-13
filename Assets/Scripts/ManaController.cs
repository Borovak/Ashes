using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaController : MonoBehaviour
{
    public float maxMp;
    public float mp;    
    public float mpRegenPerSec;

    // Start is called before the first frame update
    void Start()
    {
        mp = SaveSystem.LastLoadedSave.Mp;
        maxMp = SaveSystem.LastLoadedSave.MaxMp;
        mpRegenPerSec = SaveSystem.LastLoadedSave.MpRegenPerSec;
        SaveSystem.GameSaved += OnGameSaved;
    }

    // Update is called once per frame
    void Update()
    {
        if (mp < maxMp)
        {
            mp = System.Math.Min(maxMp, mp + mpRegenPerSec * Time.deltaTime);
        }
    }

    public bool TryCastSpell(float cost)
    {
        if (mp > cost)
        {
            mp -= cost;
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnGameSaved(bool healOnSave)
    {
        if (!healOnSave) return;
        mp = SaveSystem.LastLoadedSave.MaxHp;
    }
}
