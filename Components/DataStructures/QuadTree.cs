using System;


namespace HamstarHelpers.Components.DataStructures {
	public partial class QuadTree<T> where T : class {
		public int X { get; private set; }
		public int Y { get; private set; }

		private QuadTree<T> Parent;
		private QuadTree<T> TopLeft;
		private QuadTree<T> TopRight;
		private QuadTree<T> BotLeft;
		private QuadTree<T> BotRight;

		public T Value;

		public int Count { get; private set; }



		////////////////

		public QuadTree( int x, int y, T val = null ) {
			this.X = x;
			this.Y = y;
			this.Parent = null;
			this.Value = val;
			this.Count = val == null ? 1 : 0;
		}

		private QuadTree( QuadTree<T> parent, int x, int y ) : this(x, y) {
			this.Parent = parent;
		}

		////////////////

		public T Get( int x, int y ) {
			if( this.X == x && this.Y == y ) {
				return this.Value;
			}

			if( x < this.X ) {
				if( y < this.Y ) {
					return this.TopLeft?.Get( x, y ) ?? null;
				} else {
					return this.TopRight?.Get( x, y ) ?? null;
				}
			} else {
				if( y < this.Y ) {
					return this.BotLeft?.Get( x, y ) ?? null;
				} else {
					return this.BotRight?.Get( x, y ) ?? null;
				}
			}
		}

		////

		public void Set( int x, int y, T val ) {
			this.Count++;

			if( this.X == x && this.Y == y ) {
				this.Value = val;
				return;
			}

			int diffX, diffY, leftX, rightX, upY, downY;

			if( this.Parent != null ) {
				diffX = Math.Abs( ( this.X - this.Parent.X ) / 2 );
				diffY = Math.Abs( ( this.Y - this.Parent.Y ) / 2 );
			} else {
				diffX = this.X / 2;
				diffY = this.Y / 2;
			}

			if( diffX == 0 && diffY == 0 ) {
				throw new Exception( "I suck at algorithms, apparently..." );
			}

			leftX = this.X - diffX;
			rightX = this.X + diffX;
			upY = this.Y - diffY;
			downY = this.Y + diffY;

			if( x < this.X ) {
				if( y < this.Y ) {
					if( this.TopLeft == null ) {
						this.TopLeft = new QuadTree<T>( this, leftX, upY );
					}
					this.TopLeft.Set( x, y, val );
				} else {
					if( this.TopRight == null ) {
						this.TopRight = new QuadTree<T>( this, rightX, upY );
					}
					this.TopRight.Set( x, y, val );
				}
			} else {
				if( y < this.Y ) {
					if( this.BotLeft == null ) {
						this.BotLeft = new QuadTree<T>( this, leftX, downY );
					}
					this.BotLeft.Set( x, y, val );
				} else {
					if( this.BotRight == null ) {
						this.BotRight = new QuadTree<T>( this, rightX, downY );
					}
					this.BotRight.Set( x, y, val );
				}
			}
		}
	}
}
