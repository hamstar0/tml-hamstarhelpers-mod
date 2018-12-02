using HamstarHelpers.Items;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Walls {
	public class MagiTechScrapWall : ModWall {
		public override void SetDefaults() {
			Main.wallHouse[this.Type] = true;
			this.dustType = 1;
			this.drop = this.mod.ItemType<MagiTechScrapWallItem>();
			this.AddMapEntry( new Color( 150, 190, 174 ) );
		}

		public override void NumDust( int i, int j, bool fail, ref int num ) {
			num = fail ? 1 : 3;
		}

		public override void ModifyLight( int i, int j, ref float r, ref float g, ref float b ) {
			r = 0.4f;
			g = 0.4f;
			b = 0.4f;
		}
	}
}
