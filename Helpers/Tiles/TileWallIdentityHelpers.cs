using System;


namespace HamstarHelpers.Helpers.Tiles {
	public class TileWallIdentityHelpers {
		public static readonly IdDictionary Search = IdDictionary.Create<WallID, short>();

		public static string GetReadableUniqueKey( int wallType ) {
			if( WallID.Search.ContainsId( wallType ) ) {
				return "Terraria " + WallID.Search.GetName( wallType );
			}

			ModWall modWall = WallLoader.GetWall( wallType );
			if( modWall != null ) {
				return WallID.GetReadableUniqueKey( modWall );
			}

			throw new ArgumentOutOfRangeException( "Invalid wall type value." );
		}

		public static string GetReadableUniqueKey( ModWall wall ) {
			return wall.mod.Name + " " + wall.Name;
		}
	}
}
