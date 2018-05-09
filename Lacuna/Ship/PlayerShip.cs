using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

// If player moves, raise event of moved and send grid position data of player

namespace Lacuna {
    public class PlayerShip : Ship {

        // ------------------------------------------------------------------------------------------
        public PlayerShip(string[] texture2DPaths, Grid grid, Point gridPoint) : base(texture2DPaths, grid, gridPoint) {
        }

        // ------------------------------------------------------------------------------------------
        public void Update(KeyboardState NewKeyState, KeyboardState OldKeyState) {
            if (NewKeyState.IsKeyDown(Keys.W) && OldKeyState.IsKeyUp(Keys.W)) {
                Move(ShipMoveDirection.Forward);
            }
            if (NewKeyState.IsKeyDown(Keys.S) && OldKeyState.IsKeyUp(Keys.S)) {
                Move(ShipMoveDirection.Backward);
            }
            if (NewKeyState.IsKeyDown(Keys.D) && OldKeyState.IsKeyUp(Keys.D)) {
                Move(ShipMoveDirection.Right);
            }
            if (NewKeyState.IsKeyDown(Keys.A) && OldKeyState.IsKeyUp(Keys.A)) {
                Move(ShipMoveDirection.Left);
            }
        }
    }
}