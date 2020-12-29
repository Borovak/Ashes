using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeathScreenController : MonoBehaviour
{
    public Image backImage;
    public TextMeshProUGUI youDiedText;
    public float fadeInDuration = 1f;
    public Color backColor;
    public Color textColor;

    private readonly Color transparent = new Color(0,0,0,0);
    public float _timer;

    public float ratio;

    // Start is called before the first frame update
    void Start()
    {
    }

    void Update()
    {
        if (_timer >= fadeInDuration) return;
        _timer += Time.deltaTime;
        ratio = _timer / fadeInDuration;
        backImage.color = Color.Lerp(transparent, backColor, ratio);
        youDiedText.color = Color.Lerp(transparent, textColor, ratio);
    }

    void OnEnable()
    {
        _timer = 0;
        backImage.color = transparent;
        youDiedText.color = transparent;
    }
}
