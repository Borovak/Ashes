using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UiFlaskController : MonoBehaviour
{
    public enum FlaskTypes
    {
        Health,
        Mana
    }

    public RawImage[] images;
    public float offsetScale = 1f;
    public int index;
    public float heightAtFull;
    public Color color;
    public RectTransform quantityMaskRectTransform;
    public RectTransform liquidLineRectTransform;
    public Image liquidLineImage;
    public Image liquidBottom;
    public FlaskTypes flaskType;

    private Func<float> _getValue;
    private Func<float> _getMax;

    // Start is called before the first frame update
    void Start()
    {
        GameController.PlayerSpawned += OnPlayerSpawned;
        index = Convert.ToInt32(UnityEngine.Random.Range(0f, 5000f));
        for (int i = 0; i < images.Length; i++)
        {
            var image = images[i];
            var offset = image.uvRect;
            offset.x = UnityEngine.Random.Range(0f, 1f);
            offset.y = UnityEngine.Random.Range(0f, 1f);
            image.uvRect = offset;
        }
        liquidBottom.color = color;
        liquidLineImage.color = color;
        OnPlayerSpawned(GameObject.FindGameObjectWithTag("Player"));
    }

    // Update is called once per fraame
    void Update()
    {
        if (_getValue == null) return;
        var rect = quantityMaskRectTransform.rect;
        var liquidHeight = _getValue() / _getMax() * heightAtFull;
        quantityMaskRectTransform.sizeDelta = new Vector2(rect.width, liquidHeight);
        liquidLineRectTransform.anchoredPosition = new Vector2(liquidLineRectTransform.anchoredPosition.x, liquidHeight);
        for (int i = 0; i < images.Length; i++)
        {
            var image = images[i];
            var offset = image.uvRect;
            var direction = GetDirection(i);
            offset.x += (Mathf.PerlinNoise(Convert.ToSingle(index * i), 0f) - 0.5f) * offsetScale * direction.x;
            offset.y += (Mathf.PerlinNoise(Convert.ToSingle(index * i) - 1000f, 0f) - 0.5f) * offsetScale * direction.y;
            image.uvRect = offset;
        }
        index++;
        if (index > 100000)
        {
            index = 0;
        }
    }

    private void OnPlayerSpawned(GameObject player)
    {
        switch (flaskType)
        {
            case FlaskTypes.Health:
                var playerLifeController = player.GetComponent<PlayerLifeController>();
                _getValue = () => Convert.ToSingle(playerLifeController.hp);
                _getMax = () => Convert.ToSingle(playerLifeController.maxHp);
                break;
            case FlaskTypes.Mana:
                var playerManaController = player.GetComponent<ManaController>();
                _getValue = () => Convert.ToSingle(playerManaController.mp);
                _getMax = () => Convert.ToSingle(playerManaController.maxMp);
                break;
        }
    }

    private Vector2 GetDirection(int index)
    {
        switch (index)
        {
            case 0:
                return new Vector2(1f, 1f);
            case 1:
                return new Vector2(1f, -1f);
            case 2:
                return new Vector2(-1f, 1f);
        }
        return new Vector2(-1f, -1f);
    }
}
