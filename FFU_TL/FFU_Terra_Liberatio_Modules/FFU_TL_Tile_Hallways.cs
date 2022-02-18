#pragma warning disable CS0649

using CoOpSpRpG;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace FFU_Terra_Liberatio {
    public class FFU_TL_Tile_Hallways {
        public static void updateModules(Dictionary<byte, Dictionary<byte, Dictionary<byte, Module>>> modules) {
            ModLog.Message($"Applying module changes: Hallways & Airlocks...");
            modAirlockT1(modules, 146, 40, 22, modules[144][100][22]);
            modAirlockT1(modules, 146, 41, 22, modules[144][101][22]);
            modAirlockT1(modules, 146, 42, 22, modules[144][102][22]);
            modAirlockT1(modules, 146, 43, 22, modules[144][103][22]);
            modAirlockT2(modules, 144, 100, 22);
            modAirlockT2(modules, 144, 101, 22);
            modAirlockT2(modules, 144, 102, 22);
            modAirlockT2(modules, 144, 103, 22);
        }
        public static void updateResearch() {
            ModLog.Message($"Applying research changes: Hallways & Airlocks...");
            modAirlockT1(400495U);
            modAirlockT2(400500U);
        }
        public static void modAirlockT1(Dictionary<byte, Dictionary<byte, Dictionary<byte, Module>>> modules, byte r, byte g, byte b, Module refModule) {
            FFU_TL_Defs.unlistDynamic.Add(new Color(r, g, b));
            FFU_TL_Defs.unlistDynamic = FFU_TL_Defs.unlistDynamic.ToList();
            modAirlockT1(modules[r][g][b] as Airlock, refModule as Airlock);
        }
        public static void modAirlockT2(Dictionary<byte, Dictionary<byte, Dictionary<byte, Module>>> modules, byte r, byte g, byte b) {
            FFU_TL_Defs.unlistDynamic.Add(new Color(r, g, b));
            FFU_TL_Defs.unlistDynamic = FFU_TL_Defs.unlistDynamic.ToList();
            modAirlockT2(modules[r][g][b] as Airlock);
        }
        public static void modAirlockT1(Airlock aLock, Airlock refLock) {
			for (int i = 0; i < aLock.tiles.Length && i < refLock.tiles.Length; i++) {
                aLock.tiles[i].inside = refLock.tiles[i].inside;
                aLock.tiles[i].preferOutside = refLock.tiles[i].preferOutside;
                aLock.tiles[i].blocking = refLock.tiles[i].blocking;
                aLock.tiles[i].airBlocking = refLock.tiles[i].airBlocking;
                aLock.tiles[i].connectUp = refLock.tiles[i].connectUp;
                aLock.tiles[i].connectDown = refLock.tiles[i].connectDown;
                aLock.tiles[i].connectLeft = refLock.tiles[i].connectLeft;
                aLock.tiles[i].connectRight = refLock.tiles[i].connectRight;
                aLock.tiles[i].repairable = refLock.tiles[i].repairable;
			}
			aLock.cost = 20;
            aLock.externalReq = 2;
            aLock.toolTip = "Ancient Airlock";
            aLock.techLevel = 1;
            aLock.tip = new ToolTip();
            aLock.tip.tip = aLock.toolTip;
            aLock.tip.botLeftText = $"Elon Interstellar Classic";
            aLock.tip.description = "An ancient airlock design that was invented centuries ago, at the dawn of space exploration era. Still in working condition. Doubles as only spaceship window that opens.";
            aLock.tip.tierIcontype = (TierIcon)aLock.techLevel;
            aLock.tip.addStat($"Lock Failure Chance", $"3.7%", false);
        }
        public static void modAirlockT2(Airlock aLock) {
            aLock.cost = 50;
            aLock.externalReq = 2;
            aLock.toolTip = "Modern Airlock";
            aLock.techLevel = 2;
            aLock.tip = new ToolTip();
            aLock.tip.tip = aLock.toolTip;
            aLock.tip.botLeftText = $"Elon Interstellar Global";
            aLock.tip.description = "An airlock of modern design that used on almost any spaceship or station, except cheapest ones. Made from excellent composite alloy. Doubles as only spaceship window that opens.";
            aLock.tip.tierIcontype = (TierIcon)aLock.techLevel;
            aLock.tip.addStat($"Composite Alloy", $"1575mm", false);
        }
        public static void modAirlockT1(uint rEntry) {
            FFU_TL_Defs.checkExistingResearch(rEntry);
            FFU_TL_Defs.checkResearchDupe(new Color(146, 40, 2));
            LOOTBAG.modules[rEntry] = new Color(146, 40, 22);
            LOOTBAG.researchCosts[rEntry] = 8000;
            LOOTBAG.researchTimes[rEntry] = 4.8f;
            LOOTBAG.exclusive[rEntry] = true;
            LOOTBAG.tier[rEntry] = 1;
            LOOTBAG.researchType[rEntry] = ResearchType.module;
            FFU_TL_Defs.startingTechIDs.Add(rEntry);
        }
        public static void modAirlockT2(uint rEntry) {
            LOOTBAG.researchCosts[rEntry] = 16000;
            LOOTBAG.researchTimes[rEntry] = 9.6f;
            LOOTBAG.exclusive[rEntry] = true;
            LOOTBAG.tier[rEntry] = 2;
        }
        private enum Hallway {
            none,
            corridor_l0,
            corridor_l1,
            corridor_l2,
            corridor_l3,
            corner,
            corner_s1,
            corner_s2,
            junctionT,
            junctionX,
            dead_end,
        }
    }
}