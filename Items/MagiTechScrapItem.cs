using HamstarHelpers.Helpers.ItemHelpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Items {
	public class MagiTechScrapItem : ModItem {
		public static int Create( Vector2 pos, int stack ) {
			return ItemHelpers.CreateItem( pos, ModHelpersMod.Instance.ItemType<MagiTechScrapItem>(), stack, 24, 24 );
		}



		////////////////

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

			this.item.material = true;

			//this.item.consumable = true;
			//this.item.createTile = this.mod.TileType<MagiTechScrapTile>();
		}


		public override void AddRecipes() {
			/*var mymod = (ModHelpersMod)this.mod;

			var recipe = new ModRecipe( mymod );
			recipe.AddTile( TileID.WorkBenches );
			recipe.AddIngredient( this.mod.ItemType<MagiTechScrapWallItem>(), 4 );
			recipe.SetResult( this.mod.ItemType<MagiTechScrapItem>(), 1 );
			recipe.AddRecipe();*/

			/*var recipe1 = new MagiTechScrapRecipe1( mymod );
			var recipe2 = new MagiTechScrapRecipe2( mymod );
			recipe1.AddRecipe();
			recipe2.AddRecipe();*/
		}
	}



	
	/*class MagiTechScrapRecipe1 : ModRecipe {
		public MagiTechScrapRecipe1( ModHelpersMod mymod ) : base( mymod ) {
			this.AddTile( TileID.WorkBenches );
			this.AddIngredient( ItemID.Cog, 10 );
			this.AddIngredient( ItemID.Wire, 10 );
			this.AddIngredient( ItemID.Ectoplasm, 3 );
			this.SetResult( this.mod.ItemType<MagiTechScrapItem>(), 1 );
		}


		public override bool RecipeAvailable() {
			return ((ModHelpersMod)this.mod).Config.MagiTechScrapDropsEnabled;
		}
	}


	class MagiTechScrapRecipe2 : ModRecipe {
		public MagiTechScrapRecipe2( ModHelpersMod mymod ) : base( mymod ) {
			this.AddTile( TileID.WorkBenches );
			this.AddIngredient( ItemID.MartianConduitPlating, 10 );
			this.SetResult( this.mod.ItemType<MagiTechScrapItem>(), 1 );
		}


		public override bool RecipeAvailable() {
			return ((ModHelpersMod)this.mod).Config.MagiTechScrapDropsEnabled;
		}
	}*/
}
