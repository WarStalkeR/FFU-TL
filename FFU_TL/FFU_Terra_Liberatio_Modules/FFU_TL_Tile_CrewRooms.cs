using CoOpSpRpG;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace FFU_Terra_Liberatio {
    public class FFU_TL_Tile_CrewRooms {
        public static void updateModules(Dictionary<byte, Dictionary<byte, Dictionary<byte, Module>>> modules) {
            ModLog.Message($"Applying module changes: Barracks & Cabins...");
            modCabinT1S(modules, 145, 168, 22);
            modCabinT1S(modules, 145, 169, 22);
            modCabinT1S(modules, 145, 170, 22);
            modCabinT1S(modules, 145, 171, 22);
            modCabinT1S(modules, 145, 172, 22);
            modCabinT1S(modules, 145, 173, 22);
            modCabinT1S(modules, 145, 174, 22);
            modCabinT1S(modules, 145, 175, 22);
            modCabinT1M(modules, 145, 176, 22);
            modCabinT1M(modules, 145, 177, 22);
            modCabinT1M(modules, 145, 178, 22);
            modCabinT1M(modules, 145, 179, 22);
            modCabinT2S(modules, 144, 79, 22);
            modCabinT2S(modules, 144, 80, 22);
            modCabinT2S(modules, 144, 81, 22);
            modCabinT2S(modules, 144, 82, 22);
            modCabinT2S(modules, 144, 106, 22);
            modCabinT2S(modules, 144, 107, 22);
            modCabinT2S(modules, 144, 108, 22);
            modCabinT2S(modules, 144, 109, 22);
            modCabinT2M(modules, 144, 59, 22);
            modCabinT2M(modules, 144, 60, 22);
            modCabinT2M(modules, 144, 61, 22);
            modCabinT2M(modules, 144, 62, 22);
            modCabinT2M(modules, 144, 63, 22);
            modCabinT2M(modules, 144, 64, 22);
            modCabinT2M(modules, 144, 65, 22);
            modCabinT2M(modules, 144, 66, 22);
            modCabinT2L(modules, 144, 55, 22);
            modCabinT2L(modules, 144, 56, 22);
            modCabinT2L(modules, 144, 57, 22);
            modCabinT2L(modules, 144, 58, 22);
            modCabinT3M(modules, 146, 24, 22);
            modCabinT3M(modules, 146, 25, 22);
            modCabinT3M(modules, 146, 26, 22);
            modCabinT3M(modules, 146, 27, 22);
            modCabinT3M(modules, 146, 28, 22);
            modCabinT3M(modules, 146, 29, 22);
            modCabinT3M(modules, 146, 30, 22);
            modCabinT3M(modules, 146, 31, 22);
            modCabinT3L(modules, 146, 32, 22);
            modCabinT3L(modules, 146, 33, 22);
            modCabinT3L(modules, 146, 34, 22);
            modCabinT3L(modules, 146, 35, 22);
            modCabinT3L(modules, 146, 36, 22);
            modCabinT3L(modules, 146, 37, 22);
            modCabinT3L(modules, 146, 38, 22);
            modCabinT3L(modules, 146, 39, 22);
        }
        public static void updateResearch() {
            ModLog.Message($"Applying research changes: Barracks & Cabins...");
            modCabinT1S(400309U);
            modCabinT1M(400310U);
            modCabinT2S(400609U);
            modCabinT2M(400608U);
            modCabinT2L(400628U);
            modCabinT3M(400629U);
            modCabinT3L(400630U);
        }
        public static void modCabinT1S(Dictionary<byte, Dictionary<byte, Dictionary<byte, Module>>> modules, byte r, byte g, byte b) {
            FFU_TL_Defs.unlistDynamic.Add(new Color(r, g, b));
            FFU_TL_Defs.unlistDynamic = FFU_TL_Defs.unlistDynamic.ToList();
            modCabinT1S(modules[r][g][b] as Bedroom);
        }
        public static void modCabinT1M(Dictionary<byte, Dictionary<byte, Dictionary<byte, Module>>> modules, byte r, byte g, byte b) {
            FFU_TL_Defs.unlistDynamic.Add(new Color(r, g, b));
            FFU_TL_Defs.unlistDynamic = FFU_TL_Defs.unlistDynamic.ToList();
            modCabinT1M(modules[r][g][b] as Bedroom);
        }
        public static void modCabinT2S(Dictionary<byte, Dictionary<byte, Dictionary<byte, Module>>> modules, byte r, byte g, byte b) {
            FFU_TL_Defs.unlistDynamic.Add(new Color(r, g, b));
            FFU_TL_Defs.unlistDynamic = FFU_TL_Defs.unlistDynamic.ToList();
            modCabinT2S(modules[r][g][b] as Bedroom);
        }
        public static void modCabinT2M(Dictionary<byte, Dictionary<byte, Dictionary<byte, Module>>> modules, byte r, byte g, byte b) {
            FFU_TL_Defs.unlistDynamic.Add(new Color(r, g, b));
            FFU_TL_Defs.unlistDynamic = FFU_TL_Defs.unlistDynamic.ToList();
            modCabinT2M(modules[r][g][b] as Bedroom);
        }
        public static void modCabinT2L(Dictionary<byte, Dictionary<byte, Dictionary<byte, Module>>> modules, byte r, byte g, byte b) {
            FFU_TL_Defs.unlistDynamic.Add(new Color(r, g, b));
            FFU_TL_Defs.unlistDynamic = FFU_TL_Defs.unlistDynamic.ToList();
            modCabinT2L(modules[r][g][b] as Bedroom);
        }
        public static void modCabinT3M(Dictionary<byte, Dictionary<byte, Dictionary<byte, Module>>> modules, byte r, byte g, byte b) {
            FFU_TL_Defs.unlistDynamic.Add(new Color(r, g, b));
            FFU_TL_Defs.unlistDynamic = FFU_TL_Defs.unlistDynamic.ToList();
            modCabinT3M(modules[r][g][b] as Bedroom);
        }
        public static void modCabinT3L(Dictionary<byte, Dictionary<byte, Dictionary<byte, Module>>> modules, byte r, byte g, byte b) {
            FFU_TL_Defs.unlistDynamic.Add(new Color(r, g, b));
            FFU_TL_Defs.unlistDynamic = FFU_TL_Defs.unlistDynamic.ToList();
            modCabinT3L(modules[r][g][b] as Bedroom);
        }
        public static void modCabinT1S(Bedroom bRoom) {
            bRoom.cost = 400;
            bRoom.toolTip = "Private Cabin";
            bRoom.techLevel = 1;
            bRoom.beds = 1;
            bRoom.quality = 25;
            bRoom.tip = new ToolTip();
            bRoom.tip.tip = bRoom.toolTip;
            bRoom.tip.botLeftText = $"Elon Interstellar Classic";
            bRoom.tip.description = "A private cabin of centuries old design from dawn of space exploration era, when only richest could afford to own a spaceship. Warm and cozy. Has built-in old gaming console.";
            bRoom.tip.tierIcontype = (TierIcon)bRoom.techLevel;
            bRoom.tip.addStat("No. of Beds", $"{bRoom.beds}", false);
            bRoom.tip.addStat("Suite Quality", $"{bRoom.quality}", false);
            bRoom.tip.addStat("Console Type", "SonyBOX 16K", false);
            bRoom.tip.addStat("Console Performance", "30 FPS", false);
        }
        public static void modCabinT1M(Bedroom bRoom) {
            bRoom.cost = 700;
            bRoom.toolTip = "Double Cabin";
            bRoom.techLevel = 1;
            bRoom.beds = 2;
            bRoom.quality = 15;
            bRoom.tip = new ToolTip();
            bRoom.tip.tip = bRoom.toolTip;
            bRoom.tip.botLeftText = $"Elon Interstellar Classic";
            bRoom.tip.description = "A double cabin of centuries old design. Originally intended to be used by the service staff of the ship. Comes with small home cinema to ensure that nobody will die from boredom.";
            bRoom.tip.tierIcontype = (TierIcon)bRoom.techLevel;
            bRoom.tip.addStat("No. of Beds", $"{bRoom.beds}", false);
            bRoom.tip.addStat("Suite Quality", $"{bRoom.quality}", false);
            bRoom.tip.addStat("Bedside Table", "Yes", false);
            bRoom.tip.addStat("Home Cinema", "Yes", false);
        }
        public static void modCabinT2S(Bedroom bRoom) {
            bRoom.cost = 600;
            bRoom.toolTip = "Compact Cabin";
            bRoom.techLevel = 2;
            bRoom.beds = 1;
            bRoom.quality = 10;
            bRoom.tip = new ToolTip();
            bRoom.tip.tip = bRoom.toolTip;
            bRoom.tip.botLeftText = $"Full Metal Jacket Inc.";
            bRoom.tip.description = "A modern compact cabin. Mostly used by spaceship captains on modern military vessels. Sometimes doubles as 'decent' hotel rooms at bigger space stations.";
            bRoom.tip.tierIcontype = (TierIcon)bRoom.techLevel;
            bRoom.tip.addStat("No. of Beds", $"{bRoom.beds}", false);
            bRoom.tip.addStat("Suite Quality", $"{bRoom.quality}", false);
            bRoom.tip.addStat("Video Intercom", "Yes", false);
            bRoom.tip.addStat("Scanner Type", "RF-ID", false);
        }
        public static void modCabinT2M(Bedroom bRoom) {
            bRoom.cost = 1200;
            bRoom.toolTip = "Standard Barracks";
            bRoom.techLevel = 2;
            bRoom.beds = 4;
            bRoom.quality = 5;
            bRoom.tip = new ToolTip();
            bRoom.tip.tip = bRoom.toolTip;
            bRoom.tip.botLeftText = $"Full Metal Jacket Inc.";
            bRoom.tip.description = "A modern, standard issue barracks. Not very comfortable to live in for civilians, but very decent according to the military standards. Has built-in advanced air filtration and detection systems.";
            bRoom.tip.tierIcontype = (TierIcon)bRoom.techLevel;
            bRoom.tip.addStat("No. of Beds", $"{bRoom.beds}", false);
            bRoom.tip.addStat("Suite Quality", $"{bRoom.quality}", false);
            bRoom.tip.addStat("Air Filtration", "Yes", false);
            bRoom.tip.addStat("Shared Closet", "Yes", false);
            bRoom.tip.addStat("Entertainment", "No", false);
        }
        public static void modCabinT2L(Bedroom bRoom) {
            bRoom.cost = 1800;
            bRoom.toolTip = "Expanded Barracks";
            bRoom.techLevel = 3;
            bRoom.beds = 8;
            bRoom.quality = 5;
            bRoom.tip = new ToolTip();
            bRoom.tip.tip = bRoom.toolTip;
            bRoom.tip.botLeftText = $"Full Metal Jacket Inc.";
            bRoom.tip.description = "A standard issue barracks stretched to the limit without impacting quality much. Often installed on the large military troop transport ships and sometimes on the stealth refugee smuggling ships.";
            bRoom.tip.tierIcontype = (TierIcon)bRoom.techLevel;
            bRoom.tip.addStat("No. of Beds", $"{bRoom.beds}", false);
            bRoom.tip.addStat("Suite Quality", $"{bRoom.quality}", false);
            bRoom.tip.addStat("Air Filtration", "Yes", false);
            bRoom.tip.addStat("Shared Closet", "Yes", false);
            bRoom.tip.addStat("Mini-Workshop", "Yes", false);
            bRoom.tip.addStat("Entertainment", "No", false);
        }
        public static void modCabinT3M(Bedroom bRoom) {
            bRoom.cost = 3600;
            bRoom.toolTip = "Luxury Suite";
            bRoom.techLevel = 4;
            bRoom.beds = 1;
            bRoom.quality = 40;
            bRoom.tip = new ToolTip();
            bRoom.tip.tip = bRoom.toolTip;
            bRoom.tip.botLeftText = $"Eilat Luxury Engineering";
            bRoom.tip.description = "A modern day luxury suite for comfy interstellar travel. Comes with great gaming rig, comfortable bed, couch, built-in restroom and even shower. The plant is synthetic, but scent is real.";
            bRoom.tip.tierIcontype = (TierIcon)bRoom.techLevel;
            bRoom.tip.addStat("No. of Beds", $"{bRoom.beds}", false);
            bRoom.tip.addStat("Suite Quality", $"{bRoom.quality}", false);
            bRoom.tip.addStat("Gaming Rig", "i3000 Series", false);
            bRoom.tip.addStat("Bed Type", "Comfortable", false);
            bRoom.tip.addStat("Restroom", "Built-In", false);
            bRoom.tip.addStat("Shower Limit", "250L/day", false);
            bRoom.tip.addStat("Plant Type", "Synthetic", false);
        }
        public static void modCabinT3L(Bedroom bRoom) {
            bRoom.cost = 7200;
            bRoom.toolTip = "Royal Suite";
            bRoom.techLevel = 4;
            bRoom.beds = 1;
            bRoom.quality = 80;
            bRoom.tip = new ToolTip();
            bRoom.tip.tip = bRoom.toolTip;
            bRoom.tip.botLeftText = $"Eilat Luxury Engineering";
            bRoom.tip.description = "A best possible suite anybody could dream of during interstellar travel. Has pretty much everything: top gaming rig, royal king-sized bed, couch, restroom, bathtub and even food replicator.";
            bRoom.tip.tierIcontype = (TierIcon)bRoom.techLevel;
            bRoom.tip.addStat("No. of Beds", $"{bRoom.beds}", false);
            bRoom.tip.addStat("Suite Quality", $"{bRoom.quality}", false);
            bRoom.tip.addStat("Gaming Rig", "i9000X Series", false);
            bRoom.tip.addStat("Bed Type", "Double Royal", false);
            bRoom.tip.addStat("Restroom", "Exceptional", false);
            bRoom.tip.addStat("Bath Limit", "12,500L/day", false);
            bRoom.tip.addStat("Food Replicator", "Tetsujin VII", false);
        }
        public static void modCabinT1S(uint rEntry) {
            LOOTBAG.researchCosts[rEntry] = 8000;
            LOOTBAG.researchTimes[rEntry] = 4.8f;
            LOOTBAG.exclusive[rEntry] = true;
            LOOTBAG.tier[rEntry] = 1;
        }
        public static void modCabinT1M(uint rEntry) {
            LOOTBAG.researchCosts[rEntry] = 14000;
            LOOTBAG.researchTimes[rEntry] = 8.4f;
            LOOTBAG.exclusive[rEntry] = true;
            LOOTBAG.tier[rEntry] = 1;
        }
        public static void modCabinT2S(uint rEntry) {
            LOOTBAG.researchCosts[rEntry] = 12000;
            LOOTBAG.researchTimes[rEntry] = 7.2f;
            LOOTBAG.exclusive[rEntry] = true;
            LOOTBAG.tier[rEntry] = 2;
            LOOTBAG.researchCategories[(int)DataCategory.loot_LowChanceAdv].Add(rEntry);
            LOOTBAG.researchCategories[(int)DataCategory.loot_PhaseTwo].Add(rEntry);
            LOOTBAG.researchCategories[(int)DataCategory.rewards_Hitchhiker].Add(rEntry);
            LOOTBAG.researchCategories[(int)DataCategory.loot_TierTwo].Add(rEntry);
        }
        public static void modCabinT2M(uint rEntry) {
            LOOTBAG.researchCosts[rEntry] = 24000;
            LOOTBAG.researchTimes[rEntry] = 14.4f;
            LOOTBAG.exclusive[rEntry] = true;
            LOOTBAG.tier[rEntry] = 2;
            LOOTBAG.researchCategories[(int)DataCategory.loot_LowChanceAdv].Add(rEntry);
            LOOTBAG.researchCategories[(int)DataCategory.loot_PhaseTwo].Add(rEntry);
            LOOTBAG.researchCategories[(int)DataCategory.rewards_Hitchhiker].Add(rEntry);
            LOOTBAG.researchCategories[(int)DataCategory.loot_TierTwo].Add(rEntry);
        }
        public static void modCabinT2L(uint rEntry) {
            FFU_TL_Defs.checkExistingResearch(rEntry);
            FFU_TL_Defs.checkResearchDupe(new Color(144, 55, 22));
            LOOTBAG.modules[rEntry] = new Color(144, 55, 22);
            LOOTBAG.researchCosts[rEntry] = 36000;
            LOOTBAG.researchTimes[rEntry] = 21.6f;
            LOOTBAG.exclusive[rEntry] = true;
            LOOTBAG.tier[rEntry] = 3;
            LOOTBAG.researchCategories[(int)DataCategory.rewards_Hitchhiker].Add(rEntry);
            LOOTBAG.researchCategories[(int)DataCategory.loot_TierTwo].Add(rEntry);
            LOOTBAG.researchType[rEntry] = ResearchType.module;
        }
        public static void modCabinT3M(uint rEntry) {
            FFU_TL_Defs.checkExistingResearch(rEntry);
            FFU_TL_Defs.checkResearchDupe(new Color(146, 24, 22));
            LOOTBAG.modules[rEntry] = new Color(146, 24, 22);
            LOOTBAG.researchCosts[rEntry] = 72000;
            LOOTBAG.researchTimes[rEntry] = 43.2f;
            LOOTBAG.exclusive[rEntry] = true;
            LOOTBAG.tier[rEntry] = 4;
            LOOTBAG.researchCategories[(int)DataCategory.rewards_Hitchhiker].Add(rEntry);
            LOOTBAG.researchCategories[(int)DataCategory.loot_TierTwoPlus].Add(rEntry);
            LOOTBAG.researchCategories[(int)DataCategory.loot_TierThree].Add(rEntry);
            LOOTBAG.researchType[rEntry] = ResearchType.module;
        }
        public static void modCabinT3L(uint rEntry) {
            FFU_TL_Defs.checkExistingResearch(rEntry);
            FFU_TL_Defs.checkResearchDupe(new Color(146, 32, 22));
            LOOTBAG.modules[rEntry] = new Color(146, 32, 22);
            LOOTBAG.researchCosts[rEntry] = 144000;
            LOOTBAG.researchTimes[rEntry] = 86.4f;
            LOOTBAG.exclusive[rEntry] = true;
            LOOTBAG.tier[rEntry] = 4;
            LOOTBAG.researchCategories[(int)DataCategory.rewards_Hitchhiker].Add(rEntry);
            LOOTBAG.researchCategories[(int)DataCategory.loot_TierThree].Add(rEntry);
            LOOTBAG.researchType[rEntry] = ResearchType.module;
        }
    }
}
