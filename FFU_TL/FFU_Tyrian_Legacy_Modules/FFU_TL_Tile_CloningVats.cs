#pragma warning disable CS0108
#pragma warning disable CS0169
#pragma warning disable CS0414
#pragma warning disable CS0436
#pragma warning disable CS0649
#pragma warning disable CS0626

using MonoMod;
using CoOpSpRpG;
using FFU_Tyrian_Legacy;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace FFU_Tyrian_Legacy {
    public class FFU_TL_Tile_CloningVats {
        public static void updateModules(Dictionary<byte, Dictionary<byte, Dictionary<byte, Module>>> modules) {
            ModLog.Message($"Applying module changes: Cloning Vats...");
            modCloningVat(modules, 145, 249, 25);
        }
        public static void assignCustomCosts(Dictionary<byte, Dictionary<byte, Dictionary<byte, Module>>> modules, Dictionary<Module, Dictionary<InventoryItemType, float>> rRes, Dictionary<Module, Dictionary<InventoryItemType, float>> rExtra) {
            ModLog.Message($"Applying custom resource costs: Cloning Vats...");
            modCloningVat(modules[145][249][25], rRes, rExtra);
        }
        public static void updateResearch() {
            ModLog.Message($"Applying research changes: Cloning Vats...");
            modCloningVat(400636U);
        }
        public static void modCloningVat(Dictionary<byte, Dictionary<byte, Dictionary<byte, Module>>> modules, byte r, byte g, byte b) {
            FFU_TL_Defs.unlistDynamic.Add(new Color(r, g, b));
            FFU_TL_Defs.unlistDynamic = FFU_TL_Defs.unlistDynamic.ToList();
            modCloningVat(modules[r][g][b] as CloningVat);
        }
        public static void modCloningVat(CloningVat cVat) {
            cVat.cost = 750000;
            cVat.toolTip = $"Cloning Bay A-13";
            cVat.techLevel = 4;
            cVat.tip = new ToolTip();
            cVat.tip.tip = cVat.toolTip;
            cVat.tip.botLeftText = $"CERN Engineering Division";
            cVat.tip.description = $"Experimental, nearly forbidden tech. Despite 'A' in name standing for 'Alpha', mechanism is rather stable and has little to no consequences due to use. Uses infomorph synchronization principle to ensure that individual consciousness is transferred to a new body at moment of death, instead of being copied along with the old body.";
            cVat.tip.tierIcontype = (TierIcon)cVat.techLevel;
            cVat.tip.addStat($"Experimental Prototype", "Yes", false);
            cVat.tip.addStat($"Classified Technology", "Yes", false);
            cVat.tip.addStat($"Forbidden Technology", "Yes", false);
            cVat.tip.addStat($"Transfer Method", "Infomorph", false);
            cVat.tip.addStat($"Transfer Reliability", "99.92%", false);
            cVat.tip.addStat($"Consciousness Sync", "100%", false);
        }
        public static void modCloningVat(Module rMod, Dictionary<Module, Dictionary<InventoryItemType, float>> rRes, Dictionary<Module, Dictionary<InventoryItemType, float>> rExtra) {
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
            TILEBAG.AssignResources(rMod);
        }
        public static void modCloningVat(uint rEntry) {
            FFU_TL_Defs.checkExistingResearch(rEntry);
            FFU_TL_Defs.checkResearchDupe(new Color(145, 249, 25));
            LOOTBAG.modules[rEntry] = new Color(145, 249, 25);
            LOOTBAG.researchCosts[rEntry] = 4250000;
            LOOTBAG.researchTimes[rEntry] = 2550f;
            LOOTBAG.exclusive[rEntry] = true;
            LOOTBAG.tier[rEntry] = 4;
            LOOTBAG.researchCategories[(int)DataCategory.rewards_Hitchhiker].Add(rEntry);
            LOOTBAG.researchCategories[(int)DataCategory.loot_TierThree].Add(rEntry);
            LOOTBAG.researchType[rEntry] = ResearchType.module;
        }
    }
}

namespace CoOpSpRpG {
    [MonoModReplace] public class CloningVat : Module, Activateable {
    /// Clone Registration for Clone Jumping via death.
        public Vector2 spawnOffset;
        public CloningVat(ModTile[] list) : base(list) {
            type = ModuleType.cloning_vat;
            active = true;
        }
        public CloningVat() {
            type = ModuleType.cloning_vat;
            active = true;
        }
        public string getTip() {
            return "Register Clone";
        }
        public void setActiveTiles(MicroCosm cosm) {
            tiles[54].setTrigger(this);
        }
        public void activate(Crew crew, MicroCosm cosm, Vector2 target) {
            if (functioning && SCREEN_MANAGER.widgetChat != null) {
                SCREEN_MANAGER.widgetChat.AddMessage("Registration mechanism still doesn't work. Please be patient!", MessageTarget.Command);
            }
        }
        public void trip(ShipActor crew, MicroCosm cosm) { }
        public override void animate(float elapsed) { }
    }
    public class patch_Respawning : Respawning {
        private Dictionary<byte, ulong> reviveShipIDs;
        [MonoModIgnore] private KeyboardState oldState;
        [MonoModIgnore] private MouseState oldMouse;
        [MonoModIgnore] private Vector2 screenCenter;
        [MonoModIgnore] private Vector2 mousePos;
        [MonoModIgnore] private List<Clickable> buttons;
        [MonoModIgnore] private Dictionary<byte, Crew> replaceOptions;
        [MonoModIgnore] private Dictionary<byte, bool> replaceFree;
        [MonoModIgnore] private float wasteTimer;
        [MonoModIgnore] public patch_Respawning(GraphicsDevice device, string name) : base(device, name) { }
        [MonoModReplace] private void doButtonSetup() {
        /// Add owned ship IDs with active Cloning Vats to the respawn list.
            reviveShipIDs = new Dictionary<byte, ulong>();
            Vector2 where = screenCenter;
            where.X -= 50f;
            AnimatedButton item = new AnimatedButton("Revive at Home Station", where, 0, red: false);
            where.Y += 50f;
            buttons.Add(item);
            byte b = 1;
            Color refCloneVat = new Color(145, 249, 25);
            var rSessions = (PLAYER.currentWorld as WorldRev3).openSessions;
            foreach (var rSession in rSessions)
                foreach (var rShip in rSession.allShips.Values) 
                    if (rShip.ownershipHistory.Contains(PLAYER.avatar.faction) && rShip.botD.Contains(refCloneVat) && !FFU_TL_Defs.reviveShips.ContainsKey(rShip.id)) 
                        FFU_TL_Defs.reviveShips.Add(rShip.id, rShip);
            foreach (var rShip in FFU_TL_Defs.reviveShips) 
                if (rShip.Value == null) FFU_TL_Defs.reviveShips.Remove(rShip.Key);
            foreach (var rShip in FFU_TL_Defs.reviveShips) {
                if (rShip.Value.ownershipHistory.Contains(PLAYER.avatar.faction)) {
                    if (rShip.Value.botD.Contains(refCloneVat)) reviveShipIDs[b] = rShip.Key;
                    item = new AnimatedButton($"Revive at Ship #{rShip.Key} {rShip.Value.Height / 2f:0}m/{rShip.Value.Width / 2f:0}m", where, b, red: false);
                    where.Y += 50f;
                    buttons.Add(item);
                    b = (byte)(b + 1);
                }
            }
            if (PLAYER.currentGame.team.crew == null || PLAYER.currentGame.team.crew.Length == 0) return;
            Crew[] crewList = PLAYER.currentGame.team.crew;
            foreach (Crew crewMember in crewList) {
                if (crewMember == null || crewMember.state == CrewState.dead || crewMember.currentCosm == null || crewMember.currentCosm.ship == null || !(crewMember.currentCosm.ship.grid == PLAYER.currentSession.grid)) {
                    continue;
                }
                string reviveCrew = "Revive Instead: ";
                replaceFree[b] = false;
                if (crewMember.currentCosm.ship.id == PLAYER.currentTeam.ownedShip) {
                    foreach (Module module in crewMember.currentCosm.modules) {
                        if (module.type == ModuleType.med_bay && module.functioning) {
                            replaceFree[b] = true;
                            reviveCrew = "Revive at Medical Bay: ";
                            break;
                        }
                    }
                }
                item = new AnimatedButton(reviveCrew + crewMember.name, where, b, red: false);
                where.Y += 50f;
                buttons.Add(item);
                replaceOptions[b] = crewMember;
                b = (byte)(b + 1);
            }
        }
        [MonoModReplace] private void updateInput(float elapsed) {
        /// Link chosen ship IDs from list to the reviveShipID variable.
            KeyboardState kState = Keyboard.GetState();
            MouseState mState = Mouse.GetState();
            Rectangle bRect = new Rectangle(mState.X, mState.Y, 1, 1);
            mousePos.X = mState.X;
            mousePos.Y = mState.Y;
            if (wasteTimer >= 10f) {
                FFU_TL_Defs.reviveShipID = 0;
                foreach (Clickable tButton in buttons) tButton.hover(bRect);
                if (mState.LeftButton == ButtonState.Released && oldMouse.LeftButton == ButtonState.Pressed) {
                    foreach (Clickable rButton in buttons) {
                        if (!bRect.Intersects(rButton.region)) continue;
                        if (rButton.action == 0) {
                            PLAYER.currentSession.corpses.Remove(PLAYER.avatar);
                            PLAYER.currentSession.unpause();
                            EVENTS.reportPlayerDeath(PLAYER.avatar);
                            SCREEN_MANAGER.bulletTime = 1f;
                            PLAYER.currentWorld.triggerPlayerSpawn();
                        } else if (reviveShipIDs.ContainsKey((byte)rButton.action)) {
                            FFU_TL_Defs.reviveShipID = reviveShipIDs[(byte)rButton.action];
                            PLAYER.currentSession.corpses.Remove(PLAYER.avatar);
                            PLAYER.currentSession.unpause();
                            EVENTS.reportPlayerDeath(PLAYER.avatar);
                            SCREEN_MANAGER.bulletTime = 1f;
                            PLAYER.currentWorld.triggerPlayerSpawn();
                        } else if (replaceOptions.ContainsKey((byte)rButton.action)) {
                            FFU_TL_Defs.reviveShipID = 0;
                            Crew crew = replaceOptions[(byte)rButton.action];
                            if (crew.currentCosm == PROCESS_REGISTER.currentCosm) {
                                PLAYER.avatar.health = 1;
                                PLAYER.avatar.state = CrewState.idle;
                                PLAYER.avatar.position = crew.position;
                                PLAYER.avatar.rotation = crew.rotation;
                                if (!replaceFree.ContainsKey((byte)rButton.action) || !replaceFree[(byte)rButton.action]) crew.kill();
                                else foreach (Module module in PROCESS_REGISTER.currentCosm.modules) {
                                    if (module.type == ModuleType.med_bay && module.functioning) {
                                        MedBay medBay = module as MedBay;
                                        PLAYER.avatar.position = medBay.centerPoint;
                                    }
                                }
                            } else if (crew.currentCosm != null && crew.currentCosm.ship != null) {
                                PLAYER.avatar.health = crew.health;
                                PLAYER.avatar.state = CrewState.idle;
                                PLAYER.jumpToShip(crew.currentCosm.ship, crew.position / 16f);
                                if (!replaceFree.ContainsKey((byte)rButton.action) || !replaceFree[(byte)rButton.action]) crew.kill();
                            }
                            SCREEN_MANAGER.bulletTime = 1f;
                            SCREEN_MANAGER.goto_screen("Ship Navigation");
                            PLAYER.currentSession.unpause();
                        }
                    }
                }
            }
            if ((!kState.IsKeyDown(Keys.Enter) || (!kState.IsKeyDown(Keys.LeftAlt) && !kState.IsKeyDown(Keys.RightAlt))) && oldState.IsKeyDown(Keys.Enter) && (oldState.IsKeyDown(Keys.LeftAlt) || oldState.IsKeyDown(Keys.RightAlt))) CONFIG.toggleFullScreen();
            if (!kState.IsKeyDown(Keys.Escape) && oldState.IsKeyDown(Keys.Escape)) SCREEN_MANAGER.push_screen("pause");
            oldState = kState;
            oldMouse = mState;
        }
    }
    public class patch_WorldRev3 : WorldRev3 {
        [MonoModIgnore] private void setEarPos() { }
        [MonoModIgnore] public patch_WorldRev3(string path, bool loadStuff) : base(path, loadStuff) { }
        [MonoModReplace] public void spawnPlayer(PlayerSaveInfo spawn) {
        /// Use the reviveShipID to revive player at designated location.
            if (PLAYER.currentGame != null && PLAYER.currentGame.team != null && FFU_TL_Defs.reviveShipID == 0) {
                PLAYER.currentGame.team.killAll();
            }
            if (PROCESS_REGISTER.currentCosm != null) {
                PROCESS_REGISTER.currentCosm.crew.TryRemove(PLAYER.avatar.id, out var _);
            }
            if (!spawn.found) {
                if (PLAYER.avatar != null) {
                    PLAYER.avatar.currentCosm = null;
                    PLAYER.avatar.state = CrewState.idle;
                    PLAYER.avatar.health = byte.MaxValue;
                    PLAYER.avatar.faction = CONFIG.playerFaction;
                } else {
                    PLAYER.avatar = CHARACTER_DATA.getCrew();
                }
                Ship ship = null;
                Station station = null;
                if (FFU_TL_Defs.reviveShipID > 0) {
                    ship = FFU_TL_Defs.reviveShips[FFU_TL_Defs.reviveShipID];
                    PLAYER.currentSession = getSession(ship.grid);
                } else PLAYER.currentSession = getSession(CONFIG.spHomeGrid);
                foreach (Station station2 in PLAYER.currentSession.stations) {
                    if (station2.id == PLAYER.currentGame.homeBaseId) {
                        station = station2;
                        break;
                    }
                }
                if (ship != null) {
                    if (ship.cosm == null) {
                        MicroCosm cosm = new MicroCosm(ship, ship.topD, ship.botD, 0);
                        ship.cosm = cosm;
                    }
                    PLAYER.avatar.position = ship.cosm.validClonePosition();
                    PLAYER.avatar.pendingPosition = PLAYER.avatar.position;
                    ship.cosm.addCrew(PLAYER.avatar);
                    PLAYER.currentShip = ship;
                    PROCESS_REGISTER.currentCosm = ship.cosm;
                    PLAYER.avatar.currentCosm = ship.cosm;
                    PLAYER.avatar.team = PLAYER.currentTeam;
                    ship.cosm.init();
                    PLAYER.animateRespawn = true;
                    setEarPos();
                    SCREEN_MANAGER.goto_screen("Ship Navigation");
                    FFU_TL_Defs.reviveShipID = 0;
                } else if (station != null) {
                    if (station.cosm == null) {
                        MicroCosm cosm = new MicroCosm(station, station.topD, station.botD, 0);
                        station.cosm = cosm;
                    }
                    PLAYER.avatar.position = station.cosm.validClonePosition();
                    PLAYER.avatar.pendingPosition = PLAYER.avatar.position;
                    station.cosm.addCrew(PLAYER.avatar);
                    PLAYER.currentShip = station;
                    PROCESS_REGISTER.currentCosm = station.cosm;
                    PLAYER.avatar.currentCosm = station.cosm;
                    PLAYER.avatar.team = PLAYER.currentTeam;
                    station.cosm.init();
                    PLAYER.animateRespawn = true;
                    setEarPos();
                    SCREEN_MANAGER.goto_screen("Ship Navigation");
                    FFU_TL_Defs.reviveShipID = 0;
                } else {
                    SCREEN_MANAGER.widgetChat.AddMessage("A bug seems to have caused your home station to stop existing so you are being dumped into empty space. Sorry for the inconvenience.", MessageTarget.Command);
                    PLAYER.avatar.position = Vector2.Zero;
                    PLAYER.currentSession.corpses.Add(PLAYER.avatar);
                    FFU_TL_Defs.reviveShipID = 0;
                }
                return;
            }
            PLAYER.currentSession = getSession(spawn.grid);
            if (spawn.eva) {
                PLAYER.avatar = CHARACTER_DATA.getCrew();
                PLAYER.avatar.position = spawn.position;
                PLAYER.currentSession.corpses.Add(PLAYER.avatar);
                setEarPos();
                return;
            }
            if (spawn.ship != null) {
                PLAYER.currentShip = new WorldActor(spawn.ship).getShip(0);
                PLAYER.currentSession.addLocalShip(PLAYER.currentShip, SessionEntry.flyin);
                if (PLAYER.currentShip.cosm == null) {
                    MicroCosm microCosm2 = new MicroCosm(PLAYER.currentShip, PLAYER.currentShip.topD, PLAYER.currentShip.botD, 0);
                    PLAYER.currentShip.cosm = microCosm2;
                    PLAYER.currentShip.artOrigin = microCosm2.centerOfMass;
                }
                PROCESS_REGISTER.currentCosm = PLAYER.currentShip.cosm;
                if (PLAYER.currentShip.cosm.crew.ContainsKey(spawn.crewId)) {
                    Crew crew = PLAYER.currentShip.cosm.crew[spawn.crewId];
                    PLAYER.avatar = CHARACTER_DATA.getCrew();
                    PLAYER.avatar.position = crew.position;
                    PLAYER.avatar.rotation = crew.rotation;
                    PLAYER.avatar.id = crew.id;
                    PLAYER.avatar.team = PLAYER.currentTeam;
                    PLAYER.avatar.health = crew.health;
                    PLAYER.avatar.shieldPoints = crew.shieldPoints;
                    PLAYER.currentShip.cosm.crew[spawn.crewId] = PLAYER.avatar;
                    PLAYER.avatar.currentCosm = PLAYER.currentShip.cosm;
                } else {
                    PLAYER.avatar = CHARACTER_DATA.getCrew();
                    PLAYER.avatar.position = spawn.crewPosition;
                    PLAYER.avatar.id = spawn.crewId;
                    PLAYER.avatar.team = PLAYER.currentTeam;
                    PLAYER.currentShip.cosm.crew[spawn.crewId] = PLAYER.avatar;
                    PLAYER.avatar.currentCosm = PLAYER.currentShip.cosm;
                }
                setEarPos();
                PLAYER.currentShip.cosm.init();
                SCREEN_MANAGER.goto_screen("Ship Navigation");
                return;
            }
            foreach (Station station3 in PLAYER.currentSession.stations) {
                if (station3.id == spawn.id) {
                    PLAYER.currentShip = station3;
                    break;
                }
            }
            if (PLAYER.currentShip != null) {
                if (PLAYER.currentShip.cosm == null) {
                    MicroCosm microCosm3 = new MicroCosm(PLAYER.currentShip, PLAYER.currentShip.topD, PLAYER.currentShip.botD, 0);
                    PLAYER.currentShip.cosm = microCosm3;
                    PLAYER.currentShip.artOrigin = microCosm3.centerOfMass;
                }
                PROCESS_REGISTER.currentCosm = PLAYER.currentShip.cosm;
                if (PLAYER.currentShip.cosm.crew.ContainsKey(spawn.crewId)) {
                    Crew crew2 = PLAYER.currentShip.cosm.crew[spawn.crewId];
                    PLAYER.avatar = CHARACTER_DATA.getCrew();
                    PLAYER.avatar.position = crew2.position;
                    PLAYER.avatar.rotation = crew2.rotation;
                    PLAYER.avatar.id = crew2.id;
                    PLAYER.avatar.team = PLAYER.currentTeam;
                    PLAYER.avatar.health = crew2.health;
                    PLAYER.avatar.shieldPoints = crew2.shieldPoints;
                    PLAYER.currentShip.cosm.crew[spawn.crewId] = PLAYER.avatar;
                    PLAYER.avatar.currentCosm = PLAYER.currentShip.cosm;
                } else {
                    PLAYER.avatar = CHARACTER_DATA.getCrew();
                    PLAYER.avatar.position = spawn.crewPosition;
                    PLAYER.avatar.id = spawn.crewId;
                    PLAYER.avatar.team = PLAYER.currentTeam;
                    PLAYER.currentShip.cosm.crew[spawn.crewId] = PLAYER.avatar;
                    PLAYER.avatar.currentCosm = PLAYER.currentShip.cosm;
                }
                setEarPos();
                PLAYER.currentShip.cosm.init();
                SCREEN_MANAGER.goto_screen("Ship Navigation");
            } else {
                triggerPlayerSpawn();
            }
        }
    }
}
