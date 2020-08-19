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
    }
    public class SavePoint {
        public string Guid;
        public string Name;    
        public string ChamberGuid;
        public Chamber Chamber => Chambers[ChamberGuid];
    }

    public static void Init(){
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
        Zones = new Dictionary<string, Zone>();
        var xeZones = xeRoot.Element("Zones");
        foreach (var xeZone in xeZones.Elements("Zone")){
            var guid = xeZone.Attribute("guid")?.Value ?? string.Empty;
            var name = xeZone.Attribute("name")?.Value ?? string.Empty;
            if (guid == string.Empty || name == string.Empty) continue;
            Zones.Add(guid, new Zone {Guid = guid, Name = name});
        }
        foreach (var xeChamber in xeZones.Elements("Rooms")){
            var guid = xeChamber.Attribute("guid")?.Value ?? string.Empty;
            var name = xeChamber.Attribute("name")?.Value ?? string.Empty;
            var zoneGuid = xeChamber.Attribute("zoneGuid")?.Value ?? string.Empty;
            if (guid == string.Empty || name == string.Empty || zoneGuid == string.Empty) continue;
            Chambers.Add(guid, new Chamber {Guid = guid, Name = name, ZoneGuid = zoneGuid});
            foreach (var xeSavePoint in xeChamber.Elements("SavePoint")){
                var savePointGuid = xeSavePoint.Attribute("guid")?.Value ?? string.Empty;
                var savePointName = xeSavePoint.Attribute("name")?.Value ?? string.Empty;
                if (savePointGuid == string.Empty || savePointName == string.Empty) continue;
                SavePoints.Add(savePointGuid, new SavePoint {Guid = savePointGuid, Name = savePointName, ChamberGuid = guid});
            }
        }
    }
}