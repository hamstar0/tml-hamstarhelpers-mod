using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.World.Generation;
using HamstarHelpers.Classes.Tiles.TilePattern;
using HamstarHelpers.Helpers.Debug;


namespace HamstarHelpers.Helpers.Tiles {
	/// <summary>
	/// Assorted static "helper" functions pertaining to tile finding.
	/// </summary>
	public partial class TileFinderHelpers {
		/// <summary>
		/// Scans the entire world to find the encompassing box of the given tile pattern. Leaves a 1 tile
		/// padding around map edges.
		/// 
		/// Note: This does NOT check for contiguity.
		/// </summary>
		/// <param name="pattern"></param>
		/// <param name="minimumAdjacentMatchesPerTileMatch"></param>
		/// <returns></returns>
		public static Rectangle? FindBoxForAllOf( TilePattern pattern, int minimumAdjacentMatchesPerTileMatch = 1 ) {
			int countNeighbors( int x, int y ) {
				if( minimumAdjacentMatchesPerTileMatch == 0 ) {
					return 0;
				}

				int count = 0;

				if( pattern.Check( x - 1, y - 1 ) ) {
					count++;
				}
				if( pattern.Check( x, y - 1 ) ) {
					count++;
				}
				if( pattern.Check( x + 1, y - 1 ) ) {
					count++;
				}
				if( pattern.Check( x - 1, y ) ) {
					count++;
				}
				if( pattern.Check( x + 1, y ) ) {
					count++;
				}
				if( pattern.Check( x - 1, y + 1 ) ) {
					count++;
				}
				if( pattern.Check( x, y + 1 ) ) {
					count++;
				}
				if( pattern.Check( x + 1, y + 1 ) ) {
					count++;
				}

				return count;
			}

			//

			int maxX = Main.maxTilesX - 1;
			int maxY = Main.maxTilesY - 1;

			int leftX = maxX;
			int rightX = 0;
			int topY = maxY;
			int botY = 0;

			for( int x=1; x<maxX; x++ ) {
				for( int y=1; y<maxY; y++ ) {
					if( !pattern.Check(x, y) ) {
						continue;
					}

					if( minimumAdjacentMatchesPerTileMatch > 0 ) {
						int neighbors = countNeighbors( x, y );
						if( neighbors < minimumAdjacentMatchesPerTileMatch ) {
							continue;
						}
					}

					if( x < leftX ) {
						leftX = x;
					}
					if( x > rightX ) {
						rightX = x;
					}
					if( y < topY ) {
						topY = y;
					}
					if( y > botY ) {
						botY = y;
					}
				}
			}

			return new Rectangle(
				x: leftX,
				y: topY,
				width: Math.Max( (1 + rightX) - leftX, 0 ),
				height: Math.Max( (1 + botY) - topY, 0 )
			);
		}


		/// <summary>
		/// Finds all `Rectangle`s encompassing contiguous matches of the given `pattern`.
		/// </summary>
		/// <param name="pattern"></param>
		/// <param name="minTileX"></param>
		/// <param name="minTileY"></param>
		/// <param name="maxTileX"></param>
		/// <param name="maxTileY"></param>
		/// <param name="progress"></param>
		/// <returns></returns>
		public static ISet<Rectangle> FindBoxesOfAllContiguousMatches(
					TilePattern pattern,
					int minTileX = 1,
					int minTileY = 1,
					int maxTileX = -1,
					int maxTileY = -1,
					GenerationProgress progress = null ) {
			ushort myMaxTileX = maxTileX < 0 ? (ushort)(Main.maxTilesX - 1): (ushort)maxTileX;
			ushort myMaxTileY = maxTileY < 0 ? (ushort)(Main.maxTilesY - 1): (ushort)maxTileY;

			//

			Rectangle getRect( IEnumerable<(ushort x, ushort y)> matches ) {
				int leftX = Main.maxTilesX - 1;
				int rightX = 1;
				int topY = Main.maxTilesY - 1;
				int botY = 1;
				
				foreach( (ushort x, ushort y) in matches ) {
					if( x < leftX /*&& x >= minTileX*/ ) {
						leftX = x;
					}
					if( x > rightX /*&& x < maxTileX*/ ) {
						rightX = x;
					}
					if( y < topY /*&& y >= minTileY*/ ) {
						topY = y;
					}
					if( y > botY /*&& y < maxTileY*/ ) {
						botY = y;
					}
				}

				return new Rectangle(
					leftX,
					topY,
					(rightX - leftX) + 1,
					(botY - topY) + 1
				);
			}

			//

			var rects = new HashSet<Rectangle>();
			var matchesList = new List<ISet<(ushort, ushort)>>();

			int cols = myMaxTileX - minTileX;
			int rows = myMaxTileY - minTileY;

			for( ushort x=(ushort)minTileX; x<myMaxTileX; x++ ) {
				for( ushort y=(ushort)minTileY; y<myMaxTileY; y++ ) {
					if( progress != null ) {
						float maxProgress = cols * rows;
						float currProgress = (x * rows) + y;
						progress.Value = currProgress / maxProgress;
					}

					foreach( ISet<(ushort x, ushort y)> prevMatches in matchesList ) {
						if( prevMatches.Contains( (x, y) ) ) {
							goto SKIP;
						}
					}

					ISet<(ushort, ushort)> matches;
					matches = TileFinderHelpers.GetAllContiguousMatchingTilesAt( pattern, x, y, out _ );
					if( matches.Count == 0 ) {
						continue;
					}
//LogHelpers.Log( "x: "+x+", y: "+y+" - "+ matches.Count);

					Rectangle rect = getRect( matches );
					rects.Add( rect );

					while( matches.Contains( (x, ++y) ) && y < myMaxTileY ) { }
					y--;

					matchesList.Add( matches );

					SKIP: continue;
				}
			}

			return rects;
		}
	}
}
