﻿#pragma warning disable CS0108
#pragma warning disable CS0414
#pragma warning disable CS0649
#pragma warning disable CS0626

using MonoMod;
using CoOpSpRpG;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace FFU_Terra_Liberatio {
    public class FFU_TL_Tile_MagRails {
        public static void updateModules() {
            ModLog.Message($"Applying module changes: MagRail Stations.");
            modMagRailStationH(145, 192, 24);
            modMagRailStationH(145, 193, 24);
            modMagRailStationH(145, 194, 24);
            modMagRailStationH(145, 195, 24);
            modMagRailStationH(145, 196, 24);
            modMagRailStationH(145, 197, 24);
            modMagRailStationH(145, 198, 24);
            modMagRailStationH(145, 199, 24);
            modMagRailStationT(145, 200, 24);
            modMagRailStationT(145, 201, 24);
            modMagRailStationT(145, 202, 24);
            modMagRailStationT(145, 203, 24);
            modMagRailStationT(145, 204, 24);
            modMagRailStationT(145, 205, 24);
            modMagRailStationT(145, 206, 24);
            modMagRailStationT(145, 207, 24);
            modMagRailStationU(145, 208, 24);
            modMagRailStationU(145, 209, 24);
            modMagRailStationU(145, 210, 24);
            modMagRailStationU(145, 211, 24);
            modMagRailStationU(145, 212, 24);
            modMagRailStationU(145, 213, 24);
            modMagRailStationU(145, 214, 24);
            modMagRailStationU(145, 215, 24);
        }
        public static void updateResearch() {
            ModLog.Message($"Applying research changes: MagRail Stations.");
            modMagRailStationH(400634U, 145, 192, 24);
            modMagRailStationT(400633U, 145, 200, 24);
            modMagRailStationU(400632U, 145, 208, 24);
        }
        public static void modMagRailStationH(byte r, byte g, byte b) {
            FFU_TL_Defs.unlistDynamic.Add(new Color(r, g, b));
            FFU_TL_Defs.unlistDynamic = FFU_TL_Defs.unlistDynamic.ToList();
            modMagRailStationH(FFU_TL_Defs.rMod[r][g][b] as MagrailModule);
        }
        public static void modMagRailStationT(byte r, byte g, byte b) {
            FFU_TL_Defs.unlistDynamic.Add(new Color(r, g, b));
            FFU_TL_Defs.unlistDynamic = FFU_TL_Defs.unlistDynamic.ToList();
            modMagRailStationT(FFU_TL_Defs.rMod[r][g][b] as MagrailModule);
        }
        public static void modMagRailStationU(byte r, byte g, byte b) {
            FFU_TL_Defs.unlistDynamic.Add(new Color(r, g, b));
            FFU_TL_Defs.unlistDynamic = FFU_TL_Defs.unlistDynamic.ToList();
            modMagRailStationU(FFU_TL_Defs.rMod[r][g][b] as MagrailModule);
        }
        public static void modMagRailStationH(MagrailModule mRail) {
            mRail.cost = 6750;
            mRail.toolTip = $"MagRail H-Station";
            mRail.techLevel = 3;
            mRail.tip = new ToolTip();
            mRail.tip.tip = mRail.toolTip;
            mRail.tip.botLeftText = $"Transtellar Unlimited";
            mRail.tip.description = $"Fast, efficient and economic method of transportation that replaces prolonged hallway adventures full of uncertainty. Also grants fast access to all cargo bays for loading and unloading operations.";
            mRail.tip.tierIcontype = (TierIcon)mRail.techLevel;
            mRail.tip.addStat($"No. of Platforms", "2", false);
            mRail.tip.addStat($"MagTrain Speed", "1860 km/h", false);
        }
        public static void modMagRailStationT(MagrailModule mRail) {
            mRail.cost = 6750;
            mRail.toolTip = $"MagRail T-Station";
            mRail.techLevel = 3;
            mRail.tip = new ToolTip();
            mRail.tip.tip = mRail.toolTip;
            mRail.tip.botLeftText = $"Transtellar Unlimited";
            mRail.tip.description = $"Fast, efficient and economic method of transportation that replaces prolonged hallway adventures full of uncertainty. Also grants fast access to all cargo bays for loading and unloading operations.";
            mRail.tip.tierIcontype = (TierIcon)mRail.techLevel;
            mRail.tip.addStat($"No. of Platforms", "1", false);
            mRail.tip.addStat($"MagTrain Speed", "1860 km/h", false);
        }
        public static void modMagRailStationU(MagrailModule mRail) {
            mRail.cost = 6750;
            mRail.toolTip = $"MagRail U-Station";
            mRail.techLevel = 3;
            mRail.tip = new ToolTip();
            mRail.tip.tip = mRail.toolTip;
            mRail.tip.botLeftText = $"Transtellar Unlimited";
            mRail.tip.description = $"Fast, efficient and economic method of transportation that replaces prolonged hallway adventures full of uncertainty. Also grants fast access to all cargo bays for loading and unloading operations.";
            mRail.tip.tierIcontype = (TierIcon)mRail.techLevel;
            mRail.tip.addStat($"No. of Platforms", "1", false);
            mRail.tip.addStat($"MagTrain Speed", "1860 km/h", false);
        }
        public static void modMagRailStationH(uint rEntry, byte r, byte g, byte b) {
            FFU_TL_Defs.checkModifiedEntry(rEntry, new Color(r, g, b));
            LOOTBAG.modules[rEntry] = new Color(r, g, b);
            LOOTBAG.researchCosts[rEntry] = 101250;
            LOOTBAG.researchTimes[rEntry] = 60.75f;
            LOOTBAG.exclusive[rEntry] = true;
            LOOTBAG.tier[rEntry] = 3;
            LOOTBAG.researchCategories[(int)DataCategory.loot_LowChanceAdv].Add(rEntry);
            LOOTBAG.researchCategories[(int)DataCategory.rewards_Hitchhiker].Add(rEntry);
            LOOTBAG.researchCategories[(int)DataCategory.loot_TierTwoPlus].Add(rEntry);
            LOOTBAG.researchCategories[(int)DataCategory.loot_TierThree].Add(rEntry);
            LOOTBAG.researchType[rEntry] = ResearchType.module;
        }
        public static void modMagRailStationT(uint rEntry, byte r, byte g, byte b) {
            FFU_TL_Defs.checkModifiedEntry(rEntry, new Color(r, g, b));
            LOOTBAG.modules[rEntry] = new Color(r, g, b);
            LOOTBAG.researchCosts[rEntry] = 101250;
            LOOTBAG.researchTimes[rEntry] = 60.75f;
            LOOTBAG.exclusive[rEntry] = true;
            LOOTBAG.tier[rEntry] = 3;
            LOOTBAG.researchCategories[(int)DataCategory.loot_LowChanceAdv].Add(rEntry);
            LOOTBAG.researchCategories[(int)DataCategory.rewards_Hitchhiker].Add(rEntry);
            LOOTBAG.researchCategories[(int)DataCategory.loot_TierTwoPlus].Add(rEntry);
            LOOTBAG.researchCategories[(int)DataCategory.loot_TierThree].Add(rEntry);
            LOOTBAG.researchType[rEntry] = ResearchType.module;
        }
        public static void modMagRailStationU(uint rEntry, byte r, byte g, byte b) {
            FFU_TL_Defs.checkModifiedEntry(rEntry, new Color(r, g, b));
            LOOTBAG.modules[rEntry] = new Color(r, g, b);
            LOOTBAG.researchCosts[rEntry] = 101250;
            LOOTBAG.researchTimes[rEntry] = 60.75f;
            LOOTBAG.exclusive[rEntry] = true;
            LOOTBAG.tier[rEntry] = 3;
            LOOTBAG.researchCategories[(int)DataCategory.loot_LowChanceAdv].Add(rEntry);
            LOOTBAG.researchCategories[(int)DataCategory.rewards_Hitchhiker].Add(rEntry);
            LOOTBAG.researchCategories[(int)DataCategory.loot_TierTwoPlus].Add(rEntry);
            LOOTBAG.researchCategories[(int)DataCategory.loot_TierThree].Add(rEntry);
            LOOTBAG.researchType[rEntry] = ResearchType.module;
        }
    }
}

namespace CoOpSpRpG {
    public class patch_ShipNavigationRev3 : ShipNavigationRev3 {
		[MonoModIgnore] private bool stationTravelAnimation;
		[MonoModIgnore] private MicroCosm currentCosm;
		[MonoModIgnore] private float stationAnimTimer;
		[MonoModIgnore] private Vector2 stationAnimDestination;
		private bool stationTravelAnimationMod;
		[MonoModIgnore] public patch_ShipNavigationRev3(GraphicsDevice device, string name) : base(device, name) { }
		public extern void orig_Update(float elapsed);
		[MonoModReplace] public void RemoteCargoView(CargoBay cargoBay) {
        /// Allow cargo loading and loading via MagRail Stations.
            SCREEN_MANAGER.popupOverlay = new CharacterInventory(PLAYER.avatar, cargoBay.storage, StorageAuthority.full);
        }
		public override void Update(float elapsed) {
        /// Move only player to MagRail Station, if crew is on Hold.
			if (stationTravelAnimation) {
				stationTravelAnimation = false;
				stationTravelAnimationMod = true;
			}
			orig_Update(elapsed);
			if (stationTravelAnimationMod) {
				stationAnimTimer += elapsed;
				if (stationAnimTimer >= 1f) {
					stationAnimTimer = 0f;
                    stationTravelAnimationMod = false;
					PLAYER.avatar.position = stationAnimDestination;
					foreach (KeyValuePair<byte, Crew> crewMember in currentCosm.crew) {
						if (crewMember.Value.faction == PLAYER.avatar.faction) {
                            if (crewMember.Value.isPlayer || PLAYER.currentTeam.orderid == 0) {
                                crewMember.Value.position = stationAnimDestination;
                            }
						}
					}
					PLAYER.resetLook();
				}
			}
        }
	}
}