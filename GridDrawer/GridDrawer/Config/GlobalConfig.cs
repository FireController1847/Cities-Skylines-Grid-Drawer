using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace GridDrawer.Config {
    public class GlobalConfig {
        public const string FILENAME = "GridDrawer_GlobalConfig.xml";
        public const string BACKUP_FILENAME = "GridDrawer_GlobalConfig.bak";
        private static int LatestVersion = 1;
        private static DateTime TimeLastModified = DateTime.MinValue;

        public static GlobalConfig Instance;
        static GlobalConfig() {
            Reload();
        }

        //// Start XML ////

        /// <summary>
        /// Configuration version
        /// </summary>
        public int Version = LatestVersion;
        /// <summary>
        /// Whether or not the grid is enabled
        /// </summary>
        public bool EnableGrid = false;

        //// End XML ////
        
        internal static void WriteConfig() {
            TimeLastModified = WriteConfig(Instance);
        }

        private static GlobalConfig WriteDefaultConfig(GlobalConfig oldConfig, bool resetAll, out DateTime timeLastModified) {
            Log._Debug($"Writing default config...");
            GlobalConfig conf = new GlobalConfig();
            if (!resetAll && oldConfig != null) {
                conf.EnableGrid = oldConfig.EnableGrid;
            }
            timeLastModified = WriteConfig(conf);
            return conf;
        }
        
        private static DateTime WriteConfig(GlobalConfig config, string filename = FILENAME) {
            try {
                Log.Info($"Writing global config to file \"{filename}\"...");
                XmlSerializer serializer = new XmlSerializer(typeof (GlobalConfig));
                using (TextWriter writer = new StreamWriter(filename)) {
                    serializer.Serialize(writer, config);
                }
            } catch (Exception e) {
                Log.Error($"Could not write global config: {e.ToString()}");
            }

            try {
                return File.GetLastWriteTime(FILENAME);
            } catch (Exception e) {
                Log.Warning($"Could not determine modification date of global config: {e.ToString()}");
                return DateTime.Now;
            }
        }

        public static GlobalConfig Load(out DateTime timeLastModified) {
            try {
                timeLastModified = File.GetLastWriteTime(FILENAME);

                Log.Info($"Loading global config from file \"{FILENAME}\"...");
                using (FileStream fs = new FileStream(FILENAME, FileMode.Open)) {
                    XmlSerializer serializer = new XmlSerializer(typeof (GlobalConfig));
                    Log.Info($"Global config loaded.");
                    return (GlobalConfig) serializer.Deserialize(fs);
                }
            } catch (Exception e) {
                Log.Warning($"Could not load global config: {e}");
                return WriteDefaultConfig(null, false, out timeLastModified);
            }
        }

        public static void Reload(bool checkVersion = true) {
            DateTime timeLastModified;
            GlobalConfig conf = Load(out timeLastModified);
            if (checkVersion && conf.Version != -1 && conf.Version < LatestVersion) {
                string filename = BACKUP_FILENAME;
                try {
                    int backupIndex = 0;
                    while (File.Exists(filename)) {
                        filename = BACKUP_FILENAME + "." + backupIndex;
                        backupIndex++;
                    }
                    WriteConfig(conf, filename);
                } catch (Exception e) {
                    Log.Warning($"Error occurred while saving backup config to \"{filename}\": {e.ToString()}");
                }
                Reset(conf);
            } else {
                Instance = conf;
                TimeLastModified = WriteConfig(Instance);
            }
        }

        public static void Reset(GlobalConfig oldConfig, bool resetAll = false) {
            Log.Info($"Resetting global config.");
            DateTime timeLastModified;
            Instance = WriteDefaultConfig(oldConfig, resetAll, out timeLastModified);
            TimeLastModified = timeLastModified;
        }
    }
}
