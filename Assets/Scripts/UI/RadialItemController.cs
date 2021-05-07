using Classes;
using Interfaces;
using Static;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class RadialItemController : MonoBehaviour
    {
        public Image backImageControl;
        public Image borderImageControl;
        public Image imageControl;
        public TextMeshProUGUI nameTextControl;
        public TextMeshProUGUI quantityTextControl;
        public bool isSelected;
        public Color backColorWhenSelected;
        public Color backColorWhenNotSelected;
        public Color borderColorWhenSelected;
        public Color borderColorWhenNotSelected;
        public GameObject assignNoticeControl;

        private IIconElement _iconElement;
        private bool _isSelected;

        void OnEnable()
        {
            UpdateColor();
        }
    
        void Update()
        {
            if (isSelected == _isSelected) return;
            _isSelected = isSelected;
            UpdateColor();
            assignNoticeControl.SetActive(isSelected);
        }

        private void UpdateColor()
        {
            backImageControl.color = _isSelected ? backColorWhenSelected : backColorWhenNotSelected;
            borderImageControl.color = _isSelected ? borderColorWhenSelected : borderColorWhenNotSelected;
        }
    
        public void UpdateContent(IIconElement iconElement, float angle, float vectorMultiplier)
        {
            _iconElement = iconElement;
            imageControl.sprite = _iconElement.sprite;
            nameTextControl.text = _iconElement.name;
            quantityTextControl.text = _iconElement.iconElementType == Constants.IconElementTypes.Item ? _iconElement.quantity.ToString() : "";
            var position = MathFunctions.DegreeToVector2(angle + 90f) * vectorMultiplier;
            position.x = -position.x;
            var rectTransform = GetComponent<RectTransform>();
            rectTransform.anchoredPosition = position;
        }
    }
}
