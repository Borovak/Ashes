using System;
using UnityEngine;
using SQLite4Unity3d;

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

        private static bool TryGetConnectionString(out string connectionString)
        {
            var path = System.IO.Path.Combine(Application.streamingAssetsPath, "ashes.db");
            if (!System.IO.File.Exists(path))
            {
                Debug.Log($"Cannot find database file {path}");
                connectionString = "";
                return false;
            }
            connectionString = $"URI=file:{path}";
            return true;
        }
        private static bool TryGetPath(out string path)
        {
            path = System.IO.Path.Combine(Application.streamingAssetsPath, "ashes.db");
            if (!System.IO.File.Exists(path))
            {
                Debug.Log($"Cannot find database file {path}");
                return false;
            }
            return true;
        }

        public static bool TryConnectToDb(out SQLiteConnection connection)
        {
            // if (!TryGetConnectionString(out var connectionString))
            // {
            //     connection = null;
            //     return false;
            // }
            if (!TryGetPath(out var path))
            {
                connection = null;
                return false;
            }
            //connection = new SQLiteConnection(connectionString);
            connection = new SQLiteConnection(path);
            try
            {
                //SQLite3.Open(connectionString); 
            }
            catch (Exception ex)
            {
                Debug.Log($"Error opening connection to database: {ex.ToString()}");
                return false;
            }
            return true;
        }

        // public static bool GetTable(string tableName, out DataTable dt)
        // {
        //     var commandString = $"SELECT * FROM {tableName}";
        //     return GetInfo(commandString, out dt);
        // }

        // public static bool GetInfo(string commandString, out DataTable dt)
        // {
        //     if (!ConnectToDb(out var connection))
        //     {
        //         dt = null;
        //         return false;
        //     }
        //     dt = new DataTable();
        //     using (var da = new SqLiteDataAdapter(commandString, connection))
        //     {
        //         da.Fill(dt);
        //     }
        //     connection.Close();
        //     //Debug.Log($"Connection state: {connection.State.ToString()}");
        //     connection = null;
        //     return true;
        // }
    }
}
