using HamstarHelpers.Services.DataStore;
using Microsoft.Xna.Framework;
using Terraria;
using System.Collections.Generic;
using Terraria.ID;


namespace HamstarHelpers.Helpers.Players {
	/// <summary>
	/// Assorted static "helper" functions pertaining to player warping/teleporting/spawn return.
	/// </summary>
	public partial class PlayerWarpHelpers {
		private static object SpawnPointKey = new object();

		/// <summary></summary>
		public const int MagicMirrorWarpStyle = 0;
		/// <summary></summary>
		public const int DiscordWarpStyle = 1;
		/// <summary></summary>
		public const int RecallPotionWarpStyle = 2;
		/// <summary></summary>
		public const int WormholePotionWarpStyle = 3;
		/// <summary></summary>
		public const int PortalWarpStyle = 4;



		////////////////

		/// <summary>
		/// "Evacs" a player back to their bed or default spawn.
		/// </summary>
		/// <param name="player"></param>
		public static void Evac( Player player ) {
			player.grappling[0] = -1;
			player.grapCount = 0;

			for( int i = 0; i < 1000; i++ ) {
				if( Main.projectile[i].active && Main.projectile[i].owner == i && Main.projectile[i].aiStyle == 7 ) {
					Main.projectile[i].Kill();
				}
			}
			bool immune = player.immune;
			int immuneTime = player.immuneTime;

			player.Spawn();
			player.immune = immune;
			player.immuneTime = immuneTime;
		}


		/// <summary>
		/// Teleports a player to a given position.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="pos"></param>
		/// <param name="style">"Style" of teleportation (visual, sound, etc.).</param>
		public static void Teleport( Player player, Vector2 pos, int style = -1 ) {
			player.grappling[0] = -1;
			player.grapCount = 0;

			bool isImmune = player.immune;
			int immuneTime = player.immuneTime;

			player.Spawn();
			player.immune = isImmune;
			player.immuneTime = immuneTime;

			if( Main.netMode <= 1 ) {
				player.Teleport( pos, style );
			} else {
				//RemoteClient.CheckSection( player.whoAmI, pos );
			}

			if( Main.netMode >= 1 ) {
				style = style == -1 ? 1 : style;
				NetMessage.SendData( MessageID.Teleport, -1, -1, null, 0, (float)player.whoAmI, pos.X, pos.Y, style, 0, 0 );
			}
		}


		/// <summary>
		/// Gets the position of the player's spawn point.
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public static Vector2 GetSpawnPoint( Player player ) {
			var pos = new Vector2();

			if( player.SpawnX >= 0 && player.SpawnY >= 0 ) {
				pos.X = (float)( ( player.SpawnX * 16 ) + 8 - ( player.width / 2 ) );
				pos.Y = (float)( ( player.SpawnY * 16 ) - player.height );
			} else {
				pos.X = (float)( ( Main.spawnTileX * 16 ) + 8 - ( player.width / 2 ) );
				pos.Y = (float)( ( Main.spawnTileY * 16 ) - player.height );
			}

			return pos;
		}


		/// <summary>
		/// Sets a player's spawn point to a given arbitrary tile position.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		public static void SetSpawnPoint( Player player, int tileX, int tileY ) {
			IDictionary<string, IDictionary<int, int>> spawnMap;
			bool success = DataStore.Get( PlayerWarpHelpers.SpawnPointKey, out spawnMap );

			player.SpawnX = tileX;
			player.SpawnY = tileY;

			if( !success ) {
				spawnMap = new Dictionary<string, IDictionary<int, int>>();

				for( int i = 0; i < 200; i++ ) {
					string key1 = player.spN[i];
					int key2 = player.spI[i];

					if( key1 == null ) {
						break;
					}

					if( !spawnMap.ContainsKey( key1 ) ) {
						spawnMap[ key1 ] = new Dictionary<int, int>();
					}
					spawnMap[key1][key2] = i;
				}

				DataStore.Set( PlayerWarpHelpers.SpawnPointKey, spawnMap );
			}

			if( spawnMap.ContainsKey( Main.worldName ) && spawnMap[ Main.worldName ].ContainsKey( Main.worldID ) ) {
				int idx = spawnMap[Main.worldName][Main.worldID];

				player.spX[idx] = tileX;
				player.spY[idx] = tileY;
			} else {
				player.ChangeSpawn( tileX, tileY );

				DataStore.Remove( PlayerWarpHelpers.SpawnPointKey );	// <- Force rebuild
			}
		}
	}
}
