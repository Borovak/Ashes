using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;

public static class DropController
{
    private static Dictionary<int, Item> _items = new Dictionary<int, Item>();
    private static Dictionary<int, Dictionary<int, float>> _enemies = new Dictionary<int, Dictionary<int, float>>();
    private static bool _initDone;

    public static void Init()
    {
        if (_initDone) return;
        _initDone = true;
        if (DataHandling.GetTable("items", out var dtItems))
        {
            foreach (DataRow dr in dtItems.Rows)
            {
                var id = Convert.ToInt32(dr["id"]);
                var name = dr["name"].ToString();
                var description = dr["description"].ToString();
                var value = int.TryParse(dr["value"].ToString(), out var valueTemp) ? valueTemp : 0;
                var isCraftable = bool.TryParse(dr["iscraftable"].ToString(), out var isCraftableTemp) ? isCraftableTemp : false;
                var artFilePath = $"Items/{dr["path"].ToString()}";
                _items.Add(id, new Item { id = id, name = name, description = description, value = value, isCraftable = isCraftable, artFilePath = artFilePath });
            }
        }
        if (DataHandling.GetTable("enemies", out var dtEnemies))
        {
            foreach (DataRow dr in dtEnemies.Rows)
            {
                var id = Convert.ToInt32(dr["id"]);
                var name = dr["name"].ToString();
                var description = dr["description"].ToString();
                var artFilePath = $"Ingredients/{dr["path"].ToString()}";
                _enemies.Add(id, new Dictionary<int, float>());
            }
        }
        if (DataHandling.GetTable("drops", out var dtDrops))
        {
            foreach (DataRow dr in dtDrops.Rows)
            {
                var monsterId = Convert.ToInt32(dr["enemies_id"]);
                var itemId = Convert.ToInt32(dr["items_id"]);
                var dropRate = Convert.ToSingle(dr["droprate"]);
                _enemies[monsterId].Add(itemId, dropRate);
            }
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
                currentDrop.name = drop.name;
                currentDrops.Add(currentDrop);
            }
        }
        return currentDrops.Any();
    } 

    public static Item GetDropInfo(int id)
    {
        return _items[id];
    }

    public static List<Item> GetCraftables()
    {
        return _items.Where(x => x.Value.isCraftable).OrderBy(x => x.Value.id).Select(x => x.Value).ToList();
    }
}