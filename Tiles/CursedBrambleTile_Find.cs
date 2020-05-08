using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Tiles {
	/// <summary>
	/// Represents a tile that works like a standard corruption/crimson/jungle bramble, but cannot be removed by melee weapons,
	/// and may support additional custom behavior.
	/// </summary>
	public partial class CursedBrambleTile : ModTile {
		/// <summary>
		/// Attempts to locate brambles within a given area.
		/// </summary>
		/// <param name="leftTile"></param>
		/// <param name="topTile"></param>
		/// <param name="rightTile"></param>
		/// <param name="bottomTile"></param>
		/// <returns></returns>
		public static IList<(int tileX, int tileY)> FindBrambles( int leftTile, int topTile, int rightTile, int bottomTile ) {
			var tiles = new List<(int tileX, int tileY)>();

			for( int i=leftTile; i<rightTile; i++ ) {
				for( int j=topTile; j<bottomTile; j++ ) {
					Tile tile = Framing.GetTileSafely( i, j );
					if( !tile.active() || tile.type != ModContent.TileType<CursedBrambleTile>() ) {
						continue;
					}

					tiles.Add( (i, j) );
				}
			}

			return tiles;
		}
	}
}
