using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace HamstarHelpers.Items {
	/// <summary>
	/// Supplies an ingredient item meant to exist as the generic form of Orbs (e.g. Shadow Orbs). Another MacGuffin item.
	/// </summary>
	public class GeoResonantOrbItem : ModItem {
		/// @private
		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "Geo-Resonant Orb" );
			this.Tooltip.SetDefault( "A special magical conduit able to be tuned to world energies" );
		}


		/// @private
		public override void SetDefaults() {
			this.item.maxStack = 99;
			this.item.width = 16;
			this.item.height = 16;
			this.item.material = true;
			//this.item.UseSound = SoundID.Item108;
			this.item.value = Item.buyPrice( 0, 5, 0, 0 );
			this.item.rare = ItemRarityID.Green;
		}
	}
}