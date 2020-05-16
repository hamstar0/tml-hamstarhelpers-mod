using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Tiles;


namespace HamstarHelpers.Items {
	/// <summary>
	/// Places a cursed bramble tile. Traps players, hard to remove, slowly decays.
	/// </summary>
	public class CursedBrambleItem : ModItem {
		/// <summary></summary>
		public const int Width = 20;
		/// <summary></summary>
		public const int Height = 20;


		////////////////

		/// @private
		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "Cursed Bramble" );
			this.Tooltip.SetDefault( "Ouch." );
		}

		/// @private
		public override void SetDefaults() {
			this.item.width = CursedBrambleItem.Width;
			this.item.height = CursedBrambleItem.Height;
			this.item.value = Item.buyPrice( 0, 0, 1, 0 );
			this.item.maxStack = 999;
			this.item.useTurn = true;
			this.item.autoReuse = true;
			this.item.useAnimation = 15;
			this.item.useTime = 10;
			this.item.useStyle = ItemUseStyleID.SwingThrow;
			this.item.consumable = true;
			this.item.createTile = ModContent.TileType<CursedBrambleTile>();
		}
	}
}
