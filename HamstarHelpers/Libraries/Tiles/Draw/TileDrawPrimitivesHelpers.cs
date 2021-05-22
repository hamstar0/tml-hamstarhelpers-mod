using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Utilities;
using HamstarHelpers.Classes.Tiles.TilePattern;
using HamstarHelpers.Libraries.TModLoader;


namespace HamstarHelpers.Libraries.Tiles.Draw {
	/// <summary>
	/// Assorted static "helper" functions pertaining to 'drawing' primitive tile structures into the world.
	/// </summary>
	public class TileDrawPrimitivesLibraries {
		/// <summary>
		/// Draws a rectangle (from top to bottom) of a given tile type (where the filter allows).
		/// </summary>
		/// <param name="filter">Condition for applying each tile `place`ment.</param>
		/// <param name="area">Area to attempt to `place` tiles.</param>
		/// <param name="hollow">Area to not attempt to `place` tiles.</param>
		/// <param name="place">Return `null` to skip tile placing.</param>
		/// <returns></returns>
		public static ISet<(int TileX, int TileY)> DrawRectangle(
					TilePattern filter,
					Rectangle area,
					Rectangle? hollow,
					Func<int, int, TileDrawDefinition> place ) {
			var tiles = new HashSet<(int, int)>();
			int maxX = area.X + area.Width;
			int maxY = area.Y + area.Height;
			Rectangle myHollow = hollow.HasValue ? hollow.Value : new Rectangle();

			for( int y = area.Y; y < maxY; y++ ) {
				for( int x=area.X; x<maxX; x++ ) {
					if( hollow.HasValue ) {
						if( x >= myHollow.X && x < (myHollow.X + myHollow.Width) ) {
							if( y >= myHollow.Y && y < (myHollow.Y + myHollow.Height) ) {
								continue;
							}
						}
					}

					if( filter.Check(x, y) ) {
						if( place( x, y )?.Place(x, y) ?? false ) {
							tiles.Add( (x, y) );
						}
					}
				}
			}

			return tiles;
		}


		/// <summary>
		/// Draws a circle of a given tile type (where the filter allows).
		/// </summary>
		/// <param name="filter"></param>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <param name="minRadius"></param>
		/// <param name="maxRadius"></param>
		/// <param name="place">Return `null` to skip tile placing.</param>
		/// <returns></returns>
		public static ISet<(int TileX, int TileY)> DrawCircle(
					TilePattern filter,
					int tileX,
					int tileY,
					float minRadius,
					float maxRadius,
					Func<int, int, TileDrawDefinition> place ) {
			var filled = new HashSet<(int, int)>();
			var unfilled = new HashSet<(int, int)> { (tileX, tileY) };

			//

			float minRadSqr = minRadius * minRadius;
			float maxRadSqr = maxRadius * maxRadius;
			bool checkRadius( int x, int y ) {
				int diffX = x - tileX;
				int diffY = y - tileY;
				int distSqr = (diffX * diffX) + (diffY * diffY);

				return distSqr >= minRadSqr && distSqr < maxRadSqr;
			}

			//

			do {
				var unfilledCopy = unfilled.ToArray();
				unfilled.Clear();

				foreach( (int x, int y) in unfilledCopy ) {
					if( !checkRadius(x, y) ) {
						continue;
					}

					if( filter.Check( x, y ) ) {
						if( place( x, y )?.Place(x, y) ?? false ) {
							filled.Add( (x, y) );
						}
					}

					if( !filled.Contains( (x, y - 1) ) ) {
						unfilled.Add( (x, y - 1) );
					}
					if( !filled.Contains( (x - 1, y) ) ) {
						unfilled.Add( (x - 1, y) );
					}
					if( !filled.Contains( (x + 1, y) ) ) {
						unfilled.Add( (x + 1, y) );
					}
					if( !filled.Contains( (x, y + 1) ) ) {
						unfilled.Add( (x, y + 1) );
					}
					if( !filled.Contains( (x - 1, y - 1) ) ) {
						unfilled.Add( (x - 1, y - 1) );
					}
					if( !filled.Contains( (x + 1, y - 1) ) ) {
						unfilled.Add( (x + 1, y - 1) );
					}
					if( !filled.Contains( (x - 1, y + 1) ) ) {
						unfilled.Add( (x - 1, y + 1) );
					}
					if( !filled.Contains( (x + 1, y + 1) ) ) {
						unfilled.Add( (x + 1, y + 1) );
					}
				}
			} while( unfilled.Count > 0 );

			return filled;
		}


		/// <summary>
		/// Draws a blob of a given tile type (where the filter allows).
		/// </summary>
		/// <param name="filter"></param>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <param name="minSize"></param>
		/// <param name="maxSize"></param>
		/// <param name="place">Return `null` to skip tile placing.</param>
		/// <returns></returns>
		public static ISet<(int TileX, int TileY)> DrawBlob(
					TilePattern filter,
					int tileX,
					int tileY,
					float minSize,
					float maxSize,
					Func<int, int, TileDrawDefinition> place ) {
			UnifiedRandom rand = TmlLibraries.SafelyGetRand();
			var filled = new HashSet<(int, int)>();
			var unfilled = new HashSet<(int, int)> { (tileX, tileY) };

			float minSizeSqr = minSize * minSize;
			float maxSizeSqr = maxSize * maxSize;
			float sizeDiffSqr = maxSizeSqr - minSizeSqr;
			
			//

			bool checkRand( int x, int y ) {
				int diffX = x - tileX;
				int diffY = y - tileY;
				int distSqr = ( diffX * diffX ) + ( diffY * diffY );

				if( distSqr < minSizeSqr ) {
					return true;
				}

				float randPercent = rand.NextFloat();
				float randSizePercent = randPercent * sizeDiffSqr;

				return randSizePercent > distSqr;
			}

			//

			bool processAt( int x, int y ) {
				if( filled.Contains( (x, y) ) ) {
					return false;
				}

				bool check = checkRand( x, y );
				if( check ) {
					unfilled.Add( (x, y) );
				}

				return check;
			}

			//

			bool topAdd = false, botAdd = false, leftAdd = false, rightAdd = false;

			do {
				var unfilledCopy = unfilled.ToArray();
				unfilled.Clear();

				foreach( (int x, int y) in unfilledCopy ) {
					if( filter.Check( x, y ) ) {
						if( place( x, y )?.Place(x, y) ?? false ) {
							filled.Add( (x, y) );
						}
					}

					topAdd = processAt( x, y - 1 );
					leftAdd = processAt( x - 1, y );
					rightAdd = processAt( x + 1, y );
					botAdd = processAt( x, y + 1 );

					/*if( smooth ) {
						if( !filled.Contains( (x - 1, y - 1) ) ) {
							if( topAdd && leftAdd ) {
								unfilled.Add( (x - 1, y - 1) );
							}
						}
						if( !filled.Contains( (x + 1, y - 1) ) ) {
							if( topAdd && rightAdd ) {
								unfilled.Add( (x + 1, y - 1) );
							}
						}
						if( !filled.Contains( (x - 1, y + 1) ) ) {
							if( botAdd && leftAdd ) {
								unfilled.Add( (x - 1, y + 1) );
							}
						}
						if( !filled.Contains( (x + 1, y + 1) ) ) {
							if( botAdd && rightAdd ) {
								unfilled.Add( (x + 1, y + 1) );
							}
						}
					}*/
				}
			} while( unfilled.Count > 0 );

			return filled;
		}
	}
}
