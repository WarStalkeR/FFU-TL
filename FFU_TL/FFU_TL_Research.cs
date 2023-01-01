#pragma warning disable CS0108
#pragma warning disable CS0169
#pragma warning disable CS0626
#pragma warning disable CS0649

using CoOpSpRpG;
using FFU_Terra_Liberatio;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;

namespace FFU_Terra_Liberatio {
    public class FFU_TL_Research {
        public static void rebalanceResearch(Dictionary<uint, uint> rCosts, Dictionary<uint, float> rTimes) {
            ModLog.Message($"Updating research times based on their costs.");
            foreach (var rCost in rCosts) {
                if (rTimes.ContainsKey(rCost.Key)) {
                    rTimes[rCost.Key] = rCost.Value / 5000f * 3f;
                }
            }
        }
        public static void dumpForgottenResearch(Dictionary<uint, uint> rList, Dictionary<byte, Dictionary<byte, Dictionary<byte, Module>>> rModuleList, string dumpFile = "FFU_TL_Forgotten.txt") {
            ModLog.Warning($"Dumping all forgotten research into the {dumpFile}");
            Support.ValidateDirPath(FFU_TL_Defs.exeFilePath + FFU_TL_Defs.modDumpsDir);
            TextWriter ioDump = new StreamWriter(FFU_TL_Defs.exeFilePath + FFU_TL_Defs.modDumpsDir + dumpFile);
            foreach (var rEntry in rList) {
                var rKey = rEntry.Key;
                var isBlocked = true;
                for (int i = 0; i < 37 && isBlocked; i++) if (LOOTBAG.researchCategories[i].Contains(rKey)) isBlocked = false;
                var rModule = LOOTBAG.modules.ContainsKey(rKey) ? LOOTBAG.modules[rKey] : Color.Black;
                if (isBlocked && rModule != Color.Black) {
                    ioDump.WriteLine($"Entry #{rKey}U, " +
                    $"{(LOOTBAG.tier.ContainsKey(rKey) ? $"Tier: {LOOTBAG.tier[rKey]}, " : "")}" +
                    $"{(LOOTBAG.researchCosts.ContainsKey(rKey) ? $"Cost: {LOOTBAG.researchCosts[rKey]}, " : "")}" +
                    $"{(LOOTBAG.researchTimes.ContainsKey(rKey) ? $"Time: {LOOTBAG.researchTimes[rKey]}, " : "")}" +
                    $"Key: [{rModule.R}][{rModule.G}][{rModule.B}]/({rModule.R}, {rModule.G}, {rModule.B}), " +
                    $"Type: {rModuleList[rModule.R][rModule.G][rModule.B]?.type!}, " +
                    $"Name: {rModuleList[rModule.R][rModule.G][rModule.B]?.tip?.tip?.Replace("\n", " ")}, " +
                    $"Description: {rModuleList[rModule.R][rModule.G][rModule.B]?.tip?.description?.Replace("\n", " ")}" +
                    $"");
                } else if (isBlocked) {
                    ioDump.WriteLine($"Entry #{rKey}U" +
                    $"{(LOOTBAG.tier.ContainsKey(rKey) ? $", Tier: {LOOTBAG.tier[rKey]}" : "")}" +
                    $"{(LOOTBAG.researchCosts.ContainsKey(rKey) ? $", Cost: {LOOTBAG.researchCosts[rKey]}" : "")}" +
                    $"{(LOOTBAG.researchTimes.ContainsKey(rKey) ? $", Time: {LOOTBAG.researchTimes[rKey]}" : "")}" +
                    $"{(LOOTBAG.researchType.ContainsKey(rKey) ? $", Type: {LOOTBAG.researchType[rKey]}" : "")}" +
                    $"{(LOOTBAG.tips.ContainsKey(rKey) ? $", Tip: {LOOTBAG.tips[rKey]}" : "")}" +
                    $"{(LOOTBAG.turrets.ContainsKey(rKey) ? $", Tip: {LOOTBAG.turrets[rKey]}" : "")}" +
                    ".");
                }
            }
            for (byte r = 0; r < byte.MaxValue; r++) {
                for (byte g = 0; g < byte.MaxValue; g++) {
                    for (byte b = 0; b < byte.MaxValue; b++) {
                        if (rModuleList.ContainsKey(r) && rModuleList[r].ContainsKey(g) && rModuleList[r][g].ContainsKey(b) && rModuleList[r][g][b].rotation == 0) {
                            if (!LOOTBAG.modules.ContainsValue(new Color(r, g, b))) ioDump.WriteLine($"No Entry, " +
                            $"Key: [{r}][{g}][{b}]/({r}, {g}, {b}), " +
                            $"Type: {rModuleList[r][g][b]?.type!}, " +
                            $"Name: {rModuleList[r][g][b]?.tip?.tip?.Replace("\n", " ")}, " +
                            $"Description: {rModuleList[r][g][b]?.tip?.description?.Replace("\n", " ")}" +
                            $"");
                        }
                    }
                }
            }
            foreach (var rTurret in TURRET_BAG.turNames) {
                if (!LOOTBAG.turrets.ContainsValue(rTurret.Key)) ioDump.WriteLine($"No Entry, " +
                $"Turret: {rTurret.Key}, " +
                $"Name: {rTurret.Value}, " +
                $"CIWS: {TURRET_BAG.isFlak.Contains(rTurret.Key)}, " +
                $"Tier: {TURRET_BAG.turTiers[rTurret.Key]!}, " +
                $"Size: {TURRET_BAG.turSize[rTurret.Key]!}, " +
                $"Price: {TURRET_BAG.turPrices[rTurret.Key]!}" +
                $"");
            }
            ioDump.Close();
        }
        public static void dumpResearchCategories(Dictionary<byte, Dictionary<byte, Dictionary<byte, Module>>> rModuleList, string dumpFile = "FFU_TL_Research.txt") {
            ModLog.Warning($"Dumping all research categories into the {dumpFile}");
            Support.ValidateDirPath(FFU_TL_Defs.exeFilePath + FFU_TL_Defs.modDumpsDir);
            TextWriter ioDump = new StreamWriter(FFU_TL_Defs.exeFilePath + FFU_TL_Defs.modDumpsDir + dumpFile);
            for (int i = 0; i < LOOTBAG.researchCategories.Length; i++) {
                ioDump.WriteLine($"Research Category #{i} ({(DataCategory)i}):");
                foreach (var rKey in LOOTBAG.researchCategories[i]) {
                    var rModule = LOOTBAG.modules.ContainsKey(rKey) ? LOOTBAG.modules[rKey] : Color.Black;
                    if (rModule != Color.Black) {
                        ioDump.WriteLine($" > Entry #{rKey}U, " +
                        $"{(LOOTBAG.tier.ContainsKey(rKey) ? $"Tier: {LOOTBAG.tier[rKey]}" : "")}, " +
                        $"{(LOOTBAG.researchCosts.ContainsKey(rKey) ? $"Cost: {LOOTBAG.researchCosts[rKey]}" : "")}, " +
                        $"{(LOOTBAG.researchTimes.ContainsKey(rKey) ? $"Time: {LOOTBAG.researchTimes[rKey]}" : "")}, " +
                        $"Key: [{rModule.R}][{rModule.G}][{rModule.B}]/({rModule.R}, {rModule.G}, {rModule.B}), " +
                        $"Type: {rModuleList[rModule.R][rModule.G][rModule.B]?.type!}, " +
                        $"Name: {rModuleList[rModule.R][rModule.G][rModule.B]?.tip?.tip?.Replace("\n", " ")}, " +
                        $"Description: {rModuleList[rModule.R][rModule.G][rModule.B]?.tip?.description?.Replace("\n", " ")}" +
                        $"");
                    } else {
                        ioDump.WriteLine($" > Entry #{rKey}U" +
                        $"{(LOOTBAG.tier.ContainsKey(rKey) ? $", Tier: {LOOTBAG.tier[rKey]}" : "")}" +
                        $"{(LOOTBAG.researchCosts.ContainsKey(rKey) ? $", Cost: {LOOTBAG.researchCosts[rKey]}" : "")}" +
                        $"{(LOOTBAG.researchTimes.ContainsKey(rKey) ? $", Time: {LOOTBAG.researchTimes[rKey]}" : "")}" +
                        $"{(LOOTBAG.researchType.ContainsKey(rKey) ? $", Type: {LOOTBAG.researchType[rKey]}" : "")}" +
                        $"{(LOOTBAG.tips.ContainsKey(rKey) ? $", Tip: {LOOTBAG.tips[rKey]}" : "")}" +
                        $"{(LOOTBAG.turrets.ContainsKey(rKey) ? $", Tip: {LOOTBAG.turrets[rKey]}" : "")}" +
                        ".");
                    }
                }
            }
            ioDump.Close();
        }
    }
}

namespace CoOpSpRpG {
    public static class patch_LOOTBAG {
        public static extern void orig_prepareLoot();
        //private static extern void orig_t2unlockCorridors();
        public static void prepareLoot() {
        /// Apply research changes after loading original files.
            orig_prepareLoot();
            if (FFU_TL_Defs.doDataDump) FFU_TL_Research.dumpForgottenResearch(LOOTBAG.researchCosts, FFU_TL_Defs.refModules, "FFU_TL_Forgotten_Original.txt");
            if (FFU_TL_Defs.doDataDump) FFU_TL_Research.dumpResearchCategories(FFU_TL_Defs.refModules, "FFU_TL_Research_Original.txt");
            FFU_TL_Tile_CargoBays.updateResearch();
            FFU_TL_Tile_UtilityBays.updateResearch();
            FFU_TL_Tile_MagRails.updateResearch();
            FFU_TL_Tile_Logistics.updateResearch();
            FFU_TL_Tile_Hallways.updateResearch();
            FFU_TL_Tile_CrewRooms.updateResearch();
            FFU_TL_Tile_CloningVats.updateResearch();
            FFU_TL_Tile_Taverns.updateResearch();
            FFU_TL_Research.rebalanceResearch(LOOTBAG.researchCosts, LOOTBAG.researchTimes);
            if (FFU_TL_Defs.doDataDump) FFU_TL_Research.dumpForgottenResearch(LOOTBAG.researchCosts, FFU_TL_Defs.refModules, "FFU_TL_Forgotten_Modded.txt");
            if (FFU_TL_Defs.doDataDump) FFU_TL_Research.dumpResearchCategories(FFU_TL_Defs.refModules, "FFU_TL_Research_Modded.txt");
            FFU_TL_Defs.refModules = null;
        }
        //private static void t2unlockCorridors() {
        //    orig_t2unlockCorridors();
        //    CHARACTER_DATA.unlockMod(400500U);
        //}
    }
    public class patch_CHARACTER_DATA : CHARACTER_DATA {
        public static extern void orig_createCharacter(string name);
        public static void createCharacter(string name) {
            orig_createCharacter(name);
            if (!CONFIG.openMP) {
                foreach (uint techID in FFU_TL_Defs.startingTechIDs) {
                    addResearch(new ResearchProgress(techID));
                    excludeResearch(techID);
                }
            }
        }
    }
}