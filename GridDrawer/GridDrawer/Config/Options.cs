using ColossalFramework.UI;
using GridDrawer.Config;
using ICities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GridDrawer {
    public class Options : MonoBehaviour {
        private static UICheckBox enableGridToggle = null;

        public static void makeSettings(UIHelperBase helper) {
            UIHelper actualHelper = helper as UIHelper;
            UIComponent container = actualHelper.self as UIComponent;

            enableGridToggle = actualHelper.AddCheckbox("Enable Grid", GlobalConfig.Instance.EnableVehicleManagerGrid, onEnableGridChanged) as UICheckBox;
        }

        private static void onEnableGridChanged(bool newValue) {
            Log._Debug($"enableGrid changed to {newValue}");
            GlobalConfig.Instance.EnableVehicleManagerGrid = newValue;
            GlobalConfig.WriteConfig();
        }

        public static void setEnableGrid(bool newValue) {
            GlobalConfig.Instance.EnableVehicleManagerGrid = newValue;
            GlobalConfig.WriteConfig();

            if (enableGridToggle != null)
                enableGridToggle.isChecked = newValue;
        }
    }
}
