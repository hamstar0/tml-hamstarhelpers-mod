using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace HamstarHelpers.Tiles {
	public class CoalTile : ModTile {
		public override void SetDefaults() {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			//Main.tileBlockLight[Type] = true;
			//Main.tileLighted[Type] = true;
			this.dustType = DustID.Granite;
			this.drop = ItemID.Coal;
			AddMapEntry( new Color( 64, 48, 64 ) );
		}

		public override void NumDust( int i, int j, bool fail, ref int num ) {
			num = fail ? 1 : 3;
		}
	}
}

