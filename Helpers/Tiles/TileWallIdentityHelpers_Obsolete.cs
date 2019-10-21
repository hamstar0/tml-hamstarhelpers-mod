using ReLogic.Reflection;
using System;
using Terraria.ID;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.Tiles {
	/// <summary>
	/// Assorted static "helper" functions pertaining to wall identification.
	/// </summary>
	[Obsolete( "use WallID", true )]
	public class TileWallIdentityHelpers {
		private static readonly IdDictionary WallIdSearch = IdDictionary.Create<WallID, short>();



		////////////////

		/// <summary>
		/// Gets a (human readable) unique key from a given wall type.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		[Obsolete( "use WallID.GetUniqueKey(int)" )]
		public static string GetUniqueKey( int type ) {
			if( type < 0 || type >= WallLoader.WallCount ) {
				throw new ArgumentOutOfRangeException( "Invalid type: " + type );
			}
			if( type < WallID.Count ) {
				return "Terraria " + TileWallIdentityHelpers.WallIdSearch.GetName( type );
			}

			ModWall modWall = WallLoader.GetWall( type );
			return $"{modWall.mod.Name} {modWall.Name}";
		}

		////

		/// <summary>
		/// Gets a wall type from a given unique key.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		[Obsolete( "use WallID.TypeFromUniqueKey(string)" )]
		public static int TypeFromUniqueKey( string key ) {
			string[] parts = key.Split( new char[] { ' ' }, 2 );

			if( parts.Length != 2 ) {
				return 0;
			}
			if( parts[0] == "Terraria" ) {
				if( !TileWallIdentityHelpers.WallIdSearch.ContainsName( parts[1] ) ) {
					return 0;
				}
				return TileWallIdentityHelpers.WallIdSearch.GetId( parts[1] );
			}

			return ModLoader.GetMod( parts[0] )?.WallType( parts[1] ) ?? 0;
		}
	}
}
