using System;

[Serializable]
public static class Constants
{    
    [Serializable]
    public enum InteractionTypes
    {
        None,
        SavePoint,
        Shop,
        Talk
    }

    [Serializable]
    public enum Npc
    {
        None,
        Shopkeeper,
        HoboTom
    }
    
    public enum AssetTypes
    {
        Terrain,
        Grass,
        Overhang
    }

    public const int MONEY_ID = 0;
    public const string TAG_CHAMBER = "Chamber";
    public const string TAG_TERRAINCONTAINER = "TerrainContainer";
    public const string TAG_GRASSCONTAINER = "GrassContainer";
    public const string TAG_OVERHANGCONTAINER = "OverhangContainer";    
    public const string NAME_TERRAINCONTAINER = "Terrain";
    public const string NAME_GRASSCONTAINER = "Grass";
    public const string NAME_OVERHANGCONTAINER = "Overhangs";
    public const string NAME_ENVIRONMENTCONTAINER = "Environment";
    public const string NAME_SHADOWSCONTAINER = "Shadows";
    public const string NAME_NPCCONTAINER = "Npcs";
    public const string NAME_BACKGROUNDCONTAINER = "Background";
}