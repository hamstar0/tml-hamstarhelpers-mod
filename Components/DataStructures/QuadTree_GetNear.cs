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

				if( this.TopLeft != null && this.TopLeft.Value != null && !checks.Contains(this.TopLeft) ) {
					listDeepestTreeAt( this.TopLeft );
					done = false;
				}
				if( this.TopRight != null && this.TopRight.Value != null && !checks.Contains(this.TopRight) ) {
					listDeepestTreeAt( this.TopRight );
					done = false;
				}
				if( this.BotLeft != null && this.BotLeft.Value != null && !checks.Contains(this.BotLeft) ) {
					listDeepestTreeAt( this.BotLeft );
					done = false;
				}
				if( this.BotRight != null && this.BotRight.Value != null && !checks.Contains(this.BotRight) ) {
					listDeepestTreeAt( this.BotRight );
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
				if( x < this.X && !avoid.Contains(this.TopLeft) && !avoid.Contains(this.TopRight) ) {
					if( y < this.Y && !avoid.Contains(this.TopLeft) ) {
						quad = this.TopLeft?.GetDeepestTreeAt( x, y, avoid );
					} else {
						quad = this.TopRight?.GetDeepestTreeAt( x, y, avoid );
					}
				} else {
					if( y < this.Y && !avoid.Contains(this.BotLeft) && !avoid.Contains(this.BotRight) ) {
						quad = this.BotLeft?.GetDeepestTreeAt( x, y, avoid );
					} else {
						quad = this.BotRight?.GetDeepestTreeAt( x, y, avoid );
					}
				}
			}
			
			return quad;
		}
	}
}
