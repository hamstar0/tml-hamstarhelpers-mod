using System;
using Terraria;


namespace HamstarHelpers.PlayerHelpers {
	public static class PlayerMovementHelpers {
		public static bool IsRelaxed( Player player, bool not_mounted = true, bool not_grappled = true,
				bool not_pulleyed = true, bool not_frozen = true, bool not_inverted = true ) {
			// Unmoved/moving
			if( player.velocity.X != 0 || player.velocity.Y != 0 ) { return false; }

			// No mounts (includes minecart)
			if( not_mounted && player.mount.Active ) { return false; }

			// Not grappled
			if( not_grappled && player.grappling[0] >= 0 ) { return false; }

			// Not on a pulley
			if( not_pulleyed && player.pulley ) { return false; }

			// Not frozen
			if( not_frozen && player.frozen ) { return false; }

			// Not inverted (gravity)
			if( not_inverted && player.gravDir < 0f ) { return false; }

			return true;
		}


		public static bool IsFlying( Player player ) {
			bool wing_fly = !player.pulley && player.grappling[0] == -1 && !player.tongued &&
				player.controlJump && player.wingTime > 0f && (
				(player.wingsLogic > 0 && !player.jumpAgainCloud && player.jump == 0 && player.velocity.Y != 0f) ||
				(player.controlDown && (
					player.wingsLogic == 22 ||
					player.wingsLogic == 28 ||
					player.wingsLogic == 30 ||
					player.wingsLogic == 32 ||
					player.wingsLogic == 29 ||
					player.wingsLogic == 33 ||
					player.wingsLogic == 35 ||
					player.wingsLogic == 37)
				)
			);

			bool rocket_fly = (player.wingTime == 0f || player.wingsLogic == 0) &&
				player.rocketBoots > 0 &&
				player.controlJump &&
				player.canRocket &&
				player.rocketRelease &&
				!player.jumpAgainCloud &&
				player.rocketTime > 0;

			return wing_fly || rocket_fly;
		}


		public static bool CanPlayerJump( Player player ) {
			return (player.sliding ||
					player.velocity.Y == 0f ||
					(player.mount.Active && player.mount.Type == 3 && player.wetSlime > 0) ||
					player.jumpAgainCloud ||
					player.jumpAgainSandstorm ||
					player.jumpAgainBlizzard ||
					player.jumpAgainFart ||
					player.jumpAgainSail ||
					player.jumpAgainUnicorn ||
					(player.wet && player.accFlipper && (!player.mount.Active || !player.mount.Cart)))
				&& (player.releaseJump ||
					(player.autoJump && (player.velocity.Y == 0f || player.sliding)));
		}


		public static float MinimumRunSpeed( Player player ) {
			float max = (player.accRunSpeed + player.maxRunSpeed) / 2f;
			float wind = 0f;

			if( player.windPushed && (!player.mount.Active || player.velocity.Y != 0f) ) {
				wind = Math.Sign( Main.windSpeed ) * 0.07f;
				if( Math.Abs( Main.windSpeed ) > 0.5f ) {
					wind *= 1.37f;
				}
				if( player.velocity.Y != 0f ) {
					wind *= 1.5f;
				}
				if( player.controlLeft || player.controlRight ) {
					wind *= 0.8f;
				}
				if( Math.Sign( player.direction ) != Math.Sign( wind ) ) {
					max -= Math.Abs( wind ) * 40f;
				}
			}

			return max;
		}
	}
}
