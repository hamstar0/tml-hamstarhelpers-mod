using Microsoft.Xna.Framework;
using Terraria;


namespace HamstarHelpers.PlayerHelpers {
	public static class PlayerNPCHelpers {
		public static bool IsPlayerNearBoss( Player player ) {
			int x = ((int)player.Center.X - (Main.maxScreenW / 2)) / 16;
			int y = ((int)player.Center.Y - (Main.maxScreenH / 2)) / 16;

			Rectangle player_zone = new Rectangle( x, y, (Main.maxScreenH / 16), (Main.maxScreenH / 16) );
			int boss_radius = 5000;

			for( int i = 0; i < Main.npc.Length; i++ ) {
				NPC check_npc = Main.npc[i];
				if( !check_npc.active || !check_npc.boss ) { continue; }

				int npc_left = (int)(check_npc.position.X + (float)check_npc.width / 2f) - boss_radius;
				int npc_top = (int)(check_npc.position.Y + (float)check_npc.height / 2f) - boss_radius;
				Rectangle npc_zone = new Rectangle( npc_left, npc_top, boss_radius * 2, boss_radius * 2 );

				if( player_zone.Intersects( npc_zone ) ) { return true; }
			}

			return false;
		}


		public static bool HasUsedNurse( Player player ) {
			return Main.npcChatText == Lang.dialog( 227, false ) ||
					Main.npcChatText == Lang.dialog( 228, false ) ||
					Main.npcChatText == Lang.dialog( 229, false ) ||
					Main.npcChatText == Lang.dialog( 230, false );
		}
	}
}
