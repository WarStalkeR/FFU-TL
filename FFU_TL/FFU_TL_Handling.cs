#pragma warning disable CS0169
#pragma warning disable CS0414
#pragma warning disable CS0436
#pragma warning disable CS0626
#pragma warning disable CS0649

using MonoMod;
using Microsoft.Xna.Framework;
using System.Linq;

namespace FFU_Tyrian_Legacy {
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
	/*public class patch_MicroCosm : MicroCosm {
		[MonoModIgnore] public patch_MicroCosm(Actor world) : base(world) { }
		private extern void orig_populateMods();
		private void populateMods() {
        /// Forcefully update module rotation settings, if missing.
			orig_populateMods();
			foreach (var rModule in modules.Where(x => x.type == ModuleType.screen_access && x.rotation != x.tiles[0].rotation)) {
				rModule.rotation = rModule.tiles[0].rotation;
			}
		}
	}*/
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
}