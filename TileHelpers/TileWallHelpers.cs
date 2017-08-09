using Terraria;
using Terraria.ID;


namespace HamstarHelpers.TileHelpers {
	public static class TileWallHelpers {
		public static bool IsDungeon( Tile tile ) {
			if( tile == null ) { return false; }

			// Lihzahrd Brick Wall
			//if( tile.wall == 87 ) {
			if( tile.wall == (ushort)WallID.LihzahrdBrickUnsafe /*|| tile.wall == (ushort)WallID.LihzahrdBrick*/ ) {
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
