using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Lacuna {
    public class Ship {
        public MultiSprite Sprite { get; set; }
        public Point GridPosition { get; set; }
        public ShipMoveDirection CurrentShipDirection { get; set; }

        public event EventHandler<ShipMoveCompleteEventArgs> ShipMoved;

        private IsoGrid grid;
        private GridTile activeGridTile;

        public Ship(string[] texture2DPaths, IsoGrid grid, Point gridPosition) {            
            GridPosition = gridPosition;
            this.grid = grid;
            activeGridTile = grid.GetGridTileByPoint(gridPosition);
            if(grid.OccupyGridTileByPointAsMovable(gridPosition, ref activeGridTile, false)) {
                Sprite = new MultiSprite(texture2DPaths, grid.GetGridTileWorldPosByPoint(gridPosition), Color.White);
            }
        }

        protected void RaiseShipMovedEvent(ShipMoveCompleteEventArgs eventArgs) {
            ShipMoved?.Invoke(this, eventArgs);
        }

        public void SetDirection(ShipMoveDirection direction) {
            switch (direction) {
                case ShipMoveDirection.Forward: {
                        Sprite.SwitchSprite(0);
                        CurrentShipDirection = ShipMoveDirection.Forward;
                    }
                    break;
                case ShipMoveDirection.Backward: {
                        Sprite.SwitchSprite(1);
                        CurrentShipDirection = ShipMoveDirection.Backward;
                    }
                    break;
                case ShipMoveDirection.Right: {
                        Sprite.SwitchSprite(2);
                        CurrentShipDirection = ShipMoveDirection.Right;
                    }
                    break;
                case ShipMoveDirection.Left: {
                        Sprite.SwitchSprite(3);
                        CurrentShipDirection = ShipMoveDirection.Left;
                    }
                    break;
                default: {
                        throw new ArgumentException("Incorrect ship direction");
                    }
            }
        }

        public bool Move(ShipMoveDirection direction) {
            Point newGridPosition = Point.Zero;

            switch (direction) {
                case ShipMoveDirection.Forward: {
                        newGridPosition = new Point(GridPosition.X, GridPosition.Y - 1);
                    }
                    break;
                case ShipMoveDirection.Backward: {
                        newGridPosition = new Point(GridPosition.X, GridPosition.Y + 1);
                    }
                    break;
                case ShipMoveDirection.Right: {
                        newGridPosition = new Point(GridPosition.X + 1, GridPosition.Y);
                    }
                    break;
                case ShipMoveDirection.Left: {
                        newGridPosition = new Point(GridPosition.X - 1, GridPosition.Y);
                    }
                    break;
                default: {
                        throw new ArgumentException("Incorrect ship direction");
                    }
            }

            if (grid.OccupyGridTileByPointAsMovable(newGridPosition, ref activeGridTile, false)) {
                GridPosition = newGridPosition;
                Sprite.Position = grid.GetGridTileWorldPosByPoint(GridPosition);
                SetDirection(direction);
                RaiseShipMovedEvent(new ShipMoveCompleteEventArgs(GridPosition, direction));
                return true;
            }
            else {
                Console.WriteLine("Cannot occupy grid tile: " + newGridPosition);
                return false;
            }
        }

        public EngagementVector GetEngagementCombination(ShipMoveDirection targetDir, Point targetPos) {
            Compass c = GetRelativeCompassDirection(GridPosition, targetPos);
            Console.WriteLine("npc=" + CurrentShipDirection + "; player=" + targetDir + "; compass=" + c);

            int a = CalculateSide(CurrentShipDirection, targetDir, c);
            // reverse
            c = GetRelativeCompassDirection(targetPos, GridPosition);
            int t = CalculateSide(targetDir, CurrentShipDirection, c);

            if(a < 0 && t < 0) {
                a = 4;
                t = 4;
            }

            Console.WriteLine("EngagementVector (npc) attack=" + (ShipSide)a + "; target=" + (ShipSide)t);

            return new EngagementVector((ShipSide)a, (ShipSide)t);
        }

        // No diagonals
        // TODO: We'd pass in weapon grid tile ranges here in future
        public Compass GetRelativeCompassDirection(Point currentPos, Point targetPos) {
            if (targetPos.X == currentPos.X) {
                // target is on the relative backward grid
                if (targetPos.Y > currentPos.Y) {
                    //Console.WriteLine("target is on the relative backward grid");
                    return Compass.South;
                }
                // target is on the relative forward grid
                if (targetPos.Y < currentPos.Y) {
                    //Console.WriteLine("target is on the relative forward grid");
                    return Compass.North;
                }
            }

            if (targetPos.Y == currentPos.Y) {
                // target is on the relative right grid
                if (targetPos.X > currentPos.X) {
                    //Console.WriteLine("target is on the relative right grid");
                    return Compass.East;
                }
                // target is on the relative left grid;
                if (targetPos.X < currentPos.X) {
                    //Console.WriteLine("target is on the relative left grid");
                    return Compass.West;
                }
            }

            return Compass.Unknown;
        }

        // Match to enums numerical order
        public int CalculateSide(ShipMoveDirection currentDir, ShipMoveDirection targetDir, Compass c) {
            if (c == Compass.Unknown) {
                Console.WriteLine("Cannot calculate engagement side as it is diagonal");
                return -1;
            }

            int[] combo = new int[3];
            combo[0] = (int)currentDir;
            combo[1] = (int)targetDir;
            combo[2] = (int)c;

            int s = 0;

            if (combo[2] == 0) {
                if (combo[0] <= 1) {
                    s = combo[0];
                }
                else if (combo[0] == 2) {
                    s = combo[0] + 1;
                }
                else if (combo[0] == 3) {
                    s = combo[0] - 1;
                }
            }
            if (combo[2] == 1) {
                if (combo[0] == 0) {
                    s = combo[0] + 1;
                }
                else if (combo[0] == 1) {
                    s = combo[0] - 1;
                }
                else if (combo[0] >= 2) {
                    s = combo[0];
                }
            }
            if (combo[2] == 2) {
                if (combo[0] <= 1) {
                    s = combo[0] + 2;
                }
                else {
                    s = combo[0] - 2;
                }
            }
            if (combo[2] == 3) {
                if (combo[0] == 0) {
                    s = combo[0] + 3;
                }
                else if (combo[0] == 1) {
                    s = combo[0] + 1;
                }
                else if (combo[0] == 2) {
                    s = combo[0] - 1;
                }
                else if (combo[0] == 3) {
                    s = combo[0] - 3;
                }
            }

            return s;
        }
    }
}