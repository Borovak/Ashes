using System.Collections.Generic;
using UnityEngine;

public class EnemyInformation
{
    public static Dictionary<int, GameObject> EnemyPrefabs = new Dictionary<int, GameObject>();

    public string Guid;
    public string ChamberGuid;
    public int Id;
    public int X;
    public int Y;

    public void Instantiate(Transform parent, Vector2 chamberOffset, Vector2 chamberSize, float chamberScale)
    {
        if (!EnemyPrefabs.TryGetValue(Id, out var enemyPrefab))
        {
            if (!LoadResource(Id, out enemyPrefab)) return;
        }
        var position = new Vector3(chamberOffset.x + (X * chamberScale), chamberOffset.y + 50f - Y * chamberScale - 0.5f, 0f);
        GameObject.Instantiate<GameObject>(enemyPrefab, position, Quaternion.identity, parent);
    }

    private bool LoadResource(int id, out GameObject enemyPrefab)
    {
        if (!GetResourceName(id, out var resourceName))
        {
            enemyPrefab = null;
            return false;
        }
        enemyPrefab = Resources.Load<GameObject>($"Enemies/{resourceName}");
        return true;
    }

    private bool GetResourceName(int id, out string resourceName)
    {
        if (!DataHandling.GetInfo($"SELECT prefabname FROM enemies WHERE id = {id}", out var dt) || dt.Rows.Count == 0)
        {
            resourceName = string.Empty;
            return false;
        }
        resourceName = dt.Rows[0][0].ToString();
        return true;
    }

}