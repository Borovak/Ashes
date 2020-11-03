using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public static class LocationInformation
{
    public static Dictionary<string, Zone> Zones;
    public static Dictionary<string, Chamber> Chambers;
    public static Dictionary<string, SavePoint> SavePoints;

    public class Zone {
        public string Guid;
        public string Name;
    }
    public class Chamber {
        public string Guid;
        public string Name;    
        public string ZoneGuid;
        public string ZoneName => Zones.TryGetValue(ZoneGuid, out var zone) ? zone.Name : string.Empty;
        public List<EnemyInformation> Enemies = new List<EnemyInformation>();
    }
    public class SavePoint {
        public string Guid;
        public string Name;    
        public string ChamberGuid;
        public Chamber Chamber => Chambers[ChamberGuid];
    }

    public static void Init(){
        
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
        }
        if (textAsset == null)
        {
            throw new Exception($"Cannot load location information file 'layout'");
        }
        XElement xeRoot;
        try
        {
            xeRoot = XElement.Parse(textAsset.text);
        }
        catch (Exception)
        {
            throw new Exception($"Cannot parse location information file 'layout': {textAsset.text}");
        }
        var xeZones = xeRoot.Element("Zones");
        foreach (var xeZone in xeZones.Elements("Zone")){
            var guid = xeZone.Attribute("guid")?.Value ?? string.Empty;
            var name = xeZone.Attribute("name")?.Value ?? string.Empty;
            if (guid == string.Empty || name == string.Empty) continue;
            Zones.Add(guid, new Zone {Guid = guid, Name = name});
        }
        var xeRooms = xeRoot.Element("Rooms");
        foreach (var xeChamber in xeRooms.Elements("Room")){
            var chamberGuid = xeChamber.Attribute("guid")?.Value ?? string.Empty;
            var chamberName = xeChamber.Attribute("name")?.Value ?? string.Empty;
            var zoneGuid = xeChamber.Attribute("zoneGuid")?.Value ?? string.Empty;
            if (chamberGuid == string.Empty || chamberName == string.Empty || zoneGuid == string.Empty) continue;
            var chamber = new Chamber {Guid = chamberGuid, Name = chamberName, ZoneGuid = zoneGuid};
            Chambers.Add(chamberGuid, chamber);
            foreach (var xeSavePoint in xeChamber.Elements("SavePoint")){
                var savePointGuid = xeSavePoint.Attribute("guid")?.Value ?? string.Empty;
                var savePointName = xeSavePoint.Attribute("name")?.Value ?? string.Empty;
                if (savePointGuid == string.Empty || savePointName == string.Empty) continue;
                SavePoints.Add(savePointGuid, new SavePoint {Guid = savePointGuid, Name = savePointName, ChamberGuid = chamberGuid});
            }
            foreach (var xeEnemy in xeChamber.Elements("Enemy")){
                var enemyGuid = xeEnemy.Attribute("guid")?.Value ?? string.Empty;
                var enemyId = int.TryParse(xeEnemy.Attribute("id")?.Value ?? string.Empty, out var enemyIdValue) ? enemyIdValue : int.MinValue;
                var enemyX = int.TryParse(xeEnemy.Attribute("x")?.Value ?? string.Empty, out var enemyXValue) ? enemyXValue : int.MinValue;
                var enemyY = int.TryParse(xeEnemy.Attribute("y")?.Value ?? string.Empty, out var enemyYValue) ? enemyYValue : int.MinValue;
                if (enemyGuid == string.Empty || enemyId == int.MinValue || enemyX == int.MinValue || enemyY == int.MinValue) continue;
                chamber.Enemies.Add(new EnemyInformation {Guid = enemyGuid, Id = enemyId, ChamberGuid = chamberGuid, X = enemyX, Y = enemyY});
            }
        }
    }
}