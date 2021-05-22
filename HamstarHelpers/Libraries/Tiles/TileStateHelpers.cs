using System;
using Terraria;
using Terraria.ID;


namespace HamstarHelpers.Libraries.Tiles {
	/// <summary></summary>
	public enum TileSlopeType : byte {
		/// <summary></summary>
		None = 0,
		/// <summary></summary>
		TopRightSlope = 1,
		/// <summary></summary>
		TopLeftSlope = 2,
		/// <summary></summary>
		BottomRightSlope = 3,
		/// <summary></summary>
		BottomLeftSlope = 4,
	}




	/// <summary></summary>
	public enum TileShapeType {
		/// <summary></summary>
		None = 0,
		/// <summary></summary>
		Any = -1,
		/// <summary></summary>
		TopRightSlope = 1,
		/// <summary></summary>
		TopLeftSlope = 2,
		/// <summary></summary>
		BottomRightSlope = 4,
		/// <summary></summary>
		BottomLeftSlope = 8,
		/// <summary></summary>
		TopSlope = TopRightSlope + TopLeftSlope,
		/// <summary></summary>
		BottomSlope = BottomRightSlope + BottomLeftSlope,
		/// <summary></summary>
		LeftSlope = TopLeftSlope + BottomLeftSlope,
		/// <summary></summary>
		RightSlope = TopRightSlope + BottomRightSlope,
		/// <summary></summary>
		HalfBrick = 16
	}



	/// <summary>
	/// Credit to https://tshock.readme.io/docs/multiplayer-packet-structure
	/// </summary>
	public enum TileChangeNetMessageType : int {
		/// <summary></summary>
		KillTile = 0,
		/// <summary></summary>
		PlaceTile = 1,
		/// <summary></summary>
		KillWall = 2,
		/// <summary></summary>
		PlaceWall = 3,
		/// <summary></summary>
		KillTileNoItem = 4,
		/// <summary></summary>
		PlaceWire = 5,
		/// <summary></summary>
		KillWire = 6,
		/// <summary></summary>
		PoundTile = 7,
		/// <summary></summary>
		PlaceActuator = 8,
		/// <summary></summary>
		KillActuator = 9,
		/// <summary></summary>
		PlaceWire2 = 10,
		/// <summary></summary>
		KillWire2 = 11,
		/// <summary></summary>
		PlaceWire3 = 12,
		/// <summary></summary>
		KillWire3 = 13,
		/// <summary></summary>
		SlopeTile = 14,
		/// <summary></summary>
		FrameTrack = 15,
		/// <summary></summary>
		PlaceWire4 = 16,
		/// <summary></summary>
		KillWire4 = 17,
		/// <summary></summary>
		PokeLogicGate = 18,
		/// <summary></summary>
		Actuate = 19
	}




	/// <summary>
	/// Assorted static "helper" functions pertaining to tile state (slope, actuation, etc.).
	/// </summary>
	public class TileStateLibraries {
		/// <summary></summary>
		/// <param name="tile"></param>
		public static void FlipSlopeHorizontally( Tile tile ) {
			switch( tile.slope() ) {
			case 1:
				tile.slope( 2 );
				break;
			case 2:
				tile.slope( 1 );
				break;
			case 3:
				tile.slope( 4 );
				break;
			case 4:
				tile.slope( 3 );
				break;
			}
		}

		/// <summary></summary>
		/// <param name="tile"></param>
		public static void FlipSlopeVertically( Tile tile ) {
			switch( tile.slope() ) {
			case 1:
				tile.slope( 3 );
				break;
			case 2:
				tile.slope( 4 );
				break;
			case 3:
				tile.slope( 1 );
				break;
			case 4:
				tile.slope( 2 );
				break;
			}
		}



		/// <summary>
		/// Attempts to "smartly" smooth a given tile against its adjacent tiles. TODO: Verify correctness.
		/// </summary>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <param name="isSynced"></param>
		/// <returns></returns>
		public static bool SmartSlope( int tileX, int tileY, bool isSynced ) {
			Tile mid = Framing.GetTileSafely( tileX, tileY );
			Tile up = Framing.GetTileSafely( tileX, tileY - 1 );
			Tile down = Framing.GetTileSafely( tileX, tileY + 1 );
			Tile left = Framing.GetTileSafely( tileX - 1, tileY );
			Tile right = Framing.GetTileSafely( tileX + 1, tileY );

			bool upSolid = up.active() && up.slope() == 0 && Main.tileSolid[up.type] && !Main.tileSolidTop[up.type];
			bool downSolid = down.active() && down.slope() == 0 && Main.tileSolid[down.type] && !Main.tileSolidTop[down.type];
			bool leftSolid = left.active() && left.slope() == 0 && Main.tileSolid[left.type] && !Main.tileSolidTop[left.type];
			bool rightSolid = right.active() && right.slope() == 0 && Main.tileSolid[right.type] && !Main.tileSolidTop[right.type];

			upSolid = upSolid
				&& (up.slope() == (byte)TileSlopeType.TopLeftSlope || up.slope() == (byte)TileSlopeType.TopRightSlope);
			downSolid = downSolid
				&& (down.slope() == (byte)TileSlopeType.BottomLeftSlope || down.slope() == (byte)TileSlopeType.BottomRightSlope );
			leftSolid = leftSolid
				&& (left.slope() == (byte)TileSlopeType.TopRightSlope || left.slope() == (byte)TileSlopeType.BottomRightSlope );
			rightSolid = rightSolid
				&& ( right.slope() == (byte)TileSlopeType.TopLeftSlope || right.slope() == (byte)TileSlopeType.BottomLeftSlope );
			int changed = 0;

			// Up
			if( !upSolid && downSolid && leftSolid && !rightSolid ) {
				mid.slope( (byte)TileSlopeType.TopLeftSlope );
				changed = 1;
			}
			if( !upSolid && downSolid && !leftSolid && rightSolid ) {
				mid.slope( (byte)TileSlopeType.TopRightSlope );
				changed = 1;
			}
			if( !upSolid && downSolid && !leftSolid && !rightSolid ) {
				mid.halfBrick( true );
				changed = 2;
			}

			// Down
			if( upSolid && !downSolid && leftSolid && !rightSolid ) {
				mid.slope( (byte)TileSlopeType.BottomLeftSlope );
				changed = 1;
			}
			if( upSolid && !downSolid && !leftSolid && rightSolid ) {
				mid.slope( (byte)TileSlopeType.BottomRightSlope );
				changed = 1;
			}

			if( isSynced && Main.netMode == NetmodeID.MultiplayerClient ) {
				if( changed != 0 ) {
					NetMessage.SendData( MessageID.TileChange, -1, -1, null, (int)TileChangeNetMessageType.SlopeTile, (float)tileX, (float)tileY, (float)mid.slope(), 0, 0, 0 );
				}
			}

			return false;
		}
	}
}
