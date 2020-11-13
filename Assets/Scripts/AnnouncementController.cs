using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnnouncementController : MonoBehaviour
{
    public float timeShown;
    public float fadeOutTime;
    public TextMeshProUGUI largeAnnouncement;
    public TextMeshProUGUI smallAnnouncement;
    private float _timeLeft;
    private float _alpha;

    void Start()
    {
        largeAnnouncement.text = "";
        smallAnnouncement.text = "";
    }

    void OnEnable()
    {
        //SaveSystem.GameSaved += OnGameSaved;
        ChamberController.ZoneChanged += OnZoneChanged;
    }

    void OnDisable()
    {
        //SaveSystem.GameSaved -= OnGameSaved;
        ChamberController.ZoneChanged -= OnZoneChanged;
    }

    void Update()
    {
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

    private void OnZoneChanged(string zoneName, string chamberName)
    {
        ShowMessage(zoneName, chamberName);
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
