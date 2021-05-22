using Microsoft.Xna.Framework.Graphics;
using Terraria;


namespace HamstarHelpers.Libraries.Tiles {
	/// <summary>
	/// Assorted static "helper" functions pertaining to players relative to Tile resources (e.g. textures).
	/// </summary>
	public partial class TileResourceLibraries {
		/// <summary>
		/// Gets a tile's texture. Loads tiles as needed.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static Texture2D SafelyGetTexture( int type ) {
			Main.instance.LoadTiles( type );
			return Main.tileTexture[type];
		}
	}
}
