using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Lacuna {
    public class NpcShip : Ship {

        // ------------------------------------------------------------------------------------------
        public NpcShip(string[] texture2DPaths, IsoGrid grid, Point gridPosition, PlayerShip playerShip) : base(texture2DPaths, grid, gridPosition) {
            playerShip.ShipMoved += OnPlayerMoved;
        }

        // ------------------------------------------------------------------------------------------
        public void OnPlayerMoved(object sender, ShipMoveCompleteEventArgs e) {
            //Move(ShipMoveDirection.Backward);

            // TODO: we need a MUCH more efficient way of seeing if tile is not occupied instead of 
            // endlessly retrying, rather try just once., can fix via refactoring, although it is 
            // not a performance problem at the moment.
            //Move(DetermineNextMove(e.GridPosition));
            bool canMove = false;
            do {
                canMove = Move(DetermineNextMove(e.GridPosition));
            } while (!canMove);

            EngagementVector ev = GetEngagementCombination(e.Direction,  e.GridPosition);
        }

        // TODO: check if not moving out of bounds first then redo random
        // ------------------------------------------------------------------------------------------
        public ShipMoveDirection DetermineNextMove(Point targetPos) {
            Compass c = GetRelativeCompassDirection(GridPosition, targetPos);

            if (c == Compass.Unknown) {
                int choice = Rng.Random.Next(0, 2);
                if (choice == 0) {
                    if (targetPos.X < GridPosition.X) {
                        int choice2 = Rng.Random.Next(0, 2);
                        if (choice2 == 0) {
                            return ShipMoveDirection.Left;
                        }
                        else {
                            return ShipMoveDirection.Right;
                        }
                    }
                    else if (targetPos.X > GridPosition.X) {
                        int choice2 = Rng.Random.Next(0, 2);
                        if (choice2 == 0) {
                            return ShipMoveDirection.Left;
                        }
                        else {
                            return ShipMoveDirection.Right;
                        }
                    }
                }
                else {
                    if (targetPos.Y > GridPosition.Y) {
                        int choice2 = Rng.Random.Next(0, 2);
                        if (choice2 == 0) {
                            return ShipMoveDirection.Backward;
                        }
                        else {
                            return ShipMoveDirection.Forward;
                        }
                    }
                    else if (targetPos.Y < GridPosition.Y) {
                        int choice2 = Rng.Random.Next(0, 2);
                        if (choice2 == 0) {
                            return ShipMoveDirection.Backward;
                        }
                        else {
                            return ShipMoveDirection.Forward;
                        }
                    }
                }
            }
            else {
                int choice = Rng.Random.Next(0, 4);
                if (choice == 0) {
                    return ShipMoveDirection.Forward;
                }
                else if (choice == 1) {
                    return ShipMoveDirection.Backward;
                }
                else if (choice == 2) {
                    return ShipMoveDirection.Right;
                }
                else if (choice == 3) {
                    return ShipMoveDirection.Left;
                }
            }          

            return ShipMoveDirection.Unknown;
        }
    }
}