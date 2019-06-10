using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.Debug;
using Microsoft.Xna.Framework;
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
			int valueCount = 0;

			if( this.Width <= 1 && this.Height <= 1 ) {
				if( this.Width == 0 || this.Height == 0 ) {
					throw new HamstarException( "TEST FAILED: 0 width or height." );
				}

				if( this.Value == null ) {
					throw new HamstarException( "TEST FAILED: Null values not allowed." );
				}

				if( !overlaps.ContainsKey( this.X ) ) {
					overlaps[this.X] = new HashSet<int>();
				} else if( overlaps[this.X].Contains( this.Y ) ) {
					throw new HamstarException( "TEST FAILED: Tree layer " + layer + " (" + which + ") overlaps at " + this.X + ":" + this.Y );
				}

				overlaps[this.X].Add( this.Y );
				valueCount++;
			}

			if( layer > 0 ) {
				if( this.Width == 1 && this.Height == 1 && this.Value == null ) {
					throw new HamstarException( "TEST FAILED: Tree layer "+layer+" ("+which+") has an empty leaf" );
				}

				if( this.Width != maxQuadWidth ) {
					throw new HamstarException( "TEST FAILED: Tree layer "+layer+" ("+which+") bad width - Measured "+this.Width+", expected "+maxQuadWidth );
				}
				if( this.Height != maxQuadHeight ) {
					throw new HamstarException( "TEST FAILED: Tree layer "+layer+" ("+which+") bad height - Measured "+this.Height+", expected "+maxQuadHeight );
				}
			}

			if( this.TopLeftQuad != null ) {
				Rectangle rect = QuadTree<T>.GetQuadRect( this.TopLeftQuad.X, this.TopLeftQuad.Y, this );
				valueCount += this.TopLeftQuad.TestQuad( which + "┌", overlaps, layer + 1, rect.Width, rect.Height );
			}
			if( this.TopRightQuad != null ) {
				Rectangle rect = QuadTree<T>.GetQuadRect( this.TopRightQuad.X, this.TopRightQuad.Y, this );
				valueCount += this.TopRightQuad.TestQuad( which + "┌", overlaps, layer + 1, rect.Width, rect.Height );
			}
			if( this.BotLeftQuad != null ) {
				Rectangle rect = QuadTree<T>.GetQuadRect( this.BotLeftQuad.X, this.BotLeftQuad.Y, this );
				valueCount += this.BotLeftQuad.TestQuad( which + "┌", overlaps, layer + 1, rect.Width, rect.Height );
			}
			if( this.BotRightQuad != null ) {
				Rectangle rect = QuadTree<T>.GetQuadRect( this.BotRightQuad.X, this.BotRightQuad.Y, this );
				valueCount += this.BotRightQuad.TestQuad( which + "┌", overlaps, layer + 1, rect.Width, rect.Height );
			}

			return valueCount;
		}
	}
}
