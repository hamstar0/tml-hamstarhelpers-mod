using HamstarHelpers.Helpers.DotNetHelpers;
using System;
using System.Collections.Generic;
using System.Linq;


namespace HamstarHelpers.Components.DataStructures.QuadTree {
	public partial class QuadTree<T> where T : class {
		public T[] GetNearestElements( int x, int y, int amt ) {
			return this.GetTreesNearCoordinates( x, y, amt )
				.SafeSelect( q => q.Value )
				.ToArray();
		}

		////

		private IList<QuadTree<T>> GetTreesNearCoordinates( int x, int y, int amt ) {
			var list = new SortedList<double, QuadTree<T>>();
			var checks = new Dictionary<QuadTree<T>, bool>();
			

			////

			Func<QuadTree<T>, double> getDist = ( q ) => {
				int diffX = q.X - x;
				int diffY = q.Y - y;
				return Math.Sqrt( ( diffX * diffX ) + ( diffY * diffY ) );
			};


			Action<QuadTree<T>> listDeepestTreeAt = ( tree ) => {
				QuadTree<T> deepestTree = tree.GetDeepestTreeAt( x, y, checks );

				this.RegisterTreeChecks( checks, deepestTree );
				
				list[ getDist(deepestTree) ] = deepestTree;
			};


			////

			// Check top level node:
			if( this.Value != null ) {
				list[ getDist(this) ] = this;
				checks[ this ] = this.IsLeaf;
			}

			// Check each quad in tandem:
			for( int i = 0; i < (amt + 1); i++ ) {
				bool isDeadEnd = true;
				var tlq = this.TopLeftQuad;
				var trq = this.TopRightQuad;
				var blq = this.BotLeftQuad;
				var brq = this.BotRightQuad;

				if( tlq != null && !checks.HardGet( tlq ) ) {
					listDeepestTreeAt( tlq );
					isDeadEnd = false;
				}
				if( trq != null && !checks.HardGet( trq ) ) {
					listDeepestTreeAt( trq );
					isDeadEnd = false;
				}
				if( blq != null && !checks.HardGet( blq ) ) {
					listDeepestTreeAt( blq );
					isDeadEnd = false;
				}
				if( brq != null && !checks.HardGet( brq ) ) {
					listDeepestTreeAt( brq );
					isDeadEnd = false;
				}

				if( isDeadEnd ) { break; }
			}

			return list.Values.Take( amt ).ToList();
		}


		////////////////

		public QuadTree<T> GetDeepestTreeAt( int x, int y, IDictionary<QuadTree<T>, bool> avoid ) {
			if( x == this.X && y == this.Y ) {
				return this;
			}

			QuadTree<T> quad = this;
			
			var tlq = this.TopLeftQuad;
			var trq = this.TopRightQuad;
			var blq = this.BotLeftQuad;
			var brq = this.BotRightQuad;

			if( x < this.X && (tlq != null || blq != null) &&
					(tlq == null || !avoid.HardGet(tlq)) &&
					(blq == null || !avoid.HardGet(blq)) ) {
				if( y < this.Y && tlq != null && !avoid.HardGet( tlq ) ) {
					quad = tlq.GetDeepestTreeAt( x, y, avoid );
				} else if( blq != null && !avoid.HardGet( blq ) ) {
					quad = blq.GetDeepestTreeAt( x, y, avoid );
				}
			} else if( (trq != null || brq != null) &&
					(trq == null || !avoid.HardGet(trq)) &&
					(brq == null || !avoid.HardGet(brq)) ) {
				if( y < this.Y && trq != null && !avoid.HardGet( trq ) ) {
					quad = trq.GetDeepestTreeAt( x, y, avoid );
				} else if( brq != null && !avoid.HardGet( brq ) ) {
					quad = brq.GetDeepestTreeAt( x, y, avoid );
				}
			}

			return quad;
		}


		////////////////

		private void RegisterTreeChecks( IDictionary<QuadTree<T>, bool> checks, QuadTree<T> tree ) {
			checks[ tree ] = this.IsDeadEnd( checks, tree );

			for( QuadTree<T> currTree = tree.Parent; currTree != null; currTree = currTree.Parent ) {
				if( currTree.Value == null ) {
					checks[ currTree ] = this.IsDeadEnd( checks, currTree );
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

			if( (tlq != null && !checks.HardGet( tlq )) ||
				(trq != null && !checks.HardGet( trq )) ||
				(blq != null && !checks.HardGet( blq )) ||
				(brq != null && !checks.HardGet( brq )) ) {
			} else {
				return false;
			}
			return true;
		}
	}
}
