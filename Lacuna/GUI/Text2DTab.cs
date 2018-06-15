using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Lacuna {
    // have multiple text2d's spaced between each other uniformly
    public class Text2DTab {
        private int rows;
        private int cols;
        private Vector2 startPosLeftCorner;
        private int[] widthSpacing;
        private int heightSpacing;

        public Text2D[,] Text2Ds { get; set; }

        /// <summary>
        /// Variable size width spacing should correspond to number of cols
        /// e.g for first col it can be 200, for second 50, etc, or just add 
        /// uniformly 200 and 200.
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="cols"></param>
        /// <param name="startPosLeftCorner"></param>
        /// <param name="widthSpacing"></param>
        /// <param name="heightSpacing"></param>
        public Text2DTab(int rows, int cols, Vector2 startPosLeftCorner, int[] widthSpacing, int heightSpacing) {
            this.rows = rows;
            this.cols = cols;
            this.startPosLeftCorner = startPosLeftCorner;
            this.widthSpacing = widthSpacing;
            this.heightSpacing = heightSpacing;
            Text2Ds = new Text2D[cols, rows];
        }

        public void Construct(string spriteFontName, bool drawInScreenSpace = false, string tag = "", float layerDepth = 0.08f) {
            float rowMultiplier = startPosLeftCorner.Y;
            float colMultiplier = startPosLeftCorner.X;
            for (int y = 0; y < rows; y++) {
                for (int x = 0; x < cols; x++) {
                    Text2Ds[x, y] = new Text2D(spriteFontName, "", new Vector2(colMultiplier, rowMultiplier), Color.White, drawInScreenSpace, tag, layerDepth);
                    colMultiplier += widthSpacing[x];
                }

                colMultiplier = startPosLeftCorner.X;
                rowMultiplier += heightSpacing;
            }
        }
    }
}