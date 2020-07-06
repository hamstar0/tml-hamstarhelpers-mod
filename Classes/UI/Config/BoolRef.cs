using System;
using Terraria.ModLoader.Config;


namespace HamstarHelpers.Classes.UI.Config {
	/// <summary>
	/// Implements a ModConfig wrapper for a bool to allow nullable behavior.
	/// </summary>
	[NullAllowed]
	public class BoolRef {
		/// <summary></summary>
		public bool Value { get; set; }



		////

		/// @private
		public BoolRef() { }

		/// @private
		public BoolRef( bool value ) {
			this.Value = value;
		}
	}
}
