using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveGamePanel : MonoBehaviour
{
    public int index;
    public bool dataPresent;

    private Image _panel;
    private GameObject _newGame;
    private GameObject _zoneName;
    private GameObject _gameTime;
    private GameObject _doubleJump;

    // Start is called before the first frame update
    void Start()
    {
    }

    void OnEnable()
    {
        if (_panel == null)
        {
            _panel = GetComponent<Image>();
            _newGame = transform.Find("NewGame").gameObject;
            _zoneName = transform.Find("ZoneName").gameObject;
            _gameTime = transform.Find("GameTime").gameObject;
            _doubleJump = transform.Find("DoubleJump").gameObject;
        }
        SaveSystem.SaveDeleted += Reload;
        UpdateData();
    }

    void OnDiable()
    {
        SaveSystem.SaveDeleted -= Reload;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void Reload(int i)
    {
        if (i != index) return;
        UpdateData();
    }

    public void UpdateData()
    {
        LocationInformation.Init(out _);
        SaveSystem.index = index;
        transform.Find("Id").GetComponent<TextMeshProUGUI>().text = $"Save {index + 1}";
        dataPresent = SaveSystem.Load(out var errorMessage);
        _newGame.SetActive(SaveSystem.LastLoadedSave == null);
        _zoneName.SetActive(SaveSystem.LastLoadedSave != null);
        _gameTime.SetActive(SaveSystem.LastLoadedSave != null);
        _doubleJump.SetActive(SaveSystem.LastLoadedSave != null && SaveSystem.LastLoadedSave.HasDoubleJump);
        if (SaveSystem.LastLoadedSave != null && LocationInformation.SavePoints.ContainsKey(SaveSystem.LastLoadedSave.SavePointGuid))
        {
            var chamber = LocationInformation.SavePoints[SaveSystem.LastLoadedSave.SavePointGuid].Chamber;
            _zoneName.GetComponent<TextMeshProUGUI>().text = $"{chamber.ZoneName}/{chamber.Name}";
            var t = SaveSystem.LastLoadedSave.GameTime;
            var h = Convert.ToInt32(t / 3600f);
            t -= h * 3600f;
            var m = Convert.ToInt32(t / 60f);
            _gameTime.GetComponent<TextMeshProUGUI>().text = $"{h}:{m.ToString("00")}";
        }
        else
        {
            _zoneName.GetComponent<TextMeshProUGUI>().text = $"Ferry";
            _gameTime.GetComponent<TextMeshProUGUI>().text = $"0:00";
        }
    }

    public void LoadSaveFile()
    {
        SaveSystem.index = index;
        var dataPresent = GetComponent<SaveGamePanel>().dataPresent;
        if (!dataPresent)
        {
            SaveSystem.SaveVirgin(out _);
        }
        if (SaveSystem.Load(out var errorMessage))
        {
            SceneManager.LoadScene("Game");
        }
        else
        {
            Debug.Log(errorMessage);
        }
    }

}
