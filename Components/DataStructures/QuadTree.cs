using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;


namespace HamstarHelpers.Components.DataStructures {
	public partial class QuadTree<T> where T : class {
		public int X { get; private set; }
		public int Y { get; private set; }

		private QuadTree<T> Parent;
		private QuadTree<T> TopLeftQuad;
		private QuadTree<T> TopRightQuad;
		private QuadTree<T> BotLeftQuad;
		private QuadTree<T> BotRightQuad;

		public T Value;

		public int Count { get; private set; }



		////////////////

		public QuadTree( int x, int y ) : this( null, x, y ) {
			if( x <= 0 || y <= 0 ) {
				throw new ArgumentException( "Positive integer coordinates required." );
			}
		}

		private QuadTree( QuadTree<T> parent, int x, int y ) {
			this.Parent = parent;
			this.X = x;
			this.Y = y;
			this.Count = 0;
		}


		////////////////

		public void Clear() {
			this.TopLeftQuad = null;
			this.TopRightQuad = null;
			this.BotLeftQuad = null;
			this.BotRightQuad = null;
			this.Value = null;
		}

		////////////////

		public T Get( int x, int y ) {
			if( this.X == x && this.Y == y ) {
				return this.Value;
			}

			if( x < this.X ) {
				if( y < this.Y ) {
					return this.TopLeftQuad?.Get( x, y ) ?? null;
				} else {
					return this.TopRightQuad?.Get( x, y ) ?? null;
				}
			} else {
				if( y < this.Y ) {
					return this.BotLeftQuad?.Get( x, y ) ?? null;
				} else {
					return this.BotRightQuad?.Get( x, y ) ?? null;
				}
			}
		}

		public IEnumerable<Tuple<int, int, T>> GetAll() {
			var list = new List<Tuple<int, int, T>>();
			var enumer = (IEnumerable<Tuple<int, int, T>>)list;

			if( this.Value != null ) {
				list.Add( Tuple.Create( this.X, this.Y, this.Value ) );
			}
			if( this.TopLeftQuad != null ) {
				enumer = list.Concat( this.TopLeftQuad.GetAll() );
			}
			if( this.TopRightQuad != null ) {
				enumer = list.Concat( this.TopRightQuad.GetAll() );
			}
			if( this.BotLeftQuad != null ) {
				enumer = list.Concat( this.BotLeftQuad.GetAll() );
			}
			if( this.BotRightQuad != null ) {
				enumer = list.Concat( this.BotRightQuad.GetAll() );
			}

			return enumer;
		}


		////////////////

		public void Set( int x, int y, T val ) {
			if( this.Parent == null ) {
				if( x > ( this.X * 2 ) || y > ( this.Y * 2 ) ) {
					throw new ArgumentException( "Coordinate values will not fit inside this tree's space." );
				}
			}

			this.Count++;
			
			if( this.X == x && this.Y == y ) {
				this.Value = val;
				return;
			}

			this.SetQuadAt( x, y, val );    //GIVE ME MY BRAIN BACK!
		}
		
		private void SetQuadAt( int x, int y, T val ) {
			int quadX, quadY;
			this.GetQuadCoords( x, y, out quadX, out quadY );

			if( x < this.X ) {
				if( y < this.Y ) {
					if( this.TopLeftQuad == null ) {
						this.TopLeftQuad = new QuadTree<T>( this, quadX, quadY );
					}
					this.TopLeftQuad.Set( x, y, val );
				} else {
					if( this.BotLeftQuad == null ) {
						this.BotLeftQuad = new QuadTree<T>( this, quadX, quadY );
					}
					this.BotLeftQuad.Set( x, y, val );
				}
			} else {
				if( y < this.Y ) {
					if( this.TopRightQuad == null ) {
						this.TopRightQuad = new QuadTree<T>( this, quadX, quadY );
					}
					this.TopRightQuad.Set( x, y, val );
				} else {
					if( this.BotRightQuad == null ) {
						this.BotRightQuad = new QuadTree<T>( this, quadX, quadY );
					}
					this.BotRightQuad.Set( x, y, val );
				}
			}
		}

		private void GetQuadCoords( int x, int y, out int quadX, out int quadY ) {
			int quadWidth, quadHeight;

			if( this.Parent != null ) {
				quadWidth = Math.Abs( this.X - this.Parent.X ) / 2;
				quadHeight = Math.Abs( this.Y - this.Parent.Y ) / 2;
				//innerWidth = innerWidth == 1 ? 1 : innerWidth / 2;
				//innerHeight = innerHeight == 1 ? 1 : innerHeight / 2;
			} else {
				quadWidth = this.X / 2;
				quadHeight = this.Y / 2;
			}

//LogHelpers.Log( "[x:"+x+",y:"+y+"] x vs X("+this.X+"), y vs Y("+this.Y+")    quadWidth:" + quadWidth + ", quadHeight:" + quadHeight );
			if( quadWidth == 0 && quadHeight == 0 ) {
				string err = "X:" + this.X + " in " + ( this.Parent?.X ?? -1 ) + ", Y:" + this.Y + " in " + ( this.Parent?.Y ?? -1 );
//LogHelpers.Log( "      "+err);
				throw new Exception( "I suck at algorithms, apparently ("+err+")..." );
			}

			if( quadWidth > 1 ) {
				if( x < this.X ) {
					quadX = this.X - quadWidth;
				} else {
					quadX = this.X + quadWidth;
				}
			} else {
				quadX = x;
			}

			if( quadHeight > 1 ) {
				if( y < this.Y ) {
					quadY = this.Y - quadHeight;
				} else {
					quadY = this.Y + quadHeight;
				}
			} else {
				quadY = y;
			}
		}
	}
}
