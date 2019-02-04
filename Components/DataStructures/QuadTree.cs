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
	}
}
