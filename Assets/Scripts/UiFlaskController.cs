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
        index = Convert.ToInt32(UnityEngine.Random.Range(0f, 5000f));
        for (int i = 0; i < images.Length; i++)
        {
            var image = images[i];
            var offset = image.uvRect;
            offset.x = UnityEngine.Random.Range(0f, 1f);
            offset.y = UnityEngine.Random.Range(0f, 1f);
            image.uvRect = offset;
        }
        var player = GameObject.FindGameObjectWithTag("Player");
        switch (flaskType)
        {
            case FlaskTypes.Health:
                _getValue = () => Convert.ToSingle(player.GetComponent<LifeController>().hp);
                _getMax = () => Convert.ToSingle(player.GetComponent<LifeController>().maxHp);
                break;
            case FlaskTypes.Mana:
                _getValue = () => player.GetComponent<ManaController>().mp;
                _getMax = () => player.GetComponent<ManaController>().maxMp;
                break;
        }
        liquidBottom.color = color;
        liquidLineImage.color = color;
    }

    // Update is called once per frame
    void Update()
    {
        var rect = quantityMaskRectTransform.rect;
        var liquidHeight = _getValue() / _getMax() * heightAtFull;
        quantityMaskRectTransform.sizeDelta = new Vector2(rect.width, liquidHeight);
        liquidLineRectTransform.anchoredPosition = new Vector2(liquidLineRectTransform.anchoredPosition.x, liquidHeight);
        for (int i = 0; i < images.Length; i++)
        {
            var image = images[i];
            var offset = image.uvRect;
            offset.x += (Mathf.PerlinNoise(Convert.ToSingle(index * i), 0f) - 0.5f) * offsetScale;
            offset.y += (Mathf.PerlinNoise(Convert.ToSingle(index * i) - 1000f, 0f) - 0.5f) * offsetScale;
            image.uvRect = offset;
        }
        index++;
        if (index > 100000)
        {
            index = 0;
        }
    }
}
