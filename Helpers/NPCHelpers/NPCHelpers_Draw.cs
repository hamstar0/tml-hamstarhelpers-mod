using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;


namespace HamstarHelpers.Helpers.NPCHelpers {
	public static partial class NPCHelpers {
		public static void DrawSimple( SpriteBatch sb, NPC npc, int frame, Vector2 position, float rotation, float scale, Color color ) {
			Texture2D tex = Main.npcTexture[ npc.type ];
			int frame_count = Main.npcFrameCount[ npc.type ];
			int tex_height = tex.Height / frame_count;

			Rectangle frame_rect = new Rectangle( 0, frame * tex_height, tex.Width, tex_height );

			float y_offset = 0.0f;
			float height_offset = Main.NPCAddHeight( npc.whoAmI );
			Vector2 origin = new Vector2( (float)(tex.Width / 2), (float)((tex.Height / frame_count) / 2) );

			if( npc.type == 108 || npc.type == 124 ) {
				y_offset = 2f;
			} else if( npc.type == 357 ) {
				y_offset = npc.localAI[0];
			} else if( npc.type == 467 ) {
				y_offset = 7f;
			} else if( npc.type == 537 ) {
				y_offset = 2f;
			} else if( npc.type == 509 ) {
				y_offset = -6f;
			} else if( npc.type == 490 ) {
				y_offset = 4f;
			} else if( npc.type == 484 ) {
				y_offset = 2f;
			} else if( npc.type == 483 ) {
				y_offset = 14f;
			} else if( npc.type == 477 ) {
				height_offset = 22f;
			} else if( npc.type == 478 ) {
				y_offset -= 2f;
			} else if( npc.type == 469 && (double)npc.ai[2] == 1.0 ) {
				y_offset = 14f;
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
				height_offset += 26f;
			} else if( npc.type == 264 ) {
				origin.Y = 21f;
				height_offset += 2f;
			} else if( npc.type == 266 ) {
				height_offset += 50f;
			} else if( npc.type == 268 ) {
				height_offset += 16f;
			} else if( npc.type == 288 ) {
				height_offset += 6f;
			}
			
			//if( npc.aiStyle == 10 || npc.type == 72 )
			//	color1 = Color.White;

			SpriteEffects fx = SpriteEffects.None;
			if( npc.spriteDirection == 1 ) {
				fx = SpriteEffects.FlipHorizontally;
			}

			float y_off = height_offset + y_offset + npc.gfxOffY + 4f;
			float x = position.X + ((float)npc.width / 2f) - (((float)tex.Width * scale) / 2f) + (origin.X * scale);
			float y = position.Y + (float)npc.height - ((float)tex_height * scale) + (origin.Y * scale) + y_off;
			Vector2 pos = UIHelpers.UIHelpers.ConvertToScreenPosition( new Vector2( x, y ) );
			
			sb.Draw( tex, pos, frame_rect, color, rotation, origin, scale, fx, 1f );
		}
	}
}
