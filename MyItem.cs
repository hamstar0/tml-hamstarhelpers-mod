using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Services.RecipeHack;
using HamstarHelpers.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace HamstarHelpers {
	/// @private
	class ModHelpersItem : GlobalItem {
		internal int FromRecipeIdx = -1;

		////////////////

		public override bool InstancePerEntity => true;



		////////////////

		public override GlobalItem Clone( Item item, Item itemClone ) {
			if( this.FromRecipeIdx >= 0 ) {
				RecipeHack.AwaitCraft( Main.LocalPlayer, this.FromRecipeIdx );
			}
			return base.Clone( item, itemClone );
		}

		public override void SetDefaults( Item item ) {
//DataStore.Add( DebugHelpers.GetCurrentContext()+"_"+item.whoAmI+":"+item.type+"_A", 1 );
			if( item.type == ItemID.Coal ) {
				var mymod = ModHelpersMod.Instance;

				if( mymod.Config.CoalAsTile ) {
					item.maxStack = 999;
					item.rare = 2;
					item.value = 1000;

					item.useStyle = 1;
					item.useTurn = true;
					item.useAnimation = 15;
					item.useTime = 10;
					item.autoReuse = true;

					item.consumable = true;
					item.createTile = ModContent.TileType<CoalTile>();
				}
			}

			base.SetDefaults( item );
//DataStore.Add( DebugHelpers.GetCurrentContext()+"_"+item.whoAmI+":"+item.type+"_B", 1 );
		}
	}
}
