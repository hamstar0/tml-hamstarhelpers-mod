using ReLogic.Reflection;
using System;
using Terraria.ID;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.Tiles {
	/// <summary>
	/// Assorted static "helper" functions pertaining to wall identification.
	/// </summary>
	public class TileWallIdentityHelpers {
		private static readonly IdDictionary Search = IdDictionary.Create<WallID, short>();



		////////////////

		/// <summary>
		/// Gets the readable unique key for a given wall (e.g. for ModConfig fields).
		/// </summary>
		/// <param name="wallType"></param>
		/// <returns></returns>
		public static string GetReadableUniqueKey( int wallType ) {
			if( TileWallIdentityHelpers.Search.ContainsId( wallType ) ) {
				return "Terraria " + TileWallIdentityHelpers.Search.GetName( wallType );
			}

			ModWall modWall = WallLoader.GetWall( wallType );
			if( modWall != null ) {
				return TileWallIdentityHelpers.GetReadableUniqueKey( modWall );
			}

			throw new ArgumentOutOfRangeException( "Invalid wall type value." );
		}

		/// <summary>
		/// Gets the readable unique key for a given wall (e.g. for ModConfig fields).
		/// </summary>
		/// <param name="wall"></param>
		/// <returns></returns>
		public static string GetReadableUniqueKey( ModWall wall ) {
			return wall.mod.Name + " " + wall.Name;
		}
	}
}
