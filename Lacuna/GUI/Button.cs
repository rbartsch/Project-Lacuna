using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lacuna {
    // TODO: Be able to specify custom sound effects, improve performance by refactoring update out into
    // an event instead, otherwise if any situation needs thousands of buttons (highly unlikely) it can impact fps
    public class Button {
        public Sprite Image { get; set; }
        public string Text { get => text2D.Text; set => text2D.Text = value; }
        public Rectangle Area { get; set; }
        public Rectangle MouseArea { get; private set; }
        public Color DefaultColor { get; set; }
        public Color HoverColor { get; set; }
        public Color ClickColor { get; set; }       

        public event EventHandler Click;

        private bool activeStatus = true;

        private MouseState oldMouseState;

        private Text2D text2D;
        private SoundEffect hoverSoundEffect;
        private SoundEffect clickSoundEffect;
        private bool readyToPlay = true;

        public Button(string texture2DName, string spriteFontName, Vector2 position, string text, Color defaultColor, Color hoverColor, Color clickColor, bool drawInScreenSpace, float layerDepth = 0.12f) {
            Image = new Sprite(texture2DName, position, DefaultColor, drawInScreenSpace, "", 0, null, null, SpriteEffects.None, layerDepth);
            MouseArea = new Rectangle(0, 0, 2, 2);
            DefaultColor = defaultColor;
            HoverColor = hoverColor;
            ClickColor = clickColor;

            text2D = new Text2D(spriteFontName, text, new Vector2(0, 0), Color.White, drawInScreenSpace, "", layerDepth-0.01f);

            Area = Image.Area;
            Image.Position = new Vector2(Area.X, Area.Y);
            SetTextCenter();

            hoverSoundEffect = AssetManager.GetAsset(AssetType.SoundEffect, "PM_CS_beep_classic3_resampled");
            clickSoundEffect= AssetManager.GetAsset(AssetType.SoundEffect, "PM_CS_beep_action_resampled");
        }

        public void ToggleActiveStatus() {
            activeStatus = !activeStatus;
            Image.DoDraw = activeStatus;
            text2D.DoDraw = activeStatus;
        }

        public void ClearSubscriptions() {
            Click = null;
        }

        public void SetTextCenter() {
            text2D.Position = new Vector2(Area.X + Area.Width / 2, Area.Y + Area.Height / 2);
            text2D.SetOriginCenter();
        }

        public void SetTextBelow() {
            text2D.Position = new Vector2(Area.X + Area.Width / 2, Area.Y + Area.Height + (float)Math.Round(text2D.MeasureString().Y / 2));
            text2D.SetOriginCenter();
        }

        public void SetTextAbove() {
            text2D.Position = new Vector2(Area.X + Area.Width / 2, Area.Y - (float)Math.Round(text2D.MeasureString().Y / 2));
            text2D.SetOriginCenter();
        }

        public void OnClick(EventArgs e) {
            Click?.Invoke(this, e);
        }

        public void Update(MouseState mouseState, Camera2D camera2D = null) {
            if (!activeStatus) {
                return;
            }

            if (Click != null && Click.GetInvocationList().Length > 0) {
                MouseArea = new Rectangle(mouseState.X, mouseState.Y, MouseArea.Width, MouseArea.Height);

                // Transform Mouse.GetState() mouse pos which is screen position to world position
                // Useful for clicking on things that are in "world space" and not "screen space"
                if (camera2D != null) {
                    Matrix inverseTransform = Matrix.Invert(camera2D.Transform);
                    Vector2 mouseInWorld = Vector2.Transform(new Vector2(MouseArea.X, MouseArea.Y), inverseTransform);
                    mouseInWorld = new Vector2(mouseInWorld.X, mouseInWorld.Y);
                    MouseArea = new Rectangle((int)mouseInWorld.X, (int)mouseInWorld.Y, MouseArea.Width, MouseArea.Height);
                }

                if (Area.Contains(MouseArea)) {
                    Image.Color = HoverColor;
                    //text2D.Color = HoverColor;

                    if (readyToPlay) {
                        hoverSoundEffect.Play(0.2f, 0f, 0f);
                        readyToPlay = false;
                    }

                    if (mouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released) {
                        Image.Color = ClickColor;
                        //text2D.Color = ClickColor;

                        clickSoundEffect.Play(0.2f, 0f, 0f);

                        OnClick(new EventArgs());
                    }

                    // causes button to activate if holding button down and moving over area
                    //oldMouseState = mouseState; 
                }
                else {
                    Image.Color = DefaultColor;
                    //text2D.Color = DefaultColor;
                    readyToPlay = true;
                }
            }
            else {
                // disable button since it has nothing attached
                Image.Color = new Color(Color.Gray, 127);
            }

            oldMouseState = mouseState;
        }
    }
}