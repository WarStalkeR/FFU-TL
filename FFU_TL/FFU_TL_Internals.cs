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
    public class FFU_TL_Internals {
    }
}

namespace CoOpSpRpG {
	[MonoModIgnore] internal class SinidalCascade : TacticalSystem {
		public Vector2 home;
		public float useCost;
		public SinidalCascade(ModTile[] list) : base(list) {
			type = ModuleType.sinidal_cascade;
		}
		public SinidalCascade() {
			type = ModuleType.sinidal_cascade;
		}
		public void configure(int width, int height) {
			int num = location % width;
			int num2 = location / width;
			num -= width / 2;
			num2 -= height / 2;
			home = new Vector2((float)num + 6.5f, num2 + 2);
		}
	}
	[MonoModIgnore] internal class MatterFurnace : Module, Activateable {
		public Storage storage;
		public MatterFurnace(ModTile[] list) : base(list) {
			type = ModuleType.matter_furnace;
			active = true;
		}
		public MatterFurnace() {
			type = ModuleType.matter_furnace;
			active = true;
		}
		public void activate(Crew crew, MicroCosm cosm, Vector2 target) {
			if (crew == PLAYER.avatar) {
				SCREEN_MANAGER.popupOverlay = new CharacterInventory(PLAYER.avatar, storage, StorageAuthority.full);
			}
		}
		public void trip(ShipActor crew, MicroCosm cosm) {
		}
		public string getTip() {
			return "Access Matter Furnace";
		}
		public void setActiveTiles(MicroCosm cosm) {
			ModTile[] array = tiles;
			foreach (ModTile modTile in array) {
				if (!modTile.blocking) {
					modTile.setTrigger(this);
				}
			}
		}
		public override void animate(float elapsed) {
			if (storage != null && !storage.looking) {
				storage.working = true;
				uint num = storage.destroyFirstItem();
				for (num /= 2u; num != 0; num = storage.destroyFirstItem()) {
					PLAYER.currentGame.furnaceQueue.Enqueue(num);
				}
				storage.working = false;
			}
		}
	}
	[MonoModIgnore] internal class LRDSystem : TacticalSystem {
		public Vector2 home;
		public float useCost;
		public LRDSystem(ModTile[] list) : base(list) {
			type = ModuleType.LRD_system;
		}
		public LRDSystem() {
			type = ModuleType.LRD_system;
		}
		public void configure(int width, int height) {
			int num = location % width;
			int num2 = location / width;
			num -= width / 2;
			num2 -= height / 2;
			home = new Vector2(num + 6.5f, num2 + 2);
		}
	}
	[MonoModIgnore] internal class InhibitorPulse : TacticalSystem {
		public Vector2 home;
		public InhibitorPulse(ModTile[] list) : base(list) {
			type = ModuleType.inhibitor_pulse;
		}
		public InhibitorPulse() {
			type = ModuleType.inhibitor_pulse;
		}
		public void configure(int width, int height) {
			int num = location % width;
			int num2 = location / width;
			num -= width / 2;
			num2 -= height / 2;
			home = new Vector2(num + 6.5f, num2 + 2);
		}
		public override void animate(float elapsed) {
			base.animate(elapsed);
		}
	}
	[MonoModIgnore] internal class FissionTorpedos : TacticalSystem {
		public Vector2 home;
		public float useCost;
		public FissionTorpedos(ModTile[] list) : base(list) {
			type = ModuleType.fission_torpedoes;
		}
		public FissionTorpedos() {
			type = ModuleType.fission_torpedoes;
		}
		public void configure(int width, int height) {
			int num = location % width;
			int num2 = location / width;
			num -= width / 2;
			num2 -= height / 2;
			home = new Vector2((float)num + 6.5f, num2 + 2);
		}
	}
	[MonoModIgnore] internal class DistantAoE : TacticalSystem {
		public Vector2 home;
		private float maxRange;
		private float chargeTime;
		private float radius;
		public float useCost;
		public DistantAoE(ModTile[] list) : base(list) {
			type = ModuleType.distant_AoE;
		}
		public DistantAoE() {
			type = ModuleType.distant_AoE;
		}
		public void configure(int width, int height) {
			int num = location % width;
			int num2 = location / width;
			num -= width / 2;
			num2 -= height / 2;
			home = new Vector2((float)num + 6.5f, num2 + 2);
		}
		public override void animate(float elapsed) {
			base.animate(elapsed);
		}
	}
	[MonoModIgnore] internal class ChanneledDebuff : TacticalSystem {
		public Vector2 home;
		public float useCost;
		public ChanneledDebuff(ModTile[] list) : base(list) {
			type = ModuleType.channeled_debuff;
		}
		public ChanneledDebuff() {
			type = ModuleType.channeled_debuff;
		}
		public void configure(int width, int height) {
			int num = location % width;
			int num2 = location / width;
			num -= width / 2;
			num2 -= height / 2;
			home = new Vector2((float)num + 6.5f, num2 + 2);
		}
	}
}