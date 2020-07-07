using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegionAnnouncementController : MonoBehaviour
{

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
        if (GameController.currentChamber != null && GameController.currentChamber.region != _previousRegion)
        {
            _previousRegion = GameController.currentChamber.region;
            _timeLeft = timeShown;
            _text.text = GetText(_previousRegion);
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

    public static string GetText(int zone)
    {
        var zoneText = new Dictionary<int, string> {
            {1, "Elder Woods"},
            {2, "Cathedral of Whispers"},
            {3, "Grand River"}
        };
        return zoneText.TryGetValue(zone, out var text) ? text : "";
    }

    private void SetAlpha(){        
        var c = _text.color;
        c.a = Mathf.Clamp01(_alpha);
        _text.color = c;
    }
}
