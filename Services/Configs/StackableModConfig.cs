using System;
using Terraria.ModLoader.Config;


namespace HamstarHelpers.Services.Configs {
	/// <summary>
	/// Enables ModConfig for stackable use.
	/// </summary>
	public abstract class StackableModConfig : ModConfig {
		/// @private
		public override void OnChanged() {
			ModConfigStack.Uncache( this.GetType() );
		}
	}
}
