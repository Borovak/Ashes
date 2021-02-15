using System;
using System.Collections.Generic;
using Classes;
using UnityEngine;

namespace Player
{
    public class PlayerInventory : MonoBehaviour
    {
        public const int playerId = -1;
        public GameObject ItemGainedPrefab;
        public float timeBetweenItemGainedPanel = 0.5f;

        private List<ItemBundle> _itemGainedQueue;
        private float _timeSinceLastItemGainedPanel = 0f;

        protected void Start()
        {
            _itemGainedQueue = new List<ItemBundle>();
        }

        protected void Add(int id, int quantity)
        {
            if (GameController.gameState == GameController.GameStates.Running)
            {
                _itemGainedQueue.Add(new ItemBundle(id, quantity));
            }
        }

        void Update()
        {
            if (_timeSinceLastItemGainedPanel <= 0 && GameController.gameState == GameController.GameStates.Running)
            {
                if (_itemGainedQueue.Count > 0)
                {
                    _timeSinceLastItemGainedPanel = timeBetweenItemGainedPanel;
                    InstantiateItemGainedPanel(_itemGainedQueue[0]);
                    _itemGainedQueue.RemoveAt(0);
                }
            }
            else
            {
                _timeSinceLastItemGainedPanel -= Time.deltaTime;
            }
        }

        private void InstantiateItemGainedPanel(ItemBundle itemBundle)
        {
            if (GameController.gameState != GameController.GameStates.Running) return;
            var gameUIGameObject = GameObject.FindGameObjectWithTag("GameUI");
            var itemGained = GameObject.Instantiate(ItemGainedPrefab, gameUIGameObject.transform);
            var itemGainedController = itemGained.GetComponent<ItemGainedController>();
            itemGainedController.itemBundle = itemBundle;
        }
    }
}