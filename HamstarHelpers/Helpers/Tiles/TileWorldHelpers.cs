using System;
using Microsoft.Xna.Framework;
using Terraria;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Internals.NetProtocols;


namespace HamstarHelpers.Helpers.Tiles {
	/// <summary>
	/// Assorted static "helper" functions pertaining to tiles relative to the world.
	/// </summary>
	public static class TileWorldHelpers {
		/// <summary>
		/// Indicates if a given tiles is within the visible player-accessible world's boundaries.
		/// </summary>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <returns></returns>
		public static bool IsWithinMap( int tileX, int tileY ) {
			return (tileX > 41 && tileX < Main.maxTilesX - 42) && (tileY > 41 && tileY < Main.maxTilesY - 42);
		}


		/// <summary>
		/// Gauges the overall light amount within an area.
		/// </summary>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <returns></returns>
		public static float GaugeBrightnessWithin( int tileX, int tileY, int width, int height ) {
			int i = 0, j = 0;
			float avg = 0f;

			for( i = 0; i < width; i++ ) {
				for( j = 0; j < height; j++ ) {
					avg += Lighting.Brightness( tileX + i, tileY + j );
				}
			}

			return avg / (i * j);
		}


		////////////////

		/// <summary>
		/// Requests a given tile chunk (200x150 tile 'section') from the server.
		/// </summary>
		/// <param name="tileSectionX"></param>
		/// <param name="tileSectionY"></param>
		public static void RequestChunkFromServer( int tileSectionX, int tileSectionY ) {
			//NetMessage.SendData( MessageID.RequestTileData, -1, -1, null, x, (float)y, 0f, 0f, 0, 0, 0 );
			//NetMessage.SendSection( toWho, x/200, y/150 );
			TileSectionRequestProtocol.SendToServer( tileSectionX, tileSectionY );
		}


		/// <summary>
		/// Requests all tile chunks (200x150 tile 'sections') from the server within a given tile range.
		/// </summary>
		/// <param name="tileRange"></param>
		/// <param name="leftPadding">In chunks.</param>
		/// <param name="topPadding">In chunks.</param>
		/// <param name="rightPadding">In chunks.</param>
		/// <param name="bottomPadding">In chunks.</param>
		public static void RequestChunksFromServer(
					Rectangle tileRange,
					int leftPadding=0,
					int topPadding=0,
					int rightPadding=0,
					int bottomPadding=0 ) {
			int sectX = 200;
			int sectY = 150;

			int minX = Math.Max( (tileRange.X / sectX) - leftPadding, 0 );
			minX *= sectX;
			int minY = Math.Max( (tileRange.Y / sectY) - topPadding, 0 );
			minY *= sectY;

			int maxX = tileRange.X + tileRange.Width;
			maxX += rightPadding * sectX;
			maxX = Math.Min( maxX, Main.maxTilesX - 1 );
			int maxY = tileRange.Y + tileRange.Height;
			maxY += bottomPadding * sectY;
			maxY = Math.Min( maxY, Main.maxTilesY - 1 );
			
			for( int x = minX; x < maxX; x += sectX ) {
				for( int y = minY; y < maxY; y += sectY ) {
					TileWorldHelpers.RequestChunkFromServer( Netplay.GetSectionX(x), Netplay.GetSectionY(y) );
				}
			}
		}
	}
}
