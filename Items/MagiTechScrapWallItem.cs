using HamstarHelpers.Walls;
using Terraria.ID;
using Terraria.ModLoader;


namespace HamstarHelpers.Items {
	public class MagiTechScrapWallItem : ModItem {
		public override void SetStaticDefaults() {
			var mymod = (ModHelpersMod)this.mod;

			this.DisplayName.SetDefault( "Magi-Tech Scrap Wall" );
		}


		public override void SetDefaults() {
			this.item.width = 12;
			this.item.height = 12;
			this.item.maxStack = 999;

			this.item.useTurn = true;
			this.item.autoReuse = true;
			this.item.useAnimation = 15;
			this.item.useTime = 7;
			this.item.useStyle = 1;

			this.item.consumable = true;
			this.item.createWall = this.mod.WallType<MagiTechScrapWall>();
		}


		public override void AddRecipes() {
			var recipe = new ModRecipe( this.mod );
			recipe.AddTile( TileID.WorkBenches );
			recipe.AddIngredient( this.mod.ItemType<MagiTechScrapWallItem>(), 4 );
			recipe.SetResult( this.mod.ItemType<MagiTechScrapItem>(), 1 );
			recipe.AddRecipe();
		}
	}
}
