using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace Lacuna {
    // TODO: Have a contentmanager for global and contentmanager for each screen, so we can 
    // unload content for a screen.
    // For now we load everything at startup in Core LoadContent
    public static class AssetManager {
        public static GraphicsDevice GraphicsDevice;
        public static ContentManager Content;
        private static Dictionary<string, Texture2D> Texture2Ds = new Dictionary<string, Texture2D>();
        private static Dictionary<string, SpriteFont> SpriteFonts = new Dictionary<string, SpriteFont>();
        private static Dictionary<string, SoundEffect> SoundEffects = new Dictionary<string, SoundEffect>();
        private static Dictionary<string, Song> Songs = new Dictionary<string, Song>();

        /// <summary>
        /// Loads the original asset of which it can be used (shared) or copied throughout the game.
        /// </summary>
        /// <param name="assetType"></param>
        /// <param name="path"></param>
        public static void LoadAsset(AssetType assetType, string path) {
            Console.WriteLine("Loading asset " + assetType.ToString() + ": " + path);
            switch (assetType) {
                case AssetType.Texture2D:
                    Texture2Ds.Add(path, Content.Load<Texture2D>("Textures/" + path));
                    break;

                case AssetType.SpriteFont:
                    SpriteFonts.Add(path, Content.Load<SpriteFont>("Fonts/" + path));
                    break;

                case AssetType.SoundEffect:
                    SoundEffects.Add(path, Content.Load<SoundEffect>("Sounds/" + path));
                    break;

                case AssetType.Song:
                    Songs.Add(path, Content.Load<Song>("Music/" + path));
                    break;

                default:
                    throw new Exception("Unknown asset");
            }
        }

        /// <summary>
        /// Get shared and loaded asset, good for many of the same type where original asset isn't changed.
        /// </summary>
        /// <param name="assetType"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static dynamic GetAsset(AssetType assetType, string name) {
            Console.WriteLine("Getting asset " + assetType.ToString() + ": " + name);
            Exception notFound = new Exception(string.Format("Could not find asset ({0}): {1}", name, assetType.ToString()));

            switch (assetType) {
                case AssetType.Texture2D:
                    Texture2D texture2D;
                    Texture2Ds.TryGetValue(name, out texture2D);
                    if (texture2D == null)
                        throw notFound;

                    return texture2D;

                case AssetType.SpriteFont:
                    SpriteFont spriteFont;
                    SpriteFonts.TryGetValue(name, out spriteFont);
                    if (spriteFont == null)
                        throw notFound;

                    return spriteFont;

                case AssetType.SoundEffect:
                    SoundEffect soundEffect;
                    SoundEffects.TryGetValue(name, out soundEffect);
                    if (soundEffect == null)
                        throw notFound;

                    return soundEffect;

                case AssetType.Song:
                    Song song;
                    Songs.TryGetValue(name, out song);
                    if (song == null)
                        throw notFound;

                    return song;

                default:
                    throw new Exception("Unknown asset");
            }
        }

        /// <summary>
        /// Get a copy of a Texture2D so that it can be manipulated like changing its pixels
        /// without affecting other assets that share the original asset that don't need to
        /// be changed.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Texture2D GetCopyTexture2D(string name) {
            Console.WriteLine("Getting copy of Texture2D: " + name);
            Texture2D original = GetAsset(AssetType.Texture2D, name);
            Texture2D copy = new Texture2D(GraphicsDevice, original.Width, original.Height);
            Color[] data = new Color[copy.Width * copy.Height];
            original.GetData(data);
            copy.SetData(data);
            return copy;
        }

        public static int TotalLoaded() {
            return Texture2Ds.Count + SpriteFonts.Count + SoundEffects.Count + Songs.Count;
        }
    }
}
