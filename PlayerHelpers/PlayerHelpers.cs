using Microsoft.Xna.Framework;
using System;
using Terraria;


namespace HamstarHelpers.PlayerHelpers {
	public static class PlayerHelpers {
		public static void Evac( Player player ) {
			player.grappling[0] = -1;
			player.grapCount = 0;

			for( int i = 0; i < 1000; i++ ) {
				if( Main.projectile[i].active && Main.projectile[i].owner == i && Main.projectile[i].aiStyle == 7 ) {
					Main.projectile[i].Kill();
				}
			}
			bool immune = player.immune;
			int immune_time = player.immuneTime;

			player.Spawn();
			player.immune = immune;
			player.immuneTime = immune_time;
		}


		public static void Teleport( Player player, Vector2 pos, int style = -1 ) {
			player.grappling[0] = -1;
			player.grapCount = 0;

			bool is_immune = player.immune;
			int immune_time = player.immuneTime;
			player.Spawn();
			player.immune = is_immune;
			player.immuneTime = immune_time;

			if( Main.netMode <= 1 ) {
				player.Teleport( pos, style );
			} else {
				NetMessage.SendData( 65, -1, -1, "", 0, (float)player.whoAmI, pos.X, pos.Y, style, 0, 0 );
			}
		}


		public static Vector2 GetSpawnPoint( Player player ) {
			var pos = new Vector2();

			if( player.SpawnX >= 0 && player.SpawnY >= 0 ) {
				pos.X = (float)(player.SpawnX * 16 + 8 - player.width / 2);
				pos.Y = (float)(player.SpawnY * 16 - player.height);
			} else {
				pos.X = (float)(Main.spawnTileX * 16 + 8 - player.width / 2);
				pos.Y = (float)(Main.spawnTileY * 16 - player.height);
			}

			return pos;
		}


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


		public static int ComputeImpendingFallDamage( Player player ) {
			if( player.mount.CanFly ) {
				return 0;
			}
			if( player.mount.Cart && Minecart.OnTrack( player.position, player.width, player.height ) ) {
				return 0;
			}
			if( player.mount.Type == 1 ) {
				return 0;
			}

			int safetyMin = 25 + player.extraFall;
			int damage = (int)(player.position.Y / 16f) - player.fallStart;

			if( player.stoned ) {
				return (int)(((float)damage * player.gravDir - 2f) * 20f);
			}

			if( (player.gravDir == 1f && damage > safetyMin) || (player.gravDir == -1f && damage < -safetyMin) ) {
				if( player.noFallDmg ) {
					return 0;
				}
				for( int n = 3; n < 10; n++ ) {
					if( player.armor[n].stack > 0 && player.armor[n].wingSlot > -1 ) {
						return 0;
					}
				}

				int finalDamage = (int)((float)damage * player.gravDir - (float)safetyMin) * 10;
				if( player.mount.Active ) {
					finalDamage = (int)((float)finalDamage * player.mount.FallDamage);
				}
				return finalDamage;
			}

			return 0;
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
