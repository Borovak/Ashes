using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Classes;
using ItemActions;
using UnityEngine;

namespace Static
{
    public static class DropController
    {
        private static Dictionary<int, DB.Item> _items = new Dictionary<int, DB.Item>();
        private static Dictionary<int, Dictionary<int, float>> _enemies = new Dictionary<int, Dictionary<int, float>>();
        private static bool _initDone;

        public static void Init()
        {
            if (_initDone) return;
            _initDone = true;
            if (!DataHandling.TryConnectToDb(out var connection)) return;
            foreach (var item in connection.Table<DB.Item>().AsEnumerable())
            {
                _items.Add(item.Id, item);
                var t = Type.GetType($"ItemActions.Item{item.Id.ToString("0000")}");
                if (t == null) continue;
                item.itemAction = (IItemAction)Activator.CreateInstance(t);
            }
            foreach (var enemy in connection.Table<DB.Enemy>().AsEnumerable())
            {
                _enemies.Add(enemy.Id, new Dictionary<int, float>());
            }
            foreach (var drop in connection.Table<DB.Drop>().AsEnumerable())
            {
                _enemies[drop.EnemyId].Add(drop.ItemId, drop.DropRate);
            }
            //Debug.Log($"Drops loaded: {_items.Count} items for {_enemies.Count} enemies");
        }

        public static bool GetDrops(Vector3 position, int enemyId, out List<GameObject> currentDrops)
        {
            currentDrops = new List<GameObject>();
            if (!_enemies.ContainsKey(enemyId)) return false;
            foreach (var dropRate in _enemies[enemyId])
            {
                var drop = _items[dropRate.Key];
                var diceRoll = UnityEngine.Random.Range(0f, 1f);
                //Debug.Log($"Item drop dice roll ({drop.name}): {diceRoll} vs {dropRate.Value}");
                if (diceRoll <= dropRate.Value)
                {
                    var currentDrop = drop.Instantiate(position);
                    currentDrop.name = drop.Name;
                    currentDrops.Add(currentDrop);
                }
            }
            return currentDrops.Any();
        } 

        public static DB.Item GetDropInfo(int id)
        {
            return _items[id];
        }

        public static List<ItemBundle> GetCraftables()
        {
            return _items.Where(x => x.Value.IsCraftable)
                .OrderBy(x => x.Value.Id)
                .Select(x => new ItemBundle(x.Value, 1))
                .ToList();
        }
    }
}