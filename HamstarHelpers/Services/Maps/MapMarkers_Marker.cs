using System;
using Microsoft.Xna.Framework.Graphics;


namespace HamstarHelpers.Services.Maps {
	/// <summary></summary>
	public class MapMarker {
		/// <summary></summary>
		public string ID { get; private set; }

		/// <summary></summary>
		public Texture2D Icon { get; private set; }

		/// <summary></summary>
		public float Scale { get; private set; }


		////

		/// @private
		[Obsolete( "use ID", true )]
		public string Label {
			get => this.ID;
			private set => this.ID = value;
		}



		////////////////

		/// <summary></summary>
		public MapMarker( string id, Texture2D icon, float scale ) {
			this.ID = id;
			this.Icon = icon;
			this.Scale = scale;
		}

		////

		/// <summary></summary>
		/// <returns></returns>
		public override int GetHashCode() {
			return this.ID.GetHashCode() + this.Icon.GetHashCode();
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
