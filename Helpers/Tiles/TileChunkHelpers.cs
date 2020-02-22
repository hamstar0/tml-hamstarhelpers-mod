using System;
using Terraria;


namespace HamstarHelpers.Helpers.Tiles {
	/// <summary>
	/// Assorted static "helper" functions pertaining to tile chunks (multiplayer only).
	/// </summary>
	public class TileChunkHelpers {
		/// <summary>
		/// Reports whether a given tile is part of a synced chunk. TODO.
		/// </summary>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <returns></returns>
		public static bool IsTileSyncedForCurrentClient( int tileX, int tileY ) {
			int sectionX = Netplay.GetSectionX( tileX );
			int sectionY = Netplay.GetSectionY( tileY );

			if( !Main.sectionManager.SectionLoaded( sectionX, sectionY ) ) {
				Tile tile = Main.tile[tileX, tileY];
				return tile != null && !TileHelpers.IsEqual( tile, new Tile() );	// TODO
			}

			return true;
		}
	}
}
