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
	[MonoModIgnore] public class patch_MicroCosm : MicroCosm {
		[MonoModIgnore] public patch_MicroCosm(Actor world) : base(world) { }
		private extern void orig_populateMods();
		private void populateMods() {
        /// Forcefully update module rotation settings, if missing.
			orig_populateMods();
			foreach (var rModule in modules.Where(x => x.type == ModuleType.screen_access && x.rotation != x.tiles[0].rotation)) {
				rModule.rotation = rModule.tiles[0].rotation;
			}
		}
	}
	public class patch_ShipDesignRev4 : ShipDesignRev4 {
		[MonoModIgnore] internal Color[] design;
		[MonoModIgnore] internal ModTile[] tiles;
		[MonoModIgnore] internal int designWidth;
		[MonoModIgnore] internal int designHeight;
		[MonoModIgnore] private bool costCalcRequested;
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