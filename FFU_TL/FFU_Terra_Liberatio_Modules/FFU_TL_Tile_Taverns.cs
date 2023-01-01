#pragma warning disable IDE0003
#pragma warning disable CS0108
#pragma warning disable CS0626
#pragma warning disable CS0649

using MonoMod;
using CoOpSpRpG;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using FFU_Terra_Liberatio;
using System.Linq;

namespace FFU_Terra_Liberatio {
    public class FFU_TL_Tile_Taverns {
        public static void updateModules(Dictionary<byte, Dictionary<byte, Dictionary<byte, Module>>> modules) {
            ModLog.Message($"Applying module changes: Space Bars & Taverns.");
			modTavernSushi(modules, 145, 188, 22);
			modTavernSushi(modules, 145, 189, 22);
			modTavernSushi(modules, 145, 190, 22);
			modTavernSushi(modules, 145, 191, 22);
			modTavernSnooker(modules, 145, 184, 22);
			modTavernSnooker(modules, 145, 185, 22);
			modTavernSnooker(modules, 145, 186, 22);
			modTavernSnooker(modules, 145, 187, 22);
			modTavernTeaHouse(modules, 145, 172, 24);
			modTavernTeaHouse(modules, 145, 173, 24);
			modTavernTeaHouse(modules, 145, 174, 24);
			modTavernTeaHouse(modules, 145, 175, 24);
			modTavernTeaHouse(modules, 145, 176, 24);
			modTavernTeaHouse(modules, 145, 177, 24);
			modTavernTeaHouse(modules, 145, 178, 24);
			modTavernTeaHouse(modules, 145, 179, 24);
			modTavernLuxury(modules, 145, 123, 24);
			modTavernLuxury(modules, 145, 124, 24);
			modTavernLuxury(modules, 145, 125, 24);
			modTavernLuxury(modules, 145, 126, 24);
			modTavernLuxury(modules, 145, 127, 24);
			modTavernLuxury(modules, 145, 128, 24);
			modTavernLuxury(modules, 145, 129, 24);
			modTavernLuxury(modules, 145, 130, 24);
			modTavernDeprecated(modules, 145, 183, 22);
            patch_TILEBAG.refDeprecate(new Color(145, 183, 22));
		}
		public static void updateResearch() {
			ModLog.Message($"Applying research changes: Space Bars & Taverns.");
			modTavernSushi(400731U, 145, 188, 22);
			modTavernSnooker(400732U, 145, 184, 22);
			modTavernTeaHouse(400733U, 145, 172, 24);
			modTavernLuxury(400734U, 145, 123, 24);
		}
		public static void modTavernSushi(Dictionary<byte, Dictionary<byte, Dictionary<byte, Module>>> modules, byte r, byte g, byte b) {
			FFU_TL_Defs.unlistDynamic.Add(new Color(r, g, b));
			FFU_TL_Defs.unlistDynamic = FFU_TL_Defs.unlistDynamic.ToList();
			modTavernSushi(modules[r][g][b] as ScreenAccess);
		}
		public static void modTavernSnooker(Dictionary<byte, Dictionary<byte, Dictionary<byte, Module>>> modules, byte r, byte g, byte b) {
			FFU_TL_Defs.unlistDynamic.Add(new Color(r, g, b));
			FFU_TL_Defs.unlistDynamic = FFU_TL_Defs.unlistDynamic.ToList();
			modTavernSnooker(modules[r][g][b] as ScreenAccess);
		}
		public static void modTavernTeaHouse(Dictionary<byte, Dictionary<byte, Dictionary<byte, Module>>> modules, byte r, byte g, byte b) {
			FFU_TL_Defs.unlistDynamic.Add(new Color(r, g, b));
			FFU_TL_Defs.unlistDynamic = FFU_TL_Defs.unlistDynamic.ToList();
			modTavernTeaHouse(modules[r][g][b] as ScreenAccess);
		}
		public static void modTavernLuxury(Dictionary<byte, Dictionary<byte, Dictionary<byte, Module>>> modules, byte r, byte g, byte b) {
			FFU_TL_Defs.unlistDynamic.Add(new Color(r, g, b));
			FFU_TL_Defs.unlistDynamic = FFU_TL_Defs.unlistDynamic.ToList();
			modTavernLuxury(modules[r][g][b] as ScreenAccess);
		}
		public static void modTavernDeprecated(Dictionary<byte, Dictionary<byte, Dictionary<byte, Module>>> modules, byte r, byte g, byte b) {
			modTavernDeprecated(modules[r][g][b] as ScreenAccess);
		}
		public static void modTavernSushi(ScreenAccess rBar) {
			rBar.cost = 7500;
			rBar.toolTip = $"Sushi Bar";
			rBar.techLevel = 3;
			rBar.tip = new ToolTip();
			rBar.tip.tip = rBar.toolTip;
			rBar.tip.botLeftText = $"Alcohol Conglomerate Ist.";
			rBar.tip.description = $"Despite its cozy looking old style design, this space sushi bar is very complex piece of equipment, with hydroponics, food processors, advanced air filtration & etc. Normally a place for NPC agents to hang out and drink, but when installed on your ship or station allows you speak with your agents, re-recruit them and heed One's nagging.";
			rBar.tip.tierIcontype = (TierIcon)rBar.techLevel;
			rBar.tip.addStat($"Gravity Precision", "9.807 m/s^2", false);
			rBar.tip.addStat($"Food Processor", "Evangelion III", false);
			rBar.tip.addStat($"Air Filtration", "Exceptional", false);
			rBar.tip.addStat($"Hydroponics Type", "Mark IX", false);
			rBar.tip.addStat($"Sushi Menu Variety", "387 Recipes", false);
		}
		public static void modTavernSnooker(ScreenAccess rBar) {
			rBar.cost = 12500;
			rBar.toolTip = $"Snooker Hall";
			rBar.techLevel = 3;
			rBar.tip = new ToolTip();
			rBar.tip.tip = rBar.toolTip;
			rBar.tip.botLeftText = $"Alcohol Conglomerate Ist.";
			rBar.tip.description = $"Complicated module that servers as space bar and space snooker hall at the same time. Old style looking smart snooker tables ensures that balls will never be lost. Normally a place for NPC agents to hang out and drink, but when installed on your ship or station allows you speak with your agents, re-recruit them and heed One's nagging.";
			rBar.tip.tierIcontype = (TierIcon)rBar.techLevel;
			rBar.tip.addStat($"Gravity Precision", "9.807 m/s^2", false);
			rBar.tip.addStat($"Food Processor", "NOMinator 2000", false);
			rBar.tip.addStat($"Snooker Table Type", "Smart VII", false);
			rBar.tip.addStat($"Coffee Machine", "Coffinator 3000", false);
			rBar.tip.addStat($"Bar Menu Variety", "219 Recipes", false);
		}
		public static void modTavernTeaHouse(ScreenAccess rBar) {
			rBar.cost = 25000;
			rBar.toolTip = $"Tea House";
			rBar.techLevel = 4;
			rBar.tip = new ToolTip();
			rBar.tip.tip = rBar.toolTip;
			rBar.tip.botLeftText = $"Alcohol Conglomerate Ist.";
			rBar.tip.description = $"Allows visitors to peacefully drink exceptional tea, reach enlightenment and relax completely, even if ship or station is under heavy enemy assault. Normally a place for NPC agents to hang out and drink, but when installed on your ship or station allows you speak with your agents, re-recruit them and heed One's nagging.";
			rBar.tip.tierIcontype = (TierIcon)rBar.techLevel;
			rBar.tip.addStat($"Gravity Precision", "9.807 m/s^2", false);
			rBar.tip.addStat($"Food Processor", "Manchu-Han IX", false);
			rBar.tip.addStat($"Tea Gardens", "Pure Biological", false);
			rBar.tip.addStat($"Enlightenment Type", "Zen", false);
			rBar.tip.addStat($"Tea Menu Variety", "836 Recipes", false);
		}
		public static void modTavernLuxury(ScreenAccess rBar) {
			rBar.cost = 75000;
			rBar.toolTip = $"Luxury Restaurant";
			rBar.techLevel = 4;
			rBar.tip = new ToolTip();
			rBar.tip.tip = rBar.toolTip;
			rBar.tip.botLeftText = $"Alcohol Conglomerate Ist.";
			rBar.tip.description = $"A truly luxurious place to enjoy food. Food processor that uses memory of famous from ancient Earth to cook best possible dishes you'd ever want to eat. Normally a place for NPC agents to hang out and drink, but when installed on your ship or station allows you speak with your agents, re-recruit them and heed One's nagging.";
			rBar.tip.tierIcontype = (TierIcon)rBar.techLevel;
			rBar.tip.addStat($"Gravity Precision", "9.807 m/s^2", false);
			rBar.tip.addStat($"Food Processor", "Ramsay XIV", false);
			rBar.tip.addStat($"Food Quality", "Best Possible", false);
			rBar.tip.addStat($"Satiety Potential", "9001%", false);
			rBar.tip.addStat($"Luxury Menu Variety", "2974 Recipes", false);
		}
		public static void modTavernDeprecated(ScreenAccess rBar) {
			rBar.cost = 2500;
			rBar.toolTip = $"Deprecated Tavern";
			rBar.techLevel = 1;
			rBar.tip = new ToolTip();
			rBar.tip.tip = rBar.toolTip;
			rBar.tip.botLeftText = $"Alcohol Conglomerate Ist.";
			rBar.tip.description = $"You shouldn't be seeing this module, unless you're in debug mode.";
			rBar.tip.tierIcontype = (TierIcon)rBar.techLevel;
			rBar.tip.addStat($"Deprecated", "Yes", false);
		}
		public static void modTavernSushi(uint rEntry, byte r, byte g, byte b) {
			FFU_TL_Defs.checkModifiedEntry(rEntry, new Color(r, g, b));
			LOOTBAG.modules[rEntry] = new Color(r, g, b);
			LOOTBAG.researchCosts[rEntry] = 37500;
			LOOTBAG.researchTimes[rEntry] = 22.5f;
			LOOTBAG.exclusive[rEntry] = true;
			LOOTBAG.tier[rEntry] = 3;
			LOOTBAG.researchCategories[(int)DataCategory.loot_LowChanceAdv].Add(rEntry);
			LOOTBAG.researchCategories[(int)DataCategory.rewards_Hitchhiker].Add(rEntry);
			LOOTBAG.researchCategories[(int)DataCategory.loot_TierTwoPlus].Add(rEntry);
			LOOTBAG.researchType[rEntry] = ResearchType.module;
		}
		public static void modTavernSnooker(uint rEntry, byte r, byte g, byte b) {
			FFU_TL_Defs.checkModifiedEntry(rEntry, new Color(r, g, b));
			LOOTBAG.modules[rEntry] = new Color(r, g, b);
			LOOTBAG.researchCosts[rEntry] = 62500;
			LOOTBAG.researchTimes[rEntry] = 37.5f;
			LOOTBAG.exclusive[rEntry] = true;
			LOOTBAG.tier[rEntry] = 3;
			LOOTBAG.researchCategories[(int)DataCategory.loot_LowChanceAdv].Add(rEntry);
			LOOTBAG.researchCategories[(int)DataCategory.rewards_Hitchhiker].Add(rEntry);
			LOOTBAG.researchCategories[(int)DataCategory.loot_TierTwoPlus].Add(rEntry);
			LOOTBAG.researchType[rEntry] = ResearchType.module;
		}
		public static void modTavernTeaHouse(uint rEntry, byte r, byte g, byte b) {
			FFU_TL_Defs.checkModifiedEntry(rEntry, new Color(r, g, b));
			LOOTBAG.modules[rEntry] = new Color(r, g, b);
			LOOTBAG.researchCosts[rEntry] = 125000;
			LOOTBAG.researchTimes[rEntry] = 75f;
			LOOTBAG.exclusive[rEntry] = true;
			LOOTBAG.tier[rEntry] = 4;
			LOOTBAG.researchCategories[(int)DataCategory.rewards_Hitchhiker].Add(rEntry);
			LOOTBAG.researchCategories[(int)DataCategory.loot_TierTwoPlus].Add(rEntry);
			LOOTBAG.researchCategories[(int)DataCategory.loot_TierThree].Add(rEntry);
			LOOTBAG.researchType[rEntry] = ResearchType.module;
		}
		public static void modTavernLuxury(uint rEntry, byte r, byte g, byte b) {
			FFU_TL_Defs.checkModifiedEntry(rEntry, new Color(r, g, b));
			LOOTBAG.modules[rEntry] = new Color(r, g, b);
			LOOTBAG.researchCosts[rEntry] = 375000;
			LOOTBAG.researchTimes[rEntry] = 225f;
			LOOTBAG.exclusive[rEntry] = true;
			LOOTBAG.tier[rEntry] = 4;
			LOOTBAG.researchCategories[(int)DataCategory.rewards_Hitchhiker].Add(rEntry);
			LOOTBAG.researchCategories[(int)DataCategory.loot_TierTwoPlus].Add(rEntry);
			LOOTBAG.researchCategories[(int)DataCategory.loot_TierThree].Add(rEntry);
			LOOTBAG.researchType[rEntry] = ResearchType.module;
		}
	}
}

namespace CoOpSpRpG {
    public class patch_AgentTracker : AgentTracker {
        public extern List<BarAgentDrawer> orig_getBarAgents(ulong stationID, Point grid);
        public List<BarAgentDrawer> getBarAgents(ulong stationID, Point grid) {
        /// Allow player to recruit crew from all owned bars.
            if (PLAYER.avatar?.currentCosm?.ship?.faction == PLAYER.avatar.faction) return orig_getBarAgents(PLAYER.currentGame.homeBaseId, grid);
            else return orig_getBarAgents(stationID, grid);
        }
    }
	/*public class patch_BarScreen : BarScreen {
		[MonoModIgnore] private int hoverSpot;
		[MonoModIgnore] private Keys[] oldKeys;
		[MonoModIgnore] private Clickable close;
		[MonoModIgnore] private Vector2 mousePos;
		[MonoModIgnore] private MouseState oldMouse;
		[MonoModIgnore] private KeyboardState oldState;
		[MonoModIgnore] private BarAgentDrawer hoverAgent;
		[MonoModIgnore] private List<BarAgentDrawer> crew;
		[MonoModIgnore] private BarAgentDrawer activeConvo;
		[MonoModIgnore] private List<BarAgentDrawer> agents;
		[MonoModIgnore] private ScreenOverlay inventoryOverlay;
		[MonoModIgnore] private StationInfoRollout stationInfo;
		[MonoModIgnore] private void positionThings() { }
		[MonoModIgnore] public patch_BarScreen(GraphicsDevice device, string name) : base(device, name) { }
		[MonoModReplace] private void updateInput(float elapsed) {
		/// Allow player to change crew members equipment.
			KeyboardState state = Keyboard.GetState();
			MouseState state2 = Mouse.GetState();
			Keys[] pressedKeys = state.GetPressedKeys();
			mousePos.X = state2.X;
			mousePos.Y = state2.Y;
			Rectangle test = new Rectangle(state2.X, state2.Y, 1, 1);
			SCREEN_MANAGER.toolTip = null;
			if (activeConvo == null && inventoryOverlay == null) {
				hoverAgent = null;
				hoverSpot = 0;
				for (int i = 0; i < agents.Count; i++) if (agents[i].hover(test, elapsed)) hoverAgent = agents[i];
				for (int j = 0; j < this.crew.Count; j++) if (this.crew[j].hover(test, elapsed)) hoverAgent = this.crew[j];
				close.hover(test);
				if (state2.LeftButton == ButtonState.Released && oldMouse.LeftButton == ButtonState.Pressed) {
					if (hoverAgent != null) {
						hoverSpot = hoverAgent.getClick(test);
						switch (hoverSpot) {
							case 1:
								if (PLAYER.currentSession.GetType() == typeof(BattleSessionSC)) {
									(PLAYER.currentSession as BattleSessionSC).requestConversation(hoverAgent.agent.name);
									break;
								}
								activeConvo = hoverAgent;
								hoverAgent.agent.startConvo();
								break;
							case 3: {
								Crew[] array = PLAYER.currentGame.team.crew;
								foreach (Crew crew in array) {
									if (crew == null || !(crew.name == hoverAgent.agent.name)) continue;
									if (crew.currentCosm != null) crew.currentCosm.crew.TryRemove(crew.id, out var _);
									crew._kill();
									foreach (BarAgentDrawer item in this.crew) {
										if (item.agent.name == crew.name) {
											agents.Add(item);
											this.crew.Remove(item);
											break;
										}
									}
									PLAYER.currentGame.updateCrew();
									positionThings();
								}
								break;
							}
							case 4: {
								Crew crew2 = hoverAgent.agent.getCrew();
								crew2.name = hoverAgent.agent.name;
								crew2.team = PLAYER.currentTeam;
								crew2.faction = PLAYER.avatar.faction;
								crew2.factionless = false;
								crew2.pendingPosition = PLAYER.avatar.position;
								PROCESS_REGISTER.currentCosm.addCrew(crew2);
								PLAYER.currentGame.team.reportStatus(crew2);
								foreach (BarAgentDrawer agent in agents) {
									if (agent.agent.name == crew2.name) {
										agents.Remove(agent);
										this.crew.Add(agent);
										break;
									}
								}
								PLAYER.currentGame.updateCrew();
								positionThings();
								break;
							}
							case 5:
								if (hoverAgent.agent.crewTemplate != null) {
									CargoBay firstCargo = PLAYER.avatar?.currentCosm?.modules?.First(x => x.type == ModuleType.cargo_bay && x as CargoBay != null) as CargoBay;
									Crew refCrew = PLAYER.currentGame.team.crew.First(x => x.name == hoverAgent.agent.name);
									if (firstCargo != null && refCrew != null) inventoryOverlay = new CharacterInventory(hoverAgent.agent.getCrew(), firstCargo.storage, StorageAuthority.full);
									else inventoryOverlay = new CharacterInventory(hoverAgent.agent.getCrew(), StorageAuthority.full);
								}
								break;
						}
					} else if (test.Intersects(close.region)) SCREEN_MANAGER.goto_screen("Ship Navigation"); 
					else if (test.Intersects(stationInfo.researchHotspot) && PLAYER.currentGame != null) SCREEN_MANAGER.push_screen("research");
				}
				if (!state.IsKeyDown(Keys.Escape) && oldState.IsKeyDown(Keys.Escape)) SCREEN_MANAGER.goto_screen("Ship Navigation");
				if (CONFIG.keyBindings[29].wasJustReleased(oldKeys, pressedKeys, oldMouse, state2)) SCREEN_MANAGER.goto_screen("Ship Navigation");
				if (CONFIG.keyBindings[30].wasJustReleased(oldKeys, pressedKeys, oldMouse, state2) && PLAYER.currentGame != null) SCREEN_MANAGER.push_screen("research");
			} else if (inventoryOverlay != null) {
				try {
					if (inventoryOverlay.update(elapsed)) inventoryOverlay = null;
				} catch {
					inventoryOverlay = null;
				}
			}
			oldKeys = pressedKeys;
			oldState = state;
			oldMouse = state2;
		}
	}*/
}
