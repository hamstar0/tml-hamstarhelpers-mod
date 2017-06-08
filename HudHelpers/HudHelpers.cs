using Terraria;


namespace HamstarHelpers.HudHelpers {
	public static class HudHelpers {
		public static void GetTopHeartPosition( Player player, ref int x, ref int y ) {
			x = Main.screenWidth - 66;
			y = 60;

			if( player.statLifeMax2 < 400 && (player.statLifeMax2 / 20) % 10 != 0 ) {
				x -= (10 - ((player.statLifeMax2 / 20) % 10)) * 26;
			}
			if( player.statLifeMax2 / 20 <= 10 ) {
				y -= 32;
			}
		}
	}
}
