using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Lacuna {
    // To build an isometric grid in the game world.
    public class IsoGrid {
        public Point GridSize { get; set; }
        public Point GridTileSize { get; set; }
        // The very top grid will be the start position
        public Vector2 GridWorldStartPosition { get; set; }
        public List<GridTile> GridTiles { get; set; }
        public List<Text2D> TextMarkers { get; set; }
        public string SpriteName { get; set; }

        private static readonly string[] gridToNamesY = {
            "A","B","C","D","E","F","G","H","I","J","K","L","M",
            "N","O","P","Q","R","S","T","V","W","X","Y","Z",
        };

        // ------------------------------------------------------------------------------------------
        public IsoGrid(Point gridSize, Point gridTileSize, Vector2 gridWorldStartPosition, string spriteName) {
            GridSize = gridSize;
            GridTileSize = gridTileSize;
            GridWorldStartPosition = gridWorldStartPosition;
            GridTiles = new List<GridTile>();
            TextMarkers = new List<Text2D>();
            SpriteName = spriteName;
        }

        // ------------------------------------------------------------------------------------------
        public void Construct(bool generateMarkers = true) {
            Vector2 currGridTilePos = new Vector2(GridWorldStartPosition.X, GridWorldStartPosition.Y);
            int counter = 0;

            for (int y = 0; y < GridSize.Y; y++) {
                counter += 1;

                for (int x = 0; x < GridSize.X; x++) {
                    if (generateMarkers) {
                        TextMarkers.Add(new Text2D("Terminus", string.Format("{0},{1}", x, y), new Vector2(currGridTilePos.X, currGridTilePos.Y), Color.White));
                        if (y == 0) {
                            TextMarkers.Add(new Text2D("Terminus", (x + 1).ToString(), new Vector2(currGridTilePos.X + 121 - 40, currGridTilePos.Y - 60 + 20), Color.White));
                        }
                        if (x == 0) {
                            TextMarkers.Add(new Text2D("Terminus", gridToNamesY[y], new Vector2(currGridTilePos.X - 121 + 40, currGridTilePos.Y - 60 + 20), Color.White));
                        }
                    }

                    GridTiles.Add(new GridTile(new Sprite(SpriteName, currGridTilePos, Color.White), new Point(x, y)));
                    currGridTilePos += new Vector2((GridTileSize.X / 2) + 1, (GridTileSize.Y / 2) + 1);
                }

                currGridTilePos = new Vector2(GridWorldStartPosition.X - (counter * GridTileSize.Y) - counter, GridWorldStartPosition.Y + (counter * (GridTileSize.Y / 2)) + counter);
            }
        }

        // ------------------------------------------------------------------------------------------
        public Vector2 GetGridTileWorldPosByPoint(Point p) {
            Vector2 v = Vector2.One;
            foreach(GridTile g in GridTiles) {
                if(g.GridPosition == p) {
                    v = g.WorldPosition;
                }
            }

            return v;
        }

        // ------------------------------------------------------------------------------------------
        public GridTile GetGridTileByPoint(Point p) {
            GridTile gridTile = null;
            foreach(GridTile g in GridTiles) {
                if(g.GridPosition == p) {
                    gridTile = g;
                }
            }

            return gridTile;
        }

        // ------------------------------------------------------------------------------------------
        public bool OccupyGridTileByPoint(Point p, ref GridTile activeGridTile) {
            if (p.Y < 0 || p.X < 0 || p.Y >= GridSize.Y || p.X >= GridSize.X)
                return false;

            if (GetGridTileByPoint(p).Occupied)
                return false;

            activeGridTile.Occupied = false;
            GetGridTileByPoint(p).Occupied = true;
            activeGridTile = GetGridTileByPoint(p);

            //foreach (GridTile g in GridTiles) {
            //    if (g.Occupied)
            //        Console.WriteLine("GridTile @ " + g.GridPosition + " is occupied");
            //}

            return true;
        }
    }
}