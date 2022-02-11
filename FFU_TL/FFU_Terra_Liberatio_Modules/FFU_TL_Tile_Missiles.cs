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
			ModLog.Message($"Applying module changes: Missiles & Torpedoes...");
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
			mFactory.tip.addStat("Fabrication Rate", $"{mFactory.rate * 5f:0} Energy/s.", false);
			mFactory.tip.addStat("Standard Missile Cost", $"{MISSILEBAG.constructionCost(MissileType.standard)} En.", false);
			mFactory.tip.addStat("Interdictor Missile Cost", $"{MISSILEBAG.constructionCost(MissileType.interdict)} En.", false);
			mFactory.tip.addStat("Graviton Missile Cost", $"{MISSILEBAG.constructionCost(MissileType.graviton)} En.", false);
			mFactory.tip.addStat("Anti-Armor Missile Cost", $"{MISSILEBAG.constructionCost(MissileType.armorpiercing)} En.", false);
			mFactory.tip.addStat("Mini-Torpedo Cost", $"{MISSILEBAG.constructionCost(MissileType.red_siege)} En.", false);
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
			mFactory.tip.addStat("Fabrication Rate", $"{mFactory.rate * 5f:0} Energy/s.", false);
			mFactory.tip.addStat("Antimatter Torpedo Cost", $"{MISSILEBAG.constructionCost(MissileType.antimatter_siege)} En.", false);
			mFactory.tip.addStat("Fusion Torpedo Cost", $"{MISSILEBAG.constructionCost(MissileType.fission_siege)} En.", false);
			mFactory.tip.addStat("Seeker Torpedo Cost", $"{MISSILEBAG.constructionCost(MissileType.seeker_siege)} En.", false);
			mFactory.tip.addStat("Hellfire Torpedo Cost", $"{MISSILEBAG.constructionCost(MissileType.hellfire_siege)} En.", false);
			mFactory.tip.addStat("M.I.R.V. Torpedo Cost", $"{MISSILEBAG.constructionCost(MissileType.mirv_siege)} En.", false);
		}
	}
}

namespace CoOpSpRpG {
	public static class patch_MISSILEBAG {
		[MonoModReplace] public static int constructionCost(MissileType type) {
		/// Missiles & Torpedoes build cost rebalance.
			return type switch {
				MissileType.standard => 240,
				MissileType.interdict => 1800,
				MissileType.graviton => 360,
				MissileType.armorpiercing => 360,
				MissileType.red_siege => 480,
				MissileType.antimatter_siege => 600,
				MissileType.fission_siege => 720,
				MissileType.seeker_siege => 960,
				MissileType.hellfire_siege => 1200,
				MissileType.mirv_siege => 1440,
				_ => 600,
			};
		}
		[MonoModReplace] public static Missile createMissile(MissileType type) {
		/// Missiles & Torpedoes parameters rebalance.
			Missile missile = null;
			switch (type) {
				//Modified Data
				case MissileType.standard:
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
				case MissileType.interdict:
				missile = new Missile(SCREEN_MANAGER.GameArt[86]);
				missile.collisionData = BULLETBAG.splodelist[1];
				missile.collisionHeight = 3;
				missile.collisionWidth = 3;
				missile.explosion = ExplosionType.small_missile;
				missile.effect = WeaponEffectType.interdict_missile;
				missile.fuel = 180f;
				missile.rotationSpeed = 4.9f;
				missile.topSpeed = 100000f;
				missile.launchSpeed = 200f;
				missile.acceleration = 200f;
				missile.radius = 10f;
				missile.hp = 75;
				break;
				case MissileType.graviton:
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
				case MissileType.armorpiercing:
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
				case MissileType.red_siege:
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
				case MissileType.antimatter_siege:
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
				case MissileType.fission_siege:
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
				case MissileType.seeker_siege:
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
				case MissileType.hellfire_siege:
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
				case MissileType.mirv_siege:
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
				case MissileType.drill_bore:
				missile = new Missile(SCREEN_MANAGER.GameArt[86]);
				missile.collisionData = null;
				missile.effect = WeaponEffectType.drill_bore;
				missile.fuel = 4000f;
				missile.rotationSpeed = 5f;
				missile.topSpeed = 600f;
				missile.launchSpeed = 1f;
				missile.explosion = ExplosionType.none;
				break;
				case MissileType.repair_drone_t1:
				missile = new Missile(SCREEN_MANAGER.GameArt[86]);
				missile.fuel = 120f;
				missile.controller = new RepairDroneMissileController();
				missile.topSpeed = CONFIG.maxSpeed * 1.5f;
				missile.explosion = ExplosionType.small_missile;
				break;
				case MissileType.machinegun_drone_t1:
				missile = new Missile(SCREEN_MANAGER.GameArt[86]);
				missile.fuel = 120f;
				missile.controller = new BulletDroneMissileController();
				missile.topSpeed = CONFIG.maxSpeed * 1.5f;
				missile.explosion = ExplosionType.small_missile;
				break;
				case MissileType.supply_drone:
				missile = new Missile(SCREEN_MANAGER.GameArt[86]);
				missile.fuel = 120f;
				missile.controller = new SupplyDroneControllerAnim();
				missile.topSpeed = CONFIG.maxSpeed * 1.5f;
				missile.explosion = ExplosionType.small_missile;
				break;
				case MissileType.railarray_drone_t2:
				missile = new Missile(SCREEN_MANAGER.GameArt[86]);
				missile.fuel = 120f;
				missile.hp = 3;
				missile.controller = new RailDroneMissileController();
				missile.topSpeed = CONFIG.maxSpeed * 1.5f;
				missile.explosion = ExplosionType.small_missile;
				break;
				case MissileType.antimatter_drone_t2:
				missile = new Missile(SCREEN_MANAGER.GameArt[86]);
				missile.fuel = 120f;
				missile.hp = 3;
				missile.controller = new BombDroneMissileController();
				missile.topSpeed = CONFIG.maxSpeed * 0.0145f * 60f;
				missile.explosion = ExplosionType.small_missile;
				break;
				case MissileType.goliath_fighter_t2:
				missile = new Missile(SCREEN_MANAGER.GameArt[205]);
				missile.drawNormal = true;
				missile.fuel = 800f;
				missile.hp = 50;
				missile.controller = new FighterMissileController(MissileType.goliath_fighter_t2);
				missile.explosion = ExplosionType.small_missile;
				missile.artOrigin = new Vector2(11f, 33f);
				missile.scale = 0.8f;
				break;
				case MissileType.goliath_support_t2:
				missile = new Missile(SCREEN_MANAGER.GameArt[206]);
				missile.drawNormal = true;
				missile.fuel = 800f;
				missile.hp = 50;
				missile.controller = new FighterMissileController(MissileType.goliath_support_t2);
				missile.explosion = ExplosionType.small_missile;
				missile.artOrigin = new Vector2(SCREEN_MANAGER.GameArt[206].Width / 2, 33f);
				missile.scale = 0.8f;
				break;
				case MissileType.locust_drone_t3:
				missile = new Missile(SCREEN_MANAGER.GameArt[86]);
				missile.fuel = 220f;
				missile.hp = 25;
				missile.controller = new SwarmDroneMissileController();
				missile.topSpeed = CONFIG.maxSpeed * 0.0145f;
				missile.explosion = ExplosionType.small_missile;
				break;
				case MissileType.blue_spore:
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
				case MissileType.blue_biome_visuals:
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
				case MissileType.mine:
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
			if (missile != null) missile.type = type;
			return missile;
		}
	}
	public class patch_MissileMagazineAnim : MissileMagazineAnim {
		[MonoModIgnore] private Rectangle artSource;
		[MonoModIgnore] public patch_MissileMagazineAnim(byte r) : base(r) { }
		[MonoModReplace]
		public void setType(MissileType type) {
			/// Sprite fix for graviton and implementation for new.
			switch (type) {
				default:
				case MissileType.standard:
				artSource.X = 0;
				artSource.Y = 128;
				artSource.Width = 32;
				artSource.Height = 32;
				break;
				case MissileType.interdict:
				case MissileType.graviton:
				artSource.X = 32;
				artSource.Y = 128;
				artSource.Width = 32;
				artSource.Height = 32;
				break;
				case MissileType.armorpiercing:
				case MissileType.red_siege:
				artSource.X = 64;
				artSource.Y = 128;
				artSource.Width = 32;
				artSource.Height = 32;
				break;
				case MissileType.antimatter_siege:
				case MissileType.fission_siege:
				artSource.X = 48;
				artSource.Y = 368;
				artSource.Width = 48;
				artSource.Height = 48;
				break;
				case MissileType.seeker_siege:
				case MissileType.hellfire_siege:
				case MissileType.mirv_siege:
				artSource.X = 0;
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
		[MonoModReplace]
		public void setType(MissileType type) {
			/// Sprite fix for graviton and implementation for new.
			switch (type) {
				default:
				case MissileType.standard:
				artSource.X = 0;
				artSource.Y = 0;
				artSource.Width = 32;
				artSource.Height = 128;
				break;
				case MissileType.interdict:
				case MissileType.graviton:
				artSource.X = 32;
				artSource.Y = 0;
				artSource.Width = 32;
				artSource.Height = 128;
				break;
				case MissileType.armorpiercing:
				case MissileType.red_siege:
				artSource.X = 64;
				artSource.Y = 0;
				artSource.Width = 32;
				artSource.Height = 128;
				break;
				case MissileType.antimatter_siege:
				case MissileType.fission_siege:
				artSource.X = 48;
				artSource.Y = 160;
				artSource.Width = 48;
				artSource.Height = 208;
				break;
				case MissileType.seeker_siege:
				case MissileType.hellfire_siege:
				case MissileType.mirv_siege:
				artSource.X = 0;
				artSource.Y = 160;
				artSource.Width = 48;
				artSource.Height = 208;
				break;
			}
		}
	}
	[MonoModEnumReplace] public enum patch_MissileType : ushort {
		standard,
		drill_bore,
		repair_drone_t1,
		interdict,
		supply_drone,
		red_siege,
		machinegun_drone_t1,
		railarray_drone_t2,
		antimatter_drone_t2,
		armorpiercing,
		graviton,
		seeker_siege,
		antimatter_siege,
		mirv_siege,
		hellfire_siege,
		blue_spore,
		blue_biome_visuals,
		fission_siege,
		locust_drone_t3,
		mine,
		goliath_fighter_t2,
		goliath_support_t2,
		devastator,
		hunter,
		havoc,
		chemical,
		overload,
	}
}