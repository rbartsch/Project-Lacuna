using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Lacuna.AstronomicalObjects;

namespace Lacuna {
    // Displays the local cluster of stars/planetary systems
    public class StarMapScreen : Screen {
        public List<Button> markers = new List<Button>();

        public void Test(object s, EventArgs e) {
            Console.WriteLine("Test");
        }

        public StarMapScreen(Core core, bool initializeOnStartup = true) : base("StarMapScreen", core, initializeOnStartup) {
        }

        public override void Initialize() {
            BuildMap();

            base.Initialize();
        }

        public void BuildMap() {
            foreach(PlanetarySystem p in Persistence.cluster.PlanetarySystems) {
                markers.Add(new Button("star_map_planetary_system_button", "Terminus", p.WorldPosition, p.Name, Color.White, new Color(53, 82, 120, 255), new Color(22, 81, 221, 255)));
                markers.Last().Click += Test;
                markers.Last().SetTextBelow();
            }
        }

        public override void Update(GameTime gameTime, KeyboardState NewKeyState, KeyboardState OldKeyState) {
            foreach(Button b in markers) {
                b.Update(Mouse.GetState());
            }

            base.Update(gameTime, NewKeyState, OldKeyState);
        }
    }
}