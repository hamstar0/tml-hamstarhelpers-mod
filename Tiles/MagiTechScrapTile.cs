using HamstarHelpers.Items;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace HamstarHelpers.Tiles {
	public class MagiTechScrapTile : ModTile {
		public override void SetDefaults() {
			Main.tileSolid[this.Type] = true;
			Main.tileBlockLight[this.Type] = true;
			Main.tileLighted[this.Type] = true;
			Main.tileLargeFrames[this.Type] = 1;	//wallLargeFrames
			Main.tileBrick[this.Type] = true;

			this.dustType = 1;
			this.drop = this.mod.ItemType<MagiTechScrapItem>();
			this.AddMapEntry( new Color( 200, 240, 224 ) );
		}

		public override void NumDust( int i, int j, bool fail, ref int num ) {
			num = fail ? 1 : 3;
		}

		public override void ModifyLight( int i, int j, ref float r, ref float g, ref float b ) {
			r = 200f / 255f;
			g = 240f / 255f;
			b = 224f / 255f;
		}

		public override bool KillSound( int i, int j ) {
			Main.PlaySound( SoundID.Tink, new Vector2( i * 16, j * 16 ) );
			return false;
		}
	}
}
