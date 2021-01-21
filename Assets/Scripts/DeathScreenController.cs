using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeathScreenController : MonoBehaviour
{
    private readonly Color transparent = new Color(0f, 0f, 0f, 0f);

    public Image backImage;
    public TextMeshProUGUI youDiedText;
    public float fadeInDuration = 1f;
    public Color backColor;
    public Color textColor;

    private static DeathScreenController _instance;
    private float _timer;
    private float _ratio;

    // Start is called before the first frame update
    void Start()
    {
        _instance = this; 
        Hide();
    }

    void Update()
    {
        if (_timer >= fadeInDuration) return;
        _timer += Time.deltaTime;
        _ratio = _timer / fadeInDuration;
        backImage.color = Color.Lerp(transparent, backColor, _ratio);
        youDiedText.color = Color.Lerp(transparent, textColor, _ratio);
    }

    private void ChangeVisibility(bool visible)
    {
        backImage.enabled = true;
        youDiedText.enabled = true;
        if (visible)
        {
            _timer = 0;
        }
        else
        {
            _timer = fadeInDuration;
            backImage.color = transparent;
            youDiedText.color = transparent;
        }
    }

    public static void Show()
    {
        _instance.ChangeVisibility(true);
    }

    public static void Hide()
    {
        _instance.ChangeVisibility(false);
    }
}
