using System;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;

namespace Static
{
    public static class DataHandling
    {
        public static void Init()
        {
            //var outputPath = System.IO.Path.Combine(Application.persistentDataPath, "ashes.db");
            // if (System.IO.File.Exists(outputPath))
            // {
            //     // System.IO.File.Delete(outputPath);
            //     // CopyGameDb(outputPath);
            // } else {
            //     CopyGameDb(outputPath);
            // }
        }

        private static void CopyGameDb(string outputPath)
        {
            var db = Resources.Load<TextAsset>("ashesDb");
            byte[] data = db.bytes;
            System.IO.File.WriteAllBytes(outputPath, data);
        }

        public static bool ConnectToDb(out SqliteConnection connection)
        {
            var path = System.IO.Path.Combine(Application.streamingAssetsPath, "ashes.db");
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

        public static bool GetTable(string tableName, out DataTable dt)
        {
            var commandString = $"SELECT * FROM {tableName}";
            return GetInfo(commandString, out dt);
        }

        public static bool GetInfo(string commandString, out DataTable dt)
        {
            if (!ConnectToDb(out var connection))
            {
                dt = null;
                return false;
            }
            dt = new DataTable();
            using (var da = new SqliteDataAdapter(commandString, connection))
            {
                da.Fill(dt);
            }
            connection.Close();
            //Debug.Log($"Connection state: {connection.State.ToString()}");
            connection = null;
            return true;
        }
    }
}
