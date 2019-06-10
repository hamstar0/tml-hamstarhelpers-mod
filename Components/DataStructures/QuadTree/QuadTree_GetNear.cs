using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET;
using System;
using System.Collections.Generic;
using System.Linq;


namespace HamstarHelpers.Components.DataStructures.QuadTree {
	public partial class QuadTree<T> where T : class {
		public T[] GetNearestElements( int x, int y, int amt ) {
			return this.GetValueTreesNearCoordinates( x, y, amt )
				.SafeSelect( q => q.Value )
				.ToArray();
		}


		////

		private IList<QuadTree<T>> GetValueTreesNearCoordinates( int x, int y, int amt ) {
			var list = new SortedList<double, QuadTree<T>>();
			var checks = new Dictionary<QuadTree<T>, bool>();
			

			////

			Func<QuadTree<T>, double> getDist = ( quad ) => {
				int diffX = quad.X - x;
				int diffY = quad.Y - y;
				return Math.Sqrt( (diffX * diffX) + (diffY * diffY) );
			};


			Action<QuadTree<T>> listDeepestTreeAt = ( tree ) => {
				QuadTree<T> deepestTree = tree.GetDeepestTreeNearCoordinates( x, y, checks );

				this.ChartTree( checks, deepestTree );

				if( deepestTree != null && deepestTree.Value != null ) {
					list[ getDist(deepestTree) ] = deepestTree;
				}
			};


			////

			// Check top level node:
			if( this.Value != null ) {
				list[ getDist(this) ] = this;
			}

			// Check each quad in tandem:
			for( int i = 0; i < (amt + 1); i++ ) {
				bool isDeadEnd = true;
				var tlq = this.TopLeftQuad;
				var trq = this.TopRightQuad;
				var blq = this.BotLeftQuad;
				var brq = this.BotRightQuad;

				if( tlq != null && !checks.GetOrDefault( tlq ) ) {
					listDeepestTreeAt( tlq );
					isDeadEnd = false;
				}
				if( trq != null && !checks.GetOrDefault( trq ) ) {
					listDeepestTreeAt( trq );
					isDeadEnd = false;
				}
				if( blq != null && !checks.GetOrDefault( blq ) ) {
					listDeepestTreeAt( blq );
					isDeadEnd = false;
				}
				if( brq != null && !checks.GetOrDefault( brq ) ) {
					listDeepestTreeAt( brq );
					isDeadEnd = false;
				}

				if( isDeadEnd ) { break; }
			}

			return list.Values.Take( amt ).ToList();
		}


		////////////////

		public QuadTree<T> GetDeepestTreeNearCoordinates( int x, int y, IDictionary<QuadTree<T>, bool> avoid ) {
			if( x == this.X && y == this.Y && this.Width == 1 && this.Height == 1 ) {
				return this;
			}

			QuadTree<T> quad = this;
			
			var tlq = this.TopLeftQuad;
			var trq = this.TopRightQuad;
			var blq = this.BotLeftQuad;
			var brq = this.BotRightQuad;
			
			if( x < this.MidX && (tlq != null || blq != null) &&
					(tlq == null || !avoid.GetOrDefault(tlq)) &&
					(blq == null || !avoid.GetOrDefault(blq))
			) {
				if( y < this.MidY && tlq != null && !avoid.GetOrDefault( tlq ) ) {
					quad = tlq.GetDeepestTreeNearCoordinates( x, y, avoid );
				} else if( blq != null && !avoid.GetOrDefault( blq ) ) {
					quad = blq.GetDeepestTreeNearCoordinates( x, y, avoid );
				}
			} else if( (trq != null || brq != null) &&
					(trq == null || !avoid.GetOrDefault(trq)) &&
					(brq == null || !avoid.GetOrDefault(brq))
			) {
				if( y < this.MidY && trq != null && !avoid.GetOrDefault( trq ) ) {
					quad = trq.GetDeepestTreeNearCoordinates( x, y, avoid );
				} else if( brq != null && !avoid.GetOrDefault( brq ) ) {
					quad = brq.GetDeepestTreeNearCoordinates( x, y, avoid );
				}
			}

			return quad;
		}


		////////////////

		private void ChartTree( IDictionary<QuadTree<T>, bool> checks, QuadTree<T> tree ) {
			checks[ tree ] = true;//this.IsDeadEnd( checks, tree );

			for( QuadTree<T> currTree = tree.Parent; currTree != null; currTree = currTree.Parent ) {
				if( !this.IsDeadEnd( checks, currTree ) ) {
					break;
				}

				checks[ currTree ] = currTree.Value == null;
				if( checks[currTree] == false ) {
					break;
				}
			}
		}

		////

		private bool IsDeadEnd( IDictionary<QuadTree<T>, bool> checks, QuadTree<T> tree ) {
			if( tree.IsLeaf ) { return true; }
			
			var tlq = tree.TopLeftQuad;
			var trq = tree.TopRightQuad;
			var blq = tree.BotLeftQuad;
			var brq = tree.BotRightQuad;

			if( (tlq != null && !checks.GetOrDefault( tlq )) ||
				(trq != null && !checks.GetOrDefault( trq )) ||
				(blq != null && !checks.GetOrDefault( blq )) ||
				(brq != null && !checks.GetOrDefault( brq )) ) {
				return true;
			} else {
				return false;
			}
		}
	}
}
