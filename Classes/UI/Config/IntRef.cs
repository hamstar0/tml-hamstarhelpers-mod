using System;
using Terraria.ModLoader.Config;


namespace HamstarHelpers.Classes.UI.Config {
	/// <summary>
	/// Implements a ModConfig wrapper for a int to allow nullable behavior.
	/// </summary>
	[NullAllowed]
	public class IntRef {
		/// <summary></summary>
		public int Value { get; set; }



		////

		/// @private
		public IntRef() { }

		/// @private
		public IntRef( int value ) {
			this.Value = value;
		}
	}
}
