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
	[MonoModIfFlag("SP")] public class patch_MicroCosm : MicroCosm {
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
				} else tiles[i] = blank;
			}
			List<PixelParticleEmission> list2 = new List<PixelParticleEmission>();
			float shieldCap = 0f;
			float engineThrust = 0f;
			float thrusterOut = 0f;
			float integrityBoost = 0f;
			for (int j = 0; j < length; j++) {
				Module module = TILEBAG.getModule(bot[j]);
				if (module == null) continue;
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
						particleX -= (width / 2);
						particleY -= (height / 2);
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
						Conduit mConduit = module as Conduit;
						Conduit refConduit = new Conduit();
						refConduit.tip = mConduit.tip;
						refConduit.setTiles(ref list3);
						refConduit.currentPower = refConduit.capacity;
						refConduit.location = j;
						refConduit.rate = mConduit.rate;
						power.Add(refConduit);
						modules.Add(refConduit);
						break;
					}
					case ModuleType.bedroom: {
						Bedroom mBedroom = module as Bedroom;
						Bedroom refBedroom = new Bedroom();
						refBedroom.setTiles(ref list3);
						refBedroom.location = j;
						refBedroom.beds = mBedroom.beds;
						refBedroom.quality = mBedroom.quality;
						modules.Add(refBedroom);
						break;
					}
					case ModuleType.monster_spawner: {
						MonsterSpawner mMonsterSpawner = module as MonsterSpawner;
						MonsterSpawner refMonsterSpawner = new MonsterSpawner();
						refMonsterSpawner.tip = mMonsterSpawner.tip;
						refMonsterSpawner.setTiles(ref list3);
						refMonsterSpawner.location = j;
						refMonsterSpawner.spawnPoint = mMonsterSpawner.spawnPoint;
						refMonsterSpawner.spawnType = mMonsterSpawner.spawnType;
						refMonsterSpawner.spawnFreq = mMonsterSpawner.spawnFreq;
						modules.Add(refMonsterSpawner);
						break;
					}
					case ModuleType.engineering_room: {
						EngineeringRoom mEngineeringRoom = module as EngineeringRoom;
						EngineeringRoom refEngineeringRoom = new EngineeringRoom();
						refEngineeringRoom.tip = mEngineeringRoom.tip;
						refEngineeringRoom.setTiles(ref list3);
						refEngineeringRoom.location = j;
						refEngineeringRoom.bonusRate = mEngineeringRoom.bonusRate;
						refEngineeringRoom.routeCount = mEngineeringRoom.routeCount;
						refEngineeringRoom.rate = mEngineeringRoom.rate;
						refEngineeringRoom.capacity = mEngineeringRoom.rate;
						routes += refEngineeringRoom.routeCount;
						modules.Add(refEngineeringRoom);
						engineeringRooms.Add(refEngineeringRoom);
						break;
					}
					case ModuleType.cargo_bay: {
						CargoBay mCargoBay = module as CargoBay;
						CargoBay refCargoBay = new CargoBay();
						refCargoBay.tip = mCargoBay.tip;
						refCargoBay.setTiles(ref list3);
						refCargoBay.location = j;
						refCargoBay.storage = new Storage(mCargoBay.storage.slotCount, mCargoBay.storage.width, mCargoBay.storage.height);
						refCargoBay.miningBonus = mCargoBay.miningBonus; // FIX!
						modules.Add(refCargoBay);
						cargoBays.Add(refCargoBay);
						cargoTileCount += refCargoBay.tiles.Length;
						break;
					}
					case ModuleType.matter_furnace: {
						MatterFurnace mMatterFurnace = module as MatterFurnace;
						MatterFurnace refMatterFurnace = new MatterFurnace();
						refMatterFurnace.tip = mMatterFurnace.tip;
						refMatterFurnace.setTiles(ref list3);
						refMatterFurnace.location = j;
						refMatterFurnace.storage = new Storage(mMatterFurnace.storage.slotCount, mMatterFurnace.storage.width, mMatterFurnace.storage.height);
						modules.Add(refMatterFurnace);
						break;
					}
					case ModuleType.trade_depot: {
						TradeDepot mTradeDepot = module as TradeDepot;
						TradeDepot refTradeDepot = new TradeDepot();
						refTradeDepot.tip = mTradeDepot.tip;
						refTradeDepot.setTiles(ref list3);
						refTradeDepot.location = j;
						modules.Add(refTradeDepot);
						break;
					}
					case ModuleType.specialty_shop: {
						SpecialtyShop mSpecialtyShop = module as SpecialtyShop;
						SpecialtyShop refSpecialtyShop = new SpecialtyShop();
						refSpecialtyShop.lootType = mSpecialtyShop.lootType;
						refSpecialtyShop.quantity = mSpecialtyShop.quantity;
						refSpecialtyShop.tip = mSpecialtyShop.tip;
						refSpecialtyShop.setTiles(ref list3);
						refSpecialtyShop.location = j;
						modules.Add(refSpecialtyShop);
						break;
					}
					case ModuleType.item_spawner: {
						ItemSpawner mItemSpawner = module as ItemSpawner;
						ItemSpawner refItemSpawner = new ItemSpawner();
						refItemSpawner.itemSpawnFunction = mItemSpawner.itemSpawnFunction;
						refItemSpawner.tip = mItemSpawner.tip;
						refItemSpawner.setTiles(ref list3);
						refItemSpawner.location = j;
						modules.Add(refItemSpawner);
						break;
					}
					case ModuleType.cloning_vat: {
						CloningVat mCloningVat = module as CloningVat;
						CloningVat refCloningVat = new CloningVat();
						refCloningVat.spawnOffset = mCloningVat.spawnOffset + tileLocation(j);
						refCloningVat.tip = mCloningVat.tip;
						refCloningVat.setTiles(ref list3);
						refCloningVat.animIndex = mCloningVat.animIndex;
						refCloningVat.location = j;
						modules.Add(refCloningVat);
						break;
					}
					case ModuleType.powered: {
						Powered mPowered = module as Powered;
						Powered refPowered = new Powered();
						refPowered.tip = mPowered.tip;
						refPowered.setTiles(ref list3);
						refPowered.capacity = mPowered.capacity;
						refPowered.currentPower = refPowered.capacity;
						refPowered.location = j;
						refPowered.rate = mPowered.rate;
						power.Add(refPowered);
						modules.Add(refPowered);
						break;
					}
					case ModuleType.lift: {
						TurboLift mTurboLift = module as TurboLift;
						TurboLift refTurboLift = new TurboLift();
						refTurboLift.tip = mTurboLift.tip;
						refTurboLift.setTiles(ref list3);
						refTurboLift.destinationText = mTurboLift.destinationText;
						refTurboLift.location = j;
						refTurboLift.source = mTurboLift.source;
						refTurboLift.destIndex = mTurboLift.destIndex;
						refTurboLift.offset = mTurboLift.offset;
						modules.Add(refTurboLift);
						break;
					}
					case ModuleType.capacitor: {
						Capacitor mCapacitor = module as Capacitor;
						Capacitor refCapacitor = new Capacitor();
						refCapacitor.tip = mCapacitor.tip;
						refCapacitor.setTiles(ref list3);
						refCapacitor.capacity = mCapacitor.capacity;
						refCapacitor.currentPower = refCapacitor.capacity;
						refCapacitor.location = j;
						refCapacitor.rotation = mCapacitor.rotation;
						refCapacitor.animIndex = mCapacitor.animIndex;
						refCapacitor.rate = mCapacitor.capacity;
						power.Add(refCapacitor);
						modules.Add(refCapacitor);
						capacitors.Add(refCapacitor);
						capacitorTileCount += refCapacitor.tiles.Length;
						break;
					}
					case ModuleType.beam_controller: {
						BeamController mBeamController = module as BeamController;
						BeamController refBeamController = new BeamController();
						refBeamController.tip = mBeamController.tip;
						refBeamController.rotation = mBeamController.rotation;
						refBeamController.setTiles(ref list3);
						refBeamController.location = j;
						refBeamController.fireType = mBeamController.fireType;
						refBeamController.animIndex = mBeamController.animIndex;
						modules.Add(refBeamController);
						beamModulesTileCount += refBeamController.tiles.Length;
						break;
					}
					case ModuleType.beam_emitter: {
						BeamEmitter mBeamEmitter = module as BeamEmitter;
						BeamEmitter refBeamEmitter = new BeamEmitter();
						refBeamEmitter.tip = mBeamEmitter.tip;
						refBeamEmitter.setTiles(ref list3);
						refBeamEmitter.location = j;
						refBeamEmitter.offset = mBeamEmitter.offset;
						refBeamEmitter.capacity = mBeamEmitter.capacity;
						refBeamEmitter.rate = mBeamEmitter.rate;
						refBeamEmitter.animIndex = mBeamEmitter.animIndex;
						refBeamEmitter.rotation = mBeamEmitter.rotation;
						refBeamEmitter.arcType = mBeamEmitter.arcType;
						powerUsers.Add(refBeamEmitter);
						modules.Add(refBeamEmitter);
						refBeamEmitter.configure(ship, width, height);
						beamEmitters.Add(refBeamEmitter);
						beamModulesTileCount += refBeamEmitter.tiles.Length;
						break;
					}
					case ModuleType.powered_mount: {
						PoweredTurretMount mPoweredTurretMount = module as PoweredTurretMount;
						PoweredTurretMount refPoweredTurretMount = new PoweredTurretMount();
						refPoweredTurretMount.tip = mPoweredTurretMount.tip;
						refPoweredTurretMount.setTiles(ref list3);
						refPoweredTurretMount.location = j;
						refPoweredTurretMount.track = mPoweredTurretMount.track;
						refPoweredTurretMount.pivotX = mPoweredTurretMount.pivotX;
						refPoweredTurretMount.pivotY = mPoweredTurretMount.pivotY;
						refPoweredTurretMount.offset = mPoweredTurretMount.offset;
						refPoweredTurretMount.mountType = mPoweredTurretMount.mountType;
						refPoweredTurretMount.usesAmmo = mPoweredTurretMount.usesAmmo;
						refPoweredTurretMount.gooUse = mPoweredTurretMount.gooUse;
						if (ship.turrets != null && turretNum < ship.turrets.Count() && ship.turrets[turretNum] != null) {
							refPoweredTurretMount.mounted = ship.turrets[turretNum];
							refPoweredTurretMount.rate = mPoweredTurretMount.rate;
							refPoweredTurretMount.cosm = this;
							ship.turrets[turretNum].visible = true;
						}
						turretNum++;
						powerUsers.Add(refPoweredTurretMount);
						modules.Add(refPoweredTurretMount);
						mounts.Add(refPoweredTurretMount);
						turretTileCount += refPoweredTurretMount.tiles.Length;
						break;
					}
					case ModuleType.drill_bore: {
						DrillBore mDrillBore = module as DrillBore;
						DrillBore refDrillBore = new DrillBore();
						refDrillBore.tip = mDrillBore.tip;
						refDrillBore.setTiles(ref list3);
						refDrillBore.loadedAnim = mDrillBore.loadedAnim;
						refDrillBore.location = j;
						refDrillBore.direction = mDrillBore.direction;
						refDrillBore.offset = mDrillBore.offset;
						modules.Add(refDrillBore);
						systems.Add(refDrillBore);
						refDrillBore.configure(width, height);
						break;
					}
					case ModuleType.drone_launcher: {
						DroneLauncher mDroneLauncher = module as DroneLauncher;
						DroneLauncher refDroneLauncher = new DroneLauncher();
						refDroneLauncher.tip = mDroneLauncher.tip;
						refDroneLauncher.setTiles(ref list3);
						refDroneLauncher.loadedAnim = mDroneLauncher.loadedAnim;
						refDroneLauncher.location = j;
						refDroneLauncher.spellID = mDroneLauncher.spellID;
						refDroneLauncher.batterySize = mDroneLauncher.batterySize;
						refDroneLauncher.buildCost = mDroneLauncher.buildCost;
						refDroneLauncher.buildRate = mDroneLauncher.buildRate;
						refDroneLauncher.chargeRate = mDroneLauncher.chargeRate;
						refDroneLauncher.capacity = mDroneLauncher.capacity;
						refDroneLauncher.animIndex = mDroneLauncher.animIndex;
						if (isStation) {
							refDroneLauncher.currentPower = refDroneLauncher.capacity;
						}
						if (mDroneLauncher.launchPoints != null) {
							refDroneLauncher.configure(width, height, mDroneLauncher.launchPoints);
						}
						refDroneLauncher.rate = mDroneLauncher.rate;
						refDroneLauncher.lifeDuration = mDroneLauncher.lifeDuration;
						modules.Add(refDroneLauncher);
						systems.Add(refDroneLauncher);
						powerUsers.Add(refDroneLauncher);
						droneBaysTileCount += refDroneLauncher.tiles.Length;
						break;
					}
					case ModuleType.OgreM42: {
						OgreSystem mOgreSystem = module as OgreSystem;
						OgreSystem refOgreSystem = new OgreSystem();
						refOgreSystem.tip = mOgreSystem.tip;
						refOgreSystem.setTiles(ref list3);
						refOgreSystem.location = j;
						refOgreSystem.rate = mOgreSystem.rate;
						refOgreSystem.capacity = mOgreSystem.capacity;
						modules.Add(refOgreSystem);
						systems.Add(refOgreSystem);
						powerUsers.Add(refOgreSystem);
						refOgreSystem.configure(width, height);
						break;
					}
					case ModuleType.sinidal_cascade: {
						SinidalCascade mSinidalCascade = module as SinidalCascade;
						SinidalCascade refSinidalCascade = new SinidalCascade();
						refSinidalCascade.tip = mSinidalCascade.tip;
						refSinidalCascade.setTiles(ref list3);
						refSinidalCascade.location = j;
						refSinidalCascade.spellID = mSinidalCascade.spellID;
						refSinidalCascade.useCost = mSinidalCascade.useCost;
						refSinidalCascade.rate = mSinidalCascade.rate;
						refSinidalCascade.capacity = mSinidalCascade.capacity;
						modules.Add(refSinidalCascade);
						systems.Add(refSinidalCascade);
						powerUsers.Add(refSinidalCascade);
						refSinidalCascade.configure(width, height);
						break;
					}
					case ModuleType.energy_use_activated: {
						EnergyUseActivate mEnergyUseActivate = module as EnergyUseActivate;
						EnergyUseActivate refEnergyUseActivate = new EnergyUseActivate();
						refEnergyUseActivate.tip = mEnergyUseActivate.tip;
						refEnergyUseActivate.setTiles(ref list3);
						refEnergyUseActivate.location = j;
						refEnergyUseActivate.rate = mEnergyUseActivate.rate;
						refEnergyUseActivate.capacity = mEnergyUseActivate.capacity;
						refEnergyUseActivate.currentPower = refEnergyUseActivate.capacity;
						refEnergyUseActivate.useCost = mEnergyUseActivate.useCost;
						refEnergyUseActivate.spellID = mEnergyUseActivate.spellID;
						refEnergyUseActivate.globalCooldown = mEnergyUseActivate.globalCooldown;
						refEnergyUseActivate.animIndex = mEnergyUseActivate.animIndex;
						refEnergyUseActivate.rotation = mEnergyUseActivate.rotation;
						modules.Add(refEnergyUseActivate);
						systems.Add(refEnergyUseActivate);
						powerUsers.Add(refEnergyUseActivate);
						break;
					}
					case ModuleType.diggable: {
						Diggable mDiggable = module as Diggable;
						Diggable refDiggable = new Diggable();
						refDiggable.tip = mDiggable.tip;
						refDiggable.setTiles(ref list3);
						refDiggable.location = j;
						refDiggable.resultTile = mDiggable.resultTile;
						refDiggable.lootType = mDiggable.lootType;
						break;
					}
					case ModuleType.launcher: {
						Launcher mLauncher = module as Launcher;
						Launcher refLauncher = new Launcher();
						refLauncher.tip = mLauncher.tip;
						refLauncher.setTiles(ref list3);
						refLauncher.animIndex = mLauncher.animIndex;
						refLauncher.selfReload = mLauncher.selfReload;
						refLauncher.rotation = mLauncher.rotation;
						refLauncher.location = j;
						refLauncher.sizeCategory = mLauncher.sizeCategory;
						refLauncher.direction = mLauncher.direction;
						refLauncher.offset = mLauncher.offset;
						modules.Add(refLauncher);
						launchers.Add(refLauncher);
						refLauncher.configure(width, height);
						refLauncher.point.ship = ship;
						missileModulesTileCount += refLauncher.tiles.Length;
						break;
					}
					case ModuleType.missile_magazine: {
						MissileMagazine mMissileSilo = module as MissileMagazine;
						MissileMagazine refMissileSilo = new MissileMagazine();
						refMissileSilo.tip = mMissileSilo.tip;
						refMissileSilo.setTiles(ref list3);
						refMissileSilo.animIndex = mMissileSilo.animIndex;
						refMissileSilo.sizeCategory = mMissileSilo.sizeCategory;
						refMissileSilo.location = j;
						modules.Add(refMissileSilo);
						missileModulesTileCount += refMissileSilo.tiles.Length;
						break;
					}
					case ModuleType.missile_factory: {
						MissileFactory mMissileFactory = module as MissileFactory;
						MissileFactory refMissileFactory = new MissileFactory();
						refMissileFactory.tip = mMissileFactory.tip;
						refMissileFactory.setTiles(ref list3);
						refMissileFactory.location = j;
						refMissileFactory.capacity = mMissileFactory.capacity;
						refMissileFactory.sizeCategory = mMissileFactory.sizeCategory;
						refMissileFactory.missileType = mMissileFactory.missileType;
						refMissileFactory.rate = refMissileFactory.capacity;
						modules.Add(refMissileFactory);
						powerUsers.Add(refMissileFactory);
						factories.Add(refMissileFactory);
						missileModulesTileCount += refMissileFactory.tiles.Length;
						missileFactoryTileCount += refMissileFactory.tiles.Length;
						break;
					}
					case ModuleType.loader: {
						Loader mLoader = module as Loader;
						Loader refLoader = new Loader();
						refLoader.tip = mLoader.tip;
						refLoader.setTiles(ref list3);
						refLoader.location = j;
						refLoader.direction = mLoader.direction;
						refLoader.cosm = this;
						modules.Add(refLoader);
						loaders.Add(refLoader);
						missileModulesTileCount += refLoader.tiles.Length;
						break;
					}
					case ModuleType.gun_charger: {
						GunCharger mGunCharger = module as GunCharger;
						GunCharger refGunCharger = new GunCharger();
						refGunCharger.tip = mGunCharger.tip;
						refGunCharger.setTiles(ref list3);
						refGunCharger.location = j;
						refGunCharger.rate = mGunCharger.rate;
						refGunCharger.capacity = mGunCharger.capacity;
						powerUsers.Add(refGunCharger);
						modules.Add(refGunCharger);
						chargers.Add(refGunCharger);
						break;
					}
					case ModuleType.injector: {
						Injector mInjector = module as Injector;
						Injector refInjector = new Injector();
						refInjector.tip = mInjector.tip;
						refInjector.setTiles(ref list3);
						refInjector.output = mInjector.output;
						refInjector.location = j;
						injectors.Add(refInjector);
						modules.Add(refInjector);
						break;
					}
					case ModuleType.reactor: {
						Reactor mReactor = module as Reactor;
						Reactor refReactor = new Reactor();
						refReactor.tip = mReactor.tip;
						refReactor.setTiles(ref list3);
						refReactor.thermEfficiency = mReactor.thermEfficiency;
						refReactor.suggestedMaxOutput = mReactor.suggestedMaxOutput;
						refReactor.heatCapacity = mReactor.heatCapacity;
						refReactor.explosionOffset = mReactor.explosionOffset;
						refReactor.cycleCapacity = mReactor.cycleCapacity;
						refReactor.capacity = mReactor.cycleCapacity;
						refReactor.rate = mReactor.cycleCapacity;
						refReactor.maxTemp = mReactor.maxTemp;
						refReactor.explosionCreated = mReactor.explosionCreated;
						refReactor.location = j;
						refReactor.cosm = this;
						reactors.Add(refReactor);
						power.Add(refReactor);
						modules.Add(refReactor);
						reactorTileCount += refReactor.tiles.Length;
						break;
					}
					case ModuleType.thruster: {
						Thruster mThruster = module as Thruster;
						Thruster refThruster = new Thruster();
						refThruster.tip = mThruster.tip;
						refThruster.setTiles(ref list3);
						refThruster.artIndex = mThruster.artIndex;
						refThruster.outputCapacity = mThruster.outputCapacity;
						refThruster.useCost = mThruster.useCost;
						refThruster.location = j;
						refThruster.capacity = mThruster.capacity;
						thrusterOut += refThruster.capacity;
						refThruster.currentPower = refThruster.capacity;
						refThruster.rate = mThruster.rate;
						Vector2 vector = new Vector2(j % width, j / width);
						refThruster.offset = mThruster.offset + vector;
						refThruster.leftJet = mThruster.leftJet + vector;
						refThruster.rightJet = mThruster.rightJet + vector;
						refThruster.upJet = mThruster.upJet + vector;
						refThruster.downJet = mThruster.downJet + vector;
						refThruster.up = mThruster.up;
						refThruster.down = mThruster.down;
						refThruster.left = mThruster.left;
						refThruster.right = mThruster.right;
						thrusters.Add(refThruster);
						powerUsers.Add(refThruster);
						modules.Add(refThruster);
						publisher.animStates[j] = true;
						thrustersTileCount += refThruster.tiles.Length;
						break;
					}
					case ModuleType.door: {
						Door mDoor = module as Door;
						Door refDoor = new Door();
						refDoor.tip = mDoor.tip;
						refDoor.setTiles(ref list3);
						refDoor.rotation = mDoor.rotation;
						refDoor.location = j;
						refDoor.cosm = this;
						modules.Add(refDoor);
						break;
					}
					case ModuleType.med_bay: {
						MedBay mMedBay = module as MedBay;
						MedBay refMedBay = new MedBay();
						refMedBay.tip = mMedBay.tip;
						refMedBay.setTiles(ref list3);
						refMedBay.healAmount = mMedBay.healAmount;
						refMedBay.centerPoint = mMedBay.centerPoint;
						refMedBay.radius = mMedBay.radius;
						refMedBay.rotation = mMedBay.rotation;
						refMedBay.location = j;
						refMedBay.cosm = this;
						modules.Add(refMedBay);
						break;
					}
					case ModuleType.airlock: {
						Airlock mAirlock = module as Airlock;
						Airlock refAirlock = new Airlock();
						refAirlock.tip = mAirlock.tip;
						refAirlock.setTiles(ref list3);
						refAirlock.upSpot = mAirlock.upSpot;
						refAirlock.downSpot = mAirlock.downSpot;
						refAirlock.leftSpot = mAirlock.leftSpot;
						refAirlock.rightSpot = mAirlock.rightSpot;
						refAirlock.pivotX = mAirlock.pivotX;
						refAirlock.pivotY = mAirlock.pivotY;
						refAirlock.rotation = mAirlock.rotation;
						refAirlock.location = j;
						refAirlock.animIndex = mAirlock.animIndex;
						refAirlock.active = mAirlock.active;
						refAirlock.cosm = this;
						airlocks.Add(refAirlock);
						modules.Add(refAirlock);
						break;
					}
					case ModuleType.warp_coil: {
						WarpCoil mWarpCoil = module as WarpCoil;
						WarpCoil refWarpCoil = new WarpCoil();
						refWarpCoil.tip = mWarpCoil.tip;
						refWarpCoil.setTiles(ref list3);
						refWarpCoil.location = j;
						refWarpCoil.output = mWarpCoil.output;
						refWarpCoil.maintainCost = mWarpCoil.maintainCost;
						refWarpCoil.capacity = refWarpCoil.maintainCost * 10f;
						refWarpCoil.rate = refWarpCoil.maintainCost * 2f;
						refWarpCoil.animIndex = mWarpCoil.animIndex;
						powerUsers.Add(refWarpCoil);
						modules.Add(refWarpCoil);
						warpCoils.Add(refWarpCoil);
						break;
					}
					case ModuleType.Engine: {
						Engine mEngine = module as Engine;
						Engine refEngine = new Engine();
						refEngine.tip = mEngine.tip;
						refEngine.setTiles(ref list3);
						refEngine.outputCapacity = mEngine.outputCapacity;
						refEngine.useCost = mEngine.useCost;
						refEngine.location = j;
						refEngine.capacity = mEngine.capacity;
						engineThrust += refEngine.capacity;
						refEngine.currentPower = refEngine.capacity;
						refEngine.rate = mEngine.rate;
						engines.Add(refEngine);
						powerUsers.Add(refEngine);
						modules.Add(refEngine);
						refEngine.offset = mEngine.offset + new Vector2(j % width, j / width);
						ship.engineAnims.Add(new EngineBurn(j, mEngine.size, ship, refEngine.offset));
						publisher.animStates[j] = true;
						enginesTileCount += refEngine.tiles.Length;
						break;
					}
					case ModuleType.Interdictor: {
						InterdictorModule mInterdictorModule = module as InterdictorModule;
						InterdictorModule refInterdictorModule = new InterdictorModule();
						refInterdictorModule.tip = mInterdictorModule.tip;
						refInterdictorModule.setTiles(ref list3);
						refInterdictorModule.outputCapacity = mInterdictorModule.outputCapacity;
						refInterdictorModule.useCost = mInterdictorModule.useCost;
						refInterdictorModule.location = j;
						refInterdictorModule.capacity = mInterdictorModule.capacity;
						refInterdictorModule.rate = mInterdictorModule.rate;
						interdictors.Add(refInterdictorModule);
						powerUsers.Add(refInterdictorModule);
						modules.Add(refInterdictorModule);
						break;
					}
					case ModuleType.shield:
					list.Add(j);
					tiles[j].U = 5;
					break;
					case ModuleType.shield_emitter: {
						ShieldEmitter mShieldEmitter = module as ShieldEmitter;
						ShieldEmitter refShieldEmitter = new ShieldEmitter();
						refShieldEmitter.tip = mShieldEmitter.tip;
						refShieldEmitter.setTiles(ref list3);
						refShieldEmitter.allTiles = tiles;
						refShieldEmitter.capacity = mShieldEmitter.capacity;
						refShieldEmitter.currentPower = refShieldEmitter.capacity;
						refShieldEmitter.rate = refShieldEmitter.capacity;
						refShieldEmitter.conversionRate = mShieldEmitter.conversionRate;
						refShieldEmitter.shieldStorage = mShieldEmitter.shieldStorage;
						refShieldEmitter.costPerAlpha = mShieldEmitter.costPerAlpha;
						refShieldEmitter.homeOffset = mShieldEmitter.homeOffset;
						refShieldEmitter.width = width;
						refShieldEmitter.setHome(new Vector2(j % width, j / width));
						refShieldEmitter.location = j;
						shieldCap += refShieldEmitter.shieldStorage;
						powerUsers.Add(refShieldEmitter);
						emitters.Add(refShieldEmitter);
						modules.Add(refShieldEmitter);
						break;
					}
					case ModuleType.Console_Connect: {
						ConsoleConnect mConsoleConnect = module as ConsoleConnect;
						ConsoleConnect refConsoleConnect = new ConsoleConnect();
						refConsoleConnect.tip = mConsoleConnect.tip;
						refConsoleConnect.setTiles(ref list3);
						refConsoleConnect.group = mConsoleConnect.group;
						refConsoleConnect.location = j;
						modules.Add(refConsoleConnect);
						break;
					}
					case ModuleType.Console_Access: {
						ConsoleAccess mConsoleAccess = module as ConsoleAccess;
						ConsoleAccess refConsoleAccess = new ConsoleAccess();
						refConsoleAccess.tip = mConsoleAccess.tip;
						refConsoleAccess.setTiles(ref list3);
						refConsoleAccess.location = j;
						refConsoleAccess.systemSlots = mConsoleAccess.systemSlots;
						refConsoleAccess.viewRange = mConsoleAccess.viewRange;
						modules.Add(refConsoleAccess);
						consoles.Add(refConsoleAccess);
						break;
					}
					case ModuleType.Turret_Track: {
						Module mTurretTrack = new Module();
						mTurretTrack.type = ModuleType.Turret_Track;
						mTurretTrack.setTiles(ref list3);
						mTurretTrack.location = j;
						modules.Add(mTurretTrack);
						break;
					}
					case ModuleType.Life_Support: {
						LifeSupport mLifeSupport = module as LifeSupport;
						LifeSupport refLifeSupport = new LifeSupport();
						refLifeSupport.tip = mLifeSupport.tip;
						refLifeSupport.setTiles(ref list3);
						refLifeSupport.location = j;
						refLifeSupport.airPerPower = mLifeSupport.airPerPower;
						refLifeSupport.capacity = mLifeSupport.capacity;
						refLifeSupport.rate = refLifeSupport.capacity;
						refLifeSupport.shieldCost = mLifeSupport.shieldCost;
						refLifeSupport.conversionRate = mLifeSupport.conversionRate;
						refLifeSupport.storageSpace = mLifeSupport.storageSpace;
						refLifeSupport.rotation = mLifeSupport.rotation;
						refLifeSupport.animIndex = mLifeSupport.animIndex;
						modules.Add(refLifeSupport);
						this.lifeSupport.Add(refLifeSupport);
						powerUsers.Add(refLifeSupport);
						break;
					}
					case ModuleType.radiator: {
						Radiator mRadiator = module as Radiator;
						Radiator refRadiator = new Radiator();
						refRadiator.tip = mRadiator.tip;
						refRadiator.setTiles(ref list3);
						refRadiator.location = j;
						refRadiator.capacity = mRadiator.capacity;
						refRadiator.dissopationRate = mRadiator.dissopationRate;
						modules.Add(refRadiator);
						radiators.Add(refRadiator);
						radiatorTileCount += refRadiator.tiles.Length;
						break;
					}
					case ModuleType.structure: {
						Structure mStructure = module as Structure;
						Structure refStructure = new Structure();
						refStructure.tip = mStructure.tip;
						refStructure.setTiles(ref list3);
						refStructure.location = j;
						refStructure.integrityAdded = mStructure.integrityAdded;
						integrityBoost += mStructure.integrityAdded;
						modules.Add(refStructure);
						structures.Add(refStructure);
						break;
					}
					case ModuleType.screen_access: {
						ScreenAccess mScreenAccess = module as ScreenAccess;
						ScreenAccess refScreenAccess = new ScreenAccess();
						refScreenAccess.tip = mScreenAccess.tip;
						refScreenAccess.setTiles(ref list3);
						refScreenAccess.location = j;
						refScreenAccess.screen = mScreenAccess.screen;
						refScreenAccess.announcement = mScreenAccess.announcement;
						refScreenAccess.modString = mScreenAccess.modString;
						refScreenAccess.animIndex = mScreenAccess.animIndex;
						refScreenAccess.rotation = mScreenAccess.rotation; // FIX?
						modules.Add(refScreenAccess);
						break;
					}
					case ModuleType.quest_trigger: {
						QuestTriggerRoom mQuestTriggerRoom = module as QuestTriggerRoom;
						QuestTriggerRoom refQuestTriggerRoom = new QuestTriggerRoom();
						refQuestTriggerRoom.tip = mQuestTriggerRoom.tip;
						refQuestTriggerRoom.setTiles(ref list3);
						refQuestTriggerRoom.location = j;
						refQuestTriggerRoom.roomName = mQuestTriggerRoom.roomName;
						modules.Add(refQuestTriggerRoom);
						break;
					}
					case ModuleType.scanner: {
						Scanner mScanner = module as Scanner;
						Scanner refScanner = new Scanner();
						refScanner.tip = mScanner.tip;
						refScanner.setTiles(ref list3);
						refScanner.location = j;
						refScanner.energyCost = mScanner.energyCost;
						refScanner.rate = mScanner.rate;
						refScanner.capacity = mScanner.capacity;
						refScanner.sizeCategory = mScanner.sizeCategory;
						refScanner.radius = mScanner.radius;
						modules.Add(refScanner);
						scanners.Add(refScanner);
						powerUsers.Add(refScanner);
						break;
					}
					case ModuleType.warhead_converter: {
						WarheadConverter mWarheadConverter = module as WarheadConverter;
						WarheadConverter refWarheadConverter = new WarheadConverter();
						refWarheadConverter.tip = mWarheadConverter.tip;
						refWarheadConverter.setTiles(ref list3);
						refWarheadConverter.location = j;
						refWarheadConverter.rate = mWarheadConverter.rate;
						refWarheadConverter.capacity = mWarheadConverter.capacity;
						refWarheadConverter.sizeCategory = mWarheadConverter.sizeCategory;
						refWarheadConverter.missileType = mWarheadConverter.missileType;
						modules.Add(refWarheadConverter);
						systems.Add(refWarheadConverter);
						missileModulesTileCount += refWarheadConverter.tiles.Length;
						break;
					}
					case ModuleType.inhibitor_pulse: {
						InhibitorPulse mInhibitorPulse = module as InhibitorPulse;
						InhibitorPulse refInhibitorPulse = new InhibitorPulse();
						refInhibitorPulse.tip = mInhibitorPulse.tip;
						refInhibitorPulse.setTiles(ref list3);
						refInhibitorPulse.location = j;
						refInhibitorPulse.rate = mInhibitorPulse.rate;
						refInhibitorPulse.spellID = mInhibitorPulse.spellID;
						refInhibitorPulse.capacity = mInhibitorPulse.capacity;
						modules.Add(refInhibitorPulse);
						systems.Add(refInhibitorPulse);
						powerUsers.Add(refInhibitorPulse);
						refInhibitorPulse.configure(width, height);
						break;
					}
					case ModuleType.artifact_activator: {
						ArtifactActivator mArtifactActivator = module as ArtifactActivator;
						ArtifactActivator refArtifactActivator = new ArtifactActivator();
						refArtifactActivator.tip = mArtifactActivator.tip;
						refArtifactActivator.setTiles(ref list3);
						refArtifactActivator.location = j;
						refArtifactActivator.rate = mArtifactActivator.rate;
						refArtifactActivator.capacity = mArtifactActivator.capacity;
						refArtifactActivator.animIndex = mArtifactActivator.animIndex;
						refArtifactActivator.rotation = mArtifactActivator.rotation;
						artifactActivators.Add(refArtifactActivator);
						powerUsers.Add(refArtifactActivator);
						modules.Add(refArtifactActivator);
						break;
					}
					case ModuleType.heat_vent: {
						HeatVent mHeatVent = module as HeatVent;
						HeatVent refHeatVent = new HeatVent();
						refHeatVent.tip = mHeatVent.tip;
						refHeatVent.setTiles(ref list3);
						refHeatVent.location = j;
						refHeatVent.rate = mHeatVent.rate;
						refHeatVent.capacity = mHeatVent.capacity;
						refHeatVent.spellID = mHeatVent.spellID;
						refHeatVent.heatCapacity = mHeatVent.heatCapacity;
						refHeatVent.dissopationRate = mHeatVent.dissopationRate;
						modules.Add(refHeatVent);
						systems.Add(refHeatVent);
						radiators.Add(refHeatVent);
						powerUsers.Add(refHeatVent);
						refHeatVent.configure(width, height);
						break;
					}
					case ModuleType.distant_AoE: {
						DistantAoE mDistantAoE = module as DistantAoE;
						DistantAoE refDistantAoE = new DistantAoE();
						refDistantAoE.tip = mDistantAoE.tip;
						refDistantAoE.setTiles(ref list3);
						refDistantAoE.location = j;
						refDistantAoE.rate = mDistantAoE.rate;
						refDistantAoE.useCost = mDistantAoE.useCost;
						refDistantAoE.capacity = mDistantAoE.capacity;
						refDistantAoE.spellID = mDistantAoE.spellID;
						modules.Add(refDistantAoE);
						systems.Add(refDistantAoE);
						powerUsers.Add(refDistantAoE);
						refDistantAoE.configure(width, height);
						break;
					}
					case ModuleType.fission_torpedoes: {
						FissionTorpedos mFissionTorpedos = module as FissionTorpedos;
						FissionTorpedos refFissionTorpedos = new FissionTorpedos();
						refFissionTorpedos.tip = mFissionTorpedos.tip;
						refFissionTorpedos.setTiles(ref list3);
						refFissionTorpedos.location = j;
						refFissionTorpedos.rate = mFissionTorpedos.rate;
						refFissionTorpedos.capacity = mFissionTorpedos.capacity;
						refFissionTorpedos.useCost = mFissionTorpedos.useCost;
						refFissionTorpedos.spellID = mFissionTorpedos.spellID;
						refFissionTorpedos.rotation = mFissionTorpedos.rotation;
						modules.Add(refFissionTorpedos);
						systems.Add(refFissionTorpedos);
						powerUsers.Add(refFissionTorpedos);
						refFissionTorpedos.configure(width, height);
						break;
					}
					case ModuleType.LRD_system: {
						LRDSystem mLRDSystem = module as LRDSystem;
						LRDSystem refLRDSystem = new LRDSystem();
						refLRDSystem.tip = mLRDSystem.tip;
						refLRDSystem.setTiles(ref list3);
						refLRDSystem.location = j;
						refLRDSystem.rate = mLRDSystem.rate;
						refLRDSystem.capacity = mLRDSystem.capacity;
						refLRDSystem.useCost = mLRDSystem.useCost;
						refLRDSystem.spellID = mLRDSystem.spellID;
						modules.Add(refLRDSystem);
						systems.Add(refLRDSystem);
						powerUsers.Add(refLRDSystem);
						refLRDSystem.configure(width, height);
						break;
					}
					case ModuleType.channeled_debuff: {
						ChanneledDebuff mChanneledDebuff = module as ChanneledDebuff;
						ChanneledDebuff refChanneledDebuff = new ChanneledDebuff();
						refChanneledDebuff.tip = mChanneledDebuff.tip;
						refChanneledDebuff.setTiles(ref list3);
						refChanneledDebuff.location = j;
						refChanneledDebuff.rate = mChanneledDebuff.rate;
						refChanneledDebuff.capacity = mChanneledDebuff.capacity;
						refChanneledDebuff.spellID = mChanneledDebuff.spellID;
						refChanneledDebuff.useCost = mChanneledDebuff.useCost;
						modules.Add(refChanneledDebuff);
						systems.Add(refChanneledDebuff);
						powerUsers.Add(refChanneledDebuff);
						refChanneledDebuff.configure(width, height);
						break;
					}
					case ModuleType.anchor: {
						Anchor mAnchor = module as Anchor;
						Anchor refAnchor = new Anchor();
						refAnchor.tip = mAnchor.tip;
						refAnchor.setTiles(ref list3);
						refAnchor.location = j;
						refAnchor.rate = mAnchor.rate;
						refAnchor.capacity = mAnchor.capacity;
						refAnchor.useCost = mAnchor.useCost;
						refAnchor.spellID = mAnchor.spellID;
						refAnchor.rotation = mAnchor.rotation;
						refAnchor.animIndex = mAnchor.animIndex;
						refAnchor.active = true;
						modules.Add(refAnchor);
						systems.Add(refAnchor);
						powerUsers.Add(refAnchor);
						refAnchor.configure(width, height);
						break;
					}
					case ModuleType.magrail: {
						MagrailModule mMagrailModule = module as MagrailModule;
						MagrailModule refMagrailModule = new MagrailModule();
						refMagrailModule.tip = mMagrailModule.tip;
						refMagrailModule.setTiles(ref list3);
						refMagrailModule.location = j;
						refMagrailModule.type = mMagrailModule.type;
						refMagrailModule.rotation = mMagrailModule.rotation;
						refMagrailModule.animIndex = mMagrailModule.animIndex;
						refMagrailModule.active = true;
						refMagrailModule.announcement = mMagrailModule.announcement;
						refMagrailModule.exitSpot = mMagrailModule.exitSpot;
						modules.Add(refMagrailModule);
						break;
					}
					case ModuleType.crew_interaction: {
						InteractiveRoom mInteractiveRoom = module as InteractiveRoom;
						InteractiveRoom refInteractiveRoom = new InteractiveRoom();
						refInteractiveRoom.tip = mInteractiveRoom.tip;
						refInteractiveRoom.tipText = mInteractiveRoom.tipText;
						refInteractiveRoom.interaction = mInteractiveRoom.interaction;
						refInteractiveRoom.setTiles(ref list3);
						refInteractiveRoom.location = j;
						refInteractiveRoom.rotation = mInteractiveRoom.rotation;
						refInteractiveRoom.animIndex = mInteractiveRoom.animIndex;
						refInteractiveRoom.active = true;
						modules.Add(refInteractiveRoom);
						break;
					}
					case ModuleType.computer_core: {
						ComputerCore mComputerCore = module as ComputerCore;
						ComputerCore refComputerCore = new ComputerCore();
						refComputerCore.tip = mComputerCore.tip;
						refComputerCore.setTiles(ref list3);
						refComputerCore.location = j;
						refComputerCore.rotation = mComputerCore.rotation;
						refComputerCore.animIndex = mComputerCore.animIndex;
						refComputerCore.rate = mComputerCore.rate;
						refComputerCore.capacity = mComputerCore.capacity;
						refComputerCore.powerNeed = mComputerCore.powerNeed;
						refComputerCore.aggressivity = mComputerCore.aggressivity;
						refComputerCore.cleverness = mComputerCore.cleverness;
						refComputerCore.accuracy = mComputerCore.accuracy;
						refComputerCore.aimingBehaviorPreference = mComputerCore.aimingBehaviorPreference;
						modules.Add(refComputerCore);
						brain = refComputerCore;
						ship.aiControlled = true;
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
			foreach (ShieldEmitter emitter in emitters) emitter.applyField(list);
			foreach (PoweredTurretMount mount in mounts) mount.configure(tiles, width);
			blank.blocking = false;
			blank.airBlocking = true;
			blank.repairable = false;
			ship.shieldCap = shieldCap;
			ship.engineCap = engineThrust;
			ship.thrusterCap = thrusterOut;
			checkHealthCommit();
			setMoment();
			ship.integrityCap = (float)(integrityBoost - moment);
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
		[MonoModIfFlag("SP")] private void updateModule(Module mod, ModuleConnectionUpdateType updateType) {
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