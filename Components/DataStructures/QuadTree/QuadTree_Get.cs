using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;


namespace HamstarHelpers.Components.DataStructures.QuadTree {
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

			if( this.Value != null ) {
				list.Add( Tuple.Create( this.X, this.Y, this.Value ) );
			}
			
			if( this.TopLeftQuad != null ) {
				foreach( var item in this.TopLeftQuad.GetAll() ) {
					list.Add( item );
				}
			}
			if( this.TopRightQuad != null ) {
				foreach( var item in this.TopRightQuad.GetAll() ) {
					list.Add( item );
				}
			}
			if( this.BotLeftQuad != null ) {
				foreach( var item in this.BotLeftQuad.GetAll() ) {
					list.Add( item );
				}
			}
			if( this.BotRightQuad != null ) {
				foreach( var item in this.BotRightQuad.GetAll() ) {
					list.Add( item );
				}
			}

			return list.AsEnumerable();
		}
	}
}
