using Classes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIRecipeIngredient : MonoBehaviour
    {
        public Item Item
        {
            get => _item;
            set
            {
                _item = value;
                if (_item == null)
                {
                    backImage.color = _colorWhenNull;
                    itemImage.sprite = null;
                    itemImage.color = _colorWhenNull;
                    itemName.text = "";
                    Quantity = -1;
                }
                else
                {
                    itemImage.sprite = _item.GetArt();
                    itemImage.color = Color.white;
                    itemName.text = _item.name;
                }
            }
        }

        public int Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                itemQuantity.text = _quantity >= 0 ? $"x{_quantity.ToString()}" : "";
            }
        }

        public int PlayerInventory{
            get => _playerInventory;
            set
            {
                _playerInventory = value;
                backImage.color = _playerInventory >= Quantity ? _colorWhenEnoughQuantity : _colorWhenNotEnoughQuantity;
            }
        }

        public Image backImage;
        public Image itemImage;
        public TextMeshProUGUI itemName;
        public TextMeshProUGUI itemQuantity;

        private Item _item;
        private int _quantity;
        private int _playerInventory;
        private readonly Color _colorWhenEnoughQuantity = new Color(0.2f, 0.6f, 0.2f, 0.4f);
        private readonly Color _colorWhenNotEnoughQuantity = new Color(0.6f, 0.2f, 0.2f, 0.4f);
        private readonly Color _colorWhenNull = new Color(0f, 0f, 0f, 0f);
    }
}
