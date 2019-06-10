using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.Debug;
using Microsoft.Xna.Framework;
using System;
using System.Collections;


namespace HamstarHelpers.Components.DataStructures.QuadTree {
	public partial class QuadTree<T> : IEnumerable where T : class {
		public static Rectangle GetQuadRect( int x, int y, QuadTree<T> parent ) {
			int myX, myY, myWid, myHei;
			int midWid = Math.Max( 1, parent.Width / 2 );
			int midHei = Math.Max( 1, parent.Height / 2 );
			int rightX = parent.X + midWid;
			int bottomY = parent.Y + midHei;

			if( x != parent.X && x != rightX ) {
				throw new HamstarException( "Invalid quad X at " + x + " (parent: " + parent.Rect.ToString() + ")" );
			}
			if( y != parent.Y && y != bottomY ) {
				throw new HamstarException( "Invalid quad Y at " + y + " (parent: " + parent.Rect.ToString() + ")" );
			}
			if( midWid == parent.Width && midHei == parent.Height ) {
				throw new HamstarException( "Invalid quad width/height " + midWid + ":" + midHei + " (" + parent.Rect.ToString() + ")" );
			}

			if( x == rightX ) {
				myX = rightX;
			} else {
				myX = parent.X;
			}
			if( y == bottomY ) {
				myY = bottomY;
			} else {
				myY = parent.Y;
			}
			myWid = midWid;
			myHei = midHei;

			return new Rectangle( myX, myY, myWid, myHei );
		}



		////////////////

		private Rectangle Rect;

		private QuadTree<T> Parent;
		private QuadTree<T> TopLeftQuad;
		private QuadTree<T> TopRightQuad;
		private QuadTree<T> BotLeftQuad;
		private QuadTree<T> BotRightQuad;

		public T Value;

		////////////////

		public int X => this.Rect.X;
		public int Y => this.Rect.Y;
		public int Width => this.Rect.Width;
		public int Height => this.Rect.Height;
		public int MidX => this.X + (this.Width / 2);
		public int MidY => this.Y + (this.Height / 2);

		public bool IsLeaf => this.TopLeftQuad == null
			&& this.TopRightQuad == null
			&& this.BotLeftQuad == null
			&& this.BotRightQuad == null;

		public int Count { get; private set; }



		////////////////

		public QuadTree( int width, int height ) {
			int newWidth = 1, newHeight = 1;

			do {
				newWidth *= 2;
			} while( width >= newWidth );
			do {
				newHeight *= 2;
			} while( height >= newHeight );

			this.Parent = null;
			this.Rect = new Rectangle( 0, 0, newWidth, newHeight );
			this.Count = 0;
		}


		private QuadTree( QuadTree<T> parent, int x, int y ) {
			this.Parent = parent;
			this.Rect = QuadTree<T>.GetQuadRect( x, y, parent );
			this.Count = 0;
//LogHelpers.Log( "   x:"+x+", y:"+y+", rect:"+this.Rect+", parent:"+parent.Rect );
		}


		////////////////

		public void Clear( bool deep = false ) {
			if( deep ) {
				this.TopLeftQuad?.Clear( true );
				this.TopRightQuad?.Clear( true );
				this.BotLeftQuad?.Clear( true );
				this.BotRightQuad?.Clear( true );
			}

			this.TopLeftQuad = null;
			this.TopRightQuad = null;
			this.BotLeftQuad = null;
			this.BotRightQuad = null;
			this.Value = null;
		}


		////////////////

		public Rectangle GetRectangle() {
			return this.Rect;
		}

		public IEnumerator GetEnumerator() {
			if( this.TopLeftQuad != null ) {
				yield return this.TopLeftQuad;
				foreach( var quad in this.TopLeftQuad ) {
					yield return quad;
				}
			}

			if( this.TopRightQuad != null ) {
				yield return this.TopRightQuad;
				foreach( var quad in this.TopRightQuad ) {
					yield return quad;
				}
			}

			if( this.BotLeftQuad != null ) {
				yield return this.BotLeftQuad;
				foreach( var quad in this.BotLeftQuad ) {
					yield return quad;
				}
			}

			if( this.BotRightQuad != null ) {
				yield return this.BotRightQuad;
				foreach( var quad in this.BotRightQuad ) {
					yield return quad;
				}
			}
		}


		////////////////

		public override string ToString() {
			return this.ToStringAtLevel(0);
		}

		internal string ToStringAtLevel( int level ) {
			string indent = new String( '\t', level );
			string valueStr = this.Value == null ?
				"null" :
				this.Value is string ?
					("\"" + this.Value + "\"") :
					this.Value.ToString();

			level++;
			string tlStr = this.TopLeftQuad?.ToStringAtLevel( level ) ?? "null";
			string trStr = this.TopRightQuad?.ToStringAtLevel( level ) ?? "null";
			string blStr = this.BotLeftQuad?.ToStringAtLevel( level ) ?? "null";
			string brStr = this.BotRightQuad?.ToStringAtLevel( level ) ?? "null";

			return "{\n"
				+ indent + "\tRect: "+this.Rect.ToString()+ ",\n"
				+ indent + "\tValue: "+valueStr+",\n"
				+ indent + "\tTL: " + tlStr + ",\n"
				+ indent + "\tTR: " + trStr + ",\n"
				+ indent + "\tBL: " + blStr + ",\n"
				+ indent + "\tBR: " + brStr + "\n"
				+ indent + "}";
		}
	}
}
