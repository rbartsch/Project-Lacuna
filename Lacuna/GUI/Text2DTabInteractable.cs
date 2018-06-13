using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Lacuna {
    public class Text2DTabInteractable {
        private int rows;
        private int cols;
        private Vector2 cellStartPosTopLeft;
        private Vector2 textPosTopLeftRelative;
        private int[] widthSpacing;
        private int heightSpacing;
        private int rowSeparatorSpacing;
        private int colSeparatorSpacing;

        public Text2D[,] Text2Ds { get; set; }
        public ButtonSimple[,] Buttons { get; set; }
        public int[] SelectedIndex { get; private set; }

        /// <summary>
        /// Variable size width spacing should correspond to number of cols
        /// e.g for first col it can be 200, for second 50, etc, or just add 
        /// uniformly 200 and 200.
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="cols"></param>
        /// <param name="cellStartPosTopLeft"></param>
        /// <param name="textPosTopLeftRelative"></param>
        /// <param name="widthSpacing"></param>
        /// <param name="heightSpacing"></param>
        /// <param name="rowSeparatorSpacing"></param>
        /// <param name="colSeparatorSpacing"></param>
        public Text2DTabInteractable(int rows, int cols, Vector2 cellStartPosTopLeft, Vector2 textPosTopLeftRelative, int[] widthSpacing, int heightSpacing, int rowSeparatorSpacing, int colSeparatorSpacing) {
            this.rows = rows;
            this.cols = cols;
            this.cellStartPosTopLeft = cellStartPosTopLeft;
            this.textPosTopLeftRelative = textPosTopLeftRelative;
            this.widthSpacing = widthSpacing;
            this.heightSpacing = heightSpacing;
            this.rowSeparatorSpacing = rowSeparatorSpacing;
            this.colSeparatorSpacing = colSeparatorSpacing;
            Text2Ds = new Text2D[cols, rows];
            Buttons = new ButtonSimple[cols, rows];
        }

        public void Construct(string spriteFontName, bool drawInScreenSpace = false, string tag = "", float layerDepth = 0.08f, float hoverImageLayerDepth = 0.09f, Color? hoverColor = null) {
            float rowMultiplier = cellStartPosTopLeft.Y;
            float colMultiplier = cellStartPosTopLeft.X;
            for (int y = 0; y < rows; y++) {
                for (int x = 0; x < cols; x++) {
                    Rectangle cell = new Rectangle((int)colMultiplier, (int)rowMultiplier, widthSpacing[x], heightSpacing);

                    Buttons[x, y] = new ButtonSimple(cell, hoverImageLayerDepth, hoverColor ?? new Color(53, 82, 120));
                    int[] indexRef = { x, y };
                    Buttons[x, y].Click += delegate (object s, EventArgs e) {
                        AssignSelectedIndex(s, e, indexRef);
                    };

                    Text2Ds[x, y] = new Text2D(spriteFontName, "", new Vector2(cell.X + textPosTopLeftRelative.X, cell.Y + textPosTopLeftRelative.Y), Color.White, drawInScreenSpace, tag, layerDepth);

                    colMultiplier += widthSpacing[x] + colSeparatorSpacing;
                }

                colMultiplier = cellStartPosTopLeft.X;
                rowMultiplier += heightSpacing + rowSeparatorSpacing;
            }
        }

        public void AssignSelectedIndex(object s, EventArgs e, int[] index) {
            Console.WriteLine(index[0] + "," + index[1]);
            SelectedIndex = index;
        }

        public void Update(MouseState mouse, Camera2D camera2D = null) {
            foreach (ButtonSimple b in Buttons) {
                if (b != null) {
                    b.Update(mouse, camera2D);
                }
            }
        }
    }
}