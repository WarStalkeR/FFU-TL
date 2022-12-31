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
        public static void dumpExistingModules(Dictionary<byte, Dictionary<byte, Dictionary<byte, Module>>> modules, string dumpFile = "FFU_TL_Modules.txt") {
            ModLog.Warning($"Dumping all modules into the {dumpFile}");
            Support.ValidateDirPath(FFU_TL_Defs.exeFilePath + FFU_TL_Defs.modDumpsDir);
            TextWriter ioDump = new StreamWriter(FFU_TL_Defs.exeFilePath + FFU_TL_Defs.modDumpsDir + dumpFile);
            foreach (byte r in modules.Keys) 
                foreach (byte g in modules[r].Keys) 
                    foreach (byte b in modules[r][g].Keys) {
                ioDump.WriteLine($"Key: [{r}][{g}][{b}]/({r}, {g}, {b}), " +
                $"Type: {modules[r][g][b]?.type!}, Name: {modules[r][g][b]?.tip?.tip?.Replace("\n", " ")}, " +
                $"ToolTip: {modules[r][g][b]?.toolTip?.Replace("\n"," ")}, " +
                $"Description: {modules[r][g][b]?.tip?.description?.Replace("\n", " ")}" +
                $"");
            }
            ioDump.Close();
        }
        public static void updateToolTipCosts(Dictionary<byte, Dictionary<byte, Dictionary<byte, Module>>> modules) {
            ModLog.Message($"Updating price format for all modules.");
            foreach (byte r in modules.Keys)
                foreach (byte g in modules[r].Keys)
                    foreach (byte b in modules[r][g].Keys) {
                if (modules[r][g][b].tip != null) modules[r][g][b].tip.cost = string.Format("{0:n0}", modules[r][g][b].cost);
            }
        }
        public static void updateResourceCosts(Dictionary<Module, Dictionary<InventoryItemType, float>> res, Dictionary<Module, Dictionary<InventoryItemType, float>> extra, Dictionary<byte, Dictionary<byte, Dictionary<byte, Module>>> modules, List<Color> colorsList) {
            ModLog.Message($"Updating resource costs for altered modules.");
            foreach (byte r in modules.Keys)
                foreach (byte g in modules[r].Keys)
                    foreach (byte b in modules[r][g].Keys) {
                if (modules[r][g][b].techLevel == 25) {
                    modules[r][g][b].techLevel = 3;
                    cleanModuleResList(modules[r][g][b]);
                    if (extra.ContainsKey(modules[r][g][b])) extra.Remove(modules[r][g][b]);
                    if (res.ContainsKey(modules[r][g][b])) res.Remove(modules[r][g][b]);
                    TILEBAG.AssignResources(modules[r][g][b]);
                }
                if (colorsList.Contains(new Color(r, g, b))) {
                    if (extra.ContainsKey(modules[r][g][b])) extra.Remove(modules[r][g][b]);
                    if (res.ContainsKey(modules[r][g][b])) res.Remove(modules[r][g][b]);
                    TILEBAG.AssignResources(modules[r][g][b]);
                }
                if (modules[r][g][b].techLevel == 4) {
                    cleanModuleResList(modules[r][g][b]);
                    int moduleTiles = modules[r][g][b].tiles.Count();
                    if (extra.ContainsKey(modules[r][g][b])) extra.Remove(modules[r][g][b]);
                    if (res.ContainsKey(modules[r][g][b])) res.Remove(modules[r][g][b]);
                    extra.Add(modules[r][g][b], new Dictionary<InventoryItemType, float>() {
                    { InventoryItemType.gold_ore, moduleTiles * 1f },
                    { InventoryItemType.titanium_ore, moduleTiles * 2f },
                    { InventoryItemType.rhodium_ore, moduleTiles * 0.4f },
                    { InventoryItemType.mitraxit_ore, moduleTiles * 0.15f },
                    { InventoryItemType.ithacit_ore, moduleTiles * 0.05f }
                });
                    TILEBAG.AssignResources(modules[r][g][b]);
                }
            }
            FFU_TL_Tile_UtilityBays.assignCustomCosts(modules, res, extra);
            FFU_TL_Tile_CloningVats.assignCustomCosts(modules, res, extra);
            FFU_TL_Tile_Controllers.assignCustomCosts(modules, res, extra);
        }
        public static void updateBlacklist(List<Color> mainList, List<Color> refList, string listName = "") {
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
            FFU_TL_Modules.dumpExistingModules(modules, "FFU_TL_Modules_Original.txt");
            FFU_TL_Tile_CargoBays.updateModules(modules);
            FFU_TL_Tile_UtilityBays.updateModules(modules);
            FFU_TL_Tile_MagRails.updateModules(modules);
            FFU_TL_Tile_Logistics.updateModules(modules);
            FFU_TL_Tile_Hallways.updateModules(modules);
            FFU_TL_Tile_CrewRooms.updateModules(modules);
            FFU_TL_Tile_CloningVats.updateModules(modules);
            FFU_TL_Tile_Taverns.updateModules(modules);
            FFU_TL_Tile_Controllers.updateModules(modules);
            FFU_TL_Tile_Missiles.updateModules(modules);
            FFU_TL_Modules.updateToolTipCosts(modules);
            FFU_TL_Modules.updateResourceCosts(moduleResources, moduleExtraResources, modules, FFU_TL_Defs.unlistDynamic);
            FFU_TL_Modules.updateBlacklist(oldMods, FFU_TL_Defs.unlistDynamic, "unobtainable module list");
            FFU_TL_Modules.updateBlacklist(unlockableModsBlacklist, FFU_TL_Defs.unlistStatic, "non-removable module list");
            FFU_TL_Modules.dumpExistingModules(modules, "FFU_TL_Modules_Modded.txt");
            FFU_TL_Defs.refModules = modules;
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
    }
}