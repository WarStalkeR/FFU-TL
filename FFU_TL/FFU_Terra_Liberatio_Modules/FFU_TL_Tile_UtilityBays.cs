#pragma warning disable CS0108
#pragma warning disable IDE0004

using CoOpSpRpG;
using FFU_Terra_Liberatio;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;

namespace FFU_Terra_Liberatio {
    public class FFU_TL_Tile_UtilityBays {
        public static void updateModules(Dictionary<byte, Dictionary<byte, Dictionary<byte, Module>>> modules) {
            ModLog.Message($"Applying module changes: Utility Rooms.");
            modUtilityCloset(modules, 145, 250, 22);
            modUtilityCloset(modules, 145, 251, 22);
            modUtilityCloset(modules, 145, 252, 22);
            modUtilityCloset(modules, 145, 253, 22);
        }
        public static void assignCustomCosts(Dictionary<byte, Dictionary<byte, Dictionary<byte, Module>>> modules, Dictionary<Module, Dictionary<InventoryItemType, float>> rRes, Dictionary<Module, Dictionary<InventoryItemType, float>> rExtra) {
            ModLog.Message($"Applying custom resource costs: Utility Rooms.");
            modUtilityCloset(modules[145][250][22], rRes, rExtra);
            modUtilityCloset(modules[145][251][22], rRes, rExtra);
            modUtilityCloset(modules[145][252][22], rRes, rExtra);
            modUtilityCloset(modules[145][253][22], rRes, rExtra);
        }
        public static void updateResearch() {
            ModLog.Message($"Applying research changes: Utility Rooms.");
            modUtilityCloset(400631U, 145, 250, 22);
        }
        public static void modUtilityCloset(Dictionary<byte, Dictionary<byte, Dictionary<byte, Module>>> modules, byte r, byte g, byte b) {
            FFU_TL_Defs.unlistDynamic.Add(new Color(r, g, b));
            FFU_TL_Defs.unlistDynamic = FFU_TL_Defs.unlistDynamic.ToList();
            modUtilityCloset(modules[r][g][b] as ItemSpawner);
        }
        public static void modUtilityCloset(ItemSpawner iSpawner) {
            iSpawner.cost = FFU_TL_Defs.secretStash ? 1250000 : 1250;
            iSpawner.toolTip = $"Utility Room {(FFU_TL_Defs.secretStash ? "X-42" : "C-3")}";
            iSpawner.techLevel = 4;
            iSpawner.tip = new ToolTip();
            iSpawner.tip.tip = iSpawner.toolTip;
            iSpawner.tip.botLeftText = $"CERN Engineering Division";
            iSpawner.tip.description = $"Always has things in it you need the most, like maybe that one important thing you should've not thrown away earlier.{(FFU_TL_Defs.secretStash ? " In particular, it contains best possible tools, weapons, implants, armors, crystal genes and supplies." : "")}";
            iSpawner.tip.addStat(FFU_TL_Defs.secretStash ? $"Advanced Tools" : $"Basic Tools", "Yes", false);
            if (FFU_TL_Defs.secretStash) {
                iSpawner.tip.addStat($"Best EVA Suit", "Yes", false);
                iSpawner.tip.addStat($"Epic Weapons", "Yes", false);
                iSpawner.tip.addStat($"Superior Implants", "Yes", false);
                iSpawner.tip.addStat($"Crystal Genes", "All", false);
                iSpawner.tip.addStat($"Grey Goo No.", "512", false);
                iSpawner.tip.addStat($"Exotic Matter No.", "128", false);
                iSpawner.tip.addStat($"Quantum Cores No.", "8", false);
            }
            iSpawner.tip.tierIcontype = (TierIcon)iSpawner.techLevel;
            if (FFU_TL_Defs.secretStash) iSpawner.itemSpawnFunction = () => new InventoryItem[] {
                new CrewArmor(Support.StringToBinStream(Datas.perfectArmor)),
                new Gun(Support.StringToBinStream(Datas.perfectRailgun)),
                new Gun(Support.StringToBinStream(Datas.perfectMinigun)),
                new Gun(Support.StringToBinStream(Datas.perfectAutoSG)),
                new Gun(Support.StringToBinStream(Datas.perfectSniperRF)),
                new Gun(Support.StringToBinStream(Datas.perfectAssaultRF)),
                new Gun(Support.StringToBinStream(Datas.perfectSMG)),
                new Gun(Support.StringToBinStream(Datas.perfectHandgun)),
                new Gun(Support.StringToBinStream(Datas.perfectShotgun)),
                new Gun(Support.StringToBinStream(Datas.perfectRepeater)),
                new InventoryItem(InventoryItemType.crystal_spore) {stackSize = 8U},
                new CrystalGene((CrystalType)0x00),
                new CrystalGene((CrystalType)0x0F),
                new CrystalGene((CrystalType)0x11),
                new CrystalGene((CrystalType)0x13),
                new CrystalGene((CrystalType)0x15),
                new CrystalGene((CrystalType)0x07),
                new CrystalGene((CrystalType)0x01),
                new CrystalGene((CrystalType)0x02),
                new CrystalGene((CrystalType)0x06),
                new CrystalGene((CrystalType)0x0C),
                new CrystalGene((CrystalType)0x04),
                new CrystalGene((CrystalType)0x03),
                new CrystalGene((CrystalType)0x08),
                new CrystalGene((CrystalType)0x05),
                new CrystalGene((CrystalType)0x0E),
                new CrystalGene((CrystalType)0x10),
                new CrystalGene((CrystalType)0x12),
                new CrystalGene((CrystalType)0x14),
                new CrystalBranch(3),
                new InventoryItem(InventoryItemType.grey_goo) {stackSize = 512U},
                new InventoryItem(InventoryItemType.dense_exotic_matter) {stackSize = 128U},
                new InventoryItem(InventoryItemType.core_node) {stackSize = 8U},
                new RepairGun(Support.StringToBinStream(Datas.perfectRepairTool)),
                new Extinguisher(Support.StringToBinStream(Datas.perfectExtinguisher)),
                new Digger(),
                new StealthAura(Support.StringToBinStream(Datas.perfectStealthAura)),
                new NanoAura(Support.StringToBinStream(Datas.perfectRepairAura)),
                new NanoAura(Support.StringToBinStream(Datas.perfectShieldAura)),
                new NanoAura(Support.StringToBinStream(Datas.perfectOxygenAura)),
            };
        }
        public static void modUtilityCloset(Module rMod, Dictionary<Module, Dictionary<InventoryItemType, float>> rRes, Dictionary<Module, Dictionary<InventoryItemType, float>> rExtra) {
            if (!FFU_TL_Defs.secretStash) return;
            FFU_TL_Modules.cleanModuleResList(rMod);
            if (rRes.ContainsKey(rMod)) rRes.Remove(rMod);
            if (rExtra.ContainsKey(rMod)) rExtra.Remove(rMod);
            rExtra.Add(rMod, new Dictionary<InventoryItemType, float>() {
                {InventoryItemType.iron_ore, rMod.tiles.Count() * 10f},
                {InventoryItemType.gold_ore, rMod.tiles.Count() * 35f},
                {InventoryItemType.titanium_ore, rMod.tiles.Count() * 50f},
                {InventoryItemType.rhodium_ore, rMod.tiles.Count() * 20f},
                {InventoryItemType.mitraxit_ore, rMod.tiles.Count() * 15f},
                {InventoryItemType.ithacit_ore, rMod.tiles.Count() * 5f}
            });
            patch_TILEBAG.SafeAssignResources(rMod, rExtra[rMod]);
        }
        public static void modUtilityCloset(uint rEntry, byte r, byte g, byte b) {
            FFU_TL_Defs.checkModifiedEntry(rEntry, new Color(r, g, b));
            LOOTBAG.modules[rEntry] = new Color(r, g, b);
            LOOTBAG.researchCosts[rEntry] = 750000;
            LOOTBAG.researchTimes[rEntry] = 450f;
            LOOTBAG.exclusive[rEntry] = true;
            LOOTBAG.tier[rEntry] = 4;
            LOOTBAG.researchCategories[(int)DataCategory.rewards_Hitchhiker].Add(rEntry);
            LOOTBAG.researchCategories[(int)DataCategory.loot_TierThree].Add(rEntry);
            LOOTBAG.researchType[rEntry] = ResearchType.module;
        }
    }
}

namespace CoOpSpRpG {
    public class patch_ItemSpawner : ItemSpawner {
        public void activate(Crew crew, MicroCosm cosm, Vector2 target) {
        /// Change utility closet's max storage capacity to 64.
            if (crew == PLAYER.avatar && this.itemSpawnFunction != null) {
                InventoryItem[] array = this.itemSpawnFunction();
                if (array != null) {
                    Storage storage = new Storage(array.Length, 8, 8);
                    foreach (InventoryItem item in array) {
                        storage.placeInFirstSlot(item);
                    }
                    SCREEN_MANAGER.popupOverlay = new CharacterInventory(PLAYER.avatar, storage, StorageAuthority.takeOnly);
                }
            }
        }
    }
}