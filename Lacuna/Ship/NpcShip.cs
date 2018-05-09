using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Lacuna {
    public class NpcShip : Ship {

        // ------------------------------------------------------------------------------------------
        public NpcShip(string[] texture2DPaths, Grid grid, Point gridPosition, PlayerShip playerShip) : base(texture2DPaths, grid, gridPosition) {
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

        Random r = new Random();
        // TODO: check if not moving out of bounds first then redo random
        public ShipMoveDirection DetermineNextMove(Point targetPos) {
            Compass c = GetRelativeCompassDirection(GridPosition, targetPos);

            if (c == Compass.Unknown) {
                int choice = r.Next(0, 2);
                if (choice == 0) {
                    if (targetPos.X < GridPosition.X) {
                        int choice2 = r.Next(0, 2);
                        if (choice2 == 0) {
                            return ShipMoveDirection.Left;
                        }
                        else {
                            return ShipMoveDirection.Right;
                        }
                    }
                    else if (targetPos.X > GridPosition.X) {
                        int choice2 = r.Next(0, 2);
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
                        int choice2 = r.Next(0, 2);
                        if (choice2 == 0) {
                            return ShipMoveDirection.Backward;
                        }
                        else {
                            return ShipMoveDirection.Forward;
                        }
                    }
                    else if (targetPos.Y < GridPosition.Y) {
                        int choice2 = r.Next(0, 2);
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
                int choice = r.Next(0, 4);
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

            //if(c == Compass.Unknown) {
            //    if(targetPos.X < GridPosition.X) {
            //        return ShipMoveDirection.Left;
            //    }
            //    else if (targetPos.X > GridPosition.X) {
            //        return ShipMoveDirection.Right;
            //    }

            //    if(targetPos.Y > GridPosition.Y) {
            //        return ShipMoveDirection.Backward;
            //    }
            //    else if(targetPos.Y < GridPosition.Y) {
            //        return ShipMoveDirection.Forward;
            //    }
            //}
            //else if (c == Compass.South) {
            //    Random r = new Random();
            //    int choice = r.Next(0, 3);
            //    if (choice == 0) {
            //        return ShipMoveDirection.Forward;
            //    }
            //    else if (choice == 1) {
            //        return ShipMoveDirection.Right;
            //    }
            //    else if (choice == 2) {
            //        return ShipMoveDirection.Left;
            //    }                
            //}
            //else if(c == Compass.North) {
            //    Random r = new Random();
            //    int choice = r.Next(0, 3);
            //    if (choice == 0) {
            //        return ShipMoveDirection.Backward;
            //    }
            //    else if (choice == 1) {
            //        return ShipMoveDirection.Right;
            //    }
            //    else if (choice == 2) {
            //        return ShipMoveDirection.Left;
            //    }
            //}
            //else if (c == Compass.East) {
            //    Random r = new Random();
            //    int choice = r.Next(0, 3);
            //    if (choice == 0) {
            //        return ShipMoveDirection.Forward;
            //    }
            //    else if (choice == 1) {
            //        return ShipMoveDirection.Backward;
            //    }
            //    else if (choice == 2) {
            //        return ShipMoveDirection.Left;
            //    }
            //}
            //else if (c == Compass.West) {
            //    Random r = new Random();
            //    int choice = r.Next(0, 3);
            //    if (choice == 0) {
            //        return ShipMoveDirection.Forward;
            //    }
            //    else if (choice == 1) {
            //        return ShipMoveDirection.Backward;
            //    }
            //    else if (choice == 2) {
            //        return ShipMoveDirection.Right;
            //    }
            //}

            return ShipMoveDirection.Unknown;
        }
    }
}