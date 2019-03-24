using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;


namespace HamstarHelpers.Components.DataStructures.QuadTree {
	public partial class QuadTree<T> where T : class {
		public T Get( int x, int y ) {
			if( this.X == x && this.Y == y && this.Width == 1 && this.Height == 1 ) {
				return this.Value;
			}

			if( x < this.MidX ) {
				if( y < this.MidY ) {
					return this.TopLeftQuad?.Get( x, y ) ?? null;
				} else {
					return this.BotLeftQuad?.Get( x, y ) ?? null;
				}
			} else {
				if( y < this.MidY ) {
					return this.TopRightQuad?.Get( x, y ) ?? null;
				} else {
					return this.BotRightQuad?.Get( x, y ) ?? null;
				}
			}
		}

		public IEnumerable<Tuple<int, int, T>> GetAllValues() {
			var list = new List<Tuple<int, int, T>>();

			if( this.Value != null ) {
				list.Add( Tuple.Create( this.X, this.Y, this.Value ) );
			}
			
			if( this.TopLeftQuad != null ) {
				foreach( var item in this.TopLeftQuad.GetAllValues() ) {
					list.Add( item );
				}
			}
			if( this.TopRightQuad != null ) {
				foreach( var item in this.TopRightQuad.GetAllValues() ) {
					list.Add( item );
				}
			}
			if( this.BotLeftQuad != null ) {
				foreach( var item in this.BotLeftQuad.GetAllValues() ) {
					list.Add( item );
				}
			}
			if( this.BotRightQuad != null ) {
				foreach( var item in this.BotRightQuad.GetAllValues() ) {
					list.Add( item );
				}
			}

			return list.AsEnumerable();
		}
	}
}
