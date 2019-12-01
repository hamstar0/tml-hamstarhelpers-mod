using HamstarHelpers.Helpers.Debug;
using Terraria;


namespace HamstarHelpers.Helpers.Tiles {
	/// <summary>
	/// Assorted static "helper" functions pertaining to tiles.
	/// </summary>
	public partial class TileHelpers {
		/// @private
		public static bool IsNotVanillaBombable( int tileX, int tileY ) {
			return TileAttributeHelpers.IsNotVanillaBombable( tileX, tileY );
		}

		/// @private
		public static bool IsNotVanillaBombableType( int tileType ) {
			return TileAttributeHelpers.IsNotVanillaBombableType( tileType );
		}

		/// @private
		public static float GetDamageScale( Tile tile, float baseDamage, out bool isAbsolute ) {
			return TileAttributeHelpers.GetDamageScale( tile, baseDamage, out isAbsolute );
		}
	}
}
