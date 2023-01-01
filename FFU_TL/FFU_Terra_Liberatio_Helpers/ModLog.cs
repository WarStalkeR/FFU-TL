#pragma warning disable CS0108
#pragma warning disable CS0626

using System;
using System.IO;
using System.Reflection;
using FFU_Terra_Liberatio;

namespace FFU_Terra_Liberatio {
    internal static class ModLog {
        internal static void Init() {
            using (var modLog = File.CreateText(FFU_TL_Defs.modLogPath)) {
                Console.ForegroundColor = ConsoleColor.Green;
                modLog.WriteLine($"[Mod:    Info] Terra Liberatio v{FFU_TL_Defs.modVersion}, {DateTime.Now}");
                Console.WriteLine($"[Mod:    Info] Terra Liberatio v{FFU_TL_Defs.modVersion}, {DateTime.Now}");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }
        internal static void Info(string logEntry = "") {
            using (var modLog = File.AppendText(FFU_TL_Defs.modLogPath)) {
                Console.ForegroundColor = ConsoleColor.Green;
                modLog.WriteLine($"[Mod:    Info] {logEntry}");
                Console.WriteLine($"[Mod:    Info] {logEntry}");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }
        internal static void Debug(string logEntry = "") {
            using (var modLog = File.AppendText(FFU_TL_Defs.modLogPath)) {
                Console.ForegroundColor = ConsoleColor.Blue;
                modLog.WriteLine($"[Mod:   Debug] {logEntry}");
                Console.WriteLine($"[Mod:   Debug] {logEntry}");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }
        internal static void Message(string logEntry = "") {
            using (var modLog = File.AppendText(FFU_TL_Defs.modLogPath)) {
                Console.ForegroundColor = ConsoleColor.White;
                modLog.WriteLine($"[Mod: Message] {logEntry}");
                Console.WriteLine($"[Mod: Message] {logEntry}");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }
        internal static void Warning(string logEntry = "") {
            using (var modLog = File.AppendText(FFU_TL_Defs.modLogPath)) {
                Console.ForegroundColor = ConsoleColor.Yellow;
                modLog.WriteLine($"[Mod: Warning] {logEntry}");
                Console.WriteLine($"[Mod: Warning] {logEntry}");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }
        internal static void Error(string logEntry = "") {
            using (var modLog = File.AppendText(FFU_TL_Defs.modLogPath)) {
                Console.ForegroundColor = ConsoleColor.Red;
                modLog.WriteLine($"[Mod:   Error] {logEntry}");
                Console.WriteLine($"[Mod:   Error] {logEntry}");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }
        internal static void Fatal(string logEntry = "") {
            using (var modLog = File.AppendText(FFU_TL_Defs.modLogPath)) {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                modLog.WriteLine($"[Mod:   Fatal] {logEntry}");
                Console.WriteLine($"[Mod:   Fatal] {logEntry}");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }
    }
}
