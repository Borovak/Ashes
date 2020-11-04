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
        var position = new Vector3(chamberOffset.x + (X * chamberScale), chamberOffset.y + 50f - Y * chamberScale, 0f);
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
        switch (Id)
        {
            case 1:
                resourceName = "EnemyLarva";
                break;
            case 2:
                resourceName = "EnemyBird";
                break;
            default:
                resourceName = string.Empty;
                return false;
        }
        return true;
    }

}