using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;


namespace HamstarHelpers.Services.Messages.Player {
	/// <summary>
	/// Defines the parameters of a player message.
	/// </summary>
	public class PlayerLabelText {
		/// <summary></summary>
		public string Text;
		/// <summary></summary>
		public Color Color;
		/// <summary></summary>
		public int StartDuration;
		/// <summary></summary>
		public int Duration;
		/// <summary></summary>
		public bool Evaporates;
		/// <summary></summary>
		public bool Following;

		internal Vector2 Anchor;
	}



	/// <summary>
	/// Supplies a way to 'popup' informational text upon the player character in-game.
	/// </summary>
	public partial class PlayerMessages {
		/// <summary>
		/// Adds a player label popup.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="text"></param>
		/// <param name="color"></param>
		/// <param name="duration"></param>
		/// <param name="evaporates"></param>
		/// <param name="following">Tracks with the player.</param>
		public static void AddPlayerLabel( Terraria.Player player, string text, Color color, int duration, bool evaporates,
					bool following=true ) {
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
