using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Classes;
using UnityEditor;
using UnityEngine;

namespace Static
{
    public static class GlobalFunctions
    {

        public static DataTable ParseCsv(string path)
        {
            var textAsset = Resources.Load<TextAsset>(path);
            var fileData = textAsset.text;
            var lines = fileData.Split("\n"[0]);
            var dt = new DataTable();
            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line)) continue;
                var lineData = (line.Trim()).Split("|"[0]);
                if (dt.Columns.Count == 0)
                {
                    foreach (var cell in lineData)
                    {
                        dt.Columns.Add();
                    }
                }
                var dr = dt.Rows.Add();
                for (int i = 0; i < lineData.Length; i++)
                {
                    dr[i] = lineData[i];
                }
            }
            return dt;
        }

        public static bool TryGetPlayer(out GameObject playerGameObject)
        {
            playerGameObject = GameObject.FindGameObjectWithTag("Player");
            return playerGameObject != null;
        }

        // public static bool TryGetPlayerPlatformerController(out PlayerPlatformerController playerPlatformerController)
        // {
        //     if (!TryGetPlayer(out var playerGameObject))
        //     {
        //         playerPlatformerController = null;
        //         return false;
        //     }
        //     playerPlatformerController = playerGameObject.GetComponent<PlayerPlatformerController>();
        //     return true;
        // }

        // public static bool GetPlayerLifeController(out PlayerLifeController playerLifeController)
        // {
        //     if (!TryGetPlayer(out var playerGameObject))
        //     {
        //         playerLifeController = null;
        //         return false;
        //     }
        //     playerLifeController = playerGameObject.GetComponent<PlayerLifeController>();
        //     return true;
        // }

        // public static bool GetPlayerManaController(out ManaController manaController)
        // {
        //     if (!TryGetPlayer(out var playerGameObject))
        //     {
        //         manaController = null;
        //         return false;
        //     }
        //     manaController = playerGameObject.GetComponent<ManaController>();
        //     return true;
        // }

        // public static bool GetPlayerInventory(out PlayerInventory playerInventory)
        // {
        //     if (!TryGetPlayer(out var playerGameObject))
        //     {
        //         playerInventory = null;
        //         return false;
        //     }
        //     playerInventory = playerGameObject.GetComponent<PlayerInventory>();
        //     return true;
        // }

        public static bool TryGetPlayerComponent<T>(out T component)
        {
            if (!TryGetPlayer(out var playerGameObject))
            {
                component = default(T);
                return false;
            }
            component = playerGameObject.GetComponent<T>();
            return true;
        }

        public static List<GameObject> FindChildrenWithTag(GameObject parent, string tag, bool includeParent)
        {
            var gameObjects = new List<GameObject>();
            if (includeParent && parent.tag == tag)
            {
                gameObjects.Add(parent);
            }
            foreach (Transform tr in parent.transform)
            {
                var gameObjectsFound = FindChildrenWithTag(tr.gameObject, tag, true);
                if (gameObjectsFound.Any())
                {
                    gameObjects.AddRange(gameObjectsFound);
                }
            }
            return gameObjects;
        }

        public static float Bound(float value, float min, float max)
        {
            if (value > max) return max;
            if (value < min) return min;
            return value;
        }

        public static bool TryGetNpcSprite(Constants.Npc npc, out Sprite sprite)
        {
            string resourceName;
            switch (npc)
            {
                case Constants.Npc.Shopkeeper:
                    resourceName = "Shopkeeper";
                    break;
                case Constants.Npc.HoboTom:
                    resourceName = "HoboTom";
                    break;
                default:
                    sprite = null;
                    return false;
            }
            sprite = Resources.Load<Sprite>($"Npc/{resourceName}");
            return sprite != null;
        }

        public static float GetAngleFromPoints(Vector3 reference, Vector3 target)
        {
            return GetAngleFromPoints(new Vector2(reference.x, reference.y), new Vector2(target.x, target.y));
        }

        public static float GetAngleFromPoints(Vector2 reference, Vector2 target)
        {
            double dx = target.x - reference.x;
            // Minus to correct for coord re-mapping
            double dy = target.y - reference.y;
            double inRads = Math.Atan2(dy, dx);
            // We need to map to coord system when 0 degree is at 3 O'clock, 270 at 12 O'clock
            inRads = inRads < 0 ? Math.Abs(inRads) : 2 * Math.PI - inRads;
            return 360f - Convert.ToSingle((180 / Math.PI) * inRads);
        }

        public static void DeleteChildrenInTransform(Transform transform)
        {
            var security = 0;
            while (transform.childCount > 0 && security < 1000)
            {
                GameObject.Destroy(transform.GetChild(0).gameObject);
                security++;
            }
        }

        public static void DeleteChildrenInTransform(Transform transform, GameObject exception)
        {
            if (exception == null)
            {
                DeleteChildrenInTransform(transform);
                return;
            }
            DeleteChildrenInTransform(transform, new List<GameObject> { exception });
        }

        public static void DeleteChildrenInTransform(Transform transform, List<GameObject> exceptions)
        {
            if (exceptions.Count == 0)
            {
                DeleteChildrenInTransform(transform);
                return;
            }
            var objectsToDelete = new List<GameObject>();
            for (int i = 0; i < transform.childCount; i++)
            {
                var obj = transform.GetChild(i).gameObject;
                if (!exceptions.Contains(obj))
                {
                    objectsToDelete.Add(obj);
                }
            }
            while (objectsToDelete.Count > 0)
            {
                GameObject.Destroy(objectsToDelete[0]);
                objectsToDelete.RemoveAt(0);
            }
        }

#if UNITY_EDITOR
        public static List<Sprite> GetSpritesAtPath(string theme, Constants.AssetTypes assetType)
        {
            var type = new Dictionary<Constants.AssetTypes, string>{
                {Constants.AssetTypes.Terrain, "terrain"},
                {Constants.AssetTypes.Grass, "grass"},
                {Constants.AssetTypes.Overhang, "overhang"},
            }[assetType];
            var path = $"Assets/Sprites/{type}/{type}_{theme}.png";
            var data = AssetDatabase.LoadAllAssetsAtPath(path);
            var sprites = new List<Sprite>();
            foreach (var item in data)
            {
                var sprite = item as Sprite;
                if (sprite == null) continue;
                sprites.Add(sprite);
            }
            return sprites;
        }
#endif

    }
}