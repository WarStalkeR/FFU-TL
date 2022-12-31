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
		}
		public SinidalCascade() {
		}
		public void configure(int width, int height) {
		}
	}
	[MonoModIgnore] internal class MatterFurnace : Module, Activateable {
		public Storage storage;
		public MatterFurnace(ModTile[] list) : base(list) {
		}
		public MatterFurnace() {
		}
		public void activate(Crew crew, MicroCosm cosm, Vector2 target) {
		}
		public void trip(ShipActor crew, MicroCosm cosm) {
		}
		public string getTip() {
			return "Access Matter Furnace";
		}
		public void setActiveTiles(MicroCosm cosm) {
		}
		public override void animate(float elapsed) {
		}
	}
	[MonoModIgnore] internal class LRDSystem : TacticalSystem {
		public Vector2 home;
		public float useCost;
		public LRDSystem(ModTile[] list) : base(list) {
		}
		public LRDSystem() {
		}
		public void configure(int width, int height) {
		}
	}
	[MonoModIgnore] internal class InhibitorPulse : TacticalSystem {
		public Vector2 home;
		public InhibitorPulse(ModTile[] list) : base(list) {
		}
		public InhibitorPulse() {
		}
		public void configure(int width, int height) {
		}
		public override void animate(float elapsed) {
		}
	}
	[MonoModIgnore] internal class FissionTorpedos : TacticalSystem {
		public Vector2 home;
		public float useCost;
		public FissionTorpedos(ModTile[] list) : base(list) {
		}
		public FissionTorpedos() {
		}
		public void configure(int width, int height) {
		}
	}
	[MonoModIgnore] internal class DistantAoE : TacticalSystem {
		public Vector2 home;
		private float maxRange;
		private float chargeTime;
		private float radius;
		public float useCost;
		public DistantAoE(ModTile[] list) : base(list) {
		}
		public DistantAoE() {
		}
		public void configure(int width, int height) {
		}
		public override void animate(float elapsed) {
		}
	}
	[MonoModIgnore] internal class ChanneledDebuff : TacticalSystem {
		public Vector2 home;
		public float useCost;
		public ChanneledDebuff(ModTile[] list) : base(list) {
		}
		public ChanneledDebuff() {
		}
		public void configure(int width, int height) {
		}
	}
}