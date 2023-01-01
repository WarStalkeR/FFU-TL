#pragma warning disable CS0108
#pragma warning disable CS0169
#pragma warning disable CS0414
#pragma warning disable CS0436
#pragma warning disable CS0626
#pragma warning disable CS0649

using MonoMod;
using FFU_Terra_Liberatio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System;

namespace FFU_Terra_Liberatio {
    public class FFU_TL_Handling {
    }
}

namespace CoOpSpRpG {
	public static class patch_ANIMBAG {
		public static extern ModuleAnimation orig_getModAnim(uint index, byte rotation, Rectangle bBox, Module mod);
		public static ModuleAnimation getModAnim(uint index, byte rotation, Rectangle bBox, Module mod) {
        /// Try to use updated animation first.
			switch (index) {
				case 16U:
					mod.rotation = mod.tiles[0].rotation;
					return new LogisticsRoomAnim(mod.rotation, bBox);
			}
			return orig_getModAnim(index, rotation, bBox, mod);
		}
	}
	public class patch_MicroCosm : MicroCosm {
		public List<Module> walls;
		[MonoModIgnore] private void setMoment() { }
		[MonoModIgnore] private void checkHealthCommit() { }
		[MonoModIgnore] public int cargoTileCount { get; private set; }
		[MonoModIgnore] public int turretTileCount { get; private set; }
		[MonoModIgnore] public int enginesTileCount { get; private set; }
		[MonoModIgnore] public int reactorTileCount { get; private set; }
		[MonoModIgnore] public int radiatorTileCount { get; private set; }
		[MonoModIgnore] public int droneBaysTileCount { get; private set; }
		[MonoModIgnore] public int capacitorTileCount { get; private set; }
		[MonoModIgnore] public int thrustersTileCount { get; private set; }
		[MonoModIgnore] public int beamModulesTileCount { get; private set; }
		[MonoModIgnore] public int missileModulesTileCount { get; private set; }
		[MonoModIgnore] public int missileFactoryTileCount { get; private set; }
		[MonoModIgnore] public patch_MicroCosm(Actor world) : base(world) { }
		[MonoModReplace] private void populateMods() {
		/// Adding updates for missing module settings.
			int turretNum = 0;
			tiles = new ModTile[length];
			List<int> list = new List<int>();
			blank = new ModTile();
			blank.blocking = false;
			blank.repairable = false;
			blank.A = 0;
			for (int i = 0; i < length; i++) {
				if (bot[i].R != 0 || bot[i].G != 0 || bot[i].B != 0) {
					tiles[i] = new ModTile();
					tiles[i].A = bot[i].A;
					tiles[i].X = i;
					tiles[i].blocking = false;
					tiles[i].repairable = false;
					tiles[i].artSource = new Rectangle(0, 32, 16, 16);
					tiles[i].tileSheet = -2;
				} else {
					tiles[i] = blank;
				}
			}
			List<PixelParticleEmission> list2 = new List<PixelParticleEmission>();
			float shieldCap = 0f;
			float energyCap = 0f;
			float engineThrust = 0f;
			float thrusterOut = 0f;
			float integrityBoost = 0f;
			for (int j = 0; j < length; j++) {
				Module module = TILEBAG.getModule(bot[j]);
				if (module == null) {
					continue;
				}
				ModTile[] list3 = new ModTile[module.tiles.Count()];
				for (int k = 0; k < module.tiles.Count(); k++) {
					int tilesWidth = module.tiles[k].X / 16;
					int tilesHeight = module.tiles[k].Y / 16 * width;
					int tileNum = tilesWidth + tilesHeight + j;
					if (tileNum > 0 && tileNum < length && j % width + tilesWidth < width && j % width + tilesWidth >= 0) {
						if (tiles[tileNum] == blank) {
							tiles[tileNum] = new ModTile();
							tiles[tileNum].A = bot[tileNum].A;
							tiles[tileNum].X = tileNum;
							tiles[tileNum].blocking = false;
							tiles[tileNum].repairable = false;
							tiles[tileNum].artSource = new Rectangle(0, 32, 16, 16);
							tiles[tileNum].tileSheet = -2;
						}
						tiles[tileNum].artSource = new Rectangle(module.tiles[k].U, module.tiles[k].V, 16, 16);
						tiles[tileNum].Y = k;
						tiles[tileNum].inside = module.tiles[k].inside;
						tiles[tileNum].blocking = module.tiles[k].blocking;
						tiles[tileNum].airBlocking = module.tiles[k].airBlocking;
						tiles[tileNum].connectUp = module.tiles[k].connectUp;
						tiles[tileNum].connectDown = module.tiles[k].connectDown;
						tiles[tileNum].connectLeft = module.tiles[k].connectLeft;
						tiles[tileNum].connectRight = module.tiles[k].connectRight;
						tiles[tileNum].repairable = module.tiles[k].repairable;
						tiles[tileNum].preferOutside = module.tiles[k].preferOutside;
						tiles[tileNum].rotation = module.rotation;
						tiles[tileNum].tileSheet = module.tileSheet;
						tiles[tileNum].A = bot[tileNum].A;
						list3[k] = tiles[tileNum];
					} else {
						list3[k] = new ModTile();
						list3[k].artSource = new Rectangle(module.tiles[k].U, module.tiles[k].V, 16, 16);
						list3[k].Y = k;
						list3[k].repairable = false;
						list3[k].A = 0;
						list3[k].inside = false;
						list3[k].blocking = false;
						list3[k].airBlocking = false;
					}
				}
				if (module.particleEmitters != null) {
					for (int l = 0; l < module.particleEmitters.Length; l++) {
						PixelParticleEmission item = default(PixelParticleEmission);
						item.type = module.particleEmitters[l].type;
						float particleX = j % width;
						float particleY = j / width;
						particleX += module.particleEmitters[l].offset.X;
						particleY += module.particleEmitters[l].offset.Y;
						item.hullMaskPixelIndex = j + (int)((float)width * module.particleEmitters[l].offset.Y) + (int)module.particleEmitters[l].offset.X;
						particleX -= (float)(width / 2);
						particleY -= (float)(height / 2);
						item.offset = new Vector2(particleX, particleY);
						item.hullMasked = module.particleEmitters[l].hullMasked;
						list2.Add(item);
					}
				}
				switch (module.type) {
					case ModuleType.door:
					case ModuleType.airlock:
					case ModuleType.Life_Support:
					case ModuleType.walls: {
						integrityBoost += module.tiles.Length * 25 * (float)Math.Pow(Math.Max(1U, module.techLevel), 2);

						break;
					}
				}
				switch (module.type) {
					case ModuleType.conduit: {
						Conduit conduit = module as Conduit;
						Conduit conduit2 = new Conduit();
						conduit2.tip = conduit.tip;
						conduit2.setTiles(ref list3);
						conduit2.currentPower = conduit2.capacity;
						conduit2.location = j;
						conduit2.rate = conduit.rate;
						power.Add(conduit2);
						modules.Add(conduit2);
						break;
					}
					case ModuleType.bedroom: {
						Bedroom bedroom = module as Bedroom;
						Bedroom bedroom2 = new Bedroom();
						bedroom2.setTiles(ref list3);
						bedroom2.location = j;
						bedroom2.beds = bedroom.beds;
						bedroom2.quality = bedroom.quality;
						modules.Add(bedroom2);
						break;
					}
					case ModuleType.monster_spawner: {
						MonsterSpawner monsterSpawner = module as MonsterSpawner;
						MonsterSpawner monsterSpawner2 = new MonsterSpawner();
						monsterSpawner2.tip = monsterSpawner.tip;
						monsterSpawner2.setTiles(ref list3);
						monsterSpawner2.location = j;
						monsterSpawner2.spawnPoint = monsterSpawner.spawnPoint;
						monsterSpawner2.spawnType = monsterSpawner.spawnType;
						monsterSpawner2.spawnFreq = monsterSpawner.spawnFreq;
						modules.Add(monsterSpawner2);
						break;
					}
					case ModuleType.engineering_room: {
						EngineeringRoom engineeringRoom = module as EngineeringRoom;
						EngineeringRoom engineeringRoom2 = new EngineeringRoom();
						engineeringRoom2.tip = engineeringRoom.tip;
						engineeringRoom2.setTiles(ref list3);
						engineeringRoom2.location = j;
						engineeringRoom2.bonusRate = engineeringRoom.bonusRate;
						engineeringRoom2.routeCount = engineeringRoom.routeCount;
						engineeringRoom2.rate = engineeringRoom.rate;
						engineeringRoom2.capacity = engineeringRoom.rate;
						routes += engineeringRoom2.routeCount;
						modules.Add(engineeringRoom2);
						engineeringRooms.Add(engineeringRoom2);
						break;
					}
					case ModuleType.cargo_bay: {
						CargoBay cargoBay = module as CargoBay;
						CargoBay cargoBay2 = new CargoBay();
						cargoBay2.tip = cargoBay.tip;
						cargoBay2.setTiles(ref list3);
						cargoBay2.location = j;
						cargoBay2.storage = new Storage(cargoBay.storage.slotCount, cargoBay.storage.width, cargoBay.storage.height);
						cargoBay2.miningBonus = cargoBay.miningBonus; /// FIX
						modules.Add(cargoBay2);
						cargoBays.Add(cargoBay2);
						cargoTileCount += cargoBay2.tiles.Length;
						break;
					}
					case ModuleType.hangar: {
						Hangar hangar = module as Hangar;
						Hangar hangar2 = new Hangar();
						hangar2.tip = hangar.tip;
						hangar2.setTiles(ref list3);
						hangar2.location = j;
						hangar2.storage = new Storage(hangar.storage.slotCount, hangar.storage.width, hangar.storage.height);
						hangar2.storage.allowedTypes = new List<InventoryItemType>(1);
						hangar2.storage.allowedTypes.Add(InventoryItemType.packed_ship);
						modules.Add(hangar2);
						hangars.Add(hangar2);
						break;
					}
					case ModuleType.matter_furnace: {
						MatterFurnace matterFurnace = module as MatterFurnace;
						MatterFurnace matterFurnace2 = new MatterFurnace();
						matterFurnace2.tip = matterFurnace.tip;
						matterFurnace2.setTiles(ref list3);
						matterFurnace2.location = j;
						matterFurnace2.storage = new Storage(matterFurnace.storage.slotCount, matterFurnace.storage.width, matterFurnace.storage.height);
						modules.Add(matterFurnace2);
						break;
					}
					case ModuleType.trade_depot: {
						TradeDepot tradeDepot = module as TradeDepot;
						TradeDepot tradeDepot2 = new TradeDepot();
						tradeDepot2.tip = tradeDepot.tip;
						tradeDepot2.setTiles(ref list3);
						tradeDepot2.location = j;
						modules.Add(tradeDepot2);
						break;
					}
					case ModuleType.specialty_shop: {
						SpecialtyShop specialtyShop = module as SpecialtyShop;
						SpecialtyShop specialtyShop2 = new SpecialtyShop();
						specialtyShop2.lootType = specialtyShop.lootType;
						specialtyShop2.quantity = specialtyShop.quantity;
						specialtyShop2.tip = specialtyShop.tip;
						specialtyShop2.setTiles(ref list3);
						specialtyShop2.location = j;
						modules.Add(specialtyShop2);
						break;
					}
					case ModuleType.item_spawner: {
						ItemSpawner itemSpawner = module as ItemSpawner;
						ItemSpawner itemSpawner2 = new ItemSpawner();
						itemSpawner2.itemSpawnFunction = itemSpawner.itemSpawnFunction;
						itemSpawner2.tip = itemSpawner.tip;
						itemSpawner2.setTiles(ref list3);
						itemSpawner2.location = j;
						modules.Add(itemSpawner2);
						break;
					}
					case ModuleType.cloning_vat: {
						CloningVat cloningVat = module as CloningVat;
						CloningVat cloningVat2 = new CloningVat();
						cloningVat2.spawnOffset = cloningVat.spawnOffset + tileLocation(j);
						cloningVat2.tip = cloningVat.tip;
						cloningVat2.setTiles(ref list3);
						cloningVat2.animIndex = cloningVat.animIndex;
						cloningVat2.location = j;
						modules.Add(cloningVat2);
						break;
					}
					case ModuleType.powered: {
						Powered powered = module as Powered;
						Powered powered2 = new Powered();
						powered2.tip = powered.tip;
						powered2.setTiles(ref list3);
						powered2.capacity = powered.capacity;
						powered2.currentPower = powered2.capacity;
						powered2.location = j;
						powered2.rate = powered.rate;
						power.Add(powered2);
						modules.Add(powered2);
						break;
					}
					case ModuleType.lift: {
						TurboLift turboLift = module as TurboLift;
						TurboLift turboLift2 = new TurboLift();
						turboLift2.tip = turboLift.tip;
						turboLift2.setTiles(ref list3);
						turboLift2.destinationText = turboLift.destinationText;
						turboLift2.location = j;
						turboLift2.source = turboLift.source;
						turboLift2.destIndex = turboLift.destIndex;
						turboLift2.offset = turboLift.offset;
						modules.Add(turboLift2);
						break;
					}
					case ModuleType.capacitor: {
						Capacitor capacitor = module as Capacitor;
						Capacitor capacitor2 = new Capacitor();
						capacitor2.tip = capacitor.tip;
						capacitor2.setTiles(ref list3);
						capacitor2.capacity = capacitor.capacity;
						capacitor2.currentPower = capacitor2.capacity;
						capacitor2.location = j;
						capacitor2.rotation = capacitor.rotation;
						capacitor2.animIndex = capacitor.animIndex;
						capacitor2.rate = capacitor.capacity;
						power.Add(capacitor2);
						modules.Add(capacitor2);
						capacitors.Add(capacitor2);
						capacitorTileCount += capacitor2.tiles.Length;
						energyCap += capacitor2.capacity;
						break;
					}
					case ModuleType.beam_controller: {
						BeamController beamController = module as BeamController;
						BeamController beamController2 = new BeamController();
						beamController2.tip = beamController.tip;
						beamController2.rotation = beamController.rotation;
						beamController2.setTiles(ref list3);
						beamController2.location = j;
						beamController2.fireType = beamController.fireType;
						beamController2.animIndex = beamController.animIndex;
						modules.Add(beamController2);
						beamModulesTileCount += beamController2.tiles.Length;
						break;
					}
					case ModuleType.beam_emitter: {
						BeamEmitter beamEmitter = module as BeamEmitter;
						BeamEmitter beamEmitter2 = new BeamEmitter();
						beamEmitter2.tip = beamEmitter.tip;
						beamEmitter2.setTiles(ref list3);
						beamEmitter2.location = j;
						beamEmitter2.offset = beamEmitter.offset;
						beamEmitter2.capacity = beamEmitter.capacity;
						beamEmitter2.rate = beamEmitter.rate;
						beamEmitter2.animIndex = beamEmitter.animIndex;
						beamEmitter2.rotation = beamEmitter.rotation;
						beamEmitter2.arcType = beamEmitter.arcType;
						powerUsers.Add(beamEmitter2);
						modules.Add(beamEmitter2);
						beamEmitter2.configure(ship, width, height);
						beamEmitters.Add(beamEmitter2);
						beamModulesTileCount += beamEmitter2.tiles.Length;
						break;
					}
					case ModuleType.powered_mount: {
						PoweredTurretMount poweredTurretMount = module as PoweredTurretMount;
						PoweredTurretMount poweredTurretMount2 = new PoweredTurretMount();
						poweredTurretMount2.tip = poweredTurretMount.tip;
						poweredTurretMount2.setTiles(ref list3);
						poweredTurretMount2.location = j;
						poweredTurretMount2.track = poweredTurretMount.track;
						poweredTurretMount2.pivotX = poweredTurretMount.pivotX;
						poweredTurretMount2.pivotY = poweredTurretMount.pivotY;
						poweredTurretMount2.offset = poweredTurretMount.offset;
						poweredTurretMount2.mountType = poweredTurretMount.mountType;
						poweredTurretMount2.usesAmmo = poweredTurretMount.usesAmmo;
						poweredTurretMount2.gooUse = poweredTurretMount.gooUse;
						poweredTurretMount2.ammoBoxUse = poweredTurretMount.ammoBoxUse;
						if (ship.turrets != null && turretNum < ship.turrets.Count() && ship.turrets[turretNum] != null) {
							poweredTurretMount2.mounted = ship.turrets[turretNum];
							poweredTurretMount2.rate = poweredTurretMount.rate;
							poweredTurretMount2.cosm = this;
							ship.turrets[turretNum].visible = true;
						}
						turretNum++;
						powerUsers.Add(poweredTurretMount2);
						modules.Add(poweredTurretMount2);
						mounts.Add(poweredTurretMount2);
						turretTileCount += poweredTurretMount2.tiles.Length;
						break;
					}
					case ModuleType.drill_bore: {
						DrillBore drillBore = module as DrillBore;
						DrillBore drillBore2 = new DrillBore();
						drillBore2.tip = drillBore.tip;
						drillBore2.setTiles(ref list3);
						drillBore2.loadedAnim = drillBore.loadedAnim;
						drillBore2.location = j;
						drillBore2.direction = drillBore.direction;
						drillBore2.offset = drillBore.offset;
						modules.Add(drillBore2);
						systems.Add(drillBore2);
						drillBore2.configure(width, height);
						break;
					}
					case ModuleType.drone_launcher: {
						DroneLauncher droneLauncher = module as DroneLauncher;
						DroneLauncher droneLauncher2 = new DroneLauncher();
						droneLauncher2.tip = droneLauncher.tip;
						droneLauncher2.setTiles(ref list3);
						droneLauncher2.loadedAnim = droneLauncher.loadedAnim;
						droneLauncher2.location = j;
						droneLauncher2.spellID = droneLauncher.spellID;
						droneLauncher2.batterySize = droneLauncher.batterySize;
						droneLauncher2.buildCost = droneLauncher.buildCost;
						droneLauncher2.buildRate = droneLauncher.buildRate;
						droneLauncher2.chargeRate = droneLauncher.chargeRate;
						droneLauncher2.capacity = droneLauncher.capacity;
						droneLauncher2.animIndex = droneLauncher.animIndex;
						if (isStation) {
							droneLauncher2.currentPower = droneLauncher2.capacity;
						}
						if (droneLauncher.launchPoints != null) {
							droneLauncher2.configure(width, height, droneLauncher.launchPoints);
						}
						droneLauncher2.rate = droneLauncher.rate;
						droneLauncher2.lifeDuration = droneLauncher.lifeDuration;
						modules.Add(droneLauncher2);
						systems.Add(droneLauncher2);
						powerUsers.Add(droneLauncher2);
						droneBaysTileCount += droneLauncher2.tiles.Length;
						break;
					}
					case ModuleType.OgreM42: {
						OgreSystem ogreSystem = module as OgreSystem;
						OgreSystem ogreSystem2 = new OgreSystem();
						ogreSystem2.tip = ogreSystem.tip;
						ogreSystem2.setTiles(ref list3);
						ogreSystem2.location = j;
						ogreSystem2.rate = ogreSystem.rate;
						ogreSystem2.capacity = ogreSystem.capacity;
						modules.Add(ogreSystem2);
						systems.Add(ogreSystem2);
						powerUsers.Add(ogreSystem2);
						ogreSystem2.configure(width, height);
						break;
					}
					case ModuleType.sinidal_cascade: {
						SinidalCascade sinidalCascade = module as SinidalCascade;
						SinidalCascade sinidalCascade2 = new SinidalCascade();
						sinidalCascade2.tip = sinidalCascade.tip;
						sinidalCascade2.setTiles(ref list3);
						sinidalCascade2.location = j;
						sinidalCascade2.spellID = sinidalCascade.spellID;
						sinidalCascade2.useCost = sinidalCascade.useCost;
						sinidalCascade2.rate = sinidalCascade.rate;
						sinidalCascade2.capacity = sinidalCascade.capacity;
						modules.Add(sinidalCascade2);
						systems.Add(sinidalCascade2);
						powerUsers.Add(sinidalCascade2);
						sinidalCascade2.configure(width, height);
						break;
					}
					case ModuleType.energy_use_activated: {
						EnergyUseActivate energyUseActivate = module as EnergyUseActivate;
						EnergyUseActivate energyUseActivate2 = new EnergyUseActivate();
						energyUseActivate2.tip = energyUseActivate.tip;
						energyUseActivate2.setTiles(ref list3);
						energyUseActivate2.location = j;
						energyUseActivate2.rate = energyUseActivate.rate;
						energyUseActivate2.capacity = energyUseActivate.capacity;
						energyUseActivate2.currentPower = energyUseActivate2.capacity;
						energyUseActivate2.useCost = energyUseActivate.useCost;
						energyUseActivate2.spellID = energyUseActivate.spellID;
						energyUseActivate2.globalCooldown = energyUseActivate.globalCooldown;
						energyUseActivate2.animIndex = energyUseActivate.animIndex;
						energyUseActivate2.rotation = energyUseActivate.rotation;
						modules.Add(energyUseActivate2);
						systems.Add(energyUseActivate2);
						powerUsers.Add(energyUseActivate2);
						break;
					}
					case ModuleType.diggable: {
						Diggable diggable = module as Diggable;
						Diggable diggable2 = new Diggable();
						diggable2.tip = diggable.tip;
						diggable2.setTiles(ref list3);
						diggable2.location = j;
						diggable2.resultTile = diggable.resultTile;
						diggable2.lootType = diggable.lootType;
						break;
					}
					case ModuleType.launcher: {
						Launcher launcher = module as Launcher;
						Launcher launcher2 = new Launcher();
						launcher2.tip = launcher.tip;
						launcher2.setTiles(ref list3);
						launcher2.animIndex = launcher.animIndex;
						launcher2.selfReload = launcher.selfReload;
						launcher2.rotation = launcher.rotation;
						launcher2.location = j;
						launcher2.sizeCategory = launcher.sizeCategory;
						launcher2.direction = launcher.direction;
						launcher2.offset = launcher.offset;
						modules.Add(launcher2);
						launchers.Add(launcher2);
						launcher2.configure(width, height);
						launcher2.point.ship = ship;
						missileModulesTileCount += launcher2.tiles.Length;
						break;
					}
					case ModuleType.missile_magazine: {
						MissileMagazine missileMagazine = module as MissileMagazine;
						MissileMagazine missileMagazine2 = new MissileMagazine();
						missileMagazine2.tip = missileMagazine.tip;
						missileMagazine2.setTiles(ref list3);
						missileMagazine2.animIndex = missileMagazine.animIndex;
						missileMagazine2.sizeCategory = missileMagazine.sizeCategory;
						missileMagazine2.location = j;
						modules.Add(missileMagazine2);
						missileModulesTileCount += missileMagazine2.tiles.Length;
						break;
					}
					case ModuleType.missile_factory: {
						MissileFactory missileFactory = module as MissileFactory;
						MissileFactory missileFactory2 = new MissileFactory();
						missileFactory2.tip = missileFactory.tip;
						missileFactory2.setTiles(ref list3);
						missileFactory2.location = j;
						missileFactory2.capacity = missileFactory.capacity;
						missileFactory2.sizeCategory = missileFactory.sizeCategory;
						missileFactory2.missileType = missileFactory.missileType;
						missileFactory2.rate = missileFactory2.capacity;
						modules.Add(missileFactory2);
						powerUsers.Add(missileFactory2);
						factories.Add(missileFactory2);
						missileModulesTileCount += missileFactory2.tiles.Length;
						missileFactoryTileCount += missileFactory2.tiles.Length;
						break;
					}
					case ModuleType.loader: {
						Loader loader = module as Loader;
						Loader loader2 = new Loader();
						loader2.tip = loader.tip;
						loader2.setTiles(ref list3);
						loader2.location = j;
						loader2.direction = loader.direction;
						loader2.cosm = this;
						modules.Add(loader2);
						loaders.Add(loader2);
						missileModulesTileCount += loader2.tiles.Length;
						break;
					}
					case ModuleType.gun_charger: {
						GunCharger gunCharger = module as GunCharger;
						GunCharger gunCharger2 = new GunCharger();
						gunCharger2.tip = gunCharger.tip;
						gunCharger2.setTiles(ref list3);
						gunCharger2.location = j;
						gunCharger2.rate = gunCharger.rate;
						gunCharger2.capacity = gunCharger.capacity;
						powerUsers.Add(gunCharger2);
						modules.Add(gunCharger2);
						chargers.Add(gunCharger2);
						break;
					}
					case ModuleType.injector: {
						Injector injector = module as Injector;
						Injector injector2 = new Injector();
						injector2.tip = injector.tip;
						injector2.setTiles(ref list3);
						injector2.output = injector.output;
						injector2.location = j;
						injectors.Add(injector2);
						modules.Add(injector2);
						break;
					}
					case ModuleType.reactor: {
						Reactor reactor = module as Reactor;
						Reactor reactor2 = new Reactor();
						reactor2.tip = reactor.tip;
						reactor2.setTiles(ref list3);
						reactor2.thermEfficiency = reactor.thermEfficiency;
						reactor2.suggestedMaxOutput = reactor.suggestedMaxOutput;
						reactor2.heatCapacity = reactor.heatCapacity;
						reactor2.explosionOffset = reactor.explosionOffset;
						reactor2.cycleCapacity = reactor.cycleCapacity;
						reactor2.capacity = reactor.cycleCapacity;
						reactor2.rate = reactor.cycleCapacity;
						reactor2.maxTemp = reactor.maxTemp;
						reactor2.explosionCreated = reactor.explosionCreated;
						reactor2.location = j;
						reactor2.cosm = this;
						reactors.Add(reactor2);
						power.Add(reactor2);
						modules.Add(reactor2);
						reactorTileCount += reactor2.tiles.Length;
						break;
					}
					case ModuleType.thruster: {
						Thruster thruster = module as Thruster;
						Thruster thruster2 = new Thruster();
						thruster2.tip = thruster.tip;
						thruster2.setTiles(ref list3);
						thruster2.artIndex = thruster.artIndex;
						thruster2.outputCapacity = thruster.outputCapacity;
						thruster2.useCost = thruster.useCost;
						thruster2.location = j;
						thruster2.capacity = thruster.capacity;
						thrusterOut += thruster2.capacity;
						thruster2.currentPower = thruster2.capacity;
						thruster2.rate = thruster.rate;
						Vector2 vector = new Vector2(j % width, j / width);
						thruster2.offset = thruster.offset + vector;
						thruster2.leftJet = thruster.leftJet + vector;
						thruster2.rightJet = thruster.rightJet + vector;
						thruster2.upJet = thruster.upJet + vector;
						thruster2.downJet = thruster.downJet + vector;
						thruster2.up = thruster.up;
						thruster2.down = thruster.down;
						thruster2.left = thruster.left;
						thruster2.right = thruster.right;
						thrusters.Add(thruster2);
						powerUsers.Add(thruster2);
						modules.Add(thruster2);
						publisher.animStates[j] = true;
						thrustersTileCount += thruster2.tiles.Length;
						break;
					}
					case ModuleType.door: {
						Door door = module as Door;
						Door door2 = new Door();
						door2.tip = door.tip;
						door2.setTiles(ref list3);
						door2.rotation = door.rotation;
						door2.location = j;
						door2.cosm = this;
						modules.Add(door2);
						break;
					}
					case ModuleType.med_bay: {
						MedBay medBay = module as MedBay;
						MedBay medBay2 = new MedBay();
						medBay2.tip = medBay.tip;
						medBay2.setTiles(ref list3);
						medBay2.healAmount = medBay.healAmount;
						medBay2.centerPoint = medBay.centerPoint;
						medBay2.radius = medBay.radius;
						medBay2.rotation = medBay.rotation;
						medBay2.location = j;
						medBay2.cosm = this;
						modules.Add(medBay2);
						break;
					}
					case ModuleType.airlock: {
						Airlock airlock = module as Airlock;
						Airlock airlock2 = new Airlock();
						airlock2.tip = airlock.tip;
						airlock2.setTiles(ref list3);
						airlock2.upSpot = airlock.upSpot;
						airlock2.downSpot = airlock.downSpot;
						airlock2.leftSpot = airlock.leftSpot;
						airlock2.rightSpot = airlock.rightSpot;
						airlock2.pivotX = airlock.pivotX;
						airlock2.pivotY = airlock.pivotY;
						airlock2.rotation = airlock.rotation;
						airlock2.location = j;
						airlock2.animIndex = airlock.animIndex;
						airlock2.active = airlock.active;
						airlock2.cosm = this;
						airlocks.Add(airlock2);
						modules.Add(airlock2);
						break;
					}
					case ModuleType.warp_coil: {
						WarpCoil warpCoil = module as WarpCoil;
						WarpCoil warpCoil2 = new WarpCoil();
						warpCoil2.tip = warpCoil.tip;
						warpCoil2.setTiles(ref list3);
						warpCoil2.location = j;
						warpCoil2.output = warpCoil.output;
						warpCoil2.maintainCost = warpCoil.maintainCost;
						warpCoil2.capacity = warpCoil2.maintainCost * 10f;
						warpCoil2.rate = warpCoil2.maintainCost * 2f;
						warpCoil2.animIndex = warpCoil.animIndex;
						powerUsers.Add(warpCoil2);
						modules.Add(warpCoil2);
						warpCoils.Add(warpCoil2);
						break;
					}
					case ModuleType.Engine: {
						Engine engine = module as Engine;
						Engine engine2 = new Engine();
						engine2.tip = engine.tip;
						engine2.setTiles(ref list3);
						engine2.outputCapacity = engine.outputCapacity;
						engine2.useCost = engine.useCost;
						engine2.location = j;
						engine2.capacity = engine.capacity;
						engineThrust += engine2.capacity;
						engine2.currentPower = engine2.capacity;
						engine2.rate = engine.rate;
						engines.Add(engine2);
						powerUsers.Add(engine2);
						modules.Add(engine2);
						engine2.offset = engine.offset + new Vector2(j % width, j / width);
						ship.engineAnims.Add(new EngineBurn(j, engine.size, ship, engine2.offset));
						publisher.animStates[j] = true;
						enginesTileCount += engine2.tiles.Length;
						break;
					}
					case ModuleType.Interdictor: {
						InterdictorModule interdictorModule = module as InterdictorModule;
						InterdictorModule interdictorModule2 = new InterdictorModule();
						interdictorModule2.tip = interdictorModule.tip;
						interdictorModule2.setTiles(ref list3);
						interdictorModule2.outputCapacity = interdictorModule.outputCapacity;
						interdictorModule2.useCost = interdictorModule.useCost;
						interdictorModule2.location = j;
						interdictorModule2.capacity = interdictorModule.capacity;
						interdictorModule2.rate = interdictorModule.rate;
						interdictors.Add(interdictorModule2);
						powerUsers.Add(interdictorModule2);
						modules.Add(interdictorModule2);
						break;
					}
					case ModuleType.shield:
						list.Add(j);
						tiles[j].U = 5;
						break;
					case ModuleType.shield_emitter: {
						ShieldEmitter shieldEmitter = module as ShieldEmitter;
						ShieldEmitter shieldEmitter2 = new ShieldEmitter();
						shieldEmitter2.tip = shieldEmitter.tip;
						shieldEmitter2.setTiles(ref list3);
						shieldEmitter2.allTiles = tiles;
						shieldEmitter2.capacity = shieldEmitter.capacity;
						shieldEmitter2.currentPower = shieldEmitter2.capacity;
						shieldEmitter2.rate = shieldEmitter2.capacity;
						shieldEmitter2.conversionRate = shieldEmitter.conversionRate;
						shieldEmitter2.shieldStorage = shieldEmitter.shieldStorage;
						shieldEmitter2.costPerAlpha = shieldEmitter.costPerAlpha;
						shieldEmitter2.homeOffset = shieldEmitter.homeOffset;
						shieldEmitter2.width = width;
						shieldEmitter2.setHome(new Vector2(j % width, j / width));
						shieldEmitter2.location = j;
						shieldCap += shieldEmitter2.shieldStorage;
						powerUsers.Add(shieldEmitter2);
						emitters.Add(shieldEmitter2);
						modules.Add(shieldEmitter2);
						break;
					}
					case ModuleType.Console_Connect: {
						ConsoleConnect consoleConnect = module as ConsoleConnect;
						ConsoleConnect consoleConnect2 = new ConsoleConnect();
						consoleConnect2.tip = consoleConnect.tip;
						consoleConnect2.setTiles(ref list3);
						consoleConnect2.group = consoleConnect.group;
						consoleConnect2.location = j;
						modules.Add(consoleConnect2);
						break;
					}
					case ModuleType.Console_Access: {
						ConsoleAccess consoleAccess = module as ConsoleAccess;
						ConsoleAccess consoleAccess2 = new ConsoleAccess();
						consoleAccess2.tip = consoleAccess.tip;
						consoleAccess2.setTiles(ref list3);
						consoleAccess2.location = j;
						consoleAccess2.systemSlots = consoleAccess.systemSlots;
						consoleAccess2.viewRange = consoleAccess.viewRange;
						modules.Add(consoleAccess2);
						consoles.Add(consoleAccess2);
						break;
					}
					case ModuleType.Turret_Track: {
						Module module2 = new Module();
						module2.type = ModuleType.Turret_Track;
						module2.setTiles(ref list3);
						module2.location = j;
						modules.Add(module2);
						break;
					}
					case ModuleType.Life_Support: {
						LifeSupport lifeSupport = module as LifeSupport;
						LifeSupport lifeSupport2 = new LifeSupport();
						lifeSupport2.tip = lifeSupport.tip;
						lifeSupport2.setTiles(ref list3);
						lifeSupport2.location = j;
						lifeSupport2.airPerPower = lifeSupport.airPerPower;
						lifeSupport2.capacity = lifeSupport.capacity;
						lifeSupport2.rate = lifeSupport2.capacity;
						lifeSupport2.shieldCost = lifeSupport.shieldCost;
						lifeSupport2.conversionRate = lifeSupport.conversionRate;
						lifeSupport2.storageSpace = lifeSupport.storageSpace;
						lifeSupport2.rotation = lifeSupport.rotation;
						lifeSupport2.animIndex = lifeSupport.animIndex;
						modules.Add(lifeSupport2);
						this.lifeSupport.Add(lifeSupport2);
						powerUsers.Add(lifeSupport2);
						break;
					}
					case ModuleType.radiator: {
						Radiator radiator = module as Radiator;
						Radiator radiator2 = new Radiator();
						radiator2.tip = radiator.tip;
						radiator2.setTiles(ref list3);
						radiator2.location = j;
						radiator2.capacity = radiator.capacity;
						radiator2.dissopationRate = radiator.dissopationRate;
						modules.Add(radiator2);
						radiators.Add(radiator2);
						radiatorTileCount += radiator2.tiles.Length;
						break;
					}
					case ModuleType.structure: {
						Structure structure = module as Structure;
						Structure structure2 = new Structure();
						structure2.tip = structure.tip;
						structure2.setTiles(ref list3);
						structure2.location = j;
						structure2.integrityAdded = structure.integrityAdded;
						integrityBoost += (float)structure.integrityAdded;
						modules.Add(structure2);
						structures.Add(structure2);
						break;
					}
					case ModuleType.screen_access: {
						ScreenAccess screenAccess = module as ScreenAccess;
						ScreenAccess screenAccess2 = new ScreenAccess();
						screenAccess2.tip = screenAccess.tip;
						screenAccess2.setTiles(ref list3);
						screenAccess2.location = j;
						screenAccess2.screen = screenAccess.screen;
						screenAccess2.announcement = screenAccess.announcement;
						screenAccess2.modString = screenAccess.modString;
						screenAccess2.animIndex = screenAccess.animIndex;
						screenAccess2.rotation = screenAccess.rotation; /// FIX
						modules.Add(screenAccess2);
						break;
					}
					case ModuleType.quest_trigger: {
						QuestTriggerRoom questTriggerRoom = module as QuestTriggerRoom;
						QuestTriggerRoom questTriggerRoom2 = new QuestTriggerRoom();
						questTriggerRoom2.tip = questTriggerRoom.tip;
						questTriggerRoom2.setTiles(ref list3);
						questTriggerRoom2.location = j;
						questTriggerRoom2.roomName = questTriggerRoom.roomName;
						modules.Add(questTriggerRoom2);
						break;
					}
					case ModuleType.scanner: {
						Scanner scanner = module as Scanner;
						Scanner scanner2 = new Scanner();
						scanner2.tip = scanner.tip;
						scanner2.setTiles(ref list3);
						scanner2.location = j;
						scanner2.energyCost = scanner.energyCost;
						scanner2.rate = scanner.rate;
						scanner2.capacity = scanner.capacity;
						scanner2.sizeCategory = scanner.sizeCategory;
						scanner2.radius = scanner.radius;
						modules.Add(scanner2);
						scanners.Add(scanner2);
						powerUsers.Add(scanner2);
						break;
					}
					case ModuleType.warhead_converter: {
						WarheadConverter warheadConverter = module as WarheadConverter;
						WarheadConverter warheadConverter2 = new WarheadConverter();
						warheadConverter2.tip = warheadConverter.tip;
						warheadConverter2.setTiles(ref list3);
						warheadConverter2.location = j;
						warheadConverter2.rate = warheadConverter.rate;
						warheadConverter2.capacity = warheadConverter.capacity;
						warheadConverter2.sizeCategory = warheadConverter.sizeCategory;
						warheadConverter2.missileType = warheadConverter.missileType;
						modules.Add(warheadConverter2);
						systems.Add(warheadConverter2);
						missileModulesTileCount += warheadConverter2.tiles.Length;
						break;
					}
					case ModuleType.inhibitor_pulse: {
						InhibitorPulse inhibitorPulse = module as InhibitorPulse;
						InhibitorPulse inhibitorPulse2 = new InhibitorPulse();
						inhibitorPulse2.tip = inhibitorPulse.tip;
						inhibitorPulse2.setTiles(ref list3);
						inhibitorPulse2.location = j;
						inhibitorPulse2.rate = inhibitorPulse.rate;
						inhibitorPulse2.spellID = inhibitorPulse.spellID;
						inhibitorPulse2.capacity = inhibitorPulse.capacity;
						modules.Add(inhibitorPulse2);
						systems.Add(inhibitorPulse2);
						powerUsers.Add(inhibitorPulse2);
						inhibitorPulse2.configure(width, height);
						break;
					}
					case ModuleType.artifact_activator: {
						ArtifactActivator artifactActivator = module as ArtifactActivator;
						ArtifactActivator artifactActivator2 = new ArtifactActivator();
						artifactActivator2.tip = artifactActivator.tip;
						artifactActivator2.setTiles(ref list3);
						artifactActivator2.location = j;
						artifactActivator2.rate = artifactActivator.rate;
						artifactActivator2.capacity = artifactActivator.capacity;
						artifactActivator2.animIndex = artifactActivator.animIndex;
						artifactActivator2.rotation = artifactActivator.rotation;
						artifactActivators.Add(artifactActivator2);
						powerUsers.Add(artifactActivator2);
						modules.Add(artifactActivator2);
						break;
					}
					case ModuleType.heat_vent: {
						HeatVent heatVent = module as HeatVent;
						HeatVent heatVent2 = new HeatVent();
						heatVent2.tip = heatVent.tip;
						heatVent2.setTiles(ref list3);
						heatVent2.location = j;
						heatVent2.rate = heatVent.rate;
						heatVent2.capacity = heatVent.capacity;
						heatVent2.spellID = heatVent.spellID;
						heatVent2.heatCapacity = heatVent.heatCapacity;
						heatVent2.dissopationRate = heatVent.dissopationRate;
						modules.Add(heatVent2);
						systems.Add(heatVent2);
						radiators.Add(heatVent2);
						powerUsers.Add(heatVent2);
						heatVent2.configure(width, height);
						break;
					}
					case ModuleType.distant_AoE: {
						DistantAoE distantAoE = module as DistantAoE;
						DistantAoE distantAoE2 = new DistantAoE();
						distantAoE2.tip = distantAoE.tip;
						distantAoE2.setTiles(ref list3);
						distantAoE2.location = j;
						distantAoE2.rate = distantAoE.rate;
						distantAoE2.useCost = distantAoE.useCost;
						distantAoE2.capacity = distantAoE.capacity;
						distantAoE2.spellID = distantAoE.spellID;
						modules.Add(distantAoE2);
						systems.Add(distantAoE2);
						powerUsers.Add(distantAoE2);
						distantAoE2.configure(width, height);
						break;
					}
					case ModuleType.fission_torpedoes: {
						FissionTorpedos fissionTorpedos = module as FissionTorpedos;
						FissionTorpedos fissionTorpedos2 = new FissionTorpedos();
						fissionTorpedos2.tip = fissionTorpedos.tip;
						fissionTorpedos2.setTiles(ref list3);
						fissionTorpedos2.location = j;
						fissionTorpedos2.rate = fissionTorpedos.rate;
						fissionTorpedos2.capacity = fissionTorpedos.capacity;
						fissionTorpedos2.useCost = fissionTorpedos.useCost;
						fissionTorpedos2.spellID = fissionTorpedos.spellID;
						fissionTorpedos2.rotation = fissionTorpedos.rotation;
						modules.Add(fissionTorpedos2);
						systems.Add(fissionTorpedos2);
						powerUsers.Add(fissionTorpedos2);
						fissionTorpedos2.configure(width, height);
						break;
					}
					case ModuleType.LRD_system: {
						LRDSystem lRDSystem = module as LRDSystem;
						LRDSystem lRDSystem2 = new LRDSystem();
						lRDSystem2.tip = lRDSystem.tip;
						lRDSystem2.setTiles(ref list3);
						lRDSystem2.location = j;
						lRDSystem2.rate = lRDSystem.rate;
						lRDSystem2.capacity = lRDSystem.capacity;
						lRDSystem2.useCost = lRDSystem.useCost;
						lRDSystem2.spellID = lRDSystem.spellID;
						modules.Add(lRDSystem2);
						systems.Add(lRDSystem2);
						powerUsers.Add(lRDSystem2);
						lRDSystem2.configure(width, height);
						break;
					}
					case ModuleType.channeled_debuff: {
						ChanneledDebuff channeledDebuff = module as ChanneledDebuff;
						ChanneledDebuff channeledDebuff2 = new ChanneledDebuff();
						channeledDebuff2.tip = channeledDebuff.tip;
						channeledDebuff2.setTiles(ref list3);
						channeledDebuff2.location = j;
						channeledDebuff2.rate = channeledDebuff.rate;
						channeledDebuff2.capacity = channeledDebuff.capacity;
						channeledDebuff2.spellID = channeledDebuff.spellID;
						channeledDebuff2.useCost = channeledDebuff.useCost;
						modules.Add(channeledDebuff2);
						systems.Add(channeledDebuff2);
						powerUsers.Add(channeledDebuff2);
						channeledDebuff2.configure(width, height);
						break;
					}
					case ModuleType.anchor: {
						Anchor anchor = module as Anchor;
						Anchor anchor2 = new Anchor();
						anchor2.tip = anchor.tip;
						anchor2.setTiles(ref list3);
						anchor2.location = j;
						anchor2.rate = anchor.rate;
						anchor2.capacity = anchor.capacity;
						anchor2.useCost = anchor.useCost;
						anchor2.spellID = anchor.spellID;
						anchor2.rotation = anchor.rotation;
						anchor2.animIndex = anchor.animIndex;
						anchor2.active = true;
						modules.Add(anchor2);
						systems.Add(anchor2);
						powerUsers.Add(anchor2);
						anchor2.configure(width, height);
						break;
					}
					case ModuleType.magrail: {
						MagrailModule magrailModule = module as MagrailModule;
						MagrailModule magrailModule2 = new MagrailModule();
						magrailModule2.tip = magrailModule.tip;
						magrailModule2.setTiles(ref list3);
						magrailModule2.location = j;
						magrailModule2.type = magrailModule.type;
						magrailModule2.rotation = magrailModule.rotation;
						magrailModule2.animIndex = magrailModule.animIndex;
						magrailModule2.active = true;
						magrailModule2.announcement = magrailModule.announcement;
						magrailModule2.exitSpot = magrailModule.exitSpot;
						modules.Add(magrailModule2);
						break;
					}
					case ModuleType.crew_interaction: {
						InteractiveRoom interactiveRoom = module as InteractiveRoom;
						InteractiveRoom interactiveRoom2 = new InteractiveRoom();
						interactiveRoom2.tip = interactiveRoom.tip;
						interactiveRoom2.tipText = interactiveRoom.tipText;
						interactiveRoom2.interaction = interactiveRoom.interaction;
						interactiveRoom2.setTiles(ref list3);
						interactiveRoom2.location = j;
						interactiveRoom2.rotation = interactiveRoom.rotation;
						interactiveRoom2.animIndex = interactiveRoom.animIndex;
						interactiveRoom2.active = true;
						modules.Add(interactiveRoom2);
						break;
					}
					case ModuleType.computer_core: {
						ComputerCore computerCore = module as ComputerCore;
						ComputerCore computerCore2 = new ComputerCore();
						computerCore2.tip = computerCore.tip;
						computerCore2.setTiles(ref list3);
						computerCore2.location = j;
						computerCore2.rotation = computerCore.rotation;
						computerCore2.animIndex = computerCore.animIndex;
						computerCore2.rate = computerCore.rate;
						computerCore2.capacity = computerCore.capacity;
						computerCore2.powerNeed = computerCore.powerNeed;
						computerCore2.aggressivity = computerCore.aggressivity;
						computerCore2.cleverness = computerCore.cleverness;
						computerCore2.accuracy = computerCore.accuracy;
						computerCore2.aimingBehaviorPreference = computerCore.aimingBehaviorPreference;
						modules.Add(computerCore2);
						brain = computerCore2;
						ship.aiControlled = true;
						break;
					}
					case ModuleType.algae_tank: {
						AlgaeTank algaeTank = module as AlgaeTank;
						AlgaeTank algaeTank2 = new AlgaeTank();
						algaeTank2.tip = algaeTank.tip;
						algaeTank2.setTiles(ref list3);
						algaeTank2.location = j;
						algaeTank2.rotation = algaeTank.rotation;
						algaeTank2.wasteStorage = algaeTank.wasteStorage;
						algaeTank2.growthRate = algaeTank.growthRate;
						modules.Add(algaeTank2);
						algaeTanks.Add(algaeTank2);
						break;
					}
					case ModuleType.water_tank: {
						WaterTank waterTank = module as WaterTank;
						WaterTank waterTank2 = new WaterTank();
						waterTank2.tip = waterTank.tip;
						waterTank2.setTiles(ref list3);
						waterTank2.location = j;
						waterTank2.rotation = waterTank.rotation;
						waterTank2.storage = waterTank.storage;
						modules.Add(waterTank2);
						break;
					}
					case ModuleType.ram_scoop: {
						RamScoop ramScoop = module as RamScoop;
						RamScoop ramScoop2 = new RamScoop();
						ramScoop2.tip = ramScoop.tip;
						ramScoop2.setTiles(ref list3);
						ramScoop2.location = j;
						ramScoop2.rotation = ramScoop.rotation;
						ramScoop2.usePerSec = ramScoop.usePerSec;
						ramScoop2.capacity = ramScoop.usePerSec * 2f;
						ramScoop2.rate = ramScoop.rate;
						ramScoop2.timeToGather = ramScoop.timeToGather;
						modules.Add(ramScoop2);
						powerUsers.Add(ramScoop2);
						ramScoops.Add(ramScoop2);
						break;
					}
					case ModuleType.waste_recycler: {
						WasteRecycler wasteRecycler = module as WasteRecycler;
						WasteRecycler wasteRecycler2 = new WasteRecycler();
						wasteRecycler2.tip = wasteRecycler.tip;
						wasteRecycler2.setTiles(ref list3);
						wasteRecycler2.location = j;
						wasteRecycler2.rotation = wasteRecycler.rotation;
						wasteRecycler2.efficiency = wasteRecycler.efficiency;
						modules.Add(wasteRecycler2);
						break;
					}
					case ModuleType.water_recycler: {
						WaterRecycler waterRecycler = module as WaterRecycler;
						WaterRecycler waterRecycler2 = new WaterRecycler();
						waterRecycler2.tip = waterRecycler.tip;
						waterRecycler2.setTiles(ref list3);
						waterRecycler2.location = j;
						waterRecycler2.rotation = waterRecycler.rotation;
						waterRecycler2.efficiency = waterRecycler.efficiency;
						modules.Add(waterRecycler2);
						break;
					}
					case ModuleType.cargo_scanner: {
						CargoScanner cargoScanner = module as CargoScanner;
						CargoScanner cargoScanner2 = new CargoScanner();
						cargoScanner2.tip = cargoScanner.tip;
						cargoScanner2.setTiles(ref list3);
						cargoScanner2.location = j;
						cargoScanner2.rotation = cargoScanner.rotation;
						cargoScanner2.usePerSec = cargoScanner.usePerSec;
						cargoScanner2.capacity = cargoScanner.usePerSec * 2f;
						cargoScanner2.rate = cargoScanner.rate;
						modules.Add(cargoScanner2);
						cargoScanners.Add(cargoScanner2);
						powerUsers.Add(cargoScanner2);
						break;
					}
				}
			}
			for (int m = 0; m < length; m++) {
				if (tiles[m] != null && tiles[m].artSource.X == 0 && tiles[m].artSource.Y == 32 && tiles[m].artSource.Width == 16 && tiles[m].artSource.Height == 16) {
					tiles[m].artSource = new Rectangle(0, 0, 16, 16);
					tiles[m].repairable = true;
					tiles[m].blocking = false;
				}
			}
			if (list2.Count > 0) {
				ship.particleEmitters = list2.ToArray();
				ship.particleEmitterTimers = new float[ship.particleEmitters.Length];
			}
			foreach (ShieldEmitter emitter in emitters) {
				emitter.applyField(list);
			}
			foreach (PoweredTurretMount mount in mounts) {
				mount.configure(tiles, width);
			}
			blank.blocking = false;
			blank.airBlocking = true;
			blank.repairable = false;
			ship.shieldCap = shieldCap;
			ship.capCap = energyCap;
			ship.engineCap = engineThrust;
			ship.thrusterCap = thrusterOut;
			checkHealthCommit();
			setMoment();
			ship.integrityCap = (float)((double)integrityBoost - moment);
		}
	}
	public class patch_ShipDesignRev4 : ShipDesignRev4 {
		[MonoModIgnore] internal Color[] design;
		[MonoModIgnore] internal ModTile[] tiles;
		[MonoModIgnore] internal int designWidth;
		[MonoModIgnore] internal int designHeight;
		[MonoModIgnore] private bool costCalcRequested;
		[MonoModIgnore] private StatManager statManager;
		[MonoModIgnore] private HashSet<Module> modules;
		[MonoModIgnore] private ShipDesignScreenType ScreenType;
		[MonoModIgnore] private void saveUndoStep() { }
		[MonoModIgnore] private void removeModuleByTileData(int index) { }
		[MonoModIgnore] private void removeModuleByColorData(int index) { }
		[MonoModIgnore] private bool isInBounds(int tileIndex) { return false; }
		[MonoModIgnore] private Color findColorKeyOn(int index) { return Color.Black; }
		[MonoModIgnore] private void updateTile(int index, bool shouldClearTile = true) { }
		[MonoModIgnore] public patch_ShipDesignRev4(GraphicsDevice device, string name, ShipDesignScreenType screenType) : base(device, name, screenType) { }
		[MonoModReplace] private bool deleteModuleOn(int gridX, int gridY) {
        /// Allow to remove orphaned tiles that have no owner module.
			if (ScreenType == ShipDesignScreenType.preview) return false;
			if (gridX < 0 || gridX > designWidth) return false;
			if (gridY < 0 || gridY > designHeight) return false;
			int num = gridX + gridY * designWidth;
			if (!isInBounds(num)) return false;
			if (design[num] == Color.Transparent) return false;
			costCalcRequested = true;
			if (TILEBAG.isShieldTileColor(ref design[num])) {
				saveUndoStep();
				design[num].R = 0;
				design[num].G = 0;
				design[num].B = 0;
				design[num].A = 0;
				updateTile(num);
				return true;
			}
			if (tiles[num].owner == null && TILEBAG.isGreyTileColor(ref design[num])) {
				saveUndoStep();
				design[num] = TILEBAG.getEmptyTileColor();
				updateTile(num);
				return true;
			}
			Color color = findColorKeyOn(num);
			TILEBAG.getBaseRotationColor(color);
			if (!TILEBAG.isEmptyTileColor(ref color)) {
				if (ScreenType != ShipDesignScreenType.SinglePlayer || TILEBAG.canDeleteModule(color)) {
					saveUndoStep();
					if (tiles[num] != null && tiles[num].owner != null) removeModuleByTileData(num);
					else removeModuleByColorData(num);
				}
				return true;
			}
			return false;
		}
		private extern void orig_updateModule(Module mod, ModuleConnectionUpdateType updateType);
		private void updateModule(Module mod, ModuleConnectionUpdateType updateType) {
		/// Allow corridors, airlocks, doors and life supports to increase integrity.
			if (mod != null) {
				switch (mod.type) {
					case ModuleType.door:
					case ModuleType.airlock:
					case ModuleType.Life_Support:
					case ModuleType.walls: {
						switch (updateType) {
							case ModuleConnectionUpdateType.Connected:
							statManager.Integrity += (ulong)(mod.tiles.Length * 25 * Math.Pow(Math.Max(1U, mod.techLevel), 1.5));
							break;
							case ModuleConnectionUpdateType.Disconnected:
							statManager.Integrity -= (ulong)(mod.tiles.Length * 25 * Math.Pow(Math.Max(1U, mod.techLevel), 1.5));
							break;
						}
						break;
					}
				}
			}
			orig_updateModule(mod, updateType);
		}
		private extern Module orig_createModule(Module moduleArchetype, ref ModTile[] moduleTiles, int location);
		private Module createModule(Module moduleArchetype, ref ModTile[] moduleTiles, int location) {
		/// Ensure that tech level is assigned to newly created modules from archetypes.
			Module newModule = orig_createModule(moduleArchetype, ref moduleTiles, location);
			if (newModule == null && moduleArchetype.type == ModuleType.walls) {
				newModule = new Module();
				newModule.tip = moduleArchetype.tip;
				newModule.setTiles(ref moduleTiles);
				newModule.location = location;
				newModule.type = ModuleType.walls;
			}
			if (newModule != null) newModule.techLevel = moduleArchetype.techLevel;
			return newModule;
        }
		[MonoModIgnore] private enum ModuleConnectionUpdateType {
			Disconnected,
			Connected,
			NeighborsChanged
		}
	}
	public class patch_CrewArmor : CrewArmor {
		[MonoModIgnore] private int artEnum;
		[MonoModIgnore] public patch_CrewArmor(int art, ArmorSpawnFlags flags) : base(art, flags) { }
		[MonoModReplace] private void setArt() {
		/// Give T3 Armor T2 looks, while new art is not added.
			switch (artEnum) {
				case 0:
				iconArtSource = new Rectangle(704, 0, 64, 64);
				hasShirt = true;
				break;
				case 1:
				iconArtSource = new Rectangle(704, 0, 64, 64);
				hasShirt = true;
				break;
				case 2:
				iconArtSource = new Rectangle(768, 0, 64, 64);
				shirtSheet = 27;
				legSheet = 28;
				hasShirt = true;
				hasLegs = true;
				break;
				case 3:
				iconArtSource = new Rectangle(768, 64, 64, 64);
				shirtSheet = 27;
				legSheet = 28;
				hasShirt = true;
				hasLegs = true;
				break;
				case 4:
				iconArtSource = new Rectangle(832, 0, 64, 64);
				shirtSheet = 27;
				legSheet = 28;
				headOffset = 4;
				hasShirt = true;
				hasLegs = true;
				hasHelm = true;
				break;
				case 5:
				iconArtSource = new Rectangle(832, 64, 64, 64);
				shirtSheet = 32;
				legSheet = 28;
				headOffset = 5;
				hasShirt = true;
				hasLegs = true;
				hasHelm = true;
				break;
				case 6:
				iconArtSource = new Rectangle(768, 128, 64, 64);
				shirtSheet = 32;
				legSheet = 28;
				hasShirt = true;
				hasLegs = true;
				break;
				case 7:
				iconArtSource = new Rectangle(832, 128, 64, 64);
				shirtSheet = 32;
				legSheet = 28;
				headOffset = 5;
				hasShirt = true;
				hasLegs = true;
				hasHelm = true;
				break;
			}
		}
	}
	public class patch_DataCore : DataCore {
		[MonoModIgnore] private int _tier;
		[MonoModIgnore] public patch_DataCore(int quality) : base(quality) { }
		public int tier {
		/// Implement use of patched in T3 data core textures.
			get {
				return _tier;
			}
			set {
				_tier = value;
				switch (_tier) {
					case 0:
					iconArtSource = new Rectangle(1600, 0, 64, 64);
					break;
					case 1:
					iconArtSource = new Rectangle(1600, 64, 64, 64);
					break;
					case 2:
					iconArtSource = new Rectangle(1600, 128, 64, 64);
					break;
					case 3:
					iconArtSource = new Rectangle(1664, 0, 64, 64);
					break;
					case 4:
					iconArtSource = new Rectangle(1664, 64, 64, 64);
					break;
					case 5:
					iconArtSource = new Rectangle(1664, 128, 64, 64);
					break;
					default:
					iconArtSource = new Rectangle(704, 192, 64, 64);
					break;
				}
			}
		}
	}
}