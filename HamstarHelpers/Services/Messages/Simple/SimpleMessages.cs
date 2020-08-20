using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.UI;


namespace HamstarHelpers.Services.Messages.Simple {
	/// <summary>
	/// A simple alternative to quickly display text messages on-screen.
	/// </summary>
	public static class SimpleMessage {
		/// <summary>
		/// Tick duration of message.
		/// </summary>
		public static int MessageDuration = 0;

		/// <summary></summary>
		public static string Message = "";

		/// <summary>
		/// Smaller message under the bigger message.
		/// </summary>
		public static string SubMessage = "";


		/// <summary></summary>
		public static bool IsBordered = false;

		/// <summary></summary>
		public static Color Color = Color.White;

		
		/// <summary></summary>
		public static StyleDimension Left = new StyleDimension( 0f, 0.5f );

		/// <summary></summary>
		public static StyleDimension Top = new StyleDimension( 0f, 0.5f );



		////////////////

		/// <summary></summary>
		/// <param name="msg"></param>
		/// <param name="submsg"></param>
		/// <param name="duration">Tick duration.</param>
		public static void PostMessage( string msg, string submsg, int duration ) {
			SimpleMessage.MessageDuration = duration;
			SimpleMessage.Message = msg;
			SimpleMessage.SubMessage = submsg;
			SimpleMessage.SubMessage = submsg;
		}

		/// <summary></summary>
		/// <param name="msg"></param>
		/// <param name="submsg"></param>
		/// <param name="duration">Tick duration.</param>
		/// <param name="isBordered"></param>
		/// <param name="color"></param>
		/// <param name="left"></param>
		/// <param name="top"></param>
		public static void PostMessage(
					string msg,
					string submsg,
					int duration,
					bool isBordered,
					Color color,
					StyleDimension left,
					StyleDimension top ) {
			SimpleMessage.MessageDuration = duration;
			SimpleMessage.Message = msg;
			SimpleMessage.SubMessage = submsg;
			SimpleMessage.IsBordered = isBordered;
			SimpleMessage.Color = color;
			SimpleMessage.Left = left;
			SimpleMessage.Top = top;
		}


		////////////////

		internal static void UpdateMessage() { // Called from an Update function
			if( SimpleMessage.MessageDuration > 0 ) {
				SimpleMessage.MessageDuration--;
			}
		}

		internal static void DrawMessage( SpriteBatch sb ) { // Called from a Draw function
			if( SimpleMessage.MessageDuration == 0 ) { return; }

			string msg = SimpleMessage.Message;
			var pos = new Vector2(
				SimpleMessage.Left.GetValue( Main.screenWidth ),
				SimpleMessage.Top.GetValue( Main.screenHeight )
			);
			var size = SimpleMessage.DrawMessageText( sb, msg, pos, SimpleMessage.IsBordered, SimpleMessage.Color, 2f );

			string submsg = SimpleMessage.SubMessage;
			if( submsg != "" ) {
				var subpos = pos;
				subpos.Y += size.Y * 2;

				SimpleMessage.DrawMessageText( sb, submsg, subpos, SimpleMessage.IsBordered, SimpleMessage.Color * 0.8f, 1f );
			}
		}


		////////////////

		private static Vector2 DrawMessageText(
					SpriteBatch sb,
					string msg,
					Vector2 pos,
					bool isBordered,
					Color color,
					float scale ) {
			Vector2 size = Main.fontItemStack.MeasureString( msg );

			size *= scale;
			pos -= size / 2f;

			if( isBordered ) {
				Utils.DrawBorderStringFourWay(
					sb: sb,
					font: Main.fontItemStack,
					text: msg,
					x: pos.X,
					y: pos.Y,
					textColor: color,
					borderColor: color * 0.2f,
					origin: default(Vector2),
					scale: scale
				);
			} else {
				sb.DrawString(
					spriteFont: Main.fontItemStack,
					text: msg,
					position: pos,
					color: color,
					rotation: 0f,
					origin: new Vector2( 0f, 0f ),
					scale: scale,
					effects: SpriteEffects.None,
					layerDepth: 1f
				);
			}

			return size;
		}
	}
}
