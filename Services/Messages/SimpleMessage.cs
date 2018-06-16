using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;


namespace HamstarHelpers.Services.Messages {
	public static class SimpleMessage {
		public static int MessageDuration = 0;
		public static string Message = "";
		public static string SubMessage = "";



		////////////////

		public static void PostMessage( string msg, string submsg, int duration ) {
			SimpleMessage.MessageDuration = duration;
			SimpleMessage.Message = msg;
			SimpleMessage.SubMessage = submsg;
		}


		////////////////

		internal static void UpdateMessage() { // Called from an Update function
			if( SimpleMessage.MessageDuration > 0 ) {
				SimpleMessage.MessageDuration--;
			}
		}

		internal static void DrawMessage( SpriteBatch sb ) { // Called from a Draw function
			if( SimpleMessage.MessageDuration == 0 ) { return; }

			var pos = new Vector2( Main.screenWidth * 0.5f, Main.screenHeight * 0.5f );
			var size = SimpleMessage.DrawMessageText( sb, SimpleMessage.Message, 2f, pos );

			if( SimpleMessage.SubMessage != "" ) {
				var subpos = pos;
				subpos.Y += size.Y * 2;

				SimpleMessage.DrawMessageText( sb, SimpleMessage.SubMessage, 1f, subpos );
			}
		}


		////////////////

		private static Vector2 DrawMessageText( SpriteBatch sb, string msg, float scale, Vector2 pos ) {
			Vector2 size = Main.fontItemStack.MeasureString( msg );

			size *= scale;
			pos -= size / 2f;

			sb.DrawString( Main.fontItemStack, msg, pos, Color.White, 0f, new Vector2(0,0), scale, SpriteEffects.None, 1f );

			return size;
		}
	}
}
