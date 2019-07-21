using ReLogic.Reflection;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace DestructibleTiles.Helpers.TileHelpers {
	/// <summary>
	/// Assorted static "helper" functions pertaining to tile identification.
	/// </summary>
	public partial class TileIdentityHelpers {
		private static readonly IdDictionary Search = IdDictionary.Create<TileID, short>();



		////////////////
		d
		public static string GetReadableUniqueKey( int tileType ) {
			if( TileIdentityHelpers.Search.ContainsId( tileType ) ) {
				return "Terraria " + TileIdentityHelpers.Search.GetName( tileType );
			}

			ModTile modTile = TileLoader.GetTile( tileType );
			if( modTile != null ) {
				return TileIdentityHelpers.GetReadableUniqueKey( modTile );
			}

			throw new ArgumentOutOfRangeException( "Invalid tile type value." );
		}

		public static string GetReadableUniqueKey( ModTile tile ) {
			return tile.mod.Name + " " + tile.Name;
		}
	}
}
