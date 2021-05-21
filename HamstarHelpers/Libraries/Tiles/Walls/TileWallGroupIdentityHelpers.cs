using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Classes.DataStructures;


namespace HamstarHelpers.Helpers.Tiles.Walls {
	/// <summary>
	/// Assorted static "helper" functions pertaining to tile walls.
	/// </summary>
	public class TileWallGroupIdentityHelpers {
		/// <summary></summary>
		public static ISet<int> UnsafeDungeonWallTypes { get; } = new ReadOnlySet<int>( new HashSet<int> {
			WallID.BlueDungeonSlabUnsafe,
			WallID.GreenDungeonSlabUnsafe,
			WallID.PinkDungeonSlabUnsafe,
			WallID.BlueDungeonTileUnsafe,
			WallID.GreenDungeonTileUnsafe,
			WallID.PinkDungeonTileUnsafe,
			WallID.BlueDungeonUnsafe,
			WallID.GreenDungeonUnsafe,
			WallID.PinkDungeonUnsafe
		} );




		////////////////

		/// <summary>
		/// Indicates if a given wall is dungeon or temple "biome" wall.
		/// </summary>
		/// <param name="tile"></param>
		/// <param name="isLihzahrd"></param>
		/// <returns></returns>
		public static bool IsDungeon( Tile tile, out bool isLihzahrd ) {
			if( tile == null ) {
				isLihzahrd = false;
				return false;
			}

			isLihzahrd = tile.wall == (ushort)WallID.LihzahrdBrickUnsafe; /*|| tile.wall == (ushort)WallID.LihzahrdBrick*/

			// Lihzahrd Brick Wall
			if( isLihzahrd ) {
				return true;
			}
			// Dungeon Walls
			//if( (tile.wall >= 7 && tile.wall <= 9) || (tile.wall >= 94 && tile.wall <= 99) ) {
			if( tile.wall == (ushort)WallID.BlueDungeonSlabUnsafe ||
				tile.wall == (ushort)WallID.GreenDungeonSlabUnsafe ||
				tile.wall == (ushort)WallID.PinkDungeonSlabUnsafe ||
				tile.wall == (ushort)WallID.BlueDungeonTileUnsafe ||
				tile.wall == (ushort)WallID.GreenDungeonTileUnsafe ||
				tile.wall == (ushort)WallID.PinkDungeonTileUnsafe ||
				tile.wall == (ushort)WallID.BlueDungeonUnsafe ||
				tile.wall == (ushort)WallID.GreenDungeonUnsafe ||
				tile.wall == (ushort)WallID.PinkDungeonUnsafe ) {
				return true;
			}
			return false;
		}
	}
}
