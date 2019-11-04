using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Tiles;
using HamstarHelpers.Helpers.User;
using Microsoft.Xna.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Default;


namespace HamstarHelpers.Commands {
	/// @private
	public class CleanupModTilesCommand : ModCommand {
		private static object MyLock = new object();



		/// <summary>
		/// </summary>
		public static void CleanupAsync( Action<int> onCompletion ) {
			Task.Run( () => {
				int cleaned = 0;

				var cts = new CancellationTokenSource();
				var options = new ParallelOptions {
					CancellationToken = cts.Token
				};

				try {
					//for( int i = 0; i < Main.maxTilesX; i++ ) {
					Parallel.For( 0, Main.maxTilesX, options, ( i ) => {
						for( int j = 0; j < Main.maxTilesY; j++ ) {
							if( options.CancellationToken.IsCancellationRequested ) {
								return;
							}

							if( CleanupModTilesCommand.CleanupTile( i, j, cts ) ) {
								lock( CleanupModTilesCommand.MyLock ) {
									cleaned++;

									// To prevent hammering the server
									if( Main.netMode != 0 ) {
										if( cleaned % 100 == 0 ) {
											Thread.Sleep( 500 );
										}
									}
								}
							}
						}
					} );
				} finally {
					cts.Dispose();
				}

				onCompletion( cleaned );
			} );
		}


		private static bool CleanupTile( int i, int j, CancellationTokenSource cts ) {
			Tile tile = Main.tile[i, j];
			if( tile == null ) {
				lock( CleanupModTilesCommand.MyLock ) {
					tile = Framing.GetTileSafely( i, j );
				}
			}

			// No air or vanilla tiles
			if( !tile.active() || TileID.Search.ContainsId( tile.type ) ) {
				return false;
			}

			ModTile modTile = ModContent.GetModTile( tile.type );
			if( modTile == null ) {
				return false;
			}

			if( modTile.mod != null && !(modTile is MysteryTile) ) {
				return false;
			}

			try {
				lock( CleanupModTilesCommand.MyLock ) {
					TileHelpers.KillTile( i, j, false, false );
				}
			} catch {
				cts.Cancel();
				return false;
			}

			return true;
		}



		/// @private
		public override CommandType Type {
			get {
				if( Main.netMode == 0 && !Main.dedServ ) {
					return CommandType.World;
				}
				return CommandType.Console | CommandType.World;
			}
		}
		/// @private
		public override string Command => "mh-cleanup-pink-tiles";
		/// @private
		public override string Usage => "/" + this.Command;
		/// @private
		public override string Description => "Cleans up unloaded mod (pink colored) tiles across the world.";


		////////////////

		/// @private
		public override void Action( CommandCaller caller, string input, string[] args ) {
			if( Main.netMode == 1 ) {
				LogHelpers.Log( "CleanupModTilesCommand - Not supposed to run on client." );
				return;
			}

			if( Main.netMode == 2 && caller.CommandType != CommandType.Console ) {
				if( !UserHelpers.HasBasicServerPrivilege( caller.Player ) ) {
					caller.Reply( "Access denied.", Color.Red );
					return;
				}
			}

			CleanupModTilesCommand.CleanupAsync( ( total ) => {
				caller.Reply( "World cleaned. "+total+" unclaimed mod tiles removed.", Color.Lime );
			} );
		}
	}
}
