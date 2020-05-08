using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Tiles;


namespace HamstarHelpers.Tiles {
	/// <summary>
	/// Represents a tile that works like a standard corruption/crimson/jungle bramble, but cannot be removed by melee weapons,
	/// and may support additional custom behavior.
	/// </summary>
	public partial class CursedBrambleTile : ModTile {
		/// @private
		public override void SetDefaults() {
			//Main.tileSolid[this.Type] = true;
			//Main.tileMergeDirt[this.Type] = true;
			//Main.tileBlockLight[this.Type] = true;
			//Main.tileLighted[this.Type] = true;
			Main.tileNoAttach[this.Type] = true;
			Main.tileLavaDeath[this.Type] = false;
			this.dustType = DustID.Granite;
			this.AddMapEntry( new Color(128, 64, 128) );
		}

		/// @private
		public override void NumDust( int i, int j, bool fail, ref int num ) {
			num = fail ? 1 : 3;
		}


		/// @private
		public override void RandomUpdate( int i, int j ) {
			TileHelpers.KillTileSynced( i, j, false, false );
		}
	}
}
