#pragma warning disable CS0169
#pragma warning disable CS0414
#pragma warning disable CS0649

using MonoMod;
using CoOpSpRpG;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System;

namespace FFU_Terra_Liberatio {
    public class FFU_TL_Tile_Logistics {
        public static void updateModules(Dictionary<byte, Dictionary<byte, Dictionary<byte, Module>>> modules) {
            ModLog.Message($"Applying module changes: Logistics Terminals.");
            modLogisticsRoom(modules, 145, 151, 24, 0);
            modLogisticsRoom(modules, 145, 152, 24, 1);
            modLogisticsRoom(modules, 145, 153, 24, 5);
            modLogisticsRoom(modules, 145, 154, 24, 6);
		}
		public static void updateResearch() {
			ModLog.Message($"Applying research changes: Logistics Terminals.");
			modLogisticsRoom(400635U, 145, 151, 24);
		}
		public static void modLogisticsRoom(Dictionary<byte, Dictionary<byte, Dictionary<byte, Module>>> modules, byte r, byte g, byte b, byte rot) {
            FFU_TL_Defs.unlistDynamic.Add(new Color(r, g, b));
            FFU_TL_Defs.unlistDynamic = FFU_TL_Defs.unlistDynamic.ToList();
            modLogisticsRoom(modules[r][g][b] as ScreenAccess, rot);
        }
        public static void modLogisticsRoom(ScreenAccess logRoom, byte rot) {
            logRoom.cost = 175000;
			logRoom.rotation = rot;
            logRoom.toolTip = $"Logistics Terminal";
            logRoom.techLevel = 4;
            logRoom.tip = new ToolTip();
            logRoom.tip.tip = logRoom.toolTip;
            logRoom.tip.botLeftText = $"Deep Space Engineering Inc.";
			logRoom.tip.description = $"Specialized logistics terminal and assembly center in a single bundle. Allows spaceships and stations to construct other vessels, given required resources are available.";
            logRoom.tip.tierIcontype = (TierIcon)logRoom.techLevel;
            logRoom.tip.addStat($"Construction Drones", "8", false);
            logRoom.tip.addStat($"Resource Efficiency", "100%", false);
		}
		public static void modLogisticsRoom(uint rEntry, byte r, byte g, byte b) {
			FFU_TL_Defs.checkModifiedEntry(rEntry, new Color(r, g, b));
			LOOTBAG.modules[rEntry] = new Color(r, g, b);
			LOOTBAG.researchCosts[rEntry] = 2625000;
			LOOTBAG.researchTimes[rEntry] = 1575f;
			LOOTBAG.exclusive[rEntry] = true;
			LOOTBAG.tier[rEntry] = 4;
			LOOTBAG.researchCategories[(int)DataCategory.rewards_Hitchhiker].Add(rEntry);
			LOOTBAG.researchCategories[(int)DataCategory.loot_TierThree].Add(rEntry);
			LOOTBAG.researchType[rEntry] = ResearchType.module;
		}

	}
}

namespace CoOpSpRpG {
	[MonoModReplace] public class LogisticsRoomAnim : ModuleAnimation {
		private SheetAnimation sheet;
		private Vector2 dest;
		private float pauseTimer;
		public Texture2D art;
		public LogisticsRoomAnim(Rectangle bounding) {
			rotation = 0;
			art = SCREEN_MANAGER.AnimSheets[31];
			sheet = new SheetAnimation(45, 90, 19, 0, 71f / (678f * (float)Math.PI), 95, true);
			dest = new Vector2(bounding.X + 170, bounding.Y + 90);
		}
		public LogisticsRoomAnim(byte r, Rectangle bounding) {
			rotation = r;
			art = SCREEN_MANAGER.AnimSheets[31];
			sheet = new SheetAnimation(45, 90, 19, 0, 71f / (678f * (float)Math.PI), 95, true);
			switch (r) {
				default: dest = new Vector2(bounding.X + 170, bounding.Y + 90); break;
				case 1: dest = new Vector2(bounding.X + 89 + 45, bounding.Y + 170); break;
				case 5: dest = new Vector2(bounding.X + 170, bounding.Y + 89); break;
				case 6: dest = new Vector2(bounding.X + 90 + 45, bounding.Y + 170); break;
			}
		}
		public override void update(float elapsed) {
			if (sheet.index == 87) {
				if (pauseTimer > 1f) sheet.update(elapsed);
				else pauseTimer += elapsed;
			} else if (sheet.index == 0) {
				if (pauseTimer > 7f) sheet.update(elapsed);
				else pauseTimer += elapsed;
			} else {
				pauseTimer = 0f;
				sheet.update(elapsed);
			}
		}
		public override void drawUnderAdd(SpriteBatch batch) {
			switch (rotation) {
				default: batch.Draw(art, dest, sheet.source, Color.White, 0f, new Vector2(0f, 0f), 1f, SpriteEffects.None, 0f); break;
				case 1: batch.Draw(art, dest, sheet.source, Color.White, (float)Math.PI / 2f, new Vector2(0f, 0f), 1f, SpriteEffects.None, 0f); break;
				case 5: batch.Draw(art, dest, sheet.source, Color.White, 0f, new Vector2(0f, 0f), 1f, SpriteEffects.None, 0f); break;
				case 6: batch.Draw(art, dest, sheet.source, Color.White, (float)Math.PI / 2f, new Vector2(0f, 0f), 1f, SpriteEffects.None, 0f); break;
			}
		}
	}
}

/*namespace CoOpSpRpG {
    public class patch_LogisticsScreenRev3 : LogisticsScreenRev3 {
		[MonoModIgnore] private KeyboardState oldState;
		[MonoModIgnore] private MouseState oldMouse;
		[MonoModIgnore] private Keys[] oldKeys;
		[MonoModIgnore] private Vector2 mousePos;
		[MonoModIgnore] private bool pause;
		[MonoModIgnore] private int screenHeight;
		[MonoModIgnore] private WidgetLogistic widgetLogistic;
		[MonoModIgnore] private Vector2 mouse;
		[MonoModIgnore] private int influenceRange;
		[MonoModIgnore] private float minScale;
		[MonoModIgnore] private float maxScale;
		[MonoModIgnore] private float scrollFactor;
		[MonoModIgnore] private Vector3 cameraPos;
		[MonoModIgnore] private Vector3 cameraTarget;
		[MonoModIgnore] private Matrix view;
		[MonoModIgnore] private Matrix project;
		[MonoModIgnore] private float aspectRatio;
		[MonoModIgnore] private Plane shipPlane;
		[MonoModIgnore] private Ship selected;
		[MonoModIgnore] private Ship hover;
		[MonoModIgnore] private Clickable[] buttons;
		[MonoModIgnore] private bool drawSpawn;
		[MonoModIgnore] private DropDown activeMenu;
		[MonoModIgnore] private List<MetaNode> metaData;
		[MonoModIgnore] private MetaNode menuCallback;
		[MonoModIgnore] private TextInput cloneName;
		[MonoModIgnore] private void applyTurrets() { }
		[MonoModIgnore] private void setMountMenu() { }
		[MonoModIgnore] private void setTip(TurretType type) { }
		[MonoModIgnore] private void doRightClick(string opt) { }
		[MonoModIgnore] private bool applyTurret(MountMenu node, BattleSessionSC session) { return false; }
		[MonoModIgnore] private void updateButtons(MouseState newMouse, Rectangle clickPos) { }
		[MonoModIgnore] public patch_LogisticsScreenRev3(GraphicsDevice device, string name) : base(device, name) { }
		[MonoModReplace] public void updateInput(float elapsed) {
		/// Option to unload your own ship via Logistics Terminal into global resource stash.
			KeyboardState state = Keyboard.GetState();
			MouseState state2 = Mouse.GetState();
			Rectangle rectangle = new Rectangle(state2.X, state2.Y, 1, 1);
            Keys[] pressedKeys = state.GetPressedKeys();
			MouseAction clickState = MouseAction.none;
			if (state2.LeftButton == ButtonState.Released) {
				clickState = MouseAction.leftRelease;
			}
			if (state2.LeftButton == ButtonState.Released && oldMouse.LeftButton == ButtonState.Pressed) {
				clickState = MouseAction.leftClick;
			}
			if (state2.LeftButton == ButtonState.Pressed && oldMouse.LeftButton == ButtonState.Pressed) {
				clickState = MouseAction.leftDrag;
			}
			if (state2.RightButton == ButtonState.Pressed && oldMouse.RightButton == ButtonState.Pressed) {
				clickState = MouseAction.rightDrag;
			}
			if (state2.RightButton == ButtonState.Released && oldMouse.RightButton == ButtonState.Pressed) {
				clickState = MouseAction.rightClick;
			}
			if (state2.LeftButton == ButtonState.Released && oldMouse.LeftButton == ButtonState.Pressed && Keyboard.GetState().IsKeyDown(Keys.LeftShift)) {
				clickState = MouseAction.shiftLeftClick;
			}
			Clickable[] array = buttons;
			for (int i = 0; i < array.Length; i++) {
				array[i].hover(rectangle);
			}
			if ((!state.IsKeyDown(Keys.Enter) || (!state.IsKeyDown(Keys.LeftAlt) && !state.IsKeyDown(Keys.RightAlt))) && oldState.IsKeyDown(Keys.Enter) && (oldState.IsKeyDown(Keys.LeftAlt) || oldState.IsKeyDown(Keys.RightAlt))) {
				CONFIG.toggleFullScreen();
			}
			if (state.IsKeyDown(Keys.PageUp)) {
				cameraPos.Z += 0.8f * cameraPos.Z * elapsed;
				cameraPos.Z = MathHelper.Clamp(cameraPos.Z, minScale, maxScale);
			}
			if (state.IsKeyDown(Keys.PageDown)) {
				cameraPos.Z -= 0.8f * cameraPos.Z * elapsed;
				cameraPos.Z = MathHelper.Clamp(cameraPos.Z, minScale, maxScale);
			}
			if (!widgetLogistic.widgetActive && CONFIG.keyBindings[29].wasJustReleased(oldKeys, pressedKeys, oldMouse, state2)) {
				PLAYER.goInside = true;
			}
			if (CONFIG.keyBindings[33].wasJustReleased(oldKeys, pressedKeys, oldMouse, state2)) {
				SCREEN_MANAGER.showUI = !SCREEN_MANAGER.showUI;
			}
			if (!state.IsKeyDown(Keys.Escape) && oldState.IsKeyDown(Keys.Escape)) {
				if (widgetLogistic.widgetActive) {
					widgetLogistic.Close();
				} else {
					SCREEN_MANAGER.push_screen("pause");
				}
			}
			if (state2.ScrollWheelValue != oldMouse.ScrollWheelValue && !widgetLogistic.widgetActive) {
				cameraPos.Z -= (float)(state2.ScrollWheelValue - oldMouse.ScrollWheelValue) * scrollFactor * cameraPos.Z;
				cameraPos.Z = MathHelper.Clamp(cameraPos.Z, minScale, maxScale);
			} else {
				widgetLogistic.scrollBase.scrollWhellDelta = oldMouse.ScrollWheelValue - state2.ScrollWheelValue;
			}
			mouse = new Vector2(state2.X, state2.Y);
			if (drawSpawn) {
				cameraPos.X = PLAYER.currentShip.position.X;
				cameraPos.Y = PLAYER.currentShip.position.Y;
			}
			cameraTarget = cameraPos;
			cameraTarget.Z = 0f;
			view = Matrix.CreateLookAt(cameraPos, cameraTarget, Vector3.Up);
			project = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 4f, aspectRatio, 1f, 1000000f);
			float y = -(state2.Y - screenHeight);
			Vector3 vector = SCREEN_MANAGER.Device.Viewport.Unproject(new Vector3(state2.X, y, 0f), project, view, Matrix.Identity);
			Vector3 vector2 = SCREEN_MANAGER.Device.Viewport.Unproject(new Vector3(state2.X, y, 1f), project, view, Matrix.Identity);
			Ray ray = new Ray(vector, Vector3.Normalize(vector2 - vector));
			float? num = ray.Intersects(shipPlane);
			Vector3 vector3 = (num.HasValue ? (ray.Position + ray.Direction * num.Value) : Vector3.Zero);
			mousePos = new Vector2(vector3.X, vector3.Y);
			viewableArea.Height = (int)((double)(2f * cameraPos.Z) * Math.Tan(0.78539818525314331));
			viewableArea.Width = (int)((float)viewableArea.Height * aspectRatio);
			viewableArea.X = (int)(cameraPos.X - (float)(viewableArea.Width / 2));
			viewableArea.Y = (int)(cameraPos.Y - (float)(viewableArea.Height / 2));
			Rectangle clickPos = new Rectangle((int)mousePos.X, (int)mousePos.Y, 1, 1);
			updateButtons(state2, rectangle);
			if (!drawSpawn) {
				cloneName.hasFocus = false;
				hover = null;
				foreach (Ship value in PLAYER.currentSession.allShips.Values) {
					if (clickPos.Intersects(value.bBox) && (value.faction == PLAYER.avatar.faction || value.faction == CONFIG.deadShipFaction) && Vector2.Distance(value.position, PLAYER.currentShip.position) < (float)influenceRange) {
						hover = value;
						break;
					}
				}
				if (hover == null && clickPos.Intersects(PLAYER.currentShip.bBox) && PLAYER.currentShip.testCollision(mousePos)) {
					hover = PLAYER.currentShip;
				}
				if (activeMenu != null) {
					activeMenu.hover(clickPos);
				} else {
					TurretType tip = TurretType.none;
					foreach (MetaNode metaDatum in metaData) {
						if (clickPos.Intersects(metaDatum.region) && metaDatum.type == MetaNodeType.MountMenu) {
							tip = (metaDatum as MountMenu).selected;
						}
					}
					setTip(tip);
				}
				if (!widgetLogistic.widgetActive) {
					if ((state.IsKeyDown(Keys.D) || state.IsKeyDown(Keys.Right)) && cameraPos.X < PLAYER.currentShip.position.X + (float)influenceRange) {
						cameraPos.X += (float)Math.Round(600f * elapsed);
					}
					if ((state.IsKeyDown(Keys.A) || state.IsKeyDown(Keys.Left)) && cameraPos.X > PLAYER.currentShip.position.X - (float)influenceRange) {
						cameraPos.X -= (float)Math.Round(600f * elapsed);
					}
					if ((state.IsKeyDown(Keys.S) || state.IsKeyDown(Keys.Down)) && cameraPos.Y < PLAYER.currentShip.position.Y + (float)influenceRange) {
						cameraPos.Y += (float)Math.Round(600f * elapsed);
					}
					if ((state.IsKeyDown(Keys.W) || state.IsKeyDown(Keys.Up)) && cameraPos.Y > PLAYER.currentShip.position.Y - (float)influenceRange) {
						cameraPos.Y -= (float)Math.Round(600f * elapsed);
					}
				} else if (!widgetLogistic.popupActive) {
					if (oldState.IsKeyDown(Keys.W) && state.IsKeyUp(Keys.W)) {
						widgetLogistic.scrollBase.nextSelectedVariant(null);
					}
					if (oldState.IsKeyDown(Keys.S) && state.IsKeyUp(Keys.S)) {
						widgetLogistic.scrollBase.prevSelectedVariant(null);
					}
					if (oldState.IsKeyDown(Keys.D) && state.IsKeyUp(Keys.D)) {
						widgetLogistic.scrollBase.nextEntry();
					}
					if (oldState.IsKeyDown(Keys.A) && state.IsKeyUp(Keys.A)) {
						widgetLogistic.scrollBase.prevEntry();
					}
				}
				widgetLogistic.Update(elapsed, clickState, rectangle, 1f);
				if (SCREEN_MANAGER.toolTip != null) {
					SCREEN_MANAGER.toolTip.position = mouse;
				}
				if (!pause && state2.RightButton == ButtonState.Released && oldMouse.RightButton == ButtonState.Pressed) {
					if (activeMenu != null) {
						string text = activeMenu.processClick(clickPos);
						if (text != null && menuCallback != null) {
							if (menuCallback.type == MetaNodeType.MountMenu) {
								MountMenu mountMenu = menuCallback as MountMenu;
								TurretType key = mountMenu.selected;
								mountMenu.select(text);
								if (PLAYER.currentSession.GetType() == typeof(BattleSessionSC)) {
									if (!applyTurret(mountMenu, PLAYER.currentSession as BattleSessionSC)) {
										mountMenu.select(TURRET_BAG.turNames[key]);
									}
								} else {
									applyTurrets();
								}
							}
						} else if (text != null) {
							doRightClick(text);
						}
						activeMenu = null;
						menuCallback = null;
					} else if (hover != null) {
						activeMenu = new DropDown(mousePos);
						if (PLAYER.currentShip.docked.Contains(hover)) {
							activeMenu.addOption("Scrap", null);
							activeMenu.addOption("Repair", null);
							if ((hover.cosm != null && hover.cosm.cargoBays.Count > 0) || (hover.data != null && hover.data.storage != null)) {
								activeMenu.addOption("Unload cargo", null);
								activeMenu.addOption("Provision", null);
							} else if (PLAYER.currentSession.GetType() == typeof(BattleSessionSC)) {
								activeMenu.addOption("Unload cargo", null);
							}
							if (hover.data == null || !hover.data.reload) {
								activeMenu.addOption("Restock Ammo", null);
							}
							if (PLAYER.currentSession.GetType() == typeof(BattleSessionSC) && (PLAYER.currentSession as BattleSessionSC).packPossible) {
								activeMenu.addOption("Pack ship", null);
							}
							selected = hover;
							setMountMenu();
						} else if (!CONFIG.openMP && hover.constructionAnim == null && hover != PLAYER.currentShip) {
							if (PLAYER.currentShip.docked.Count < PLAYER.currentShip.docking.Count) {
								activeMenu.addOption("Tractor to dock", null);
								selected = hover;
								setMountMenu();
							}
						} else if (hover == PLAYER.currentShip) {
							activeMenu.addOption("Unload cargo", null);
							selected = hover;
							setMountMenu();
						} else {
							activeMenu = null;
						}
					}
				}
				if (!pause && state2.LeftButton == ButtonState.Released && oldMouse.LeftButton == ButtonState.Pressed) {
					if (activeMenu != null) {
						string text2 = activeMenu.processClick(clickPos);
						if (text2 != null && menuCallback != null) {
							if (menuCallback.type == MetaNodeType.MountMenu) {
								MountMenu mountMenu2 = menuCallback as MountMenu;
								mountMenu2.select(text2);
								if (PLAYER.currentSession.GetType() == typeof(BattleSessionSC)) {
									applyTurret(mountMenu2, PLAYER.currentSession as BattleSessionSC);
								} else {
									applyTurrets();
								}
							}
						} else if (text2 != null) {
							doRightClick(text2);
						}
						activeMenu = null;
						menuCallback = null;
					} else {
						bool flag = false;
						foreach (MetaNode metaDatum2 in metaData) {
							if (clickPos.Intersects(metaDatum2.region) && metaDatum2.type == MetaNodeType.MountMenu) {
								MountMenu mountMenu3 = metaDatum2 as MountMenu;
								activeMenu = mountMenu3.getMenu(metaDatum2.position);
								menuCallback = mountMenu3;
								flag = true;
							}
						}
						if (!flag && selected != hover) {
							selected = hover;
							setMountMenu();
						}
					}
				}
			}
			oldKeys = pressedKeys;
			oldState = state;
			oldMouse = state2;
		}
	}
}*/
