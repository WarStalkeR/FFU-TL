#pragma warning disable CS0108
#pragma warning disable CS0169
#pragma warning disable CS0414
#pragma warning disable CS0649
#pragma warning disable CS0626

using MonoMod;
using CoOpSpRpG;
using FFU_Terra_Liberatio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace FFU_Terra_Liberatio {
    public class FFU_TL_Tile_Missiles {
		public static void updateModules(Dictionary<byte, Dictionary<byte, Dictionary<byte, Module>>> modules) {
			ModLog.Message($"Applying module changes: Missiles & Torpedoes.");
			MissileFactory(modules, 144, 223, 22);
			MissileFactory(modules, 144, 224, 22);
			MissileFactory(modules, 144, 225, 22);
			MissileFactory(modules, 144, 226, 22);
			MissileFactory(modules, 144, 227, 22);
			MissileFactory(modules, 144, 228, 22);
			MissileFactory(modules, 144, 229, 22);
			MissileFactory(modules, 144, 230, 22);
			TorpedoFactory(modules, 145, 167, 24);
			TorpedoFactory(modules, 145, 168, 24);
		}
		public static void MissileFactory(Dictionary<byte, Dictionary<byte, Dictionary<byte, Module>>> modules, byte r, byte g, byte b) {
			FFU_TL_Defs.unlistDynamic.Add(new Color(r, g, b));
			FFU_TL_Defs.unlistDynamic = FFU_TL_Defs.unlistDynamic.ToList();
			MissileFactory(modules[r][g][b] as MissileFactory);
		}
		public static void TorpedoFactory(Dictionary<byte, Dictionary<byte, Dictionary<byte, Module>>> modules, byte r, byte g, byte b) {
			FFU_TL_Defs.unlistDynamic.Add(new Color(r, g, b));
			FFU_TL_Defs.unlistDynamic = FFU_TL_Defs.unlistDynamic.ToList();
			TorpedoFactory(modules[r][g][b] as MissileFactory);
		}
		public static void MissileFactory(MissileFactory mFactory) {
			mFactory.cost = 8500;
			mFactory.toolTip = "Missile Factory";
			mFactory.techLevel = 3;
			mFactory.tip = new ToolTip();
			mFactory.tip.tip = mFactory.toolTip;
			mFactory.tip.botLeftText = $"Almaz-Antey Interstellar";
			mFactory.tip.description = "Modular missile factory that can be installed on any ship that has enough space and energy to support it. Uses grey goo and high temperature plasma to manufacture missiles via efficient energy-matter converter. Connect linked missile silos to at least 1 of 4 outlets to unload produced ammo.";
			mFactory.tip.tierIcontype = (TierIcon)mFactory.techLevel;
			mFactory.tip.addStat($"Fabrication Rate", $"{mFactory.rate * 5f:0} Energy/s.", false);
			mFactory.tip.addStat($"{patch_MISSILEBAG.typeName(patch_MissileType.standard)} Cost", $"{patch_MISSILEBAG.constructionCost(patch_MissileType.standard)} En.", false);
			mFactory.tip.addStat($"{patch_MISSILEBAG.typeName(patch_MissileType.graviton)} Cost", $"{patch_MISSILEBAG.constructionCost(patch_MissileType.graviton)} En.", false);
			mFactory.tip.addStat($"{patch_MISSILEBAG.typeName(patch_MissileType.armorpiercing)} Cost", $"{patch_MISSILEBAG.constructionCost(patch_MissileType.armorpiercing)} En.", false);
			mFactory.tip.addStat($"{patch_MISSILEBAG.typeName(patch_MissileType.devastator)} Cost", $"{patch_MISSILEBAG.constructionCost(patch_MissileType.devastator)} En.", false);
			mFactory.tip.addStat($"{patch_MISSILEBAG.typeName(patch_MissileType.hunter)} Cost", $"{patch_MISSILEBAG.constructionCost(patch_MissileType.hunter)} En.", false);
			mFactory.tip.addStat($"{patch_MISSILEBAG.typeName(patch_MissileType.impactor)} Cost", $"{patch_MISSILEBAG.constructionCost(patch_MissileType.impactor)} En.", false);
			mFactory.tip.addStat($"{patch_MISSILEBAG.typeName(patch_MissileType.corrosive)} Cost", $"{patch_MISSILEBAG.constructionCost(patch_MissileType.corrosive)} En.", false);
			mFactory.tip.addStat($"{patch_MISSILEBAG.typeName(patch_MissileType.terror)} Cost", $"{patch_MISSILEBAG.constructionCost(patch_MissileType.terror)} En.", false);
			mFactory.tip.addStat($"{patch_MISSILEBAG.typeName(patch_MissileType.interdict)} Cost", $"{patch_MISSILEBAG.constructionCost(patch_MissileType.interdict)} En.", false);
		}
		public static void TorpedoFactory(MissileFactory mFactory) {
			mFactory.cost = 17000;
			mFactory.toolTip = "Torpedo Factory";
			mFactory.techLevel = 3;
			mFactory.tip = new ToolTip();
			mFactory.tip.tip = mFactory.toolTip;
			mFactory.tip.botLeftText = $"Almaz-Antey Interstellar";
			mFactory.tip.description = "Modular torpedo factory that can be installed on any ship that has enough space and energy to support it. Uses grey goo and high temperature plasma to manufacture torpedoes via efficient energy-matter converter. Connect linked torpedo silos to at least 1 of 4 outlets to unload produced ammo.";
			mFactory.tip.tierIcontype = (TierIcon)mFactory.techLevel;
			mFactory.tip.addStat($"Fabrication Rate", $"{mFactory.rate * 5f:0} Energy/s.", false);
			mFactory.tip.addStat($"{patch_MISSILEBAG.typeName(patch_MissileType.antimatter_siege)} Cost", $"{patch_MISSILEBAG.constructionCost(patch_MissileType.antimatter_siege)} En.", false);
			mFactory.tip.addStat($"{patch_MISSILEBAG.typeName(patch_MissileType.seeker_siege)} Cost", $"{patch_MISSILEBAG.constructionCost(patch_MissileType.seeker_siege)} En.", false);
			mFactory.tip.addStat($"{patch_MISSILEBAG.typeName(patch_MissileType.fission_siege)} Cost", $"{patch_MISSILEBAG.constructionCost(patch_MissileType.fission_siege)} En.", false);
			mFactory.tip.addStat($"{patch_MISSILEBAG.typeName(patch_MissileType.hellfire_siege)} Cost", $"{patch_MISSILEBAG.constructionCost(patch_MissileType.hellfire_siege)} En.", false);
			mFactory.tip.addStat($"{patch_MISSILEBAG.typeName(patch_MissileType.mirv_siege)} Cost", $"{patch_MISSILEBAG.constructionCost(patch_MissileType.mirv_siege)} En.", false);
			mFactory.tip.addStat($"{patch_MISSILEBAG.typeName(patch_MissileType.red_siege)} Cost", $"{patch_MISSILEBAG.constructionCost(patch_MissileType.red_siege)} En.", false);
		}
	}
}

namespace CoOpSpRpG {
	public static class patch_MISSILEBAG {
		[MonoModAdded] public static string typeName(patch_MissileType type) {
		/// Missiles & Torpedoes custom reload time.
			return type switch {
				patch_MissileType.standard => "Standard Missile",
				patch_MissileType.graviton => "Graviton Missile",
				patch_MissileType.armorpiercing => "Anti-Armor Missile",
				patch_MissileType.devastator => "Devastator Missile",
				patch_MissileType.hunter => "Hunter Missile",
				patch_MissileType.impactor => "Impactor Missile",
				patch_MissileType.corrosive => "Corrosive Missile",
				patch_MissileType.terror => "Terror Missile",
				patch_MissileType.interdict => "Interdictor Missile",
				patch_MissileType.antimatter_siege => "Antimatter Torpedo",
				patch_MissileType.seeker_siege => "Seeker Torpedo",
				patch_MissileType.fission_siege => "Oblivion Torpedo",
				patch_MissileType.hellfire_siege => "Hellfire Torpedo",
				patch_MissileType.mirv_siege => "Retaliator Torpedo",
				patch_MissileType.red_siege => "Annihilator Torpedo",
				_ => "Unknown Object",
			};
		}
		[MonoModAdded] public static float reloadTime(patch_MissileType type) {
		/// Missiles & Torpedoes custom reload time.
			return type switch {
				patch_MissileType.standard => 3.5f,
				patch_MissileType.graviton => 4.5f,
				patch_MissileType.armorpiercing => 4.5f,
				patch_MissileType.devastator => 5.0f,
				patch_MissileType.hunter => 2.0f,
				patch_MissileType.impactor => 5.0f,
				patch_MissileType.corrosive => 6.0f,
				patch_MissileType.terror => 6.0f,
				patch_MissileType.interdict => 45.0f,
				patch_MissileType.antimatter_siege => 12.0f,
				patch_MissileType.seeker_siege => 8.0f,
				patch_MissileType.fission_siege => 13.0f,
				patch_MissileType.hellfire_siege => 15.0f,
				patch_MissileType.mirv_siege => 15.0f,
				patch_MissileType.red_siege => 18.0f,
				_ => 10.0f,
			};
		}
		[MonoModReplace] public static int constructionCost(patch_MissileType type) {
		/// Missiles & Torpedoes build cost rebalance.
			return type switch {
				patch_MissileType.standard => 240,
				patch_MissileType.graviton => 360,
				patch_MissileType.armorpiercing => 320,
				patch_MissileType.devastator => 420,
				patch_MissileType.hunter => 160,
				patch_MissileType.impactor => 400,
				patch_MissileType.corrosive => 480,
				patch_MissileType.terror => 560,
				patch_MissileType.interdict => 1200,
				patch_MissileType.antimatter_siege => 600,
				patch_MissileType.seeker_siege => 720,
				patch_MissileType.fission_siege => 960,
				patch_MissileType.hellfire_siege => 1320,
				patch_MissileType.mirv_siege => 1560,
				patch_MissileType.red_siege => 1800,
				_ => 600,
			};
		}
		[MonoModReplace] public static Missile createMissile(patch_MissileType type) {
		/// Missiles & Torpedoes parameters rebalance.
			Missile missile = null;
			switch (type) {
				//Modified Data
				case patch_MissileType.standard:
				missile = new Missile(SCREEN_MANAGER.GameArt[86]);
				missile.collisionData = BULLETBAG.splodelist[14];
				missile.collisionHeight = 28;
				missile.collisionWidth = 28;
				missile.explosion = ExplosionType.small_missile;
				missile.effect = WeaponEffectType.none;
				missile.fuel = 44f;
				missile.rotationSpeed = 0.5f;
				missile.topSpeed = 600f;
				missile.launchSpeed = 60f;
				missile.acceleration = 60f;
				missile.radius = 6f;
				missile.hp = 1;
				break;
				case patch_MissileType.graviton:
				missile = new Missile(SCREEN_MANAGER.GameArt[86]);
				missile.collisionData = BULLETBAG.splodelist[14];
				missile.collisionHeight = 25;
				missile.collisionWidth = 25;
				missile.explosion = ExplosionType.graviton_missile;
				missile.effect = WeaponEffectType.graviton_eplosion_missile;
				missile.fuel = 32f;
				missile.rotationSpeed = 0.45f;
				missile.topSpeed = 600f;
				missile.launchSpeed = 70f;
				missile.acceleration = 90f;
				missile.radius = 5.6f;
				missile.hp = 2;
				missile.trailType = 3;
				break;
				case patch_MissileType.armorpiercing:
				missile = new Missile(SCREEN_MANAGER.GameArt[86]);
				missile.collisionData = BULLETBAG.splodelist[14];
				missile.collisionHeight = 42;
				missile.collisionWidth = 12;
				missile.explosion = ExplosionType.ap_missile;
				missile.effect = WeaponEffectType.none;
				missile.fuel = 32f;
				missile.rotationSpeed = 0.45f;
				missile.topSpeed = 600f;
				missile.launchSpeed = 70f;
				missile.acceleration = 90f;
				missile.radius = 5.6f;
				missile.hp = 2;
				missile.trailType = 4;
				break;
				case patch_MissileType.interdict:
				missile = new Missile(SCREEN_MANAGER.GameArt[86]);
				missile.collisionData = BULLETBAG.splodelist[1];
				missile.collisionHeight = 3;
				missile.collisionWidth = 3;
				missile.explosion = ExplosionType.small_missile;
				missile.effect = WeaponEffectType.interdict_missile;
				missile.fuel = 180f;
				missile.rotationSpeed = 4.9f;
				missile.topSpeed = 12500f;
				missile.launchSpeed = 250f;
				missile.acceleration = 250f;
				missile.radius = 6f;
				missile.hp = 25;
				break;
				case patch_MissileType.red_siege:
				missile = new Missile(SCREEN_MANAGER.GameArt[86]);
				missile.collisionData = BULLETBAG.splodelist[14];
				missile.collisionHeight = 42;
				missile.collisionWidth = 12;
				missile.explosion = ExplosionType.ap_missile;
				missile.effect = WeaponEffectType.none;
				missile.fuel = 32f;
				missile.rotationSpeed = 0.45f;
				missile.topSpeed = 600f;
				missile.launchSpeed = 70f;
				missile.acceleration = 90f;
				missile.radius = 5.6f;
				missile.hp = 2;
				missile.trailType = 4;
				break;
				case patch_MissileType.antimatter_siege:
				missile = new Missile(SCREEN_MANAGER.GameArt[128]);
				missile.collisionData = BULLETBAG.splodelist[13];
				missile.collisionHeight = 48;
				missile.collisionWidth = 48;
				missile.explosion = ExplosionType.antimatter_large;
				missile.effect = WeaponEffectType.none;
				missile.fuel = 52f;
				missile.rotationSpeed = 0.2f;
				missile.topSpeed = 300f;
				missile.launchSpeed = 70f;
				missile.acceleration = 40f;
				missile.radius = 12f;
				missile.hp = 5;
				missile.trailType = 1;
				break;
				case patch_MissileType.fission_siege:
				missile = new Missile(SCREEN_MANAGER.GameArt[86]);
				missile.collisionData = BULLETBAG.splodelist[14];
				missile.collisionHeight = 38;
				missile.collisionWidth = 20;
				missile.explosion = ExplosionType.fission_torpedo;
				missile.effect = WeaponEffectType.none;
				missile.fuel = 16f;
				missile.rotationSpeed = 0f;
				missile.topSpeed = 70f;
				missile.launchSpeed = 20f;
				missile.acceleration = 30f;
				missile.radius = 7.6f;
				missile.hp = 4;
				missile.trailType = 3;
				break;
				case patch_MissileType.seeker_siege:
				missile = new Missile(SCREEN_MANAGER.GameArt[86]);
				missile.collisionData = BULLETBAG.splodelist[14];
				missile.collisionHeight = 38;
				missile.collisionWidth = 20;
				missile.explosion = ExplosionType.small_missile;
				missile.effect = WeaponEffectType.none;
				missile.fuel = 92f;
				missile.rotationSpeed = 1.35f;
				missile.topSpeed = 400f;
				missile.launchSpeed = 100f;
				missile.acceleration = 120f;
				missile.radius = 5.6f;
				missile.hp = 12;
				missile.trailType = 5;
				break;
				case patch_MissileType.hellfire_siege:
				missile = new Missile(SCREEN_MANAGER.GameArt[86]);
				missile.collisionData = BULLETBAG.splodelist[14];
				missile.collisionHeight = 38;
				missile.collisionWidth = 20;
				missile.explosion = ExplosionType.small_missile;
				missile.effect = WeaponEffectType.none;
				missile.fuel = 32f;
				missile.rotationSpeed = 0.45f;
				missile.topSpeed = 300f;
				missile.launchSpeed = 70f;
				missile.acceleration = 90f;
				missile.radius = 5.6f;
				missile.hp = 2;
				missile.trailType = 3;
				break;
				case patch_MissileType.mirv_siege:
				missile = new Missile(SCREEN_MANAGER.GameArt[86]);
				missile.collisionData = BULLETBAG.splodelist[14];
				missile.collisionHeight = 38;
				missile.collisionWidth = 20;
				missile.explosion = ExplosionType.small_missile;
				missile.effect = WeaponEffectType.none;
				missile.fuel = 32f;
				missile.rotationSpeed = 0.45f;
				missile.topSpeed = 300f;
				missile.launchSpeed = 70f;
				missile.acceleration = 90f;
				missile.radius = 5.6f;
				missile.hp = 2;
				missile.trailType = 3;
				break;
				//Original Data
				case patch_MissileType.drill_bore:
				missile = new Missile(SCREEN_MANAGER.GameArt[86]);
				missile.collisionData = null;
				missile.effect = WeaponEffectType.drill_bore;
				missile.fuel = 4000f;
				missile.rotationSpeed = 5f;
				missile.topSpeed = 600f;
				missile.launchSpeed = 1f;
				missile.explosion = ExplosionType.none;
				break;
				case patch_MissileType.repair_drone_t1:
				missile = new Missile(SCREEN_MANAGER.GameArt[86]);
				missile.fuel = 120f;
				missile.controller = new RepairDroneMissileController();
				missile.topSpeed = CONFIG.maxSpeed * 1.5f;
				missile.explosion = ExplosionType.small_missile;
				break;
				case patch_MissileType.machinegun_drone_t1:
				missile = new Missile(SCREEN_MANAGER.GameArt[86]);
				missile.fuel = 120f;
				missile.controller = new BulletDroneMissileController();
				missile.topSpeed = CONFIG.maxSpeed * 1.5f;
				missile.explosion = ExplosionType.small_missile;
				break;
				case patch_MissileType.supply_drone:
				missile = new Missile(SCREEN_MANAGER.GameArt[86]);
				missile.fuel = 120f;
				missile.controller = new SupplyDroneControllerAnim();
				missile.topSpeed = CONFIG.maxSpeed * 1.5f;
				missile.explosion = ExplosionType.small_missile;
				break;
				case patch_MissileType.railarray_drone_t2:
				missile = new Missile(SCREEN_MANAGER.GameArt[86]);
				missile.fuel = 120f;
				missile.hp = 3;
				missile.controller = new RailDroneMissileController();
				missile.topSpeed = CONFIG.maxSpeed * 1.5f;
				missile.explosion = ExplosionType.small_missile;
				break;
				case patch_MissileType.antimatter_drone_t2:
				missile = new Missile(SCREEN_MANAGER.GameArt[86]);
				missile.fuel = 120f;
				missile.hp = 3;
				missile.controller = new BombDroneMissileController();
				missile.topSpeed = CONFIG.maxSpeed * 0.0145f * 60f;
				missile.explosion = ExplosionType.small_missile;
				break;
				case patch_MissileType.goliath_fighter_t2:
				missile = new Missile(SCREEN_MANAGER.GameArt[205]);
				missile.drawNormal = true;
				missile.fuel = 800f;
				missile.hp = 50;
				missile.controller = new FighterMissileController(MissileType.goliath_fighter_t2);
				missile.explosion = ExplosionType.small_missile;
				missile.artOrigin = new Vector2(11f, 33f);
				missile.scale = 0.8f;
				break;
				case patch_MissileType.goliath_support_t2:
				missile = new Missile(SCREEN_MANAGER.GameArt[206]);
				missile.drawNormal = true;
				missile.fuel = 800f;
				missile.hp = 50;
				missile.controller = new FighterMissileController(MissileType.goliath_support_t2);
				missile.explosion = ExplosionType.small_missile;
				missile.artOrigin = new Vector2(SCREEN_MANAGER.GameArt[206].Width / 2, 33f);
				missile.scale = 0.8f;
				break;
				case patch_MissileType.locust_drone_t3:
				missile = new Missile(SCREEN_MANAGER.GameArt[86]);
				missile.fuel = 220f;
				missile.hp = 25;
				missile.controller = new SwarmDroneMissileController();
				missile.topSpeed = CONFIG.maxSpeed * 0.0145f;
				missile.explosion = ExplosionType.small_missile;
				break;
				case patch_MissileType.blue_spore:
				missile = new Missile(SCREEN_MANAGER.GameArt[86]);
				missile.collisionData = BULLETBAG.splodelist[10];
				missile.collisionHeight = 3;
				missile.collisionWidth = 3;
				missile.explosion = ExplosionType.none;
				missile.effect = WeaponEffectType.add_monster_spawner;
				missile.fuel = 32f;
				missile.rotationSpeed = 1.45f;
				missile.topSpeed = 420f;
				missile.launchSpeed = 55f;
				missile.acceleration = 0f;
				missile.radius = 5.6f;
				missile.hp = 5;
				missile.trailType = 3;
				break;
				case patch_MissileType.blue_biome_visuals:
				missile = new Missile(SCREEN_MANAGER.GameArt[86]);
				missile.collisionData = BULLETBAG.splodelist[10];
				missile.collisionHeight = 3;
				missile.collisionWidth = 3;
				missile.explosion = ExplosionType.none;
				missile.effect = WeaponEffectType.none;
				missile.fuel = 16f;
				missile.rotationSpeed = 1.45f;
				missile.topSpeed = 0f;
				missile.launchSpeed = 0f;
				missile.acceleration = 0f;
				missile.radius = 5.6f;
				missile.hp = 5;
				missile.trailType = 3;
				missile.controller = new BlueBiomeVisualSpawner();
				break;
				case patch_MissileType.mine:
				missile = new Missile(SCREEN_MANAGER.GameArt[101]);
				missile.collisionData = BULLETBAG.splodelist[14];
				missile.collisionHeight = 38;
				missile.collisionWidth = 20;
				missile.explosion = ExplosionType.fission_torpedo;
				missile.effect = WeaponEffectType.none;
				missile.fuel = 16f;
				missile.rotationSpeed = 5f;
				missile.topSpeed = 120f;
				missile.launchSpeed = 20f;
				missile.acceleration = 30f;
				missile.radius = 7.6f;
				missile.hp = 4;
				missile.trailType = 3;
				break;
			}
			if (missile != null) missile.type = (MissileType)type;
			return missile;
		}
	}
	public class patch_MissileMagazineAnim : MissileMagazineAnim {
		[MonoModIgnore] private Rectangle artSource;
		[MonoModIgnore] public patch_MissileMagazineAnim(byte r) : base(r) { }
		[MonoModReplace] public void setType(patch_MissileType type) {
		/// Sprite fix for graviton and implementation for new.
			switch (type) {
				default:
				case patch_MissileType.drill_bore:
				case patch_MissileType.supply_drone:
				case patch_MissileType.repair_drone_t1:
				case patch_MissileType.machinegun_drone_t1:
				case patch_MissileType.railarray_drone_t2:
				case patch_MissileType.antimatter_drone_t2:
				case patch_MissileType.goliath_fighter_t2:
				case patch_MissileType.goliath_support_t2:
				case patch_MissileType.locust_drone_t3:
				case patch_MissileType.blue_biome_visuals:
				case patch_MissileType.blue_spore:
				case patch_MissileType.mine:
				case patch_MissileType.mirv_srm:
				case patch_MissileType.standard:
				artSource.X = 0;
				artSource.Y = 128;
				artSource.Width = 32;
				artSource.Height = 32;
				break;
				case patch_MissileType.graviton:
				artSource.X = 32;
				artSource.Y = 128;
				artSource.Width = 32;
				artSource.Height = 32;
				break;
				case patch_MissileType.armorpiercing:
				artSource.X = 64;
				artSource.Y = 128;
				artSource.Width = 32;
				artSource.Height = 32;
				break;
				case patch_MissileType.devastator:
				artSource.X = 96;
				artSource.Y = 128;
				artSource.Width = 32;
				artSource.Height = 32;
				break;
				case patch_MissileType.hunter:
				artSource.X = 128;
				artSource.Y = 128;
				artSource.Width = 32;
				artSource.Height = 32;
				break;
				case patch_MissileType.impactor:
				artSource.X = 160;
				artSource.Y = 128;
				artSource.Width = 32;
				artSource.Height = 32;
				break;
				case patch_MissileType.corrosive:
				artSource.X = 192;
				artSource.Y = 128;
				artSource.Width = 32;
				artSource.Height = 32;
				break;
				case patch_MissileType.terror:
				artSource.X = 224;
				artSource.Y = 128;
				artSource.Width = 32;
				artSource.Height = 32;
				break;
				case patch_MissileType.interdict:
				artSource.X = 256;
				artSource.Y = 128;
				artSource.Width = 32;
				artSource.Height = 32;
				break;
				case patch_MissileType.antimatter_siege:
				artSource.X = 0;
				artSource.Y = 368;
				artSource.Width = 48;
				artSource.Height = 48;
				break;
				case patch_MissileType.seeker_siege:
				artSource.X = 48;
				artSource.Y = 368;
				artSource.Width = 48;
				artSource.Height = 48;
				break;
				case patch_MissileType.fission_siege:
				artSource.X = 96;
				artSource.Y = 368;
				artSource.Width = 48;
				artSource.Height = 48;
				break;
				case patch_MissileType.hellfire_siege:
				artSource.X = 144;
				artSource.Y = 368;
				artSource.Width = 48;
				artSource.Height = 48;
				break;
				case patch_MissileType.red_siege:
				artSource.X = 192;
				artSource.Y = 368;
				artSource.Width = 48;
				artSource.Height = 48;
				break;
				case patch_MissileType.mirv_siege:
				artSource.X = 240;
				artSource.Y = 368;
				artSource.Width = 48;
				artSource.Height = 48;
				break;
			}
		}
	}
	public class patch_MissileTubeAnim : MissileTubeAnim {
		[MonoModIgnore] private Rectangle artSource;
		[MonoModIgnore] public patch_MissileTubeAnim(byte r, bool selfLoad, bool siege) : base(r, selfLoad, siege) { }
		[MonoModReplace] public void setType(patch_MissileType type) {
		/// Sprite fix for graviton and implementation for new.
			switch (type) {
				default:
				case patch_MissileType.drill_bore:
				case patch_MissileType.supply_drone:
				case patch_MissileType.repair_drone_t1:
				case patch_MissileType.machinegun_drone_t1:
				case patch_MissileType.railarray_drone_t2:
				case patch_MissileType.antimatter_drone_t2:
				case patch_MissileType.goliath_fighter_t2:
				case patch_MissileType.goliath_support_t2:
				case patch_MissileType.locust_drone_t3:
				case patch_MissileType.blue_biome_visuals:
				case patch_MissileType.blue_spore:
				case patch_MissileType.mine:
				case patch_MissileType.mirv_srm:
				case patch_MissileType.standard:
				artSource.X = 0;
				artSource.Y = 0;
				artSource.Width = 32;
				artSource.Height = 128;
				break;
				case patch_MissileType.graviton:
				artSource.X = 32;
				artSource.Y = 0;
				artSource.Width = 32;
				artSource.Height = 128;
				break;
				case patch_MissileType.armorpiercing:
				artSource.X = 64;
				artSource.Y = 0;
				artSource.Width = 32;
				artSource.Height = 128;
				break;
				case patch_MissileType.devastator:
				artSource.X = 96;
				artSource.Y = 0;
				artSource.Width = 32;
				artSource.Height = 128;
				break;
				case patch_MissileType.hunter:
				artSource.X = 128;
				artSource.Y = 0;
				artSource.Width = 32;
				artSource.Height = 128;
				break;
				case patch_MissileType.impactor:
				artSource.X = 160;
				artSource.Y = 0;
				artSource.Width = 32;
				artSource.Height = 128;
				break;
				case patch_MissileType.corrosive:
				artSource.X = 192;
				artSource.Y = 0;
				artSource.Width = 32;
				artSource.Height = 128;
				break;
				case patch_MissileType.terror:
				artSource.X = 224;
				artSource.Y = 0;
				artSource.Width = 32;
				artSource.Height = 128;
				break;
				case patch_MissileType.interdict:
				artSource.X = 256;
				artSource.Y = 0;
				artSource.Width = 32;
				artSource.Height = 128;
				break;
				case patch_MissileType.antimatter_siege:
				artSource.X = 0;
				artSource.Y = 160;
				artSource.Width = 48;
				artSource.Height = 208;
				break;
				case patch_MissileType.seeker_siege:
				artSource.X = 48;
				artSource.Y = 160;
				artSource.Width = 48;
				artSource.Height = 208;
				break;
				case patch_MissileType.fission_siege:
				artSource.X = 96;
				artSource.Y = 160;
				artSource.Width = 48;
				artSource.Height = 208;
				break;
				case patch_MissileType.hellfire_siege:
				artSource.X = 144;
				artSource.Y = 160;
				artSource.Width = 48;
				artSource.Height = 208;
				break;
				case patch_MissileType.red_siege:
				artSource.X = 192;
				artSource.Y = 160;
				artSource.Width = 48;
				artSource.Height = 208;
				break;
				case patch_MissileType.mirv_siege:
				artSource.X = 240;
				artSource.Y = 160;
				artSource.Width = 48;
				artSource.Height = 208;
				break;
			}
		}
	}
	[MonoModEnumReplace] public enum patch_MissileType : ushort {
		standard = 0,
		drill_bore = 1,
		repair_drone_t1 = 2,
		interdict = 3,
		supply_drone = 4,
		red_siege = 5,
		machinegun_drone_t1 = 6,
		railarray_drone_t2 = 7,
		antimatter_drone_t2 = 8,
		armorpiercing = 9,
		graviton = 10,
		seeker_siege = 11,
		antimatter_siege = 12,
		mirv_siege = 13,
		hellfire_siege = 14,
		blue_spore = 15,
		blue_biome_visuals = 16,
		fission_siege = 17,
		locust_drone_t3 = 18,
		mine = 19,
		goliath_fighter_t2 = 20,
		goliath_support_t2 = 21,
		devastator = 40,
		hunter = 41,
		impactor = 42,
		corrosive = 43,
		terror = 44,
		mirv_srm = 80
	}
}