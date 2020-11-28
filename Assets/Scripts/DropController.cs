using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Mono.Data.Sqlite;
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
        if (GetTable("items", out var dtItems))
        {
            foreach (DataRow dr in dtItems.Rows)
            {
                var id = Convert.ToInt32(dr["id"]);
                var name = dr["name"].ToString();
                var description = dr["description"].ToString();
                var artFilePath = $"Ingredients/{dr["path"].ToString()}";
                _items.Add(id, new Item { id = id, name = name, description = description, artFilePath = artFilePath });
            }
        }
        if (GetTable("enemies", out var dtEnemies))
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
        if (GetTable("drops", out var dtDrops))
        {
            foreach (DataRow dr in dtDrops.Rows)
            {
                var monsterId = Convert.ToInt32(dr["enemies_id"]);
                var itemId = Convert.ToInt32(dr["items_id"]);
                var dropRate = Convert.ToSingle(dr["droprate"]);
                _enemies[monsterId].Add(itemId, dropRate);
            }
        }
        Debug.Log($"Drops loaded: {_items.Count} items for {_enemies.Count} enemies");
    }

    public static bool GetDrops(Vector3 position, int enemyId, out List<GameObject> currentDrops)
    {
        currentDrops = new List<GameObject>();
        if (!_enemies.ContainsKey(enemyId)) return false;
        foreach (var dropRate in _enemies[enemyId])
        {
            var drop = _items[dropRate.Key];
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
        return _items[id];
    }

    private static bool ConnectToDb(out SqliteConnection connection)
    {
        var path = $"{Application.dataPath}/Resources/ashes.db";
        if (!System.IO.File.Exists(path))
        {
            Debug.Log($"Cannot find database file {path}");
            connection = null;
            return false;
        }
        string connectionString = $"URI=file:{path}";
        connection = new SqliteConnection(connectionString);
        try
        {
            connection.Open();
        }
        catch (Exception ex)
        {
            Debug.Log($"Error opening connection to database file {path}: {ex.ToString()}");
            return false;
        }
        return true;
    }

    private static bool GetTable(string tableName, out DataTable dt)
    {
        if (!ConnectToDb(out var connection))
        {
            dt = null;
            return false;
        }
        dt = new DataTable();
        var commandString = $"SELECT * FROM {tableName}";
        using (var da = new SqliteDataAdapter(commandString, connection))
        {
            da.Fill(dt);
        }
        connection.Close();
        return true;
    }
}