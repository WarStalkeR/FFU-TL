using System;

namespace FFU_Tyrian_Legacy {
	public static class Mathx {
		public static float RoundToFloat(float value, int dp = 0) {
			return (float)Math.Round(value, dp);
		}
	}
}
