using HamstarHelpers.Helpers.Debug;
using System;
using Terraria;


namespace HamstarHelpers.Helpers.Tiles {
	/// <summary>
	/// Assorted static "helper" functions pertaining to tiles.
	/// </summary>
	public partial class TileHelpers {
		/// @private
		[Obsolete("use PlaceTileSynced", true)]
		public static bool PlaceTile( int tileX, int tileY, int tileType, int placeStyle = 0, bool muted = false, bool forced = false, int plrWho = -1 ) {
			return TileHelpers.PlaceTileSynced( tileX, tileY, tileType, placeStyle, muted, forced, plrWho );
		}

		/// @private
		[Obsolete( "use KillTileSynced", true )]
		public static void KillTile( int tileX, int tileY, bool effectOnly, bool dropsItem ) {
			TileHelpers.KillTileSynced( tileX, tileY, effectOnly, dropsItem );
		}

		/// @private
		[Obsolete( "use Swap1x1Synced", true )]
		public static void Swap1x1( int fromTileX, int fromTileY, int toTileX, int toTileY, bool preserveWall = false, bool preserveWire = false, bool preserveLiquid = false ) {
			TileHelpers.Swap1x1Synced( fromTileX, fromTileY, toTileX, toTileY, preserveWall, preserveWire, preserveLiquid );
		}

		/// @private
		public static bool IsNotVanillaBombable( int tileX, int tileY ) {
			return Attributes.TileAttributeHelpers.IsNotVanillaBombable( tileX, tileY );
		}

		/// @private
		public static bool IsNotVanillaBombableType( int tileType ) {
			return Attributes.TileAttributeHelpers.IsNotVanillaBombableType( tileType );
		}

		/// @private
		public static float GetDamageScale( Tile tile, float baseDamage, out bool isAbsolute ) {
			return Attributes.TileAttributeHelpers.GetDamageScale( tile, baseDamage, out isAbsolute );
		}
	}
}
