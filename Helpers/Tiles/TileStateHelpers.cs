using System;
using Terraria;


namespace HamstarHelpers.Helpers.Tiles {
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
	/// Assorted static "helper" functions pertaining to tile state (slope, actuation, etc.).
	/// </summary>
	public class TileStateHelpers {
		/// <summary>
		/// Attempts to "smartly" smooth a given tile against its adjacent tiles.
		/// </summary>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <returns></returns>
		public static bool SmartSlope( int tileX, int tileY ) {
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

			// Up
			if( !upSolid && downSolid && leftSolid && !rightSolid ) {
				mid.slope( (byte)TileSlopeType.TopLeftSlope );
				return true;
			}
			if( !upSolid && downSolid && !leftSolid && rightSolid ) {
				mid.slope( (byte)TileSlopeType.TopRightSlope );
				return true;
			}
			if( !upSolid && downSolid && !leftSolid && !rightSolid ) {
				mid.halfBrick( true );
				return true;
			}

			// Down
			if( upSolid && !downSolid && leftSolid && !rightSolid ) {
				mid.slope( (byte)TileSlopeType.BottomLeftSlope );
				return true;
			}
			if( upSolid && !downSolid && !leftSolid && rightSolid ) {
				mid.slope( (byte)TileSlopeType.BottomRightSlope );
				return true;
			}

			return false;
		}
	}
}
