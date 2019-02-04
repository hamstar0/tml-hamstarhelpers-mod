using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;


namespace HamstarHelpers.Components.DataStructures {
	public partial class QuadTree<T> where T : class {
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
	}
}
