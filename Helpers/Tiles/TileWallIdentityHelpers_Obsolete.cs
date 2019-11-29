using System;
using Terraria.ID;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.Tiles {
	/// @private
	[Obsolete( "use WallID", true )]
	public class TileWallIdentityHelpers {
		/// @private
		[Obsolete( "use WallID.GetUniqueKey(int)" )]
		public static string GetUniqueKey( int type ) {
			if( type < 0 || type >= WallLoader.WallCount ) {
				throw new ArgumentOutOfRangeException( "Invalid type: " + type );
			}
			if( type < WallID.Count ) {
				return "Terraria " + WallID.Search.GetName( type );
			}

			ModWall modWall = WallLoader.GetWall( type );
			return $"{modWall.mod.Name} {modWall.Name}";
		}

		/// @private
		[Obsolete( "use WallID.TypeFromUniqueKey(string)" )]
		public static int TypeFromUniqueKey( string key ) {
			string[] parts = key.Split( new char[] { ' ' }, 2 );

			if( parts.Length != 2 ) {
				return 0;
			}
			if( parts[0] == "Terraria" ) {
				if( !WallID.Search.ContainsName( parts[1] ) ) {
					return 0;
				}
				return WallID.Search.GetId( parts[1] );
			}

			return ModLoader.GetMod( parts[0] )?.WallType( parts[1] ) ?? 0;
		}
	}
}
