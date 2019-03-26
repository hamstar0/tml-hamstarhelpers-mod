using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Services.Messages {
	public class PlayerLabelText {
		public string Text;
		public Color Color;
		public int StartDuration;
		public int Duration;
		public bool Evaporates;
		public bool Following;

		internal Vector2 Anchor;
	}



	public partial class PlayerMessages {
		public static void AddPlayerLabel( Player player, string text, Color color, int duration, bool evaporates, bool following=true ) {
			var pm = ModHelpersMod.Instance.PlayerMessages;

			if( !pm.PlayerTexts.ContainsKey(player.whoAmI) ) {
				pm.PlayerTexts[ player.whoAmI ] = new List<PlayerLabelText>();
			}

			var pt = new PlayerLabelText {
				Text = text,
				Color = color,
				StartDuration = duration,
				Duration = duration,
				Evaporates = evaporates,
				Following = following,
				Anchor = new Vector2( player.Center.X, player.position.Y )
			};

			pm.PlayerTexts[ player.whoAmI ].Add( pt );
		}
	}
}
