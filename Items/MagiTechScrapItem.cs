using HamstarHelpers.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace HamstarHelpers.Items {
	public class MagiTechScrapItem : ModItem {
		public override void SetStaticDefaults() {
			var mymod = (ModHelpersMod)this.mod;

			this.DisplayName.SetDefault( "Magi-Tech Scrap" );
			this.Tooltip.SetDefault( "Magical technology. Or is it technological magic?" );
		}


		public override void SetDefaults() {
			this.item.maxStack = 999;
			this.item.width = 24;
			this.item.height = 24;
			this.item.value = Item.buyPrice( 0, 0, 10, 0 );
			this.item.rare = 1;

			this.item.useStyle = 1;
			this.item.useTurn = true;
			this.item.useAnimation = 15;
			this.item.useTime = 10;
			this.item.autoReuse = true;

			this.item.consumable = true;
			this.item.material = true;
			this.item.createTile = this.mod.TileType<MagiTechScrapTile>();
		}


		public override void AddRecipes() {
			var recipe = new ModRecipe( this.mod );
			recipe.AddTile( TileID.WorkBenches );
			recipe.AddIngredient( this.item.type, 1 );
			recipe.SetResult( this.mod.ItemType<MagiTechScrapWallItem>(), 4 );
			recipe.AddRecipe();
		}
	}
}
