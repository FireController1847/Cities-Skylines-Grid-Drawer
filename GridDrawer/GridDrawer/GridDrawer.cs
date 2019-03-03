using ICities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GridDrawer {
    public class GridDrawer : IUserMod {
        public string Name => "Grid Drawer";

        public string Description => "Draws useful grids overlayed on top of the land.";

        public void OnEnabled() {
            Log.Info($"Grid Drawer enabled. Build {Assembly.GetExecutingAssembly().GetName().Version}");
        }

        public void OnDisabled() {
            Log.Info($"Grid Drawer disabled.");
        }

        public void OnSettingsUI(UIHelperBase helper) {
            Options.makeSettings(helper);
        }
    }
}