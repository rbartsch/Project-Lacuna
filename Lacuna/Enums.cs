namespace Lacuna {
    public enum AssetType {
        Texture2D,
        SpriteFont,
        SoundEffect,
        Song
    }

    public enum NpcShipState {
        Attack,
        Defend,
        Pursue,
        Evade,
        Idle
    }

    public enum ShipMoveDirection {
        Forward,                            // 0
        Backward,                           // 1
        Right,                              // 2
        Left,                               // 3
        Unknown                             // 4
    }

    public enum ShipSide {
        Bow,                                // 0 (FRONT)
        Stern,                              // 1 (BACK)
        Starboard,                          // 2 (RIGHT)
        Port,                               // 3 (LEFT)
        Unknown                             // 4
    }

    public enum Compass {
        North,                              // 0
        South,                              // 1
        East,                               // 2
        West,                               // 3
        Unknown                             // 4
    }

    public enum PlanetType {
        Rocky,
        Terra,
        Oceanic,
        Icy,
        Gas
    }

    public enum MoonType {
        Regolith,
        Icy,
    }

    public enum StarType {
        MainSequence,
        Neutron
    }

    public enum CommodityType {
        Food,
        Medicine,
        Narcotics,
        Implants,
        Weapons,
        Information,
        ShipComponents,
        Machinery,
        Construction,
        RawMaterials,
        ProcessedMaterials
    }

    public enum Legality {
        Legal,
        Illegal,
        Unknown
    }
}