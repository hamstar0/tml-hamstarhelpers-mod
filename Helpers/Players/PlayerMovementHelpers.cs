using System;
using Terraria;
using Terraria.ID;


namespace HamstarHelpers.Helpers.Players {
	/// <summary>
	/// Assorted static "helper" functions pertaining to player movement.
	/// </summary>
	public class PlayerMovementHelpers {
		/// <summary>
		/// Indicates if player is "relaxed".
		/// </summary>
		/// <param name="player"></param>
		/// <param name="notMounted"></param>
		/// <param name="notGrappled"></param>
		/// <param name="notPulleyed"></param>
		/// <param name="notHurting"></param>
		/// <param name="notCCed">Player is not webbed, stoned, or frozen.</param>
		/// <param name="notInverted"></param>
		/// <returns></returns>
		public static bool IsRelaxed( Player player,
					bool notMounted = true,
					bool notGrappled = true,
					bool notPulleyed = true,
					bool notHurting = true,
					bool notCCed = true,
					bool notInverted = true ) {
			// Unmoved/moving
			if( player.velocity.X != 0 || player.velocity.Y != 0 ) { return false; }

			// No mounts (includes minecart)
			if( notMounted && player.mount.Active ) { return false; }

			// Not grappled
			if( notGrappled && player.grappling[0] >= 0 ) { return false; }

			// Not on a pulley
			if( notPulleyed && player.pulley ) { return false; }

			// Not hurting
			if( notHurting && (player.poisoned
				|| player.venom
				|| player.onFire
				|| player.onFire2
				|| player.electrified
				|| player.suffocating
				|| player.burned
				|| player.onFrostBurn
				|| player.HasBuff(BuffID.Rabies)) ) { return false; }

			// Not "CCed"
			if( notCCed && player.CCed ) { return false; }

			// Not inverted (gravity)
			if( notInverted && player.gravDir < 0f ) { return false; }
			
			//if( player.HasBuff(BuffID.Horrified) ) { return false; }

			return true;
		}


		/// <summary>
		/// Indicates if a given player is "floying" (using rocket boots or wings).
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public static bool IsFlying( Player player ) {
			if( player.dead ) {
				return false;
			}

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


		/// <summary>
		/// Indicates if a player is on a floor.
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public static bool IsOnFloor( Player player ) {
			return player.velocity.Y == 0f || player.sliding;
		}


		/// <summary>
		/// Indicates if a player has a jump "primed" for use (including double, triple, etc. jumps).
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public static bool IsJumpPrimed( Player player ) {
			if( player.dead ) {
				return false;
			}
			if( player.CCed ) {
				return false;
			}

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


		/// <summary>
		/// Indicates if a player can jump.
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public static bool CanPlayerJump( Player player ) {
			if( player.dead ) {
				return false;
			}
			if( player.CCed ) {
				return false;
			}

			if( !PlayerMovementHelpers.IsJumpPrimed( player ) ) {
				return false;
			}
			
			if( !player.releaseJump && !(player.autoJump && PlayerMovementHelpers.IsOnFloor(player)) ) {
				return false;
			}

			return true;
		}


		/// <summary>
		/// Minimum speed before player begins "running" (shoes kick up dirt, scuffing sounds begin, etc.).
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
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
