using System;


namespace HamstarHelpers.DotNetHelpers.DataStructures {
	public class SimpleQuadTree<T> {
		public SimpleQuadTree<T> NextLeft { get; private set; }
		public SimpleQuadTree<T> NextRight { get; private set; }
		public SimpleQuadTree<T> NextAbove { get; private set; }
		public SimpleQuadTree<T> NextBelow { get; private set; }

		public SimpleQuadTree<T> Parent { get; private set; }

		public SimpleQuadTree<T> TopLeft { get; private set; }
		public SimpleQuadTree<T> TopRight { get; private set; }
		public SimpleQuadTree<T> BottomLeft { get; private set; }
		public SimpleQuadTree<T> BottomRight { get; private set; }

		public int X { get; private set; }
		public int Y { get; private set; }
		public T Item { get; private set; }
		private SimpleQuadTree<T> AKA;


		////////////////

		public SimpleQuadTree( SimpleQuadTree<T> parent, int x, int y, T item ) {
			this.NextLeft = (SimpleQuadTree<T>)null;
			this.NextRight = (SimpleQuadTree<T>)null;
			this.NextAbove = (SimpleQuadTree<T>)null;
			this.NextBelow = (SimpleQuadTree<T>)null;
			this.Parent = parent;
			this.TopLeft = (SimpleQuadTree<T>)null;
			this.TopRight = (SimpleQuadTree<T>)null;
			this.BottomLeft = (SimpleQuadTree<T>)null;
			this.BottomRight = (SimpleQuadTree<T>)null;
			this.X = x;
			this.Y = y;
			this.Item = item;
			this.AKA = item == null ? (SimpleQuadTree<T>)null : this;
		}


		////////////////
		
		private SimpleQuadTree<T> DefineQuad( int x, int y, int x_range, int y_range ) {
			if( this.X == x && this.Y == y ) {
				return this;
			}

			SimpleQuadTree<T> quad;

			if( x > this.X ) {
				if( y > this.Y ) {
					if( this.BottomRight == null ) {
						quad = new SimpleQuadTree<T>( this, this.X + x_range, this.Y + y_range, (T)(object)null );
						this.BottomRight = quad;
					} else {
						quad = this.BottomRight;
					}
				} else {
					if( this.TopRight == null ) {
						quad = new SimpleQuadTree<T>( this, this.X + x_range, this.Y - y_range, (T)(object)null );
						this.TopRight = quad;
					} else {
						quad = this.TopRight;
					}
				}
			} else {
				if( y > this.Y ) {
					if( this.BottomLeft == null ) {
						quad = new SimpleQuadTree<T>( this, this.X - x_range, this.Y + y_range, (T)(object)null );
						this.BottomLeft = quad;
					} else {
						quad = this.BottomLeft;
					}
				} else {
					if( this.TopLeft == null ) {
						quad = new SimpleQuadTree<T>( this, this.X - x_range, this.Y - y_range, (T)(object)null );
						this.TopLeft = quad;
					} else {
						quad = this.TopLeft;
					}
				}
			}

			SimpleQuadTree<T> new_quad = quad.DefineQuad( x, y, x_range / 2, y_range / 2);
			
			new_quad.NextAbove = this.GetQuadOf( x, y - (y_range*2) );
			if( new_quad.NextAbove != null ) {
				new_quad.NextAbove.NextBelow = this;
			}
			new_quad.NextLeft = this.GetQuadOf( x - (x_range*2), y );
			if( new_quad.NextLeft != null ) {
				new_quad.NextLeft.NextRight = this;
			}
			new_quad.NextBelow = this.GetQuadOf( x, y + (y_range*2) );
			if( new_quad.NextBelow != null ) {
				new_quad.NextBelow.NextAbove = this;
			}
			new_quad.NextRight = this.GetQuadOf( x + (x_range*2), y );
			if( new_quad.NextRight != null ) {
				new_quad.NextRight.NextLeft = this;
			}

			return new_quad;
		}

		////////////////

		public SimpleQuadTree<T> GetQuadOf( int x, int y ) {
			if( this.X == x && this.Y == y ) {
				return this;
			}

			if( x > this.X ) {
				if( y > this.Y ) {
					if( this.BottomRight != null ) {
						this.BottomRight.GetQuadOf( x, y );
					}
				} else {
					if( this.TopRight != null ) {
						this.TopRight.GetQuadOf( x, y );
					}
				}
			} else {
				if( y > this.Y ) {
					if( this.BottomLeft != null ) {
						this.BottomLeft.GetQuadOf( x, y );
					}
				} else {
					if( this.TopLeft != null ) {
						this.TopLeft.GetQuadOf( x, y );
					}
				}
			}

			return null;
		}

		
		////////////////

		public void Add( int x, int y, T item ) {
			if( this.X == x && this.Y == y ) {
				this.Item = item;
				this.AKA = this;
				return;
			}

			SimpleQuadTree<T> quad = this.DefineQuad( x, y, this.X / 2, this.Y / 2 );
			quad.Item = item;

			for( SimpleQuadTree<T> up_quad = quad.Parent; up_quad != null; quad = up_quad.Parent ) {
				if( up_quad.AKA != null ) {
					up_quad.AKA = null;
				} else {
					up_quad.AKA = quad;
				}
			}
		}

		////////////////
	}
}
