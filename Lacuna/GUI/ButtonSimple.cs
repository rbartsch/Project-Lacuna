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
    public class ButtonSimple {
        public Rectangle Area { get; set; }
        public Rectangle MouseArea { get; private set; }

        public event EventHandler Click;

        private Sprite image;

        private bool activeStatus = true;

        private MouseState oldMouseState;

        private SoundEffect hoverSoundEffect;
        private SoundEffect clickSoundEffect;
        private bool readyToPlay = true;

        public ButtonSimple(Rectangle area, float hoverImageLayerDepth, Color hoverColor) {
            MouseArea = new Rectangle(0, 0, 2, 2);
            Area = area;

            hoverSoundEffect = AssetManager.GetAsset(AssetType.SoundEffect, "PM_CS_beep_classic3_resampled");
            clickSoundEffect = AssetManager.GetAsset(AssetType.SoundEffect, "PM_CS_beep_action_resampled");

            image = new Sprite(TextureUtils.GenerateFlatTexture(Core.graphics.GraphicsDevice, area.Width, area.Height, hoverColor), new Vector2(area.X, area.Y), Color.White, true, "", 0, null, null, SpriteEffects.None, hoverImageLayerDepth);
            image.DoDraw = false;
        }

        public void SetActiveStatus(bool status) {
            activeStatus = status;
        }

        public void ClearSubscriptions() {
            Click = null;
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
                    image.DoDraw = true;
                    if (readyToPlay) {
                        hoverSoundEffect.Play(0.05f, 0f, 0f);
                        readyToPlay = false;
                    }

                    if (mouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released) {
                        clickSoundEffect.Play(0.1f, 0f, 0f);

                        OnClick(new EventArgs());
                    }

                    // causes button to activate if holding button down and moving over area
                    //oldMouseState = mouseState; 
                }
                else {
                    image.DoDraw = false;
                    readyToPlay = true;
                }
            }
            else {
            }

            oldMouseState = mouseState;
        }
    }
}