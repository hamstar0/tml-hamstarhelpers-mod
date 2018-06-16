using Microsoft.Xna.Framework;
using System;
using Terraria;


namespace HamstarHelpers.Utilities.Messages {
	[Obsolete( "use Services.Messages.PlayerMessages", true )]
	public class PlayerMessages {
		public static void AddPlayerLabel( Player player, string text, Color color, int duration, bool evaporates, bool following=true ) {
			Services.Messages.PlayerMessages.AddPlayerLabel( player, text, color, duration, evaporates );
		}
	}


	[Obsolete( "use Services.Messages.PlayerMessages", true )]
	public class PlayerMessage {
		public static void AddPlayerLabel( Player player, string text, Color color, int duration, bool evaporates ) {
			Services.Messages.PlayerMessages.AddPlayerLabel( player, text, color, duration, evaporates );
		}
	}
}
