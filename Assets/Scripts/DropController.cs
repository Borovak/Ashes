using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class DropController
{
    private static Dictionary<int, Item> _drops = new Dictionary<int, Item>();
    private static Dictionary<int, Dictionary<int, float>> _enemies = new Dictionary<int, Dictionary<int, float>>();
    private static bool _initDone;

    public static void Init()
    {
        if (_initDone) return;
        _initDone = true;
        var dtDrops = GlobalFunctions.ParseCsv("drops");
        for (int i = 3; i < dtDrops.Columns.Count; i++)
        {
            var id = int.Parse(dtDrops.Rows[0][i].ToString());
            var name = dtDrops.Rows[1][i].ToString();
            var artFilePath = dtDrops.Rows[2][i].ToString();
            _drops.Add(id, new Item { id = id, name = name, artFilePath = artFilePath });
            //Debug.Log($"Drop loaded: {id}, {name}, {artFilePath}");
        }
        for (int i = 4; i < dtDrops.Rows.Count; i++)
        {
            var id = int.Parse(dtDrops.Rows[i][0].ToString());
            var name = dtDrops.Rows[i][1].ToString();
            var rates = new Dictionary<int, float>();
            _enemies.Add(id, rates);
            for (int k = 3; k < dtDrops.Columns.Count; k++)
            {
                var prob = dtDrops.Rows[i][k].ToString();
                if (string.IsNullOrEmpty(prob)) continue;
                var dropId = int.Parse(dtDrops.Rows[0][k].ToString());
                rates.Add(dropId, float.Parse(prob));
            }
            //Debug.Log($"Enemy drop rates loaded: {id}, {name}, {rates.Count} drops");
        }
        Debug.Log($"Drops loaded: {_drops.Count} drops for {_enemies.Count} enemies");
    }

    public static bool GetDrops(Vector3 position, int enemyId, out List<GameObject> currentDrops)
    {
        currentDrops = new List<GameObject>();
        if (!_enemies.ContainsKey(enemyId)) return false;
        foreach (var dropRate in _enemies[enemyId])
        {
            var drop = _drops[dropRate.Key];
            var diceRoll = UnityEngine.Random.Range(0f, 1f);
            Debug.Log($"Item drop dice roll ({drop.name}): {diceRoll} vs {dropRate.Value}");
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
        return _drops[id];
    }
}