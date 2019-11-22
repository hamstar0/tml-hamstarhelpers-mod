using System;
using Terraria.ModLoader.Config;


namespace HamstarHelpers.Services.Configs {
	/// <summary>
	/// Helps implement mod config stacking behavior. Replace your ModConfig classes with this class.
	/// </summary>
	public abstract class StackableModConfig : ModConfig {
		/// @private
		public override void OnChanged() {
			ModConfigStack.Uncache( this.GetType() );
		}
	}
}
