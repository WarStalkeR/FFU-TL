#pragma warning disable CS0108
#pragma warning disable CS0626

namespace FFU_Tyrian_Legacy {
    public static class ModLog {
        #if DEBUG
        private static readonly BepInEx.Logging.ManualLogSource Log = BepInEx.Logging.Logger.CreateLogSource("FFU_TL");
        #endif
        public static void Info() {
            #if DEBUG
            Log.Log(BepInEx.Logging.LogLevel.Info, (object)$"");
            #endif
        }
        public static void Info(string logEntry) {
            #if DEBUG
            Log.Log(BepInEx.Logging.LogLevel.Info, logEntry);
            #endif
        }
        public static void Debug() {
            #if DEBUG
            Log.Log(BepInEx.Logging.LogLevel.Debug, (object)$"");
            #endif
        }
        public static void Debug(string logEntry) {
            #if DEBUG
            Log.Log(BepInEx.Logging.LogLevel.Debug, logEntry);
            #endif
        }
        public static void Message() {
            #if DEBUG
            Log.Log(BepInEx.Logging.LogLevel.Message, (object)$"");
            #endif
        }
        public static void Message(string logEntry) {
            #if DEBUG
            Log.Log(BepInEx.Logging.LogLevel.Message, logEntry);
            #endif
        }
        public static void Warning() {
            #if DEBUG
            Log.Log(BepInEx.Logging.LogLevel.Warning, (object)$"");
            #endif
        }
        public static void Warning(string logEntry) {
            #if DEBUG
            Log.Log(BepInEx.Logging.LogLevel.Warning, logEntry);
            #endif
        }
        public static void Error() {
            #if DEBUG
            Log.Log(BepInEx.Logging.LogLevel.Error, (object)$"");
            #endif
        }
        public static void Error(string logEntry) {
            #if DEBUG
            Log.Log(BepInEx.Logging.LogLevel.Error, logEntry);
            #endif
        }
        public static void Fatal() {
            #if DEBUG
            Log.Log(BepInEx.Logging.LogLevel.Fatal, (object)$"");
            #endif
        }
        public static void Fatal(string logEntry) {
            #if DEBUG
            Log.Log(BepInEx.Logging.LogLevel.Fatal, logEntry);
            #endif
        }
    }
}
