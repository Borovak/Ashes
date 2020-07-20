using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnnouncementController : MonoBehaviour
{


    public float timeShown;
    public float fadeOutTime;
    public Text largeAnnouncement;
    public Text smallAnnouncement;
    private float _timeLeft;
    private float _alpha;
    private static int _previousZone = -1;

    void Start()
    {
        SaveSystem.GameSaved += OnGameSaved;
    }

    void OnDisable()
    {
        SaveSystem.GameSaved -= OnGameSaved;
    }

    void Update()
    {
        ZoneChange();
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

    private void OnGameSaved()
    {
        ShowMessage("Game saved", "");
    }

    private void ZoneChange()
    {
        if (LocationManager.currentChamberId == -1) return;
        var chamber = LocationManager.GetChamber(LocationManager.currentChamberId);
        if (chamber.zoneId == _previousZone) return;
        _previousZone = chamber.zoneId;
        Debug.Log($"Zone change to {chamber.zoneId}");
        ShowMessage(chamber.zoneName, chamber.name);
    }

    private void ShowMessage(string largeMessage, string smallMessage)
    {
        _timeLeft = timeShown;
        largeAnnouncement.text = largeMessage;
        smallAnnouncement.text = smallMessage;
        _alpha = 1f;
        SetAlpha();
    }

    private void SetAlpha()
    {
        var c = largeAnnouncement.color;
        c.a = Mathf.Clamp01(_alpha);
        largeAnnouncement.color = c;
        smallAnnouncement.color = c;
    }
}
