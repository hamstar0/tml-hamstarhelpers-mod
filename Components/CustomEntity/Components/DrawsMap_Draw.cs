using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.HudHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;


namespace HamstarHelpers.Components.CustomEntity.Components {
	public partial class DrawsOnMapEntityComponent : CustomEntityComponent {
		public static void DrawToMiniMap( SpriteBatch sb, Texture2D tex, Color color, Vector2 center, bool isZoomed, float scale ) {
			float myScale = ( isZoomed ? Main.mapMinimapScale : 1f ) * scale;

			int tileX = (int)center.X - (int)( (float)tex.Width * scale * 8 );
			int tileY = (int)center.Y - (int)( (float)tex.Height * scale * 8 );

			var mapRectOrigin = new Rectangle( tileX, tileY, tex.Width, tex.Height );
			var miniMapData = HudMapHelpers.GetMiniMapScreenPosition( mapRectOrigin );

			if( miniMapData.Item2 ) {
				sb.Draw( tex, miniMapData.Item1, null, color, 0f, default( Vector2 ), myScale, SpriteEffects.None, 1f );
			}
		}


		public static void DrawToOverlayMap( SpriteBatch sb, Texture2D tex, Color color, Vector2 center, bool isZoomed, float scale ) {
			float myScale = ( isZoomed ? Main.mapOverlayScale : 1f ) * scale;

			int tileX = (int)center.X - (int)( (float)tex.Width * scale * 8 );
			int tileY = (int)center.Y - (int)( (float)tex.Height * scale * 8 );

			var mapRectOrigin = new Rectangle( tileX, tileY, tex.Width, tex.Height );
			var overMapData = HudMapHelpers.GetOverlayMapScreenPosition( mapRectOrigin );

			if( overMapData.Item2 ) {
				sb.Draw( tex, overMapData.Item1, null, color, 0f, default( Vector2 ), myScale, SpriteEffects.None, 1f );
			}
		}


		public static void DrawToFullMap( SpriteBatch sb, Texture2D tex, Color color, Vector2 center, bool isZoomed, float scale ) {
			float myScale = ( isZoomed ? Main.mapFullscreenScale : 1f ) * scale;

			int tileX = (int)center.X - (int)( (float)tex.Width * scale * 8 );
			int tileY = (int)center.Y - (int)( (float)tex.Height * scale * 8 );

			var mapRectOrigin = new Rectangle( tileX, tileY, tex.Width, tex.Height );
			var overMapData = HudMapHelpers.GetFullMapScreenPosition( mapRectOrigin );

			if( overMapData.Item2 ) {
				sb.Draw( tex, overMapData.Item1, null, color, 0f, default( Vector2 ), myScale, SpriteEffects.None, 1f );
			}
		}
	}
}
