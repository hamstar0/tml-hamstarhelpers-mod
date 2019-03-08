using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Collections.Generic;


namespace HamstarHelpers.Components.DataStructures.QuadTree {
	public partial class QuadTree<T> where T : class {
		public void Test() {
			var overlaps = new Dictionary<int, ISet<int>>();
			int count = this.TestQuad( "", overlaps, 0, this.X, this.Y );

			if( count != this.Count ) {
				throw new HamstarException( "TEST FAILED: Count mismatch - Counted "+count+", expected "+this.Count );
			}
		}


		private int TestQuad( string which, IDictionary<int, ISet<int>> overlaps, int layer, int maxQuadWidth, int maxQuadHeight ) {
			if( !overlaps.ContainsKey(this.X) ) {
				overlaps[ this.X ] = new HashSet<int>();
			} else if( overlaps[this.X].Contains( this.Y ) ) {
				throw new HamstarException( "TEST FAILED: Tree layer "+layer+" ("+which+") overlaps at "+this.X+":"+this.Y );
			}
			overlaps[this.X].Add( this.Y );

			int count = this.Value != null ? 1 : 0;

			if( layer > 0 ) {
				int myWidth = Math.Abs( this.X - this.Parent.X );
				int myHeight = Math.Abs( this.Y - this.Parent.Y );

				if( myWidth == 0 && myHeight == 0 && this.Value == null ) {
					throw new HamstarException( "TEST FAILED: Tree layer "+layer+" ("+which+") has an empty leaf" );
				}

				/*if( myWidth != maxQuadWidth ) {
					throw new HamstarException( "TEST FAILED: Tree layer "+layer+" ("+which+") bad width - Measured "+myWidth+", expected "+maxQuadWidth );
				}
				if( myHeight != maxQuadHeight ) {
					throw new HamstarException( "TEST FAILED: Tree layer "+layer+" ("+which+") bad height - Measured "+myHeight+", expected "+maxQuadHeight );
				}*/
			}

			count += this.TopLeftQuad?.TestQuad( which+"┌", overlaps, layer+1, maxQuadWidth/2, maxQuadHeight/2 ) ?? 0;
			count += this.TopRightQuad?.TestQuad( which+"┐", overlaps, layer+1, maxQuadWidth/2, maxQuadHeight/2 ) ?? 0;
			count += this.BotLeftQuad?.TestQuad( which+"└", overlaps, layer+1, maxQuadWidth/2, maxQuadHeight/2 ) ?? 0;
			count += this.BotRightQuad?.TestQuad( which+"┘", overlaps, layer+1, maxQuadWidth/2, maxQuadHeight/2 ) ?? 0;

			return count;
		}
	}
}
