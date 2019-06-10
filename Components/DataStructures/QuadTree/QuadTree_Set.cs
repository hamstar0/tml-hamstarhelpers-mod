using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.Debug;
using System;


namespace HamstarHelpers.Components.DataStructures.QuadTree {
	public partial class QuadTree<T> where T : class {
		public void Set( int x, int y, T val ) {
			if( x < this.X || y < this.Y || x >= (this.X + this.Width) || y >= (this.Y + this.Height) ) {
				throw new HamstarException( "Coordinates ("+x+","+y+") will not fit inside this tree's space ("+this.Rect.ToString()+")" );
			}

			this.Count++;

			if( this.X == x && this.Y == y && this.Width == 1 && this.Height == 1 ) {
				this.Value = val;
			} else {
				this.SetQuadAt( x, y, val );
			}
		}

		private void SetQuadAt( int x, int y, T val ) {
			if( this.Width <= 1 && this.Height <= 1 ) {
				throw new HamstarException( "Already a leaf ("+this.Rect.ToString()+")" );
			}

			if( x < this.MidX ) {
				if( y < this.MidY ) {
					if( this.TopLeftQuad == null ) {
						this.TopLeftQuad = new QuadTree<T>( this, this.X, this.Y );
					}
					this.TopLeftQuad.Set( x, y, val );
				} else {
					if( this.BotLeftQuad == null ) {
						this.BotLeftQuad = new QuadTree<T>( this, this.X, this.MidY );
					}
					this.BotLeftQuad.Set( x, y, val );
				}
			} else {
				if( y < this.MidY ) {
					if( this.TopRightQuad == null ) {
						this.TopRightQuad = new QuadTree<T>( this, this.MidX, this.Y );
					}
					this.TopRightQuad.Set( x, y, val );
				} else {
					if( this.BotRightQuad == null ) {
						this.BotRightQuad = new QuadTree<T>( this, this.MidX, this.MidY );
					}
					this.BotRightQuad.Set( x, y, val );
				}
			}
		}
	}
}
