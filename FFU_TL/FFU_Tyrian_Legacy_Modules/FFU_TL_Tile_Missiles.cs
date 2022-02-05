#pragma warning disable CS0108
#pragma warning disable CS0169
#pragma warning disable CS0414
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
    public class FFU_TL_Tile_Missiles {
    }
}

namespace CoOpSpRpG {
    public class patch_MissileMagazineAnim : MissileMagazineAnim {
        [MonoModIgnore] private Rectangle artSource;
		[MonoModIgnore] public patch_MissileMagazineAnim(byte r) : base(r) { }
		[MonoModReplace] public void setType(MissileType type) {
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
				case MissileType.red_siege:
				case MissileType.armorpiercing:
				artSource.X = 64;
				artSource.Y = 128;
				artSource.Width = 32;
				artSource.Height = 32;
				break;
				case MissileType.seeker_siege:
				case MissileType.hellfire_siege:
				case MissileType.mirv_siege:
				artSource.X = 0;
				artSource.Y = 368;
				artSource.Width = 48;
				artSource.Height = 48;
				break;
				case MissileType.antimatter_siege:
				case MissileType.fission_siege:
				artSource.X = 48;
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
		[MonoModReplace] public void setType(MissileType type) {
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
				case MissileType.seeker_siege:
				case MissileType.hellfire_siege:
				case MissileType.mirv_siege:
				artSource.X = 0;
				artSource.Y = 160;
				artSource.Width = 48;
				artSource.Height = 208;
				break;
				case MissileType.antimatter_siege:
				case MissileType.fission_siege:
				artSource.X = 48;
				artSource.Y = 160;
				artSource.Width = 48;
				artSource.Height = 208;
				break;
			}
		}
	}
}