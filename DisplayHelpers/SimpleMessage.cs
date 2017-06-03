using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;


namespace HamstarHelpers.DisplayHelpers {
	public static class SimpleMessage {
		public static int MessageDuration = 0;
		public static string Message = "";


		public static void PostMessage( string msg, int duration ) {
			SimpleMessage.MessageDuration = duration;
			SimpleMessage.Message = msg;
		}

		public static void UpdateMessage() { // Called from an Update function
			if( SimpleMessage.MessageDuration <= 0 ) { return; }
			SimpleMessage.MessageDuration--;
		}

		public static void DrawMessage( SpriteBatch sb ) { // Called from a Draw function
			float scale = 2f;
			Vector2 pos = new Vector2( Main.screenWidth / 2f, Main.screenHeight / 2f );
			Vector2 size = Main.fontItemStack.MeasureString( SimpleMessage.Message );

			pos.X -= (size.X * scale) / 2f;
			pos.Y -= (size.Y * scale) * 2f;

			sb.DrawString( Main.fontItemStack, SimpleMessage.Message, pos, Color.White, 0f, new Vector2(), scale, SpriteEffects.None, 1f );
		}
	}
}
