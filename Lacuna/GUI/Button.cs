using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;

namespace Lacuna {
    // TODO: Needs serious refactoring
    public class Button {
        public Sprite Image { get; set; }
        public string Text { get => text2D.Text; set => text2D.Text = value; }
        public Rectangle Area { get; set; }
        public Rectangle MouseArea { get; private set; }
        public Color DefaultColor { get; set; }
        public Color HoverColor { get; set; }
        public Color ClickColor { get; set; }

        public event EventHandler Click;

        private Text2D text2D;
        private SoundEffect hoverSoundEffect;
        private SoundEffectInstance hoverSoundEffectInstance;
        private SoundEffect clickSoundEffect;
        private SoundEffectInstance clickSoundEffectInstance;
        private bool readyToPlay = true;

        public Button(Sprite image, string text, Color defaultColor, Color hoverColor, Color clickColor) {
            Image = image;
            MouseArea = new Rectangle(0, 0, 2, 2);
            DefaultColor = defaultColor;
            HoverColor = hoverColor;
            ClickColor = clickColor;
            // Call this here because if done in Button Load() it is loaded too late, drawables need to be loaded first before Button Load()
            text2D = new Text2D("Terminus", text, new Vector2(0, 0), Color.White);
            hoverSoundEffect = AssetManager.GetAsset(AssetType.SoundEffect, "PM_CS_beep_classic3_resampled");
            hoverSoundEffectInstance = hoverSoundEffect.CreateInstance();
            hoverSoundEffectInstance.Volume = 0.2f;
            clickSoundEffect= AssetManager.GetAsset(AssetType.SoundEffect, "PM_CS_beep_action_resampled");
            clickSoundEffectInstance = clickSoundEffect.CreateInstance();
            clickSoundEffectInstance.Volume = 0.2f;
        }

        public void OnClick(EventArgs e) {
            Click?.Invoke(this, e);
        }

        public void Initialize() {
            Area = Image.Area;
            Image.Position = new Vector2(Area.X, Area.Y);
            text2D.Position = new Vector2(Area.X + Area.Width / 2, Area.Y + Area.Height / 2);
            text2D.SetOriginCenter();
        }

        private MouseState oldMouseState;
        public void Update(MouseState mouseState) {
            if (Click != null && Click.GetInvocationList().Length > 0) {
                MouseArea = new Rectangle(mouseState.X, mouseState.Y, MouseArea.Width, MouseArea.Height);

                if (Area.Contains(MouseArea)) {
                    Image.Color = HoverColor;

                    if (readyToPlay) {
                        hoverSoundEffect.Play(0.2f, 0f, 0f);
                        readyToPlay = false;
                    }

                    if (mouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released) {
                        Image.Color = ClickColor;

                        clickSoundEffect.Play(0.2f, 0f, 0f);

                        OnClick(new EventArgs());
                    }

                    // causes button to activate if holding button down and moving over area
                    //oldMouseState = mouseState; 
                }
                else {
                    Image.Color = DefaultColor;
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