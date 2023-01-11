#pragma warning disable CS0169
#pragma warning disable CS0626
#pragma warning disable CS0649

using MonoMod;
using CoOpSpRpG;
using FFU_Terra_Liberatio;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FFU_Terra_Liberatio {
    public class FFU_TL_Modules {
        public static void dumpExistingModules(string dumpFile = "FFU_TL_Modules.txt") {
            ModLog.Warning($"Dumping all modules into the {dumpFile}");
            Support.ValidateDirPath(FFU_TL_Defs.exeFilePath + FFU_TL_Defs.modDumpsDir);
            TextWriter ioDump = new StreamWriter(FFU_TL_Defs.exeFilePath + FFU_TL_Defs.modDumpsDir + dumpFile);
            foreach (byte r in FFU_TL_Defs.rMod.Keys) 
                foreach (byte g in FFU_TL_Defs.rMod[r].Keys) 
                    foreach (byte b in FFU_TL_Defs.rMod[r][g].Keys) {
                ioDump.WriteLine($"Key: [{r}][{g}][{b}]/({r}, {g}, {b}), " +
                $"Type: {FFU_TL_Defs.rMod[r][g][b]?.type!}, Name: {FFU_TL_Defs.rMod[r][g][b]?.tip?.tip?.Replace("\n", " ")}, " +
                $"ToolTip: {FFU_TL_Defs.rMod[r][g][b]?.toolTip?.Replace("\n"," ")}, " +
                $"Description: {FFU_TL_Defs.rMod[r][g][b]?.tip?.description?.Replace("\n", " ")}" +
                $"");
            }
            ioDump.Close();
        }
        public static void updateToolTipCosts() {
            ModLog.Message($"Updating price format for all modules.");
            foreach (byte r in FFU_TL_Defs.rMod.Keys)
                foreach (byte g in FFU_TL_Defs.rMod[r].Keys)
                    foreach (byte b in FFU_TL_Defs.rMod[r][g].Keys) {
                if (FFU_TL_Defs.rMod[r][g][b].tip != null) FFU_TL_Defs.rMod[r][g][b].tip.cost = string.Format("{0:n0}", FFU_TL_Defs.rMod[r][g][b].cost);
            }
        }
        public static void updateResourceCosts() {
            ModLog.Message($"Updating resource costs for altered modules.");
            foreach (byte r in FFU_TL_Defs.rMod.Keys)
                foreach (byte g in FFU_TL_Defs.rMod[r].Keys)
                    foreach (byte b in FFU_TL_Defs.rMod[r][g].Keys) {
                if (FFU_TL_Defs.rMod[r][g][b].techLevel == 25) {
                            FFU_TL_Defs.rMod[r][g][b].techLevel = 3;
                    cleanModuleResList(FFU_TL_Defs.rMod[r][g][b]);
                    if (FFU_TL_Defs.rExt.ContainsKey(FFU_TL_Defs.rMod[r][g][b])) FFU_TL_Defs.rExt.Remove(FFU_TL_Defs.rMod[r][g][b]);
                    if (FFU_TL_Defs.rRes.ContainsKey(FFU_TL_Defs.rMod[r][g][b])) FFU_TL_Defs.rRes.Remove(FFU_TL_Defs.rMod[r][g][b]);
                    TILEBAG.AssignResources(FFU_TL_Defs.rMod[r][g][b]);
                }
                if (FFU_TL_Defs.unlistDynamic.Contains(new Color(r, g, b))) {
                    if (FFU_TL_Defs.rExt.ContainsKey(FFU_TL_Defs.rMod[r][g][b])) FFU_TL_Defs.rExt.Remove(FFU_TL_Defs.rMod[r][g][b]);
                    if (FFU_TL_Defs.rRes.ContainsKey(FFU_TL_Defs.rMod[r][g][b])) FFU_TL_Defs.rRes.Remove(FFU_TL_Defs.rMod[r][g][b]);
                    TILEBAG.AssignResources(FFU_TL_Defs.rMod[r][g][b]);
                }
                if (FFU_TL_Defs.rMod[r][g][b].techLevel == 4) {
                    cleanModuleResList(FFU_TL_Defs.rMod[r][g][b]);
                    int moduleTiles = FFU_TL_Defs.rMod[r][g][b].tiles.Count();
                    if (FFU_TL_Defs.rExt.ContainsKey(FFU_TL_Defs.rMod[r][g][b])) FFU_TL_Defs.rExt.Remove(FFU_TL_Defs.rMod[r][g][b]);
                    if (FFU_TL_Defs.rRes.ContainsKey(FFU_TL_Defs.rMod[r][g][b])) FFU_TL_Defs.rRes.Remove(FFU_TL_Defs.rMod[r][g][b]);
                    FFU_TL_Defs.rExt.Add(FFU_TL_Defs.rMod[r][g][b], new Dictionary<InventoryItemType, float>() {
                        { InventoryItemType.gold_ore, moduleTiles * 1f },
                        { InventoryItemType.titanium_ore, moduleTiles * 2f },
                        { InventoryItemType.rhodium_ore, moduleTiles * 0.4f },
                        { InventoryItemType.mitraxit_ore, moduleTiles * 0.15f },
                        { InventoryItemType.ithacit_ore, moduleTiles * 0.05f }
                    });
                    TILEBAG.AssignResources(FFU_TL_Defs.rMod[r][g][b]);
                }
            }
            FFU_TL_Tile_UtilityBays.assignCustomCosts();
            FFU_TL_Tile_CloningVats.assignCustomCosts();
            FFU_TL_Tile_Controllers.assignCustomCosts();
        }
        public static void updateBlacklist(ref List<Color> mainList, List<Color> refList, string listName = "") {
            ModLog.Message($"Updating {(string.IsNullOrEmpty(listName) ? $"the list" : $"{listName}")} according to the changes.");
            var tempList = mainList;
            foreach (Color cItem in refList) {
                if (tempList.Contains(cItem)) tempList.Remove(cItem);
            }
            mainList = tempList.ToList();
        }
        public static void cleanModuleResList(Module refModule) {
            if (refModule.tip == null) return;
            for (int i = 0; i < refModule.tip.elements.Count(); i++) {
                if (refModule.tip.elements[i] is TipStatResources) refModule.tip.elements.RemoveAt(i);
            }
        }
        public static Dictionary<InventoryItemType, float> makeModuleResList(int tiles, 
            float iron, float gold, float titanium, float rhodium, float mitraxit, float ithacit, 
            float lead, float troilite, float silicate, float cliftonite, float ilmenite) {
            var resList = new Dictionary<InventoryItemType, float>();
            // Can be stashed into infinite storage.
            if (iron > 0) resList.Add(InventoryItemType.iron_ore, tiles * iron);
            if (gold > 0) resList.Add(InventoryItemType.gold_ore, tiles * gold);
            if (titanium > 0) resList.Add(InventoryItemType.titanium_ore, tiles * titanium);
            if (rhodium > 0) resList.Add(InventoryItemType.rhodium_ore, tiles * rhodium);
            if (mitraxit > 0) resList.Add(InventoryItemType.mitraxit_ore, tiles * mitraxit);
            if (ithacit > 0) resList.Add(InventoryItemType.ithacit_ore, tiles * ithacit);
            // Can't be stashed into infinite storage.
            if (lead > 0) resList.Add(InventoryItemType.lead_ore, tiles * lead);
            if (troilite > 0) resList.Add(InventoryItemType.troilite_ore, tiles * troilite);
            if (silicate > 0) resList.Add(InventoryItemType.silicate_ore, tiles * silicate);
            if (cliftonite > 0) resList.Add(InventoryItemType.cliftonite_ore, tiles * cliftonite);
            if (ilmenite > 0) resList.Add(InventoryItemType.ilmenite_ore, tiles * ilmenite);
            return resList;
        }
        public static void applyModuleResList(byte r, byte g, byte b,
            float iron = 0, float gold = 0, float titanium = 0, float rhodium = 0, float mitraxit = 0, float ithacit = 0,
            float lead = 0, float troilite = 0, float silicate = 0, float cliftonite = 0, float ilmenite = 0) {
            cleanModuleResList(FFU_TL_Defs.rMod[r][g][b]);
            if (FFU_TL_Defs.rRes.ContainsKey(FFU_TL_Defs.rMod[r][g][b])) FFU_TL_Defs.rRes.Remove(FFU_TL_Defs.rMod[r][g][b]);
            if (FFU_TL_Defs.rExt.ContainsKey(FFU_TL_Defs.rMod[r][g][b])) FFU_TL_Defs.rExt.Remove(FFU_TL_Defs.rMod[r][g][b]);
            FFU_TL_Defs.rExt.Add(FFU_TL_Defs.rMod[r][g][b], makeModuleResList(FFU_TL_Defs.rMod[r][g][b].tiles.Count(), iron, gold, titanium, rhodium, mitraxit, ithacit, 0, 0, 0, 0, 0));
            patch_TILEBAG.SafeAssignResources(FFU_TL_Defs.rMod[r][g][b], FFU_TL_Defs.rExt[FFU_TL_Defs.rMod[r][g][b]]);
        }
    }
}

namespace CoOpSpRpG {
    public static class patch_TILEBAG {
        [MonoModIgnore] private static List<Color> oldMods;
        [MonoModIgnore] private static List<Color> unlockableModsBlacklist;
        [MonoModIgnore] private static Dictionary<InventoryItemType, float> tempResources = new Dictionary<InventoryItemType, float>();
        [MonoModIgnore] private static Dictionary<byte, Dictionary<byte, Dictionary<byte, Module>>> modules = new Dictionary<byte, Dictionary<byte, Dictionary<byte, Module>>>();
        [MonoModIgnore] private static Dictionary<Module, Dictionary<InventoryItemType, float>> moduleResources = new Dictionary<Module, Dictionary<InventoryItemType, float>>();
        [MonoModIgnore] private static Dictionary<Module, Dictionary<InventoryItemType, float>> moduleExtraResources = new Dictionary<Module, Dictionary<InventoryItemType, float>>();
        [MonoModIgnore] private static Color[][][] getModData(string append) { return null; }
        [MonoModIgnore] private static void deprecate(Color c) { }
        public static extern void orig_init();
        public static void init() {
        /// Apply module changes after loading original files.
            orig_init();
            if (FFU_TL_Defs.doDataDump) FFU_TL_Modules.dumpExistingModules("FFU_TL_Modules_Original.txt");
            FFU_TL_Tile_CargoBays.updateModules();
            FFU_TL_Tile_UtilityBays.updateModules();
            FFU_TL_Tile_MagRails.updateModules();
            FFU_TL_Tile_Logistics.updateModules();
            FFU_TL_Tile_Hallways.updateModules();
            FFU_TL_Tile_CrewRooms.updateModules();
            FFU_TL_Tile_CloningVats.updateModules();
            FFU_TL_Tile_Taverns.updateModules();
            FFU_TL_Tile_Controllers.updateModules();
            FFU_TL_Tile_Missiles.updateModules();
            FFU_TL_Modules.updateToolTipCosts();
            FFU_TL_Modules.updateResourceCosts();
            FFU_TL_Modules.updateBlacklist(ref oldMods, FFU_TL_Defs.unlistDynamic, "unobtainable module list");
            FFU_TL_Modules.updateBlacklist(ref unlockableModsBlacklist, FFU_TL_Defs.unlistStatic, "non-removable module list");
            if (FFU_TL_Defs.doDataDump) FFU_TL_Modules.dumpExistingModules("FFU_TL_Modules_Modded.txt");
        }
        public static Color[][][] getModDataRef(string append) { 
            return getModData(append);
        }
        public static void refDeprecate(Color c) {
            deprecate(c);
        }
        public static void SafeAssignResources(Module refModule, Dictionary<InventoryItemType, float> refResources) {
            if (moduleResources.ContainsKey(refModule)) moduleResources.Remove(refModule);
            if (refModule.tip != null && refResources.Count != 0) refModule.tip.addResources(refResources);
            moduleResources.Add(refModule, refResources);
        }
        public static ref Dictionary<byte, Dictionary<byte, Dictionary<byte, Module>>> refModules() {
            return ref modules;
        }
        public static ref Dictionary<Module, Dictionary<InventoryItemType, float>> refResources() {
            return ref moduleResources;
        }
        public static ref Dictionary<Module, Dictionary<InventoryItemType, float>> refExtraResources() {
            return ref moduleExtraResources;
        }
    }
}