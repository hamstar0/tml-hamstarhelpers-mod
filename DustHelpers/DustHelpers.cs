using Terraria;


namespace HamstarHelpers.DustHelpers {
	public static class DustHelpers {
		public static bool IsActive( int who ) {
			return who != 6000 && Main.dust[who].active && Main.dust[who].type != 0;
		}
	}
}
