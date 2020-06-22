using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiLifeController : MonoBehaviour
{

    private Dictionary<int, Image> _images;
    private bool _initDone;
    private LifeController _playerLifeController;

    void Awake()
    {
        _images = new Dictionary<int, Image>();
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            if (!child.name.StartsWith("Life")) continue;
            var number = Convert.ToInt32(child.name.Substring(4));
            _images.Add(number, child.GetComponent<Image>());
        }
        _playerLifeController = PlayerPlatformerController.Instance.GetComponent<LifeController>();
        _playerLifeController.HpChanged += OnHpChanged;
    }

    void OnDisable()
    {
        _playerLifeController.HpChanged -= OnHpChanged;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_initDone)
        {
            _initDone = true;
            OnHpChanged(_playerLifeController.hp);
        }
    }

    private void OnHpChanged(int hp)
    {
        Debug.Log($"Player changed to {hp}");
        foreach (var d in _images)
        {
            var c = d.Value.color;
            c.a = hp >= d.Key ? 1f : 0f;
            d.Value.color = c;
        }
    }
}
