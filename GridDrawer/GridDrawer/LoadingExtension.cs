using ColossalFramework;
using ICities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GridDrawer {
    public class LoadingExtension : LoadingExtensionBase {

        public static FastList<IRenderableManager> RenderManagers { get {
            return (FastList<IRenderableManager>) typeof (RenderManager).GetField("m_renderables", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
        } }

        public override void OnLevelLoaded(LoadMode mode) {
            RenderManagers.Add(GridRenderManager.Instance);
        }

        public override void OnLevelUnloading() {
            RenderManagers.Remove(GridRenderManager.Instance);
        }
    }
}
