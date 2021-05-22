using Microsoft.Xna.Framework.Graphics;
using Terraria;


namespace HamstarHelpers.Libraries.NPCs {
	/// <summary>
	/// Assorted static "helper" functions pertaining to players relative to NPC resources (e.g. textures).
	/// </summary>
	public partial class NPCResourceLibraries {
		/// <summary>
		/// Gets a NPC's texture. Loads NPC as needed.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static Texture2D SafelyGetTexture( int type ) {
			Main.instance.LoadNPC( type );
			return Main.npcTexture[type];
		}
	}
}
