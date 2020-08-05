using HamstarHelpers.Helpers.Draw;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;


namespace HamstarHelpers.Helpers.HUD {
	/// <summary>
	/// Assorted static "helper" functions pertaining to general HUD. 
	/// </summary>
	public class HUDHelpers {
		/// @private
		[Obsolete("use DrawHelpers", true)]
		public static void DrawBorderedRect( SpriteBatch sb, Color bgColor, Color borderColor, Vector2 position, Vector2 size, int borderWidth ) {
			DrawHelpers.DrawBorderedRect( sb, bgColor, borderColor, position, size, borderWidth );
		}
		/// @private
		[Obsolete( "use DrawHelpers", true )]
		public static void DrawBorderedRect( SpriteBatch sb, Color bgColor, Color borderColor, Rectangle rect, int borderWidth ) {
			DrawHelpers.DrawBorderedRect( sb, bgColor, borderColor, rect, borderWidth );
		}

		/// @private
		[Obsolete( "use DrawHelpers", true )]
		public static void DrawBorderedRect( SpriteBatch sb, Color? bgColor, Color? borderColor, Vector2 position, Vector2 size, int borderWidth ) {
			DrawHelpers.DrawBorderedRect( sb, bgColor, borderColor, position, size, borderWidth );
		}
		/// @private
		[Obsolete( "use DrawHelpers", true )]
		public static void DrawBorderedRect( SpriteBatch sb, Color? bgColor, Color? borderColor, Rectangle rect, int borderWidth ) {
			DrawHelpers.DrawBorderedRect( sb, bgColor, borderColor, rect, borderWidth );
		}
		/// @private
		[Obsolete( "use DrawHelpers", true )]
		public static void DrawTerrariaString( string text, Vector2 pos, float scale ) {
			DrawHelpers.DrawTerrariaString( Main.spriteBatch, text, pos, scale );
		}
		/// @private
		[Obsolete( "use DrawHelpers", true )]
		public static void DrawTerrariaString( SpriteBatch sb, string text, Vector2 pos, float scale ) {
			DrawHelpers.DrawTerrariaString( sb, text, pos, scale );
		}
	}
}
