using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Services.RecipeHack;
using HamstarHelpers.Tiles;


namespace HamstarHelpers {
	/// @private
	class ModHelpersItem : GlobalItem {
		internal int FromRecipeIdx = -1;

		////////////////

		public override bool InstancePerEntity => true;



		////////////////

		public override GlobalItem Clone( Item item, Item itemClone ) {
			if( this.FromRecipeIdx >= 0 ) {
#pragma warning disable CS0618 // Type or member is obsolete
				RecipeHack.AwaitCraft( Main.LocalPlayer, this.FromRecipeIdx );
#pragma warning restore CS0618 // Type or member is obsolete
			}
			return base.Clone( item, itemClone );
		}

		public override void SetDefaults( Item item ) {
//DataStore.Add( DebugHelpers.GetCurrentContext()+"_"+item.whoAmI+":"+item.type+"_A", 1 );
			if( item.type == ItemID.Coal ) {
				if( ModHelpersConfig.Instance.CoalAsTile ) {
					item.maxStack = 999;
					item.rare = ItemRarityID.Green;
					item.value = 1000;

					item.useStyle = ItemUseStyleID.SwingThrow;
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
