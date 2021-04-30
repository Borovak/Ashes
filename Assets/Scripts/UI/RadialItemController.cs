using Classes;
using Static;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class RadialItemController : MonoBehaviour
    {
        public Image backImageControl;
        public Image imageControl;
        public TextMeshProUGUI nameTextControl;
        public TextMeshProUGUI quantityTextControl;
        public bool isSelected;
        public Color colorWhenSelected;
        public Color colorWhenNotSelected;

        private ItemBundle _itemBundle;
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
        }

        private void UpdateColor()
        {
            backImageControl.color = _isSelected ? colorWhenSelected : colorWhenNotSelected;
        }
    
        public void UpdateContent(ItemBundle itemBundle, float angle, float vectorMultiplier)
        {
            _itemBundle = itemBundle;
            imageControl.sprite = _itemBundle.Item.GetArt();
            nameTextControl.text = _itemBundle.Item.Name;
            quantityTextControl.text = _itemBundle.Quantity.ToString();
            var position = MathFunctions.DegreeToVector2(angle) * vectorMultiplier;
            var rectTransform = GetComponent<RectTransform>();
            rectTransform.anchoredPosition = position;
        }
    }
}
