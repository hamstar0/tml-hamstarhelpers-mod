using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TModLoader;
using HamstarHelpers.Services.Timers;


namespace HamstarHelpers.Tiles {
	/// <summary>
	/// Represents a tile that works like a standard corruption/crimson/jungle bramble, but cannot be removed by melee weapons,
	/// and may support additional custom behavior.
	/// </summary>
	public partial class CursedBrambleTile : ModTile {
		/// <summary>
		/// Creates a cluster of brambles randomly at a given set of points.
		/// </summary>
		/// <param name="tilePositions">Center positions of bramble patches. Array will be shuffled.</param>
		/// <param name="thickness"></param>
		/// <param name="density"></param>
		public static void CreateBramblePatchesAt( ref (int TileX, int TileY)[] tilePositions, int thickness, float density ) {
			UnifiedRandom rand = TmlHelpers.SafelyGetRand();

			// Shuffle positions
			for( int i = tilePositions.Length - 1; i > 0; i-- ) {
				int randPos = rand.Next( i );
				(int, int) tmp = tilePositions[i];

				tilePositions[i] = tilePositions[randPos];
				tilePositions[randPos] = tmp;
			}

			CursedBrambleTile.CreateBramblePatchesAtIntervals( tilePositions, tilePositions.Length - 1, thickness, density );
		}

		////

		private static void CreateBramblePatchesAtIntervals(
					(int TileX, int TileY)[] randTilePositions,
					int lastIdx,
					int thickness,
					float density ) {
			(int tileX, int tileY) tilePos = randTilePositions[ lastIdx ];

			int bramblesPlaced = CursedBrambleTile.CreateBramblePatchAt( tilePos.tileX, tilePos.tileY, thickness, density );

			if( ModHelpersConfig.Instance.DebugModeMiscInfo ) {
				LogHelpers.Log(
					"Created " + bramblesPlaced
					+ " brambles in patch " + lastIdx
					+ " of " + randTilePositions.Length
					+ " (thickness: "+thickness+", density: "+density+")" );
			}

			if( lastIdx > 0 ) {
				lastIdx--;

				string timerName = "AmbushesEntrapAsync_"+randTilePositions[lastIdx].TileX+"_"+randTilePositions[lastIdx].TileY;
				Timers.SetTimer( timerName, 2, false, () => {
					CursedBrambleTile.CreateBramblePatchesAtIntervals( randTilePositions, lastIdx, thickness, density );
					return false;
				} );
			}
		}


		////

		/// <summary>
		/// Creates a patch of cursed brambles at the given location with the given settings.
		/// </summary>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <param name="thickness"></param>
		/// <param name="density"></param>
		/// <returns></returns>
		public static int CreateBramblePatchAt( int tileX, int tileY, int thickness, float density ) {
			int brambleTileType = ModContent.TileType<CursedBrambleTile>();
			var rand = TmlHelpers.SafelyGetRand();

			Tile tileAt = Main.tile[tileX, tileY];
			if( tileAt != null && tileAt.active() && tileAt.type == brambleTileType ) {
				return 0;
			}

			int bramblesPlaced = 0;

			int max = thickness / 2;
			int min = -max;
			for( int i = min; i < max; i++ ) {
				for( int j = min; j < max; j++ ) {
					if( (1f - rand.NextFloat()) > density ) {
						continue;
					}

					Tile tile = CursedBrambleTile.CreateBrambleAt( tileX + i, tileY + j );
					if( tile != null ) {
						bramblesPlaced++;
					}
				}
			}

			return bramblesPlaced;
		}


		/// <summary>
		/// Creates a specific bramble tile. Preserves existing wire, wall, and liquid.
		/// </summary>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <returns></returns>
		public static Tile CreateBrambleAt( int tileX, int tileY ) {
			int brambleTileType = ModContent.TileType<CursedBrambleTile>();

			Tile tileAt = Main.tile[tileX, tileY];
			if( tileAt != null && tileAt.active() && tileAt.type == brambleTileType ) {
				return null;
			}

			Tile tile = Framing.GetTileSafely( tileX, tileY );
			if( tile.active() ) {
				return null;
			}

			if( !WorldGen.PlaceTile( tileX, tileY, brambleTileType ) ) {
				return null;
			}

			Tile newTile = Main.tile[ tileX, tileY ];
			newTile.wall = tile.wall;
			newTile.wallFrameNumber( tile.wallFrameNumber() );
			newTile.wallFrameX( tile.wallFrameX() );
			newTile.wallFrameY( tile.wallFrameY() );
			newTile.wallColor( tile.wallColor() );
			newTile.wire( tile.wire() );
			newTile.wire2( tile.wire2() );
			newTile.wire3( tile.wire3() );
			newTile.wire4( tile.wire4() );
			newTile.liquid = tile.liquid;
			newTile.liquidType( tile.liquidType() );

			if( Main.netMode != NetmodeID.SinglePlayer ) {
				NetMessage.SendData( MessageID.TileChange, -1, -1, null, 1, (float)tileX, (float)tileY, (float)brambleTileType, 0, 0, 0 );
			}

			return newTile;
		}
	}
}