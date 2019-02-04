using System;
using System.Collections.Generic;
using System.Linq;


namespace HamstarHelpers.Components.DataStructures {
	public partial class QuadTree<T> where T : class {
		public T[] GetNearestElements( int x, int y, int amt ) {
			return this.GetTreesNearCoordinates( x, y, amt )
				.Select( q=>q.Value )
				.ToArray();
		}

		////

		private IList<QuadTree<T>> GetTreesNearCoordinates( int x, int y, int amt ) {
			var list = new SortedList<double, QuadTree<T>>();
			var checks = new HashSet<QuadTree<T>>();
			double leastDist = Double.MaxValue;
			QuadTree<T> leastTree;


			Func<QuadTree<T>, double> getDist = ( q ) => {
				int diffX = q.X - x;
				int diffY = q.Y - y;
				return Math.Sqrt( (diffX*diffX) + (diffY*diffY) );
			};

			Action<QuadTree<T>> listDeepestTreeAt = ( tree ) => {
				QuadTree<T> deepestTree = tree.GetDeepestTreeAt( x, y, checks );
				checks.Add( deepestTree );

				double dist = getDist( deepestTree );

				if( dist < leastDist ) {
					leastDist = dist;
					leastTree = deepestTree;
				}

				list[ getDist(deepestTree) ] = deepestTree;
			};


			if( this.Value != null ) {
				list[ getDist(this) ] = this;
			}
			
			for( int i=0; i < (amt+1); i++ ) {
				bool done = true;

				leastDist = Double.MaxValue;
				leastTree = null;

				if( this.TopLeftQuad != null && this.TopLeftQuad.Value != null && !checks.Contains(this.TopLeftQuad) ) {
					listDeepestTreeAt( this.TopLeftQuad );
					done = false;
				}
				if( this.TopRightQuad != null && this.TopRightQuad.Value != null && !checks.Contains(this.TopRightQuad) ) {
					listDeepestTreeAt( this.TopRightQuad );
					done = false;
				}
				if( this.BotLeftQuad != null && this.BotLeftQuad.Value != null && !checks.Contains(this.BotLeftQuad) ) {
					listDeepestTreeAt( this.BotLeftQuad );
					done = false;
				}
				if( this.BotRightQuad != null && this.BotRightQuad.Value != null && !checks.Contains(this.BotRightQuad) ) {
					listDeepestTreeAt( this.BotRightQuad );
					done = false;
				}

				if( done ) { break; }
			}

			return list.Values.Take( amt ).ToList();
		}


		////////////////

		public QuadTree<T> GetDeepestTreeAt( int x, int y, ISet<QuadTree<T>> avoid ) {
			QuadTree<T> quad = this;

			if( x != this.X || y != this.Y ) {
				if( x < this.X && !avoid.Contains(this.TopLeftQuad) && !avoid.Contains(this.TopRightQuad) ) {
					if( y < this.Y && !avoid.Contains(this.TopLeftQuad) ) {
						quad = this.TopLeftQuad?.GetDeepestTreeAt( x, y, avoid );
					} else {
						quad = this.TopRightQuad?.GetDeepestTreeAt( x, y, avoid );
					}
				} else {
					if( y < this.Y && !avoid.Contains(this.BotLeftQuad) && !avoid.Contains(this.BotRightQuad) ) {
						quad = this.BotLeftQuad?.GetDeepestTreeAt( x, y, avoid );
					} else {
						quad = this.BotRightQuad?.GetDeepestTreeAt( x, y, avoid );
					}
				}
			}
			
			return quad;
		}
	}
}
