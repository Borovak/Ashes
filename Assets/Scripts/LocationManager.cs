using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public static class LocationManager
{
    public static Dictionary<int, string> zones;
    public static Dictionary<int, ChamberInfo> chambers;
    public static int currentChamberId = -1;

    public class ChamberInfo
    {
        public int id;
        public string name;
        public int zoneId;
        public string zoneName => LocationManager.zones.TryGetValue(zoneId, out var name) ? name : "";
        public string sceneName;
    }

    public static void Load()
    {
        zones = new Dictionary<int, string>();
        chambers = new Dictionary<int, ChamberInfo>();
        TextAsset textAsset;
        var filePath = "chambers.xml";
        try
        {
            textAsset = Resources.Load<TextAsset>(filePath);
        }
        catch (Exception)
        {
            throw new Exception($"Cannot load location information file '{filePath}'");
        }
        XElement xeRoot;
        try
        {
            xeRoot = XElement.Parse(textAsset.text);
        }
        catch (Exception)
        {
            throw new Exception($"Cannot parse location information file '{filePath}'");
        }
        foreach (var xe in xeRoot.Elements("zone"))
        {
            var sId = xe.Attribute("id")?.Value ?? "";
            if (!int.TryParse(sId, out var id))
            {
                throw new Exception($"Cannot parse one zone element id in location information file '{filePath}'");
            }
            var name = xe.Attribute("name")?.Value ?? "";
            if (name == "")
            {
                throw new Exception($"Cannot parse one zone element name location information file '{filePath}'");
            }
            zones.Add(id, name);
        }
        foreach (var xe in xeRoot.Elements("chamber"))
        {
            var sId = xe.Attribute("id")?.Value ?? "";
            if (!int.TryParse(sId, out var id))
            {
                throw new Exception($"Cannot parse one chamber element id in location information file '{filePath}'");
            }
            var name = xe.Attribute("name")?.Value ?? "";
            if (name == "")
            {
                throw new Exception($"Cannot parse one chamber element name in location information file '{filePath}'");
            }
            var sZoneId = xe.Attribute("zoneId")?.Value ?? "";
            if (!int.TryParse(sId, out var zoneId))
            {
                throw new Exception($"Cannot parse one chamber element zoneId in location information file '{filePath}'");
            }
            var sceneName = xe.Attribute("sceneName")?.Value ?? "";
            if (name == "")
            {
                throw new Exception($"Cannot parse one chamber element sceneName in location information file '{filePath}'");
            }
            chambers.Add(id, new ChamberInfo { id = id, name = name, zoneId = zoneId, sceneName = sceneName });
        }

    }

    public static ChamberInfo GetChamber(int id = -1)
    {
        return id == -1 ? chambers[currentChamberId] : chambers[id];
    }

}