using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Helpers.UI;


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
		public static void DrawSimple(
					SpriteBatch sb,
					NPC npc,
					int frame,
					Vector2 position,
					float rotation,
					float scale,
					Color color,
					bool applyZoom = false ) {
			Texture2D tex = Main.npcTexture[npc.type];
			int frameCount = Main.npcFrameCount[npc.type];
			int texHeight = tex.Height / frameCount;

			Rectangle frameRect = new Rectangle( 0, frame * texHeight, tex.Width, texHeight );

			float yOffset = 0.0f;
			float heightOffset = Main.NPCAddHeight( npc.whoAmI );
			Vector2 origin = new Vector2( (float)( tex.Width / 2 ), (float)( ( tex.Height / frameCount ) / 2 ) );

			if( npc.type == NPCID.Wizard || npc.type == NPCID.Mechanic ) {
				yOffset = 2f;
			} else if( npc.type == NPCID.Worm ) {
				yOffset = npc.localAI[0];
			} else if( npc.type == NPCID.DeadlySphere ) {
				yOffset = 7f;
			} else if( npc.type == NPCID.SandSlime ) {
				yOffset = 2f;
			} else if( npc.type == NPCID.FlyingAntlion ) {
				yOffset = -6f;
			} else if( npc.type == NPCID.Drippler ) {
				yOffset = 4f;
			} else if( npc.type == NPCID.EnchantedNightcrawler ) {
				yOffset = 2f;
			} else if( npc.type == NPCID.GraniteFlyer ) {
				yOffset = 14f;
			} else if( npc.type == NPCID.Mothron ) {
				heightOffset = 22f;
			} else if( npc.type == NPCID.MothronEgg ) {
				yOffset -= 2f;
			} else if( npc.type == NPCID.ThePossessed && (double)npc.ai[2] == 1.0 ) {
				yOffset = 14f;
			} else if( npc.type == NPCID.EyeofCthulhu ) {
				origin = new Vector2( 55f, 107f );
			} else if( npc.type == NPCID.Retinazer ) {
				origin = new Vector2( 55f, 107f );
			} else if( npc.type == NPCID.Spazmatism ) {
				origin = new Vector2( 55f, 107f );
			} else if( npc.type == NPCID.BlueJellyfish || npc.type == NPCID.PinkJellyfish || npc.type == NPCID.GreenJellyfish ) {
				origin.Y += 4f;
			} else if( npc.type == NPCID.Antlion ) {
				origin.Y += 8f;
			} else if( npc.type == NPCID.Plantera ) {
				origin.Y = 77f;
				heightOffset += 26f;
			} else if( npc.type == NPCID.PlanterasTentacle ) {
				origin.Y = 21f;
				heightOffset += 2f;
			} else if( npc.type == NPCID.BrainofCthulhu ) {
				heightOffset += 50f;
			} else if( npc.type == NPCID.IchorSticker ) {
				heightOffset += 16f;
			} else if( npc.type == NPCID.DungeonSpirit ) {
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
