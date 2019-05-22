using System;
using Terraria;
using Terraria.ID;


namespace HamstarHelpers.Helpers.TileHelpers {
	public static class TileWallHelpers {
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
