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
		private static readonly IdDictionary TileIdSearch = IdDictionary.Create<TileID, short>();



		////////////////

		/// <summary>
		/// Gets a (human readable) unique key from a given tile type.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static string GetUniqueKey( int type ) {
			if( type < 0 || type >= TileLoader.TileCount ) {
				throw new ArgumentOutOfRangeException( "Invalid type: " + type );
			}
			if( type < TileID.Count ) {
				return "Terraria " + TileIdentityHelpers.TileIdSearch.GetName( type );
			}

			ModTile modTile = TileLoader.GetTile( type );
			return $"{modTile.mod.Name} {modTile.Name}";
		}


		////

		/// <summary>
		/// Gets a tile type from a given unique key.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static int TypeFromUniqueKey( string key ) {
			string[] parts = key.Split( new char[] { ' ' }, 2 );

			if( parts.Length != 2 ) {
				return 0;
			}
			if( parts[0] == "Terraria" ) {
				if( !TileIdentityHelpers.TileIdSearch.ContainsName( parts[1] ) ) {
					return 0;
				}
				return TileIdentityHelpers.TileIdSearch.GetId( parts[1] );
			}

			// Should probably return -1 if not found since 0 is Dirt, but TileType also does 0.
			return ModLoader.GetMod( parts[0] )?.TileType( parts[1] ) ?? 0;
		}
	}
}
