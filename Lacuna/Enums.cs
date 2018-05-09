using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}