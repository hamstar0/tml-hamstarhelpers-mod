using Microsoft.Xna.Framework;
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
				NetMessage.SendData( 65, -1, -1, null, 0, (float)player.whoAmI, pos.X, pos.Y, style, 0, 0 );
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

			int safety_min = 25 + player.extraFall;
			int damage = (int)(player.position.Y / 16f) - player.fallStart;

			if( player.stoned ) {
				return (int)(((float)damage * player.gravDir - 2f) * 20f);
			}

			if( (player.gravDir == 1f && damage > safety_min) || (player.gravDir == -1f && damage < -safety_min) ) {
				if( player.noFallDmg ) {
					return 0;
				}
				for( int n = 3; n < 10; n++ ) {
					if( player.armor[n].stack > 0 && player.armor[n].wingSlot > -1 ) {
						return 0;
					}
				}

				int final_damage = (int)((float)damage * player.gravDir - (float)safety_min) * 10;
				if( player.mount.Active ) {
					final_damage = (int)((float)final_damage * player.mount.FallDamage);
				}
				return final_damage;
			}

			return 0;
		}


		[System.Obsolete( "use PlayerNPCHelpers.HasUsedNurse", true )]
		public static bool HasUsedNurse( Player player ) {
			return PlayerNPCHelpers.HasUsedNurse( player );
		}


		[System.Obsolete( "use PlayerMovementHelpers.IsRelaxed", true )]
		public static bool IsRelaxed( Player player, bool not_mounted = true, bool not_grappled = true,
				bool not_pulleyed = true, bool not_frozen = true, bool not_inverted = true ) {
			return PlayerMovementHelpers.IsRelaxed( player, not_mounted, not_grappled, not_pulleyed, not_frozen, not_inverted );
		}


		[System.Obsolete( "use PlayerMovementHelpers.IsFlying", true )]
		public static bool IsFlying( Player player ) {
			return PlayerMovementHelpers.IsFlying( player );
		}


		[System.Obsolete( "use PlayerMovementHelpers.MinimumRunSpeed", true )]
		public static float MinimumRunSpeed( Player player ) {
			return PlayerMovementHelpers.MinimumRunSpeed( player );
		}


		[System.Obsolete( "use PlayerMovementHelpers.CanPlayerJump", true )]
		public static bool CanPlayerJump( Player player ) {
			return PlayerMovementHelpers.CanPlayerJump( player );
		}
	}
}
