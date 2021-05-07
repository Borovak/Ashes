using System.Collections.Generic;
using System.Linq;
using Classes;
using Interfaces;
using UnityEngine;

namespace UI
{
    public class RadialSelectionController : MonoBehaviour
    {

        public GameObject radialItemPrefab;
        public Transform radialItemsFolder;
    
        private float vectorMultiplier = 800f;
        private List<RadialItem> _radialItems;
        private static RadialItem _selectedRadialItem;
        private static float _angleStep = 90f;
        private static Constants.IconElementTypes iconElementType = Constants.IconElementTypes.Item;

        private class RadialItem
        {
            internal float angle;
            internal float angleMin => angleMinTemp < 0f ? angleMinTemp + 360f : angleMinTemp;
            internal float angleMinTemp => angle - _angleStep / 2f;
            internal float angleMax => angleMaxTemp >= 360f ? angleMaxTemp - 360f : angleMaxTemp;
            internal float angleMaxTemp => angle + _angleStep / 2f;
            internal IIconElement iconElement;
            internal RadialItemController radialItemController;
        }
    
        void OnEnable()
        {
            _radialItems = new List<RadialItem>();
            _selectedRadialItem = null;
            ControllerInputs.controllerButtons[Constants.ControllerButtons.X].Pressed += OnButtonXPressed;
            ControllerInputs.controllerButtons[Constants.ControllerButtons.B].Pressed += OnButtonBPressed;
            ControllerInputs.controllerButtons[Constants.ControllerButtons.Y].Pressed += OnButtonYPressed;
            ControllerInputs.controllerButtons[Constants.ControllerButtons.RB].Pressed += OnButtonRBPressed;
            ControllerInputs.controllerButtons[Constants.ControllerButtons.DRight].Pressed += OnModeChange;
            ControllerInputs.leftJoystick.AngleChanged += OnAngleChanged;
            UpdateContent();
        }
        void OnDisable()
        {
            _radialItems = null;
            _selectedRadialItem = null;
            ControllerInputs.controllerButtons[Constants.ControllerButtons.X].Pressed -= OnButtonXPressed;
            ControllerInputs.controllerButtons[Constants.ControllerButtons.B].Pressed -= OnButtonBPressed;
            ControllerInputs.controllerButtons[Constants.ControllerButtons.Y].Pressed -= OnButtonYPressed;
            ControllerInputs.controllerButtons[Constants.ControllerButtons.RB].Pressed -= OnButtonRBPressed;
            ControllerInputs.controllerButtons[Constants.ControllerButtons.DRight].Pressed -= OnModeChange;
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
            _radialItems.Clear();
            if (!GlobalInventoryManager.TryGetInventory(-1, out var inventory)) return;
            //Cleaning content
            var transforms = radialItemsFolder.transform.Cast<Transform>().ToList();
            foreach (Transform child in transforms)
            {
                Destroy(child.gameObject);
            }
            //Retrieving icon elements
            List<IIconElement> iconElements = null;
            switch (iconElementType)
            {
                case Constants.IconElementTypes.Item:
                    iconElements = inventory.GetItemBundles(false).Where(x => x.Item.IsDrinkable).OrderBy(x => x.Item.Id).Cast<IIconElement>().ToList();
                    break;
                case Constants.IconElementTypes.Action:
                    iconElements = ActionAssignmentController.actionElements.Cast<IIconElement>().ToList();
                    break;
                default:
                    return;
            }
            //Creating game objects
            var angle = 0f;
            _angleStep = 360f / iconElements.Count;
            foreach (var iconElement in iconElements)
            {
                var radialItemGameObject = Instantiate(radialItemPrefab, radialItemsFolder.transform);
                var radialItemController = radialItemGameObject.GetComponent<RadialItemController>();
                radialItemController.UpdateContent(iconElement, angle, vectorMultiplier);
                _radialItems.Add(new RadialItem {angle = angle, iconElement = iconElement, radialItemController = radialItemController});
                angle += _angleStep;
            }
        }

        private void OnButtonYPressed()
        {
            if (iconElementType != Constants.IconElementTypes.Item || _selectedRadialItem == null) return;
            ItemAssignmentController.Attach(Constants.ControllerButtons.Y, _selectedRadialItem.iconElement.id);
        }

        private void OnButtonRBPressed()
        {
            if (iconElementType != Constants.IconElementTypes.Item || _selectedRadialItem == null) return;
            ItemAssignmentController.Attach(Constants.ControllerButtons.RB, _selectedRadialItem.iconElement.id);
        }

        private void OnButtonXPressed()
        {
            if (iconElementType != Constants.IconElementTypes.Action || _selectedRadialItem == null) return;
            ActionAssignmentController.Attach(Constants.ControllerButtons.X, _selectedRadialItem.iconElement.id);
        }

        private void OnButtonBPressed()
        {
            if (iconElementType != Constants.IconElementTypes.Action || _selectedRadialItem == null) return;
            ActionAssignmentController.Attach(Constants.ControllerButtons.B, _selectedRadialItem.iconElement.id);
        }

        private void OnModeChange()
        {
            iconElementType = iconElementType == Constants.IconElementTypes.Item ? Constants.IconElementTypes.Action : Constants.IconElementTypes.Item;
            UpdateContent();
        }
        
    }
}
