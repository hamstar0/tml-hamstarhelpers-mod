using HamstarHelpers.Services.RecipeHack;
using System;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers {
	class ModHelpersRecipe : GlobalRecipe {
		public override void OnCraft( Item item, Recipe recipe ) {
			RecipeHack.ConfirmCraft( Main.LocalPlayer, recipe );
		}
	}
}
