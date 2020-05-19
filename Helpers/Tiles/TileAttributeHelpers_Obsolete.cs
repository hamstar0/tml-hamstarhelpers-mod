using HamstarHelpers.Helpers.Debug;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Enums;


namespace HamstarHelpers.Helpers.Tiles {
	/// @private
	[Obsolete( "use Attributes/TileAttributeHelpers", true )]
	public partial class TileAttributeHelpers {
		/// @private
		[Obsolete( "use Attributes/TileAttributeHelpers", true )]
		public static bool IsObject( int tileType ) {
			return Attributes.TileAttributeHelpers.IsObject( tileType );
		}

		/*/// @private
		[Obsolete( "use Attributes/TileAttributeHelpers", true )]
		public static bool IsBreakable( int tileX, int tileY, TileCuttingContext? context = null ) {
			return Attributes.TileAttributeHelpers.IsBreakable(
				tileX,
				tileY,
				context.HasValue ? context.Value : TileCuttingContext.AttackMelee
			);
		}*/
		
		/// @private
		[Obsolete( "use Attributes/TileAttributeHelpers", true )]
		public static bool IsNotVanillaBombable( int tileX, int tileY ) {
			return Attributes.TileAttributeHelpers.IsNotVanillaBombable( tileX, tileY );
		}

		/// @private
		[Obsolete( "use Attributes/TileAttributeHelpers", true )]
		public static bool IsNotVanillaBombableType( int tileType ) {
			return Attributes.TileAttributeHelpers.IsNotVanillaBombableType( tileType );
		}

		/// @private
		[Obsolete( "use Attributes/TileAttributeHelpers", true )]
		public static float GetDamageScale( Tile tile, float baseDamage, out bool isAbsolute ) {
			return Attributes.TileAttributeHelpers.GetDamageScale( tile, baseDamage, out isAbsolute );
		}

		/// @private
		[Obsolete( "use Attributes/TileAttributeHelpers", true )]
		public static KeyValuePair<int, string>[] GetVanillaTileDisplayNames( int tileType ) {
			return Attributes.TileAttributeHelpers.GetVanillaTileDisplayNames( tileType );
		}

		/// @private
		[Obsolete( "use Attributes/TileAttributeHelpers", true )]
		public static string GetVanillaTileDisplayName( int tileType, int subType = -1 ) {
			return Attributes.TileAttributeHelpers.GetVanillaTileDisplayName( tileType, subType );
		}
	}
}
