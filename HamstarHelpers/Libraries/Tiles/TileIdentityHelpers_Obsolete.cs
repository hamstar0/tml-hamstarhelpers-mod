using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.TileHelpers {
	/// @private
	[Obsolete( "use TileID" )]
	public partial class TileIdentityHelpers {
		/// @private
		[Obsolete( "use TileID.GetUniqueKey(int)" )]
		public static string GetUniqueKey( int type ) {
			if( type < 0 || type >= TileLoader.TileCount ) {
				throw new ArgumentOutOfRangeException( "Invalid type: " + type );
			}
			if( type < TileID.Count ) {
				return "Terraria " + TileID.Search.GetName( type );
			}

			ModTile modTile = TileLoader.GetTile( type );
			return $"{modTile.mod.Name} {modTile.Name}";
		}

		/// @private
		[Obsolete( "use TileID.TypeFromUniqueKey(string)" )]
		public static int TypeFromUniqueKey( string key ) {
			string[] parts = key.Split( new char[] { ' ' }, 2 );

			if( parts.Length != 2 ) {
				return 0;
			}
			if( parts[0] == "Terraria" ) {
				if( !TileID.Search.ContainsName( parts[1] ) ) {
					return 0;
				}
				return TileID.Search.GetId( parts[1] );
			}

			// Should probably return -1 if not found since 0 is Dirt, but TileType also does 0.
			return ModLoader.GetMod( parts[0] )?.TileType( parts[1] ) ?? 0;
		}
	}
}
