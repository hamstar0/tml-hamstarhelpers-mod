using HamstarHelpers.Services.RecipeHack;
using System;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers {
	/// @private
	class ModHelpersRecipe : GlobalRecipe {
		public override void OnCraft( Item item, Recipe recipe ) {
#pragma warning disable CS0618 // Type or member is obsolete
			RecipeHack.ConfirmCraft( Main.LocalPlayer, recipe );
#pragma warning restore CS0618 // Type or member is obsolete
		}
	}
}
