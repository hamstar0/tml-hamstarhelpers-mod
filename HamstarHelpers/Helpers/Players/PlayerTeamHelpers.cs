using Microsoft.Xna.Framework;
using Terraria;


namespace HamstarHelpers.Helpers.Players {
	/// <summary></summary>
	public enum PlayerTeamName {
		/// <summary></summary>
		White = 0,
		/// <summary></summary>
		Red = 1,
		/// <summary></summary>
		Green = 2,
		/// <summary></summary>
		Cyan = 4,
		/// <summary></summary>
		Yellow = 8,
		/// <summary></summary>
		Pink = 16
	}



	/// <summary>
	/// Assorted static "helper" functions pertaining to player multiplayer teams.
	/// </summary>
	public partial class PlayerTeamHelpers {
		/// <summary>
		/// Gets the team name and color by a given player team code number.
		/// </summary>
		/// <param name="team"></param>
		/// <param name="color"></param>
		/// <returns></returns>
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
