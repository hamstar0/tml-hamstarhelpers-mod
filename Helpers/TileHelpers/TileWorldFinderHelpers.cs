using Microsoft.Xna.Framework;
using System;


namespace HamstarHelpers.Helpers.Tiles {
	public static class TileWorldFinderHelpers {
		public static Tuple<int, int> FindWithin( TileType tileType, Rectangle within ) {
			return TileWorldFinderHelpers.FindWithin( tileType, within, 1, 1 );
		}


		public static Tuple<int, int> FindWithin( TileType tileType, Rectangle within, int width, int height ) {
			int maxX = within.X + within.Width - width;
			int maxY = within.Y + within.Height - height;

			for( int i=within.X; i<maxX; i++ ) {
				for( int j=within.Y; j<maxY; j++ ) {
					if( tileType.CheckArea( i, j, width, height) ) {
						return Tuple.Create( i, j );
					}
				}
			}

			return null;
		}


		public static Tuple<int, int> FindWithinFromCenter( TileType tileType, Rectangle within, int width, int height ) {
			int midX = within.X + ( within.Width / 2 ) - ( width / 2 );
			int midY = within.Y + ( within.Height / 2 ) - ( height / 2 );
			int maxX = within.X + within.Width - width;
			int maxY = within.Y + within.Height - height;
			int currMinX = midX;
			int currMaxX = midX;
			int currMinY = midY;
			int currMaxY = midY;

			int i = midX;
			int j = midY;
			int turn = 0;

			while( !( currMinX == within.X && currMaxX == maxX && currMinY == within.Y && currMaxY == maxY ) ) {
				if( tileType.CheckArea(i, j, width, height) ) {	// TODO Optimize currMin and currMax from data from CheckArea
					return Tuple.Create( i, j );
				}

				switch( turn ) {
				case 0:
					if( i < currMaxX ) {
						i++;
					} else {
						if( j < currMaxY ) { j++; }
						turn++;
					}
					break;
				case 1:
					if( j < currMaxY ) {
						j++;
					} else {
						if( i > currMinX ) { i--; }
						turn++;
					}
					break;
				case 2:
					if( i > currMinX ) {
						i--;
					} else {
						if( j > currMinY ) { j--; }
						turn++;
					}
					break;
				case 3:
					if( j > currMinY ) {
						j--;
					} else {
						if( i < currMaxX ) { i--; }
						turn = 0;

						if( currMinX > within.X ) { currMinX--; }
						if( currMinY > within.Y ) { currMinY--; }
						if( currMaxX < maxX ) { currMaxX++; }
						if( currMaxY < maxY ) { currMaxY++; }
					}
					break;
				}
			}

			return null;
		}
	}
}
