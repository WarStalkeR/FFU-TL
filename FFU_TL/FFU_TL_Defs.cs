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
        public static readonly string modVersion = "0.0.1.2";
        public static readonly bool isDebug = true;
        public static bool secretStash = false;
        public static List<Color> unlistStatic = new List<Color>(new Color[] {
            new Color(145, 117, 24) //Sinidal Cascade
        });
        public static List<Color> unlistDynamic = new List<Color>();
        public static List<uint> startingTechIDs = new List<uint>();
        public static Dictionary<byte, Dictionary<byte, Dictionary<byte, CoOpSpRpG.Module>>> refModules = new Dictionary<byte, Dictionary<byte, Dictionary<byte, CoOpSpRpG.Module>>>();
        public static Dictionary<ulong, Ship> reviveShips = new Dictionary<ulong, Ship>();
        public static ulong reviveShipID = 0;
        public static void checkExistingResearch(uint rEntry) {
            if (LOOTBAG.modules.ContainsKey(rEntry)) ModLog.Warning($"Warning! You're overwriting research entry of an existing module: {rEntry}");
        }
        public static void checkResearchDupe(Color cEntry) {
            if (LOOTBAG.modules.ContainsValue(cEntry)) ModLog.Warning($"Warning! You're adding duplicate module research for entry: {LOOTBAG.modules.FirstOrDefault(x => x.Value == cEntry).Key}");
        }
    }
}
