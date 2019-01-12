using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.HudHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;


namespace HamstarHelpers.Components.CustomEntity.Components {
	public partial class DrawsInGameEntityComponent : CustomEntityComponent {
		public static void DrawTexture( SpriteBatch sb, CustomEntity ent, Texture2D tex, int frameCount, Color color, float scale, float rotation=0f, Vector2 origin=default(Vector2) ) {
			var core = ent.Core;
			var worldScrRect = new Rectangle( (int)Main.screenPosition.X, (int)Main.screenPosition.Y, Main.screenWidth, Main.screenHeight );

			if( !core.Hitbox.Intersects( worldScrRect ) ) { return; }

			var scrScrPos = core.position - Main.screenPosition;
			var texRect = new Rectangle( 0, 0, tex.Width, tex.Height / frameCount );
			
			SpriteEffects dir = DrawsInGameEntityComponent.GetOrientation( core );

			sb.Draw( tex, scrScrPos, texRect, color, rotation, origin, scale, dir, 1f );

			if( ModHelpersMod.Instance.Config.DebugModeCustomEntityInfo ) {
				var rect = new Rectangle(
					(int)(core.position.X - Main.screenPosition.X - ((float)origin.X * scale)),
					(int)(core.position.Y - Main.screenPosition.Y - ((float)origin.Y * scale)),
					(int)((float)core.width * scale),
					(int)((float)core.height * scale)
				);
				HudHelpers.DrawBorderedRect( sb, null, Color.Red, rect, 1 );
			}
		}
		
		public static SpriteEffects GetOrientation( Entity ent ) {
			SpriteEffects dir = ent.direction > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			//dir |= ( Main.LocalPlayer.gravDir < 0 ) ? SpriteEffects.FlipVertically : SpriteEffects.None;
			return dir;
		}
	}
}
