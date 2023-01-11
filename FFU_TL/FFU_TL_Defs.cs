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
        public static ref Dictionary<byte, Dictionary<byte, Dictionary<byte, Module>>> rMod => ref patch_TILEBAG.refModules();
        public static ref Dictionary<Module, Dictionary<InventoryItemType, float>> rRes => ref patch_TILEBAG.refResources();
        public static ref Dictionary<Module, Dictionary<InventoryItemType, float>> rExt => ref patch_TILEBAG.refExtraResources();
        public static Dictionary<ulong, Ship> reviveShips = new Dictionary<ulong, Ship>();
        public static ulong reviveShipID = 0;
        public static Module refModuleIfExists(Color cEntry) {
            return refModuleIfExists(cEntry.R, cEntry.G, cEntry.B);
        }
        public static Module refModuleIfExists(byte r, byte g, byte b) {
            if (rMod.ContainsKey(r) && rMod[r].ContainsKey(g) && rMod[r][g].ContainsKey(b)) 
                return rMod[r][g][b];
            else return null;
        }
        public static void checkModifiedEntry(uint rEntry, Color cEntry) {
            bool foundEntry = LOOTBAG.modules.ContainsKey(rEntry);
            bool foundColor = LOOTBAG.modules.Count(x => x.Value == cEntry) > 0;
            Color fEntry = foundEntry ? LOOTBAG.modules[rEntry] : new Color(255, 255, 255);
            Module foundModule = refModuleIfExists(cEntry);
            if (foundModule != null) {
                string moduleName = foundModule.tip?.tip?.Replace("\n", " ");
                if (string.IsNullOrEmpty(moduleName)) moduleName = foundModule.toolTip?.Replace("\n", " ");
                if (string.IsNullOrEmpty(moduleName)) moduleName = "MISSING_NAME";
                if (foundEntry && foundColor) {
                    if (fEntry != cEntry) {
                        ModLog.Warning($" > The Research Entry #{rEntry} you want overwrite exists, but for [{moduleName}] Module ({fEntry.R}, {fEntry.G}, {fEntry.B}).");
                        ModLog.Warning($" > The [{moduleName}] Module ({cEntry.R}, {cEntry.G}, {cEntry.B}) you want to add already exists, but in Research Entry #{LOOTBAG.modules.FirstOrDefault(x => x.Value == cEntry).Key}.");
                    } else if (isPatchDebug) ModLog.Message($" > Overwriting Research Entry #{LOOTBAG.modules.FirstOrDefault(x => x.Value == cEntry).Key}, [{moduleName}] Module: {cEntry.R}, {cEntry.G}, {cEntry.B}.");
                } else if (!foundEntry && foundColor) {
                    ModLog.Warning($" > The [{moduleName}] Module ({cEntry.R}, {cEntry.G}, {cEntry.B}) you want to add already exists, but in Research Entry #{LOOTBAG.modules.FirstOrDefault(x => x.Value == cEntry).Key}.");
                } else if (foundEntry && !foundColor) {
                    ModLog.Warning($" > The Research Entry #{rEntry} you want overwrite exists, but with [{moduleName}] Module ({fEntry.R}, {fEntry.G}, {fEntry.B}).");
                } else if (isPatchDebug) ModLog.Message($" > Adding Entry #{rEntry}, Color Code: {cEntry.R}, {cEntry.G}, {cEntry.B}.");
            } else ModLog.Error($" > The Module with Color Code ({cEntry.R}, {cEntry.G}, {cEntry.B}) you want to make researchable isn't implemented!");
        }
    }
}
