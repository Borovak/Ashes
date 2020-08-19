using System;
using System.Collections;
using System.Collections.Generic;
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
        _panel = GetComponent<Image>();
        _newGame = transform.Find("NewGame").gameObject;
        _zoneName = transform.Find("ZoneName").gameObject;
        _gameTime = transform.Find("GameTime").gameObject;
        _doubleJump = transform.Find("DoubleJump").gameObject;
        Load();
    }

    void OnEnable()
    {
        SaveSystem.SaveDeleted += Reload;
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
        Load();
    }

    public void Load()
    {
        SaveSystem.index = index;
        transform.Find("Id").GetComponent<Text>().text = $"Save {index + 1}";
        dataPresent = SaveSystem.Load(out var data, out _);
        _newGame.SetActive(data == null);
        _zoneName.SetActive(data != null);
        _gameTime.SetActive(data != null);
        _doubleJump.SetActive(data != null && data.HasDoubleJump);
        if (data != null)
        {
            var chamber = LocationInformation.SavePoints[data.SavePointGuid].Chamber;
            _zoneName.GetComponent<Text>().text = $"{chamber.ZoneName}/{chamber.Name}";
            var t = data.GameTime;
            var h = Convert.ToInt32(t / 3600f);
            t -= h * 3600f;
            var m = Convert.ToInt32(t / 60f);
            _gameTime.GetComponent<Text>().text = $"{h}:{m.ToString("00")}";
        }
    }

}
