using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Services.NPCChat;
using HamstarHelpers.Services.AnimatedColor;
using ReLogic.Graphics;


namespace HamstarHelpers {
	/// @private
	partial class ModHelpersNPC : GlobalNPC {
		public override void PostDraw( NPC npc, SpriteBatch sb, Color drawColor ) {
			if( !npc.townNPC ) {
				return;
			}

			Func<string, string> hiChatFunc = NPCChat.GetPriorityChat( npc.type );
			if( hiChatFunc?.Invoke(null) != null ) {
				this.DrawAlertFlag( npc, sb );
			}
		}


		////

		private void DrawAlertFlag( NPC npc, SpriteBatch sb ) {
			Vector2 scrPos = npc.Center - Main.screenPosition;
			scrPos.X -= 4;
			scrPos.Y -= ( npc.height / 2 ) + 56;

			sb.DrawString(
				spriteFont: Main.fontMouseText,
				text: "!",
				position: scrPos,
				color: AnimatedColors.Alert.CurrentColor,
				rotation: 0f,
				origin: default( Vector2 ),
				scale: 2f,
				effects: SpriteEffects.None,
				layerDepth: 1f
			);
		}
	}
}
