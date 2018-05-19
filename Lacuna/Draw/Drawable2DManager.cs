using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lacuna {
    public static class Drawable2DManager {
        private static List<Drawable2D> currentDrawable2Ds;
        private static List<Drawable2D> currentScreenSpaceDrawable2Ds;

        // ------------------------------------------------------------------------------------------
        public static bool IsUnassignedScreenDrawable2DsList() {
            if (currentDrawable2Ds == null || currentScreenSpaceDrawable2Ds == null)
                return true;
            else
                return false;
        }

        // ------------------------------------------------------------------------------------------
        public static void AssignScreenDrawable2DList(Screen screen) {
            currentDrawable2Ds = screen.Drawable2Ds;
            currentScreenSpaceDrawable2Ds = screen.ScreenSpaceDrawable2Ds;
        }

        // ------------------------------------------------------------------------------------------
        public static void AddToDrawable2DList(Drawable2D drawable2D) {
            currentDrawable2Ds.Add(drawable2D);
        }

        // ------------------------------------------------------------------------------------------
        public static void AddToScreenSpaceDrawable2DList(Drawable2D drawable2D) {
            currentScreenSpaceDrawable2Ds.Add(drawable2D);
        }
    }
}
