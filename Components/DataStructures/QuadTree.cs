using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using Microsoft.Xna.Framework;
using System;
using System.Collections;
using Terraria;


namespace HamstarHelpers.Components.DataStructures {
	public partial class QuadTree<T> : IEnumerable where T : class {
		public int X { get; private set; }
		public int Y { get; private set; }

		private QuadTree<T> Parent;
		private QuadTree<T> TopLeftQuad;
		private QuadTree<T> TopRightQuad;
		private QuadTree<T> BotLeftQuad;
		private QuadTree<T> BotRightQuad;

		public T Value;

		////////////////

		public bool IsLeaf => this.TopLeftQuad == null
			&& this.TopRightQuad == null
			&& this.BotLeftQuad == null
			&& this.BotRightQuad == null;

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

		public void Clear( bool deep=false ) {
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

		////

		public Rectangle GetRectangle() {
			if( this.Parent == null ) {
				return new Rectangle( 0, 0, this.X * 2, this.Y * 2 );
			}

			int width = Math.Abs( this.Parent.X - this.X ) * 2;
			int height = Math.Abs( this.Parent.Y - this.Y ) * 2;
			int x = this.X - ( width / 2 );
			int y = this.Y - ( height / 2 );
			x = Utils.Clamp<int>( x, 0, this.Parent.X * 2 );
			y = Utils.Clamp<int>( y, 0, this.Parent.Y * 2 );

			return new Rectangle( x, y, width, height );
		}
	}
}
