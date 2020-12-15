using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemGainedController : MonoBehaviour
{
    const float x = -100;

    public Image image;
    public TextMeshProUGUI text;
    public ItemBundle itemBundle;
    public float TimeIn = 0.5f;
    public float TimeBetween = 2f;
    public float TimeOut = 0.5f;


    private Image _panel;
    private RectTransform _rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        _panel = GetComponent<Image>();
        _rectTransform = GetComponent<RectTransform>();
        //Setting initial info
        image.sprite = itemBundle.Item.GetArt();
        text.text = $"x{itemBundle.Quantity} {itemBundle.Item.name} acquired";
        //Getting default colors
        var panelColor = _panel.color;
        var imageColor = image.color;
        var textColor = text.color;
        var invisible = new Color(0f, 0f, 0f, 0f);
        //
        _rectTransform.anchoredPosition = new Vector2(x, -1200);
        var ltY = LeanTween.value(_rectTransform.gameObject, _rectTransform.anchoredPosition, new Vector2(x, -100), TimeIn).setEaseOutSine().setOnUpdate((Vector2 val) => { _rectTransform.anchoredPosition = val; });
        LeanTween.value(_panel.gameObject, invisible, panelColor, TimeIn).setOnUpdate((Color val) => { _panel.color = val; });
        LeanTween.value(image.gameObject, invisible, imageColor, TimeIn).setOnUpdate((Color val) => { image.color = val; });
        LeanTween.value(text.gameObject, invisible, textColor, TimeIn).setOnUpdate((Color val) => { text.color = val; });
        ltY.setOnComplete(() =>
        {
            ltY = LeanTween.value(_rectTransform.gameObject, _rectTransform.anchoredPosition, new Vector2(x, 100), TimeBetween).setOnUpdate((Vector2 val) => { _rectTransform.anchoredPosition = val; });
            ltY.setOnComplete(() =>
            {
                ltY = LeanTween.value(_rectTransform.gameObject, _rectTransform.anchoredPosition, new Vector2(-100, 1200), TimeOut).setEaseInSine().setOnUpdate((Vector2 val) => { _rectTransform.anchoredPosition = val; });
                ltY.setOnComplete(() => GameObject.Destroy(gameObject));
                LeanTween.value(_panel.gameObject, panelColor, invisible, TimeIn).setOnUpdate((Color val) => { _panel.color = val; });
                LeanTween.value(image.gameObject, imageColor, invisible, TimeIn).setOnUpdate((Color val) => { image.color = val; });
                LeanTween.value(text.gameObject, textColor, invisible, TimeIn).setOnUpdate((Color val) => { text.color = val; });
            });
        });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
