using CoOpSpRpG;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace FFU_Terra_Liberatio {
    public class FFU_TL_Defs {
        public static readonly string exeFilePath = Directory.GetCurrentDirectory() + @"\";
        public static readonly string modConfDir = @"Mods\";
        public static readonly string modDumpsDir = @"Dumps\";
        public static readonly string modConfFile = @"FFU_Terra_Liberatio.ini";
        public static readonly string modLogFile = @"FFU_Terra_Liberatio.log";
        public static readonly string modVersion = "0.0.1.3";
        public static readonly string modLogPath = exeFilePath + modConfDir + modLogFile;
        public static readonly bool isPatchDebug = false;
        public static readonly bool doDataDump = true;
        public static readonly bool doArtDump = false;
        public static readonly bool isDebug = true;
        public static bool secretStash = false;
        public static List<Color> unlistStatic = new List<Color>(new Color[] {
            new Color(145, 117, 24) //Sinidal Cascade
        });
        public static List<Color> unlistDynamic = new List<Color>();
        public static List<uint> startingTechIDs = new List<uint>();
        public static Dictionary<byte, Dictionary<byte, Dictionary<byte, Module>>> refModules = new Dictionary<byte, Dictionary<byte, Dictionary<byte, Module>>>();
        public static Dictionary<ulong, Ship> reviveShips = new Dictionary<ulong, Ship>();
        public static ulong reviveShipID = 0;
        public static void checkModifiedEntry(uint rEntry, Color cEntry) {
            bool foundEntry = LOOTBAG.modules.ContainsKey(rEntry);
            bool foundColor = LOOTBAG.modules.Count(x => x.Value == cEntry) > 0;
            if (foundEntry && foundColor) {
                if (LOOTBAG.modules[rEntry] != cEntry) {
                    ModLog.Warning($" > The Entry #{rEntry} you want overwrite exists, but with Color Code ({LOOTBAG.modules[rEntry].R}, {LOOTBAG.modules[rEntry].G}, {LOOTBAG.modules[rEntry].B}).");
                    ModLog.Warning($" > The Color Code ({cEntry.R}, {cEntry.G}, {cEntry.B}) you want to add already exists, but under Entry #{LOOTBAG.modules.FirstOrDefault(x => x.Value == cEntry).Key}.");
                } else if (isPatchDebug) ModLog.Message($" > Overwriting Entry #{LOOTBAG.modules.FirstOrDefault(x => x.Value == cEntry).Key}, Color Code: {cEntry.R}, {cEntry.G}, {cEntry.B}.");
            } else if (!foundEntry && foundColor) {
                ModLog.Warning($" > The Color Code ({cEntry.R}, {cEntry.G}, {cEntry.B}) you want to add already exists, but under Entry #{LOOTBAG.modules.FirstOrDefault(x => x.Value == cEntry).Key}.");
            } else if (foundEntry && !foundColor) {
                ModLog.Warning($" > The Entry #{rEntry} you want overwrite exists, but with Color Code ({LOOTBAG.modules[rEntry].R}, {LOOTBAG.modules[rEntry].G}, {LOOTBAG.modules[rEntry].B}).");
            } else if (isPatchDebug) ModLog.Message($" > Adding Entry #{rEntry}, Color Code: {cEntry.R}, {cEntry.G}, {cEntry.B}.");
        }
    }
}
