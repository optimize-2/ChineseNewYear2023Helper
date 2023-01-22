using System;
using Celeste.Mod.ChineseNewYear2023Helper.POMR;

namespace Celeste.Mod.ChineseNewYear2023Helper {
    public class ChineseNewYear2023HelperModule : EverestModule {
        public static readonly String NAME = "ChineseNewYear2023Helper";

        public static ChineseNewYear2023HelperModule Instance { get; private set; }

        public override Type SettingsType => typeof(ChineseNewYear2023HelperModuleSettings);
        public static ChineseNewYear2023HelperModuleSettings Settings => (ChineseNewYear2023HelperModuleSettings) Instance._Settings;

        public override Type SessionType => typeof(ChineseNewYear2023HelperModuleSession);
        public static ChineseNewYear2023HelperModuleSession Session => (ChineseNewYear2023HelperModuleSession) Instance._Session;

        private static POMRController pomrController;

        public ChineseNewYear2023HelperModule() {
            Instance = this;
        }

        public override void Load() {
            Logger.SetLogLevel(NAME, LogLevel.Info);
            pomrController = new POMRController(Celeste.Instance);
            Celeste.Instance.Components.Add(pomrController);
        }

        public override void Unload() {
            pomrController.Unload();
            Celeste.Instance.Components.Remove(pomrController);
        }

        public static void info(string s) {
            Logger.Log(LogLevel.Info, NAME, s);
        }
    }
}