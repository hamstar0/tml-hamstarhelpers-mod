using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace HamstarHelpers.UIHelpers {
	public static partial class UIHelpers {
		[System.Obsolete( "use HudHelpers.DrawBorderedRect", true )]
		public static void DrawBorderedRect( SpriteBatch sb, Color color, Color border_color, Vector2 position, Vector2 size, int border_width ) {
			HudHelpers.HudHelpers.DrawBorderedRect( sb, color, border_color, position, size, border_width );
		}
	}
}
