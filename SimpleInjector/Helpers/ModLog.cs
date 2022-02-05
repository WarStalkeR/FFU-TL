#pragma warning disable CS1591

using BepInEx.Logging;

namespace SimpleInjector {
    public class InjLog {
        private static readonly ManualLogSource Log = Logger.CreateLogSource("Sim_Inj");
        public static void Info() {
            Log.Log(LogLevel.Info, (object)$"");
        }
        public static void Info(string logEntry) {
            Log.Log(LogLevel.Info, logEntry);
        }
        public static void Debug() {
            Log.Log(LogLevel.Debug, (object)$"");
        }
        public static void Debug(string logEntry) {
            Log.Log(LogLevel.Debug, logEntry);
        }
        public static void Message() {
            Log.Log(LogLevel.Message, (object)$"");
        }
        public static void Message(string logEntry) {
            Log.Log(LogLevel.Message, logEntry);
        }
        public static void Warning() {
            Log.Log(LogLevel.Warning, (object)$"");
        }
        public static void Warning(string logEntry) {
            Log.Log(LogLevel.Warning, logEntry);
        }
        public static void Error() {
            Log.Log(LogLevel.Error, (object)$"");
        }
        public static void Error(string logEntry) {
            Log.Log(LogLevel.Error, logEntry);
        }
        public static void Fatal() {
            Log.Log(LogLevel.Fatal, (object)$"");
        }
        public static void Fatal(string logEntry) {
            Log.Log(LogLevel.Fatal, logEntry);
        }
    }
}
