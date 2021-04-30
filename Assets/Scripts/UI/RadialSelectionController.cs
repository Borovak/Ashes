using System.Collections.Generic;
using System.Linq;
using Classes;
using UnityEngine;

namespace UI
{
    public class RadialSelectionController : MonoBehaviour
    {

        public GameObject radialItemPrefab;
    
        private float vectorMultiplier = 800f;
        private List<RadialItem> _radialItems;
        private static RadialItem _selectedRadialItem;
        private static float _angleStep = 90f;

        private class RadialItem
        {
            internal float angle;
            internal float angleMin => angleMinTemp < 0f ? angleMinTemp + 360f : angleMinTemp;
            internal float angleMinTemp => angle - _angleStep / 2f;
            internal float angleMax => angleMaxTemp >= 360f ? angleMaxTemp - 360f : angleMaxTemp;
            internal float angleMaxTemp => angle + _angleStep / 2f;
            internal ItemBundle itemBundle;
            internal RadialItemController radialItemController;
        }
    
        void OnEnable()
        {
            _radialItems = new List<RadialItem>();
            _selectedRadialItem = null;
            ControllerInputs.controllerButtons[Constants.ControllerButtons.Y].Pressed += OnButtonYPressed;
            ControllerInputs.controllerButtons[Constants.ControllerButtons.RB].Pressed += OnButtonRBPressed;
            ControllerInputs.leftJoystick.AngleChanged += OnAngleChanged;
            UpdateContent();
        }
        void OnDisable()
        {
            _radialItems = null;
            _selectedRadialItem = null;
            ControllerInputs.controllerButtons[Constants.ControllerButtons.Y].Pressed -= OnButtonYPressed;
            ControllerInputs.controllerButtons[Constants.ControllerButtons.RB].Pressed -= OnButtonRBPressed;
            ControllerInputs.leftJoystick.AngleChanged -= OnAngleChanged;
        }

        private void OnAngleChanged(float angle)
        {
            if (angle < 0f)
            {
                _selectedRadialItem = null;
            }
            else
            {
                foreach (var radialItem in _radialItems)
                {
                    if (radialItem.angleMin > radialItem.angleMax)
                    {
                        if (angle >= radialItem.angleMin || angle < radialItem.angleMax)
                        {
                            _selectedRadialItem = radialItem;
                            break;
                        }
                    } else if (angle >= radialItem.angleMin && angle < radialItem.angleMax)
                    {
                        _selectedRadialItem = radialItem;
                        break;
                    }
                }
            }
            foreach (var radialItem in _radialItems)
            {
                radialItem.radialItemController.isSelected = radialItem == _selectedRadialItem;
            }
        }

        private void UpdateContent()
        {
            if (!GlobalInventoryManager.TryGetInventory(-1, out var inventory)) return;
            _radialItems.Clear();
            var itemBundles = inventory.GetItemBundles(false).Where(x => x.Item.IsDrinkable).ToList();
            var transforms = transform.Cast<Transform>().ToList();
            foreach (Transform child in transforms)
            {
                Destroy(child.gameObject);
            }
            var angle = 0f;
            _angleStep = 360f / itemBundles.Count;
            foreach (var itemBundle in itemBundles)
            {
                angle += _angleStep;
                var radialItemGameObject = Instantiate(radialItemPrefab, transform);
                var radialItemController = radialItemGameObject.GetComponent<RadialItemController>();
                radialItemController.UpdateContent(itemBundle, angle, vectorMultiplier);
                _radialItems.Add(new RadialItem {angle = angle, itemBundle = itemBundle, radialItemController = radialItemController});
            }
        }

        private void OnButtonYPressed()
        {
            if (_selectedRadialItem == null) return;
            ItemAssignmentController.Attach(Constants.ControllerButtons.Y, _selectedRadialItem.itemBundle.Item.Id);
        }

        private void OnButtonRBPressed()
        {
            if (_selectedRadialItem == null) return;
            ItemAssignmentController.Attach(Constants.ControllerButtons.RB, _selectedRadialItem.itemBundle.Item.Id);
        }

    }
}
