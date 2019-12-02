using Microsoft.Xna.Framework.Graphics;
using Terraria;


namespace HamstarHelpers.Helpers.Tiles.Walls {
	/// <summary>
	/// Assorted static "helper" functions pertaining to players relative to tile Wall resources (e.g. textures).
	/// </summary>
	public partial class TileWallResourceHelpers {
		/// <summary>
		/// Gets a wall's texture. Loads walls as needed.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static Texture2D SafelyGetTexture( int type ) {
			Main.instance.LoadWall( type );
			return Main.wallTexture[type];
		}
	}
}
