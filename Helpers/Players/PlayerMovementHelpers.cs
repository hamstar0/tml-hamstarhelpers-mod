using System;
using Terraria;
using Terraria.ID;


namespace HamstarHelpers.Helpers.Players {
	/** <summary>Assorted static "helper" functions pertaining to player movement.</summary> */
	public static class PlayerMovementHelpers {
		public static bool IsRelaxed( Player player, bool notMounted = true, bool notGrappled = true,
				bool notPulleyed = true, bool notFrozen = true, bool notInverted = true ) {
			// Unmoved/moving
			if( player.velocity.X != 0 || player.velocity.Y != 0 ) { return false; }

			// No mounts (includes minecart)
			if( notMounted && player.mount.Active ) { return false; }

			// Not grappled
			if( notGrappled && player.grappling[0] >= 0 ) { return false; }

			// Not on a pulley
			if( notPulleyed && player.pulley ) { return false; }

			// Not frozen
			if( notFrozen && player.frozen ) { return false; }

			// Not inverted (gravity)
			if( notInverted && player.gravDir < 0f ) { return false; }

			return true;
		}


		public static bool IsFlying( Player player ) {
			bool wingFly = !player.pulley && player.grappling[0] == -1 && !player.tongued &&
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

			bool rocketFly = (player.wingTime == 0f || player.wingsLogic == 0) &&
				player.rocketBoots > 0 &&
				player.controlJump &&
				player.canRocket &&
				player.rocketRelease &&
				!player.jumpAgainCloud &&
				player.rocketTime > 0;

			return wingFly || rocketFly;
		}


		public static bool IsOnFloor( Player player ) {
			return player.velocity.Y == 0f || player.sliding;
		}


		public static bool IsJumpPrimed( Player player ) {
			return player.jumpAgainCloud ||
				player.jumpAgainSandstorm ||
				player.jumpAgainBlizzard ||
				player.jumpAgainFart ||
				player.jumpAgainSail ||
				player.jumpAgainUnicorn ||
				PlayerMovementHelpers.IsOnFloor( player ) ||
				(player.mount.Active && player.mount.Type == MountID.Slime && player.wetSlime > 0) ||
				(player.wet && player.accFlipper && (!player.mount.Active || !player.mount.Cart));
		}


		public static bool CanPlayerJump( Player player ) {
			return PlayerMovementHelpers.IsJumpPrimed( player ) &&
				( player.releaseJump || (player.autoJump && PlayerMovementHelpers.IsOnFloor(player)) );
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
