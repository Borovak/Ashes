using System.Data;
using UnityEngine;

public static class GlobalFunctions
{
    public static DataTable ParseCsv(string path)
    {
        var textAsset = Resources.Load<TextAsset>(path);
        var fileData = textAsset.text;
        var lines = fileData.Split("\n"[0]);
        var dt = new DataTable();
        foreach (var line in lines){
            if (string.IsNullOrEmpty(line)) continue;
            var lineData = (line.Trim()).Split(","[0]);
            if (dt.Columns.Count == 0){
                foreach (var cell in lineData){
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

    public static GameController GetGameController(){
        var gameObject = GameObject.FindGameObjectWithTag("GameController");
        return gameObject.GetComponent<GameController>();
    }
}