using CoOpSpRpG;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace FFU_Tyrian_Legacy {
    public class FFU_TL_Tile_CargoBays {
        public static void updateModules(Dictionary<byte, Dictionary<byte, Dictionary<byte, Module>>> modules) {
            ModLog.Message($"Applying module changes: Cargo Bays...");
            modCargoBayT0S(modules, 144, 246, 22);
            modCargoBayT0S(modules, 144, 247, 22);
            modCargoBayT0S(modules, 144, 248, 22);
            modCargoBayT0S(modules, 144, 249, 22);
            modCargoBayT1S(modules, 145, 156, 22);
            modCargoBayT1S(modules, 145, 157, 22);
            modCargoBayT1S(modules, 145, 158, 22);
            modCargoBayT1S(modules, 145, 159, 22);
            modCargoBayT1S(modules, 145, 160, 22);
            modCargoBayT1S(modules, 145, 161, 22);
            modCargoBayT1S(modules, 145, 162, 22);
            modCargoBayT1S(modules, 145, 163, 22);
            modCargoBayT1L(modules, 145, 164, 22);
            modCargoBayT1L(modules, 145, 165, 22);
            modCargoBayT1L(modules, 145, 166, 22);
            modCargoBayT1L(modules, 145, 167, 22);
            modCargoBayT2S(modules, 144, 67, 22);
            modCargoBayT2S(modules, 144, 68, 22);
            modCargoBayT2S(modules, 144, 69, 22);
            modCargoBayT2S(modules, 144, 70, 22);
            modCargoBayT2L(modules, 144, 51, 22);
            modCargoBayT2L(modules, 144, 52, 22);
            modCargoBayT2L(modules, 144, 53, 22);
            modCargoBayT2L(modules, 144, 54, 22);
            modCargoBayT2X(modules, 144, 71, 22);
            modCargoBayT2X(modules, 144, 72, 22);
            modCargoBayT2X(modules, 144, 73, 22);
            modCargoBayT2X(modules, 144, 74, 22);
            modCargoBayT2X(modules, 144, 75, 22);
            modCargoBayT2X(modules, 144, 76, 22);
            modCargoBayT2X(modules, 144, 77, 22);
            modCargoBayT2X(modules, 144, 78, 22);
            modCargoBayT3X(modules, 145, 2, 26);
            modCargoBayT3X(modules, 145, 3, 26);
            modCargoBayT3X(modules, 145, 4, 26);
            modCargoBayT3X(modules, 145, 5, 26);
        }
        public static void updateResearch() {
            ModLog.Message($"Applying research changes: Cargo Bays...");
            modCargoBayT0S(400480U);
            modCargoBayT1S(400502U);
            modCargoBayT1L(400503U);
            modCargoBayT2S(400701U);
            modCargoBayT2L(400700U);
            modCargoBayT2X(400702U);
            modCargoBayT3X(400735U);
        }
        public static void modCargoBayT0S(Dictionary<byte, Dictionary<byte, Dictionary<byte, Module>>> modules, byte r, byte g, byte b) {
            FFU_TL_Defs.unlistDynamic.Add(new Color(r, g, b));
            FFU_TL_Defs.unlistDynamic = FFU_TL_Defs.unlistDynamic.ToList();
            modCargoBayT0S(modules[r][g][b] as CargoBay);
        }
        public static void modCargoBayT1S(Dictionary<byte, Dictionary<byte, Dictionary<byte, Module>>> modules, byte r, byte g, byte b) {
            FFU_TL_Defs.unlistDynamic.Add(new Color(r, g, b));
            FFU_TL_Defs.unlistDynamic = FFU_TL_Defs.unlistDynamic.ToList();
            modCargoBayT1S(modules[r][g][b] as CargoBay);
        }
        public static void modCargoBayT1L(Dictionary<byte, Dictionary<byte, Dictionary<byte, Module>>> modules, byte r, byte g, byte b) {
            FFU_TL_Defs.unlistDynamic.Add(new Color(r, g, b));
            FFU_TL_Defs.unlistDynamic = FFU_TL_Defs.unlistDynamic.ToList();
            modCargoBayT1L(modules[r][g][b] as CargoBay);
        }
        public static void modCargoBayT2S(Dictionary<byte, Dictionary<byte, Dictionary<byte, Module>>> modules, byte r, byte g, byte b) {
            FFU_TL_Defs.unlistDynamic.Add(new Color(r, g, b));
            FFU_TL_Defs.unlistDynamic = FFU_TL_Defs.unlistDynamic.ToList();
            modCargoBayT2S(modules[r][g][b] as CargoBay);
        }
        public static void modCargoBayT2L(Dictionary<byte, Dictionary<byte, Dictionary<byte, Module>>> modules, byte r, byte g, byte b) {
            FFU_TL_Defs.unlistDynamic.Add(new Color(r, g, b));
            FFU_TL_Defs.unlistDynamic = FFU_TL_Defs.unlistDynamic.ToList();
            modCargoBayT2L(modules[r][g][b] as CargoBay);
        }
        public static void modCargoBayT2X(Dictionary<byte, Dictionary<byte, Dictionary<byte, Module>>> modules, byte r, byte g, byte b) {
            FFU_TL_Defs.unlistDynamic.Add(new Color(r, g, b));
            FFU_TL_Defs.unlistDynamic = FFU_TL_Defs.unlistDynamic.ToList();
            modCargoBayT2X(modules[r][g][b] as CargoBay);
        }
        public static void modCargoBayT3X(Dictionary<byte, Dictionary<byte, Dictionary<byte, Module>>> modules, byte r, byte g, byte b) {
            FFU_TL_Defs.unlistDynamic.Add(new Color(r, g, b));
            FFU_TL_Defs.unlistDynamic = FFU_TL_Defs.unlistDynamic.ToList();
            modCargoBayT3X(modules[r][g][b] as CargoBay);
        }
        public static void modCargoBayT0S(CargoBay cBay) {
            cBay.cost = 440;
            cBay.toolTip = "Cargo Bay C-4";
            cBay.storage = new Storage(4, 2, 2);
            cBay.techLevel = 2;
            cBay.tip = new ToolTip();
            cBay.tip.tip = cBay.toolTip;
            cBay.tip.botLeftText = $"Transtellar Unlimited";
            cBay.tip.description = "A hallway segment without much use can double as a bit of a storage space.";
            cBay.tip.tierIcontype = (TierIcon)cBay.techLevel;
            cBay.tip.addStat($"Storage Capacity", $"{cBay.storage.slotCount}", false);
        }
        public static void modCargoBayT1S(CargoBay cBay) {
            cBay.cost = 1650;
            cBay.toolTip = "Cargo Bay C-15";
            cBay.storage = new Storage(15, 5, 3);
            cBay.techLevel = 1;
            cBay.tip = new ToolTip();
            cBay.tip.tip = cBay.toolTip;
            cBay.tip.botLeftText = $"Transtellar Classic";
            cBay.tip.description = "A standard issue cargo bay with an external port that allows to transfer cargo through external port.";
            cBay.tip.tierIcontype = (TierIcon)cBay.techLevel;
            cBay.tip.addStat($"Storage Capacity", $"{cBay.storage.slotCount}", false);
        }
        public static void modCargoBayT1L(CargoBay cBay) {
            cBay.cost = 2640;
            cBay.toolTip = "Cargo Bay C-24";
            cBay.storage = new Storage(24, 8, 3);
            cBay.techLevel = 1;
            cBay.tip = new ToolTip();
            cBay.tip.tip = cBay.toolTip;
            cBay.tip.botLeftText = $"Transtellar Classic";
            cBay.tip.description = "Improved variant of standard issue cargo bay. Has increased storage capacity at expense of increased module size. External cargo transfer port is carefully hidden to ensure that storage capacity isn't affected.";
            cBay.tip.tierIcontype = (TierIcon)cBay.techLevel;
            cBay.tip.addStat($"Storage Capacity", $"{cBay.storage.slotCount}", false);
        }
        public static void modCargoBayT2S(CargoBay cBay) {
            cBay.cost = 2160;
            cBay.toolTip = "Cargo Bay C-18";
            cBay.storage = new Storage(18, 6, 3);
            cBay.techLevel = 2;
            cBay.tip = new ToolTip();
            cBay.tip.tip = cBay.toolTip;
            cBay.tip.botLeftText = $"Transtellar Unlimited";
            cBay.tip.description = "A military issue cargo bay that mostly used by modern combat vessels. External cargo transfer port was hardened and carefully integrated into modern design.";
            cBay.tip.tierIcontype = (TierIcon)cBay.techLevel;
            cBay.tip.addStat($"Storage Capacity", $"{cBay.storage.slotCount}", false);
        }
        public static void modCargoBayT2L(CargoBay cBay) {
            cBay.cost = 6480;
            cBay.toolTip = "Cargo Bay C-54";
            cBay.storage = new Storage(54, 9, 6);
            cBay.techLevel = 2;
            cBay.tip = new ToolTip();
            cBay.tip.tip = cBay.toolTip;
            cBay.tip.botLeftText = $"Transtellar Unlimited";
            cBay.tip.description = "A military issue cargo bay with extended capacity. Mostly used by modern combat cruisers and bigger ships. External cargo transfer port is protected by composite airlock.";
            cBay.tip.tierIcontype = (TierIcon)cBay.techLevel;
            cBay.tip.addStat($"Storage Capacity", $"{cBay.storage.slotCount}", false);
        }
        public static void modCargoBayT2X(CargoBay cBay) {
            cBay.cost = 9000;
            cBay.toolTip = "Cargo Bay C-72";
            cBay.storage = new Storage(72, 9, 8);
            cBay.techLevel = 3;
            cBay.tip = new ToolTip();
            cBay.tip.tip = cBay.toolTip;
            cBay.tip.botLeftText = $"Transtellar Unlimited";
            cBay.tip.description = "An advanced, freight-specialized cargo bay that mostly used by armored, long-range freighters and other specialized logistic ships. Has dual external cargo transfer port for accelerated load and unload.";
            cBay.tip.tierIcontype = (TierIcon)cBay.techLevel;
            cBay.tip.addStat($"Storage Capacity", $"{cBay.storage.slotCount}", false);
        }
        public static void modCargoBayT3X(CargoBay cBay) {
            cBay.cost = 14400;
            cBay.toolTip = "Cargo Bay C-96";
            cBay.storage = new Storage(96, 12, 8);
            cBay.miningBonus = 0.05f;
            cBay.techLevel = 4;
            cBay.tip = new ToolTip();
            cBay.tip.tip = cBay.toolTip;
            cBay.tip.botLeftText = $"Goliath Industries";
            cBay.tip.description = "A huge cargo bay with impressive storage capacity. Has built-in ore sorting and extraction facility that increases yield by 5% for all mining operations. Mostly used by specialized mining ships.";
            cBay.tip.tierIcontype = (TierIcon)cBay.techLevel;
            cBay.tip.addStat($"Storage Capacity", $"{cBay.storage.slotCount}", false);
            cBay.tip.addStat($"Mining Bonus", $"{cBay.miningBonus * 100f}%", false);
        }
        public static void modCargoBayT0S(uint rEntry) {
            LOOTBAG.researchCosts[rEntry] = 2200;
            LOOTBAG.researchTimes[rEntry] = 1.32f;
            LOOTBAG.exclusive[rEntry] = true;
        }
        public static void modCargoBayT1S(uint rEntry) {
            LOOTBAG.researchCosts[rEntry] = 8250;
            LOOTBAG.researchTimes[rEntry] = 4.95f;
            LOOTBAG.exclusive[rEntry] = true;
        }
        public static void modCargoBayT1L(uint rEntry) {
            LOOTBAG.researchCosts[rEntry] = 13200;
            LOOTBAG.researchTimes[rEntry] = 7.92f;
            LOOTBAG.exclusive[rEntry] = true;
            LOOTBAG.researchCategories[13].Add(rEntry);
        }
        public static void modCargoBayT2S(uint rEntry) {
            LOOTBAG.researchCosts[rEntry] = 10800;
            LOOTBAG.researchTimes[rEntry] = 6.48f;
            LOOTBAG.exclusive[rEntry] = true;
        }
        public static void modCargoBayT2L(uint rEntry) {
            LOOTBAG.researchCosts[rEntry] = 32400;
            LOOTBAG.researchTimes[rEntry] = 19.44f;
            LOOTBAG.exclusive[rEntry] = true;
        }
        public static void modCargoBayT2X(uint rEntry) {
            LOOTBAG.researchCosts[rEntry] = 45000;
            LOOTBAG.researchTimes[rEntry] = 27f;
            LOOTBAG.exclusive[rEntry] = true;
            LOOTBAG.researchCategories[(int)DataCategory.loot_LowChanceAdv].Remove(rEntry);
            LOOTBAG.researchCategories[(int)DataCategory.loot_PhaseTwo].Remove(rEntry);
            LOOTBAG.researchCategories[(int)DataCategory.loot_TierTwoPlus].Add(rEntry);
        }
        public static void modCargoBayT3X(uint rEntry) {
            FFU_TL_Defs.checkExistingResearch(rEntry);
            FFU_TL_Defs.checkResearchDupe(new Color(145, 2, 26));
            LOOTBAG.modules[rEntry] = new Color(145, 2, 26);
            LOOTBAG.researchCosts[rEntry] = 72000;
            LOOTBAG.researchTimes[rEntry] = 43.2f;
            LOOTBAG.exclusive[rEntry] = true;
            LOOTBAG.tier[rEntry] = 4;
            LOOTBAG.researchCategories[(int)DataCategory.rewards_Hitchhiker].Add(rEntry);
            LOOTBAG.researchCategories[(int)DataCategory.loot_TierTwoPlus].Add(rEntry);
            LOOTBAG.researchCategories[(int)DataCategory.loot_TierThree].Add(rEntry);
            LOOTBAG.researchType[rEntry] = ResearchType.module;
        }
    }
}