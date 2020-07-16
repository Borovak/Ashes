using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegionAnnouncementController : MonoBehaviour
{

    public static string LastRegionVisited = "";
    public float timeShown;
    public float fadeOutTime;

    private int _previousRegion = -1;
    private float _timeLeft;
    private Text _text;
    private float _alpha;

    void Start()
    {
        _text = GetComponent<Text>();
    }

    void Update()
    {
        if (LocationManager.currentChamberId >= 0)
        {
            var chamber = LocationManager.GetChamber();
            _previousRegion = chamber.zoneId;
            _timeLeft = timeShown;
            _text.text = chamber.zoneName;
            _alpha = 1f;
            SetAlpha();
        }
        if (_timeLeft > 0)
        {
            _timeLeft -= Time.deltaTime;
        }
        else if (_alpha > 0)
        {
            _alpha -= (1f / fadeOutTime) * Time.deltaTime;
            SetAlpha();
        }
    }

    private void SetAlpha(){        
        var c = _text.color;
        c.a = Mathf.Clamp01(_alpha);
        _text.color = c;
    }
}
