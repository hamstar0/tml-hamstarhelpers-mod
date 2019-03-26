using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Services.Messages {
	public partial class PlayerMessages {
		private IDictionary<int, IList<PlayerLabelText>> PlayerTexts = new Dictionary<int, IList<PlayerLabelText>>();



		////////////////
		
		internal void Update() {   // Called from an Update function
			foreach( var kv in this.PlayerTexts.ToArray() ) {
				int who = kv.Key;
				IList<PlayerLabelText> list = kv.Value;
				Player player = Main.player[who];

				if( player == null || !player.active ) {
					this.PlayerTexts.Remove( who );
					continue;
				}

				for( int i=0; i<list.Count; i++ ) {
					PlayerLabelText txt = list[i];

					if( txt.Duration <= 0 ) {
						list.Remove( txt );
					} else {
						txt.Duration--;
					}
				}
			}
		}

		internal void Draw( SpriteBatch sb ) { // Called from a Draw function
			foreach( var kv in this.PlayerTexts ) {
				int who = kv.Key;
				IList<PlayerLabelText> list = kv.Value;
				Player player = Main.player[who];

				if( player == null || !player.active || player.dead ) { continue; }

				for( int i = 0; i < list.Count; i++ ) {
					PlayerLabelText txt = list[i];

					var pos = txt.Following ? new Vector2( player.Center.X, player.position.Y ) : txt.Anchor;
					pos.X -= Main.screenPosition.X;
					pos.Y -= Main.screenPosition.Y;

					var color = txt.Color;

					if( txt.Evaporates ) {
						pos.Y -= txt.StartDuration - txt.Duration;

						float scale = (float)txt.Duration / (float)txt.StartDuration;
						color.R = (byte)( (float)color.R * scale );
						color.G = (byte)( (float)color.G * scale );
						color.B = (byte)( (float)color.B * scale );
						color.A = (byte)( (float)color.A * scale );
					}
					pos.X -= ( Main.fontItemStack.MeasureString( txt.Text ).X * 1.5f ) / 2f;

					sb.DrawString( Main.fontItemStack, txt.Text, pos, color, 0f, default(Vector2), 1.5f, SpriteEffects.None, 1f );
				}
			}
		}
	}
}
