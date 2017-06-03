using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;


namespace HamstarHelpers.DisplayHelpers {
	public static class SimpleMessage {
		public static int MessageDuration = 0;
		public static string Message = "";
		public static string SubMessage = "";



		public static void PostMessage( string msg, string submsg, int duration ) {
			SimpleMessage.MessageDuration = duration;
			SimpleMessage.Message = msg;
			SimpleMessage.SubMessage = submsg;
		}


		public static void UpdateMessage() { // Called from an Update function
			if( SimpleMessage.MessageDuration <= 0 ) { return; }
			SimpleMessage.MessageDuration--;
		}

		public static void DrawMessage( SpriteBatch sb ) { // Called from a Draw function
			if( SimpleMessage.MessageDuration == 0 ) { return; }

			var pos = new Vector2( Main.screenWidth / 2f, Main.screenHeight / 2f );
			var size = SimpleMessage.DrawMessageText( sb, SimpleMessage.Message, 2f, pos );

			if( SimpleMessage.SubMessage != "" ) {
				var subpos = new Vector2( Main.screenWidth / 2f, (Main.screenHeight / 2f) + size.Y );
				SimpleMessage.DrawMessageText( sb, SimpleMessage.SubMessage, 1f, subpos );
			}
		}


		private static Vector2 DrawMessageText( SpriteBatch sb, string msg, float scale, Vector2 pos ) {
			Vector2 size = Main.fontItemStack.MeasureString( msg );
			size.X *= scale;
			size.Y *= scale;

			pos.X -= size.X / 2f;
			pos.Y -= size.Y * 2f;

			sb.DrawString( Main.fontItemStack, msg, pos, Color.White, 0f, new Vector2(), scale, SpriteEffects.None, 1f );

			return size;
		}
	}
}
