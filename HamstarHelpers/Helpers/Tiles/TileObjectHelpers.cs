using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ObjectData;


namespace HamstarHelpers.Helpers.Tiles {
	/// <summary>
	/// Assorted static "helper" functions pertaining to tile objects.
	/// </summary>
	public class TileObjectHelpers {
		/// <summary>
		/// Predicts the top left position of the given tile object of a given tile, assuming a contiguous object.
		/// </summary>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <returns></returns>
		public static Point PredictTopLeftOfObject( int tileX, int tileY ) {
			Tile tile = Framing.GetTileSafely( tileX, tileY );
			TileObjectData tileData = TileObjectData.GetTileData( tile.type, 0, 0 );
			if( tileData == null ) {
				return new Point( tileX, tileY );
			}

			int frameX = tile.frameX;
			int frameY = tile.frameY;
			int frameCol = frameX / tileData.CoordinateFullWidth;
			int frameRow = frameY / tileData.CoordinateFullHeight;
			int wrap = tileData.StyleWrapLimit;
			if( wrap == 0 ) {
				wrap = 1;
			}
			int subTile = tileData.StyleHorizontal
				? frameRow * wrap + frameCol
				: frameCol * wrap + frameRow;
			int style = subTile / tileData.StyleMultiplier;
			int alternate = subTile % tileData.StyleMultiplier;
			//for( int k = 0; k < tileData.AlternatesCount; k++ ) {
			//	if( alternate >= tileData.Alternates[k].Style && alternate <= tileData.Alternates[k].Style + tileData.RandomStyleRange ) {
			//		alternate = k;
			//		break;
			//	}
			//}

			tileData = TileObjectData.GetTileData( tile.type, style, alternate + 1 );
			int subFrameX = frameX % tileData.CoordinateFullWidth;
			int subFrameY = frameY % tileData.CoordinateFullHeight;
			int partX = subFrameX / ( tileData.CoordinateWidth + tileData.CoordinatePadding );
			int partY = 0;

			int remainingFrameY = subFrameY;
			while( remainingFrameY > 0 ) {
				remainingFrameY -= tileData.CoordinateHeights[partY] + tileData.CoordinatePadding;
				partY++;
			}

			tileX -= partX;
			tileY -= partY;

			int originX = tileX + tileData.Origin.X;
			int originY = tileY + tileData.Origin.Y;
			return new Point( originX, originY );
		}
	}
}
