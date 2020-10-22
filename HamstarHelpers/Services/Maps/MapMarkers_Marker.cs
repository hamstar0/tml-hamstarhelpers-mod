using System;
using Microsoft.Xna.Framework.Graphics;


namespace HamstarHelpers.Services.Maps {
	/// <summary></summary>
	public class MapMarker {
		/// <summary></summary>
		public string Label { get; private set; }

		/// <summary></summary>
		public Texture2D Icon { get; private set; }



		////////////////

		/// <summary></summary>
		public MapMarker( string label, Texture2D icon ) {
			this.Label = label;
			this.Icon = icon;
		}

		/// <summary></summary>
		/// <returns></returns>
		public override int GetHashCode() {
			return this.Label.GetHashCode() + this.Icon.GetHashCode();
		}

		/// <summary></summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals( object obj ) {
			if( obj == null ) {
				return false;
			}
			if( obj.GetType() != typeof(MapMarker) ) {
				return false;
			}
			return obj.GetHashCode() == this.GetHashCode();
		}
	}
}
