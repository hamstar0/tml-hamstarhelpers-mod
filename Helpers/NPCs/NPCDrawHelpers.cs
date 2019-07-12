using HamstarHelpers.Helpers.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;


namespace HamstarHelpers.Helpers.NPCs {
	/// <summary>
	/// Assorted static "helper" functions pertaining to NPC drawing.
	/// </summary>
	public partial class NPCDrawHelpers {
		/// <summary>
		/// Draws an NPC.
		/// </summary>
		/// <param name="sb">SpriteBatch to draw to. Typically `Main.spriteBatch`.</param>
		/// <param name="npc">NPC to draw.</param>
		/// <param name="frame">Frame of the NPC's animation to draw.</param>
		/// <param name="position"></param>
		/// <param name="rotation"></param>
		/// <param name="scale"></param>
		/// <param name="color"></param>
		/// <param name="applyZoom">Whether to convert to current zoom amount or draw it directly.</param>
		public static void DrawSimple( SpriteBatch sb, NPC npc, int frame, Vector2 position, float rotation, float scale,
					Color color, bool applyZoom = false ) {
			Texture2D tex = Main.npcTexture[npc.type];
			int frameCount = Main.npcFrameCount[npc.type];
			int texHeight = tex.Height / frameCount;

			Rectangle frameRect = new Rectangle( 0, frame * texHeight, tex.Width, texHeight );

			float yOffset = 0.0f;
			float heightOffset = Main.NPCAddHeight( npc.whoAmI );
			Vector2 origin = new Vector2( (float)( tex.Width / 2 ), (float)( ( tex.Height / frameCount ) / 2 ) );

			if( npc.type == 108 || npc.type == 124 ) {
				yOffset = 2f;
			} else if( npc.type == 357 ) {
				yOffset = npc.localAI[0];
			} else if( npc.type == 467 ) {
				yOffset = 7f;
			} else if( npc.type == 537 ) {
				yOffset = 2f;
			} else if( npc.type == 509 ) {
				yOffset = -6f;
			} else if( npc.type == 490 ) {
				yOffset = 4f;
			} else if( npc.type == 484 ) {
				yOffset = 2f;
			} else if( npc.type == 483 ) {
				yOffset = 14f;
			} else if( npc.type == 477 ) {
				heightOffset = 22f;
			} else if( npc.type == 478 ) {
				yOffset -= 2f;
			} else if( npc.type == 469 && (double)npc.ai[2] == 1.0 ) {
				yOffset = 14f;
			} else if( npc.type == 4 ) {
				origin = new Vector2( 55f, 107f );
			} else if( npc.type == 125 ) {
				origin = new Vector2( 55f, 107f );
			} else if( npc.type == 126 ) {
				origin = new Vector2( 55f, 107f );
			} else if( npc.type == 63 || npc.type == 64 || npc.type == 103 ) {
				origin.Y += 4f;
			} else if( npc.type == 69 ) {
				origin.Y += 8f;
			} else if( npc.type == 262 ) {
				origin.Y = 77f;
				heightOffset += 26f;
			} else if( npc.type == 264 ) {
				origin.Y = 21f;
				heightOffset += 2f;
			} else if( npc.type == 266 ) {
				heightOffset += 50f;
			} else if( npc.type == 268 ) {
				heightOffset += 16f;
			} else if( npc.type == 288 ) {
				heightOffset += 6f;
			}

			//if( npc.aiStyle == 10 || npc.type == 72 )
			//	color1 = Color.White;

			SpriteEffects fx = SpriteEffects.None;
			if( npc.spriteDirection == 1 ) {
				fx = SpriteEffects.FlipHorizontally;
			}

			float yOff = heightOffset + yOffset + npc.gfxOffY + 4f;
			float x = position.X + ( (float)npc.width / 2f ) - ( ( (float)tex.Width * scale ) / 2f ) + ( origin.X * scale );
			float y = position.Y + (float)npc.height - ( (float)texHeight * scale ) + ( origin.Y * scale ) + yOff;

			Vector2 pos;
			if( applyZoom ) {
				pos = UIHelpers.ConvertToScreenPosition( new Vector2( x, y ) );
			} else {
				pos = new Vector2( x, y ) - Main.screenPosition;
			}

			sb.Draw( tex, pos, frameRect, color, rotation, origin, scale, fx, 1f );
		}
	}
}
