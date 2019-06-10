using Microsoft.Xna.Framework;
using Terraria;


namespace HamstarHelpers.Helpers.Players {
	public enum PlayerTeamName {
		White = 0,
		Red = 1,
		Green = 2,
		Cyan = 4,
		Yellow = 8,
		Pink = 16
	}



	public static partial class PlayerHelpers {
		public static PlayerTeamName GetTeamName( int team, out Color color ) {
			switch( team ) {
			case 1:
				color = new Color( 218, 59, 59 );
				return PlayerTeamName.Red;
			case 2:
				color = new Color( 59, 218, 85 );
				return PlayerTeamName.Green;
			case 3:
				color = new Color( 59, 149, 218 );
				return PlayerTeamName.Cyan;
			case 4:
				color = new Color( 242, 221, 100 );
				return PlayerTeamName.Yellow;
			case 5:
				color = new Color( 224, 100, 242 );
				return PlayerTeamName.Pink;
			case 0:
			default:
				color = Color.White;
				return PlayerTeamName.White;
			}
		}
	}
}
