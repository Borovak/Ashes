using System;

namespace Classes
{
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
    
        [Serializable]
        public enum PanelTypes
        {
            Craftables,
            Inventory,
            ShopBuy,
            ShopSell
        }
        
        [Serializable]
        public enum ControllerButtons
        {
            A,
            B,
            X,
            Y,
            Start,
            Select,
            LB,
            RB,
            LT,
            RT,
            LJ,
            RJ,
            DUp,
            DDown,
            DLeft,
            DRight,
            L3,
            R3
        }

        public const int MONEY_ID = 0;
        public const float BUTTON_FILLRATE = 2f;
        public const string TAG_CHAMBER = "Chamber";
        public const string TAG_TERRAINCONTAINER = "TerrainContainer";
        public const string TAG_GRASSCONTAINER = "GrassContainer";
        public const string TAG_OVERHANGCONTAINER = "OverhangContainer";    
        public const string TAG_SAVEPOINT = "SavePoint";  
        public const string NAME_TERRAINCONTAINER = "Terrain";
        public const string NAME_GRASSCONTAINER = "Grass";
        public const string NAME_OVERHANGCONTAINER = "Overhangs";
        public const string NAME_ENVIRONMENTCONTAINER = "Environment";
        public const string NAME_SHADOWSCONTAINER = "Shadows";
        public const string NAME_NPCCONTAINER = "Npcs";
        public const string NAME_BACKGROUNDCONTAINER = "Background";
        
    }
}