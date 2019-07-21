using HamstarHelpers.Helpers.Debug;
using System;
using Terraria;


namespace HamstarHelpers.Helpers.Tiles {
	/// <summary>
	/// Assorted static "helper" functions pertaining to tile attributes.
	/// </summary>
	public partial class TileAttributeHelpers {
		/// <summary>
		/// Indicates if a given tile type is an "object" (container, sign, station, etc.).
		/// </summary>
		/// <param name="tileType"></param>
		/// <returns></returns>
		public static bool IsObject( int tileType ) {
			return Main.tileFrameImportant[tileType]
				|| Main.tileContainer[tileType]
				|| Main.tileSign[tileType]
				|| Main.tileAlch[tileType]
				|| Main.tileTable[tileType]; //tileFlame
		}
	}
}
