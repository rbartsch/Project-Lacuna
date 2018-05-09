using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lacuna {
    public static class Drawable2DManager {
        private static List<Drawable2D> currentScreenDrawable2Ds;

        public static bool IsUnassignedScreenDrawable2DList() {
            if (currentScreenDrawable2Ds == null)
                return true;
            else
                return false;
        }

        public static void AssignScreenDrawable2DList(Screen screen) {
            currentScreenDrawable2Ds = screen.Drawable2Ds;
        }

        public static void Add(Drawable2D drawable2D) {
            currentScreenDrawable2Ds.Add(drawable2D);
        }
    }
}
