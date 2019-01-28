using System;
using System.Collections.Generic;
using System.Linq;


namespace HamstarHelpers.Components.DataStructures {
	public partial class QuadTree<T> where T : class {
		public T[] GetNearCoordinates( int x, int y, int amt ) {
			return this.GetQuadsNearCoordinates( x, y, amt )
				.Select( q=>q.Value )
				.ToArray();
		}

		////

		private IList<QuadTree<T>> GetQuadsNearCoordinates( int x, int y, int amt ) {
			var list = new SortedList<double, QuadTree<T>>();
			Func<QuadTree<T>, double> getDist = ( q ) => {
				int diffX = q.X - x;
				int diffY = q.Y - y;
				return Math.Sqrt( (diffX*diffX) + (diffY*diffY) );
			};

			if( this.Value != null ) {
				list[ getDist(this) ] = this;
			}
			
			ISet<QuadTree<T>> quads = new HashSet<QuadTree<T>>();
			if( this.TopLeft != null && this.TopLeft.Value != null ) { quads.Add( this.TopLeft ); }
			if( this.TopRight != null && this.TopRight.Value != null ) { quads.Add( this.TopRight ); }
			if( this.BotLeft != null && this.BotLeft.Value != null ) { quads.Add( this.BotLeft ); }
			if( this.BotRight != null && this.BotRight.Value != null ) { quads.Add( this.BotRight ); }

			var checks = new HashSet<QuadTree<T>>();
			double leastDist;
			QuadTree<T> leastQuad;
			int count = 0;

			do {
				leastDist = Double.MaxValue;
				leastQuad = null;

				foreach( var quad in quads ) {
					if( quad.Value == null ) { continue; }

					var stack = new List<QuadTree<T>>();
					this.TraceTreeStack( x, y, stack, checks );

					if( stack.Count > 0 && quad.Value != null ) {
						double dist = getDist( quad );

						if( dist < leastDist ) {
							leastDist = dist;
							leastQuad = quad;
						}

						list[ getDist(quad) ] = quad;
						count++;
					}
				}

				if( leastQuad != null ) {
					list[leastDist] = leastQuad;
					count++;
				} else {
					break;
				}
			} while( count < (amt + 1) );

			return list.Values.Take( amt ).ToList();
		}


		////////////////

		private void TraceTreeStack( int x, int y, IList<QuadTree<T>> stack, ISet<QuadTree<T>> checks ) {
			if( x != this.X || y != this.Y ) {
				if( x < this.X && !checks.Contains(this.TopLeft) && !checks.Contains(this.TopRight) ) {
					if( y < this.Y && !checks.Contains(this.TopLeft) ) {
						this.TopLeft?.TraceTreeStack( x, y, stack, checks );
					} else {
						this.TopRight?.TraceTreeStack( x, y, stack, checks );
					}
				} else {
					if( y < this.Y && !checks.Contains(this.BotLeft) && !checks.Contains(this.BotRight) ) {
						this.BotLeft?.TraceTreeStack( x, y, stack, checks );
					} else {
						this.BotRight?.TraceTreeStack( x, y, stack, checks );
					}
				}
			}

			stack.Add( this );
			checks.Add( this );
		}
	}
}
