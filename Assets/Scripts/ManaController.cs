using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaController : MonoBehaviour
{
    public float maxMp => SaveData.workingData.MaxMp;
    public float mp {
        get => SaveData.workingData.Mp;
        set => SaveData.workingData.Mp = value;
    }
    public float mpRegenPerSec;

    // Start is called before the first frame update
    void Start()
    {

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
}
