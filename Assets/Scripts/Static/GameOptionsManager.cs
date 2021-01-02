using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

public static class GameOptionsManager
{
    public const string OPTION_SHADOWS_ENABLE = "shadowsEnable";
    public const string OPTION_MUSIC_VOLUME = "musicVolume";
    public const string OPTION_AMBIENT_VOLUME = "ambientVolume";
    public const string OPTION_SFX_VOLUME = "sfxVolume";

    private static Dictionary<string, GameOption> _gameOptions;
    private static string _filePath => Application.persistentDataPath + $"/options.xml";
    private static object saveLoadInterlock;

    public static void Init()
    {
        saveLoadInterlock = new object();
        _gameOptions = new Dictionary<string, GameOption>();
        Add(OPTION_SHADOWS_ENABLE, "Shadows", typeof(bool));
        Add(OPTION_MUSIC_VOLUME, "Music Volume", typeof(float));
        Add(OPTION_AMBIENT_VOLUME, "Ambient Volume", typeof(float));
        Add(OPTION_SFX_VOLUME, "SFX Volume", typeof(float));
        Load(out var loadNote);
        if (!string.IsNullOrEmpty(loadNote))
        {
            Debug.Log(loadNote);
        }
    }

    public static bool Save(out string note)
    {
        lock (saveLoadInterlock)
        {
            var xeRoot = new XElement("Options");
            foreach (var gameOption in _gameOptions)
            {
                var xe = new XElement("Option");
                xe.SetAttributeValue("id", gameOption.Value.id);
                xe.SetAttributeValue("value", gameOption.Value.value);
                xeRoot.Add(xe);
            }
            try
            {
                xeRoot.Save(_filePath);
                note = "Options saved";
                return true;
            }
            catch (Exception ex)
            {
                note = ex.Message;
                return false;
            }
        }
    }

    public static bool Load(out string note)
    {
        lock (saveLoadInterlock)
        {
            XElement xeRoot;
            if (!System.IO.File.Exists(_filePath))
            {
                note = "Options did not exist";
                if (Save(out var noteIfError))
                {
                    note += " and were created";
                    return true;
                }
                else
                {
                    note += $" and couldn't be created: {noteIfError}";
                    return false;
                }
            }
            try
            {
                xeRoot = XElement.Load(_filePath);
            }
            catch (Exception)
            {
                note = "Options were corrupted";
                if (Save(out var noteIfError))
                {
                    note += " and were overwritten";
                    return true;
                }
                else
                {
                    note += $" and couldn't be created: {noteIfError}";
                    return false;
                }
            }
            foreach (var xe in xeRoot.Elements("Option"))
            {
                var id = xe.Attribute("id")?.Value ?? "";
                var value = xe.Attribute("value")?.Value ?? "";
                if (TryGetOption(id, out var gameOption))
                {
                    gameOption.value = value;
                }
            }
            note = "";
            return true;
        }
    }

    private static void Add(string id, string name, Type type)
    {
        _gameOptions.Add(id, new GameOption { id = id, name = name, type = type });
    }

    public static bool TryGetOption(string id, out GameOption gameOption)
    {
        if (_gameOptions == null)
        {
            gameOption = null;
            return false;
        }
        return _gameOptions.TryGetValue(id, out gameOption);
    }

    public static bool TryGetAllOptions(out List<GameOption> gameOptions)
    {
        gameOptions = new List<GameOption>();
        if (_gameOptions == null) return false;
        gameOptions = _gameOptions.Values.ToList();
        return true;
    }
}