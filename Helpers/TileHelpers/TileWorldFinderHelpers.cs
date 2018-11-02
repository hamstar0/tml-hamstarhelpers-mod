using Microsoft.Xna.Framework;
using System;


namespace HamstarHelpers.Helpers.TileHelpers {
	public static class TileWorldFinderHelpers {
		public static Tuple<int, int> FindWithin( TileType tile_type, Rectangle within ) {
			return TileWorldFinderHelpers.FindWithin( tile_type, within, 1, 1 );
		}


		public static Tuple<int, int> FindWithin( TileType tile_type, Rectangle within, int width, int height ) {
			int max_x = within.X + within.Width - width;
			int max_y = within.Y + within.Height - height;

			for( int i=within.X; i<max_x; i++ ) {
				for( int j=within.Y; j<max_y; j++ ) {
					if( tile_type.CheckArea( i, j, width, height) ) {
						return Tuple.Create( i, j );
					}
				}
			}

			return null;
		}


		public static Tuple<int, int> FindWithinFromCenter( TileType tile_type, Rectangle within, int width, int height ) {
			int mid_x = within.X + ( within.Width / 2 ) - ( width / 2 );
			int mid_y = within.Y + ( within.Height / 2 ) - ( height / 2 );
			int max_x = within.X + within.Width - width;
			int max_y = within.Y + within.Height - height;
			int curr_min_x = mid_x;
			int curr_max_x = mid_x;
			int curr_min_y = mid_y;
			int curr_max_y = mid_y;

			int i = mid_x;
			int j = mid_y;
			int turn = 0;

			while( !( curr_min_x == within.X && curr_max_x == max_x && curr_min_y == within.Y && curr_max_y == max_y ) ) {
				if( tile_type.CheckArea(i, j, width, height) ) {	// TODO Optimize curr_min and curr_max from data from CheckArea
					return Tuple.Create( i, j );
				}

				switch( turn ) {
				case 0:
					if( i < curr_max_x ) {
						i++;
					} else {
						if( j < curr_max_y ) { j++; }
						turn++;
					}
					break;
				case 1:
					if( j < curr_max_y ) {
						j++;
					} else {
						if( i > curr_min_x ) { i--; }
						turn++;
					}
					break;
				case 2:
					if( i > curr_min_x ) {
						i--;
					} else {
						if( j > curr_min_y ) { j--; }
						turn++;
					}
					break;
				case 3:
					if( j > curr_min_y ) {
						j--;
					} else {
						if( i < curr_max_x ) { i--; }
						turn = 0;

						if( curr_min_x > within.X ) { curr_min_x--; }
						if( curr_min_y > within.Y ) { curr_min_y--; }
						if( curr_max_x < max_x ) { curr_max_x++; }
						if( curr_max_y < max_y ) { curr_max_y++; }
					}
					break;
				}
			}

			return null;
		}
	}
}
