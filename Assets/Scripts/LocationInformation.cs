using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Classes;
using UnityEngine;

public static class LocationInformation
{
    public static Dictionary<string, Zone> Zones;
    public static Dictionary<string, Chamber> Chambers;
    public static Dictionary<string, SavePoint> SavePoints;

    private static string _summary = "";
    private static bool _initDone;

    public class Zone
    {
        public string Guid;
        public string Name;
    }
    public class Chamber
    {
        public string Guid;
        public string Name;
        public string ZoneGuid;
        public string ZoneName => Zone?.Name ?? "";
        public Zone Zone => Zones.TryGetValue(ZoneGuid, out var zone) ? zone : null;
        public List<EnemyInformation> Enemies = new List<EnemyInformation>();
    }
    public class SavePoint
    {
        public string Guid;
        public string ChamberGuid;
        public Chamber Chamber => Chambers[ChamberGuid];
    }

    public static bool Init(out string summary)
    {
        if (_initDone)
        {
            summary = $"(Buffered) {_summary}";
            return true;
        }
        _initDone = true;
        var zoneCount = 0;
        var chamberCount = 0;
        var savePointCount = 0;
        var enemyCount = 0;
        Zones = new Dictionary<string, Zone>();
        Chambers = new Dictionary<string, Chamber>();
        SavePoints = new Dictionary<string, SavePoint>();
        TextAsset textAsset;
        try
        {
            textAsset = Resources.Load<TextAsset>("layout");
        }
        catch (Exception)
        {
            textAsset = null;
            summary = "";
            return false;
        }
        if (textAsset == null)
        {
            summary = "";
            throw new Exception($"Cannot load location information file 'layout'");
        }
        XElement xeRoot;
        try
        {
            xeRoot = XElement.Parse(textAsset.text);
        }
        catch (Exception)
        {
            summary = "";
            throw new Exception($"Cannot parse location information file 'layout': {textAsset.text}");
        }
        var xeZones = xeRoot.Element("Zones");
        foreach (var xeZone in xeZones.Elements("Zone"))
        {
            var guid = xeZone.Attribute("guid")?.Value ?? string.Empty;
            var name = xeZone.Attribute("name")?.Value ?? string.Empty;
            if (guid == string.Empty || name == string.Empty) continue;
            zoneCount++;
            Zones.Add(guid, new Zone { Guid = guid, Name = name });
        }
        var xeRooms = xeRoot.Element("Rooms");
        foreach (var xeChamber in xeRooms.Elements("Room"))
        {
            var chamberGuid = xeChamber.Attribute("guid")?.Value ?? string.Empty;
            var chamberName = xeChamber.Attribute("name")?.Value ?? string.Empty;
            var zoneGuid = xeChamber.Attribute("zoneGuid")?.Value ?? string.Empty;
            if (chamberGuid == string.Empty || chamberName == string.Empty || zoneGuid == string.Empty) continue;
            var chamber = new Chamber { Guid = chamberGuid, Name = chamberName, ZoneGuid = zoneGuid };
            chamberCount++;
            Chambers.Add(chamberGuid, chamber);
            foreach (var xeSavePoint in xeChamber.Elements("SavePoint"))
            {
                var savePointGuid = xeSavePoint.Attribute("guid")?.Value ?? string.Empty;
                if (savePointGuid == string.Empty) continue;
                savePointCount++;
                SavePoints.Add(savePointGuid, new SavePoint { Guid = savePointGuid, ChamberGuid = chamberGuid });
            }
            foreach (var xeEnemy in xeChamber.Elements("Enemy"))
            {
                var enemyGuid = xeEnemy.Attribute("guid")?.Value ?? string.Empty;
                var enemyId = int.TryParse(xeEnemy.Attribute("id")?.Value ?? string.Empty, out var enemyIdValue) ? enemyIdValue : int.MinValue;
                var enemyX = int.TryParse(xeEnemy.Attribute("x")?.Value ?? string.Empty, out var enemyXValue) ? enemyXValue : int.MinValue;
                var enemyY = int.TryParse(xeEnemy.Attribute("y")?.Value ?? string.Empty, out var enemyYValue) ? enemyYValue : int.MinValue;
                if (enemyGuid == string.Empty || enemyId == int.MinValue || enemyX == int.MinValue || enemyY == int.MinValue) continue;
                enemyCount++;
                chamber.Enemies.Add(new EnemyInformation { Guid = enemyGuid, Id = enemyId, ChamberGuid = chamberGuid, X = enemyX, Y = enemyY });
            }
        }
        _summary = summary = $"{zoneCount} zones, {chamberCount} chambers, {savePointCount} save points, {enemyCount} enemies";
        return true;
    }
}