using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Lacuna {
    // have multiple text2d's spaced between each other uniformly
    public class Text2DTabular {
        private int rows;
        private int cols;
        private Vector2 startPosLeftCorner;
        private int widthSpacing;
        private int heightSpacing;

        public Text2D[,] text2Ds;

        public Text2DTabular(int rows, int cols, Vector2 startPosLeftCorner, int widthSpacing, int heightSpacing) {
            this.rows = rows;
            this.cols = cols;
            this.startPosLeftCorner = startPosLeftCorner;
            this.widthSpacing = widthSpacing;
            this.heightSpacing = heightSpacing;
            text2Ds = new Text2D[cols, rows];
        }

        public void Construct(string spriteFontName, bool drawInScreenSpace = false, string tag ="", float layerDepth = 0.11f) {
            float rowMultiplier = startPosLeftCorner.Y;
            float colMultiplier = startPosLeftCorner.X;
            for(int y = 0; y < rows; y++) {
                for(int x = 0; x < cols; x++) {
                    text2Ds[x,y] = new Text2D(spriteFontName, "", new Vector2(colMultiplier, rowMultiplier), Color.White, drawInScreenSpace, tag, layerDepth);
                    colMultiplier += widthSpacing;
                }

                colMultiplier = startPosLeftCorner.X;
                rowMultiplier += heightSpacing;
            }
        }
    }
}