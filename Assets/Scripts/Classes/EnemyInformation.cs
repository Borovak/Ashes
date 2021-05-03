using System.Collections.Generic;
using System.Linq;
using Static;
using UnityEngine;

namespace Classes
{
    public class EnemyInformation
    {
        public static Dictionary<int, GameObject> EnemyPrefabs = new Dictionary<int, GameObject>();

        public string Guid;
        public string ChamberGuid;
        public int Id;
        public int X;
        public int Y;
        
        private static int _generationIndex;

        public void Instantiate(Transform parent, Vector2 chamberOffset, Vector2 chamberSize, float chamberScale)
        {
            if (!EnemyPrefabs.TryGetValue(Id, out var enemyPrefab))
            {
                if (!LoadResource(Id, out enemyPrefab)) return;
            }
            var position = new Vector3(chamberOffset.x + (X * chamberScale), chamberOffset.y + 50f - Y * chamberScale - 0.5f, 0f);
            var enemyGameObject = GameObject.Instantiate<GameObject>(enemyPrefab, position, Quaternion.identity, parent);
            enemyGameObject.name = $"{enemyPrefab.name} {_generationIndex++}";
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
            if (!DataHandling.TryConnectToDb(out var connection))
            {
                resourceName = string.Empty;
                return false;
            }
            resourceName = connection.Table<DB.Enemy>().AsEnumerable().FirstOrDefault(x => x.Id == id)?.PrefabName ?? string.Empty;
            return resourceName != string.Empty;
        }

    }
}