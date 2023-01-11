#pragma warning disable CS0649

using CoOpSpRpG;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System;

namespace FFU_Terra_Liberatio {
    public class FFU_TL_Tile_Hallways {
        public static void updateModules() {
            ModLog.Message($"Applying module changes: Hallways & Airlocks.");
            modAirlockT1(146, 40, 22, 144, 100, 22);
            modAirlockT1(146, 41, 22, 144, 101, 22);
            modAirlockT1(146, 42, 22, 144, 102, 22);
            modAirlockT1(146, 43, 22, 144, 103, 22);
            modAirlockT2(144, 100, 22);
            modAirlockT2(144, 101, 22);
            modAirlockT2(144, 102, 22);
            modAirlockT2(144, 103, 22);
            modHallway(145, 122, 22, Hallway.t1_corridor_l0);
            modHallway(145, 123, 22, Hallway.t1_corridor_l0);
            modHallway(145, 112, 22, Hallway.t1_corridor_l1);
            modHallway(145, 113, 22, Hallway.t1_corridor_l1);
        }
        public static void updateResearch() {
            ModLog.Message($"Applying research changes: Hallways & Airlocks.");
            modAirlockT1(400495U, 146, 40, 22);
            modAirlockT2(400500U, 144, 100, 22);
        }
        public static void modAirlockT1(byte r, byte g, byte b, byte x, byte y, byte z) {
            FFU_TL_Defs.unlistDynamic.Add(new Color(r, g, b));
            FFU_TL_Defs.unlistDynamic = FFU_TL_Defs.unlistDynamic.ToList();
            modAirlockT1(FFU_TL_Defs.rMod[r][g][b] as Airlock, FFU_TL_Defs.rMod[x][y][z] as Airlock);
        }
        public static void modAirlockT2(byte r, byte g, byte b) {
            FFU_TL_Defs.unlistDynamic.Add(new Color(r, g, b));
            FFU_TL_Defs.unlistDynamic = FFU_TL_Defs.unlistDynamic.ToList();
            modAirlockT2(FFU_TL_Defs.rMod[r][g][b] as Airlock);
        }
        public static void modHallway(byte r, byte g, byte b, Hallway hType) {
            FFU_TL_Defs.unlistDynamic.Add(new Color(r, g, b));
            FFU_TL_Defs.unlistDynamic = FFU_TL_Defs.unlistDynamic.ToList();
            modHallway(FFU_TL_Defs.rMod[r][g][b], hType);
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
            aLock.tip.addStat($"Integrity Multiplier", $"{(int)(Math.Pow(Math.Max(1U, aLock.techLevel), 1.5) * 100)}%", false);
            aLock.tip.addStat($"Integrity Per Tile", $"~{Math.Pow(Math.Max(1U, aLock.techLevel), 1.5) * 25:0.0}", false);
            aLock.tip.addStat($"Integrity Total", $"{(int)(Math.Pow(Math.Max(1U, aLock.techLevel), 1.5) * 25 * aLock.tiles.Length)}", false);
            aLock.tip.addStat($"Integrity Effect", $"+{(int)(Math.Pow(Math.Max(1U, aLock.techLevel), 1.5) * 25 * aLock.tiles.Length - aLock.tiles.Length * 25)}", false);
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
            aLock.tip.addStat($"Integrity Multiplier", $"{(int)(Math.Pow(Math.Max(1U, aLock.techLevel), 1.5) * 100)}%", false);
            aLock.tip.addStat($"Integrity Per Tile", $"~{Math.Pow(Math.Max(1U, aLock.techLevel), 1.5) * 25:0.0}", false);
            aLock.tip.addStat($"Integrity Total", $"{(int)(Math.Pow(Math.Max(1U, aLock.techLevel), 1.5) * 25 * aLock.tiles.Length)}", false);
            aLock.tip.addStat($"Integrity Effect", $"+{(int)(Math.Pow(Math.Max(1U, aLock.techLevel), 1.5) * 25 * aLock.tiles.Length - aLock.tiles.Length * 25)}", false);
            aLock.tip.addStat($"Composite Alloy", $"1575mm", false);
        }
        public static void modHallway(Module rWall, Hallway rType) {
            rWall.toolTip = GetName(rType);
            rWall.techLevel = TechLevel(rType);
            rWall.cost = TileCost(rType) * rWall.tiles.Length;
            rWall.tip = new ToolTip();
            rWall.tip.tip = rWall.toolTip;
            rWall.tip.botLeftText = $"{Designer(rType)}";
            rWall.tip.description = $"{Description(rType)}";
            rWall.tip.tierIcontype = (TierIcon)rWall.techLevel;
            rWall.tip.addStat($"Integrity Multiplier", $"{(int)(Math.Pow(Math.Max(1U, rWall.techLevel), 1.5) * 100)}%", false);
            rWall.tip.addStat($"Integrity Per Tile", $"~{Math.Pow(Math.Max(1U, rWall.techLevel), 1.5) * 25:0.0}", false);
            rWall.tip.addStat($"Integrity Total", $"{(int)(Math.Pow(Math.Max(1U, rWall.techLevel), 1.5) * 25 * rWall.tiles.Length)}", false);
            rWall.tip.addStat($"Integrity Effect", $"+{(int)(Math.Pow(Math.Max(1U, rWall.techLevel), 1.5) * 25 * rWall.tiles.Length - rWall.tiles.Length * 25)}", false);
        }
        public static void modAirlockT1(uint rEntry, byte r, byte g, byte b) {
            FFU_TL_Defs.checkModifiedEntry(rEntry, new Color(r, g, b));
            LOOTBAG.modules[rEntry] = new Color(r, g, b);
            LOOTBAG.researchCosts[rEntry] = 8000;
            LOOTBAG.researchTimes[rEntry] = 4.8f;
            LOOTBAG.exclusive[rEntry] = true;
            LOOTBAG.tier[rEntry] = 1;
            LOOTBAG.researchType[rEntry] = ResearchType.module;
            FFU_TL_Defs.startingTechIDs.Add(rEntry);
        }
        public static void modAirlockT2(uint rEntry, byte r, byte g, byte b) {
            FFU_TL_Defs.checkModifiedEntry(rEntry, new Color(r, g, b));
            LOOTBAG.researchCosts[rEntry] = 16000;
            LOOTBAG.researchTimes[rEntry] = 9.6f;
            LOOTBAG.exclusive[rEntry] = true;
            LOOTBAG.tier[rEntry] = 2;
        }
        public static string Designer(Hallway hType) {
            var hRef = hType.ToString().Split('_');
            return hRef[0] switch {
                "t1" => "Elon Interstellar Classic",
                "t2" => "Elon Interstellar Global",
                "t2e" => "Microsol Corporation",
                "t2r" => "Deep Space Engineering Inc.",
                "t2l" => "Eilat Luxury Engineering",
                "t3" => "Mars Security Unlimited",
                _ => ""
            };
        }
        public static string Description(Hallway hType) {
            var hRef = hType.ToString().Split('_');
            return hRef[0] switch {
                "t1" => "A classic hallway of sturdy and simple design from dawn of space exploration era. Looks old and feels old, but at very least doesn't affect ship's integrity negatively.",
                "t2" => "Modern, efficiently designed hallway. Utilizes advanced composites and ingenious structural architecture to strengthen ship's integrity, despite being \"hollow\" inside.",
                "t2e" => "Specialized external hallway for decompressed environments. Has built-in self-sufficient magnetic plates to ensure that crew that uses it won't drift away.",
                "t2r" => "An advanced structural hallway engineered for a long-term durability. Initially expected to be used on deep space stations, but sometimes is used at capital ships.",
                "t2l" => "Luxurious and durable hallway that helps people to forget that they are living in deep space, despite the fact that they actually live in deep space anyway.",
                "t3" => "An extra durable hallway. Designed with idea in mind to withstand army of demons and one angry, rampaging marine in suspiciously green-colored uniform. But only one.",
                _ => ""
            };
        }
        public static byte TechLevel(Hallway hType) {
            var hRef = hType.ToString().Split('_');
            return hRef[0] switch {
                "t1" => 1,
                "t2" => 2,
                "t2e" => 2,
                "t2r" => 3,
                "t2l" => 3,
                "t3" => 4,
                _ => 0
            };
        }
        public static byte TileCost(Hallway hType) {
            var hRef = hType.ToString().Split('_');
            return hRef[0] switch {
                "t1" => 5,
                "t2" => 10,
                "t2e" => 15,
                "t2r" => 20,
                "t2l" => 30,
                "t3" => 50,
                _ => 1
            };
        }
        public static string GetName(Hallway hType) {
            return hType switch {
                Hallway.t1_corridor_l0 => "Ancient Hallway L-0",
                Hallway.t1_corridor_l1 => "Ancient Hallway L-1",
                Hallway.t1_corridor_l2 => "Ancient Hallway L-2",
                Hallway.t1_corner => "Ancient Hallway C-1",
                Hallway.t1_junction_t => "Ancient Hallway J-T",
                Hallway.t1_junction_x => "Ancient Hallway J-X",
                Hallway.t1_dead_end => "Ancient Hallway E-1",
                Hallway.t2_corridor_l0 => "Modern Hallway L-0",
                Hallway.t2_corridor_l1 => "Modern Hallway L-1",
                Hallway.t2_corridor_l2 => "Modern Hallway L-2",
                Hallway.t2_corridor_l3 => "Modern Hallway L-3",
                Hallway.t2_corner => "Modern Hallway C-1",
                Hallway.t2_corner_s1 => "Modern Hallway S-1",
                Hallway.t2_corner_s2 => "Modern Hallway S-2",
                Hallway.t2_junction_t => "Modern Hallway J-T",
                Hallway.t2_junction_x => "Modern Hallway J-X",
                Hallway.t2_dead_end => "Modern Hallway E-1",
                Hallway.t2r_corridor_l0 => "Station Hallway L-0",
                Hallway.t2r_corridor_l1 => "Station Hallway L-1",
                Hallway.t2r_corridor_l2 => "Station Hallway L-2",
                Hallway.t2r_corridor_l3 => "Station Hallway L-3",
                Hallway.t2r_corridor_l4 => "Station Hallway L-4",
                Hallway.t2r_corner => "Station Hallway C-1",
                Hallway.t2r_corner_a => "Station Hallway C-A",
                Hallway.t2r_junction_t => "Station Hallway J-T",
                Hallway.t2r_junction_t1 => "Station Hallway T-1",
                Hallway.t2r_junction_t2 => "Station Hallway T-2",
                Hallway.t2r_junction_tbar => "Station Hallway T-B",
                Hallway.t2r_junction_x => "Station Hallway J-X",
                Hallway.t2r_junction_x1 => "Station Hallway X-1",
                Hallway.t2r_dead_end => "Station Hallway E-1",
                Hallway.t2r_adapter_s => "Station Hallway A-S",
                Hallway.t2r_adapter_e => "Station Hallway A-E",
                Hallway.t2e_hallway_s => "External Hallway L-S",
                Hallway.t2e_hallway_l => "External Hallway L-L",
                Hallway.t2e_hallway_p => "External Hallway L-P",
                Hallway.t2e_hallway_a1 => "External Hallway A-1",
                Hallway.t2e_hallway_a2 => "External Hallway A-2",
                Hallway.t2e_hallway_a3 => "External Hallway A-3",
                Hallway.t2l_corridor_s => "Luxury Hallway L-S",
                Hallway.t2l_corridor_l => "Luxury Hallway L-L",
                Hallway.t2l_corridor_d => "Luxury Hallway L-D",
                Hallway.t2l_corner => "Luxury Hallway C-1",
                Hallway.t2l_dead_end => "Luxury Hallway E-1",
                Hallway.t2l_adapter => "Luxury Hallway A-1",
                Hallway.t2l_balcony => "Luxury Hallway B-1",
                Hallway.t3_corridor_l0 => "Armored Hallway L-0",
                Hallway.t3_corridor_l1 => "Armored Hallway L-1",
                Hallway.t3_corridor_l2 => "Armored Hallway L-2",
                Hallway.t3_corridor_l3 => "Armored Hallway L-3",
                Hallway.t3_corner => "Armored Hallway C-1",
                Hallway.t3_corner_s1 => "Armored Hallway S-1",
                Hallway.t3_corner_s2 => "Armored Hallway S-2",
                Hallway.t3_junction_t => "Armored Hallway J-T",
                Hallway.t3_junction_x => "Armored Hallway J-X",
                Hallway.t3_dead_end => "Armored Hallway E-1",
                _ => "Unknown Hallway"
            };
        }
        public enum Hallway {
            none,
            t1_corridor_l0,
            t1_corridor_l1,
            t1_corridor_l2,
            t1_corner,
            t1_junction_t,
            t1_junction_x,
            t1_dead_end,
            t2_corridor_l0,
            t2_corridor_l1,
            t2_corridor_l2,
            t2_corridor_l3,
            t2_corner,
            t2_corner_s1,
            t2_corner_s2,
            t2_junction_t,
            t2_junction_x,
            t2_dead_end,
            t2r_corridor_l0,
            t2r_corridor_l1,
            t2r_corridor_l2,
            t2r_corridor_l3,
            t2r_corridor_l4,
            t2r_corner,
            t2r_corner_a,
            t2r_junction_t,
            t2r_junction_t1,
            t2r_junction_t2,
            t2r_junction_tbar,
            t2r_junction_x,
            t2r_junction_x1,
            t2r_dead_end,
            t2r_adapter_s,
            t2r_adapter_e,
            t2e_hallway_s,
            t2e_hallway_l,
            t2e_hallway_p,
            t2e_hallway_a1,
            t2e_hallway_a2,
            t2e_hallway_a3,
            t2l_corridor_s,
            t2l_corridor_l,
            t2l_corridor_d,
            t2l_corner,
            t2l_dead_end,
            t2l_adapter,
            t2l_balcony,
            t3_corridor_l0,
            t3_corridor_l1,
            t3_corridor_l2,
            t3_corridor_l3,
            t3_corridor_lz,
            t3_corner,
            t3_corner_s1,
            t3_corner_s2,
            t3_junction_t,
            t3_junction_x,
            t3_dead_end
        }
    }
}