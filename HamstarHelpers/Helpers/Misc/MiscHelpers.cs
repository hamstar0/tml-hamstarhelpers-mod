using Microsoft.Xna.Framework;
using Terraria.Localization;


namespace HamstarHelpers.Helpers.Misc {
	/// <summary>
	/// Assorted static "helper" misc. functions.
	/// </summary>
	public partial class MiscHelpers {
		/// <summary></summary>
		public static Color PlatinumCoinColor { get; } = new Color( 220, 220, 198 );

		/// <summary></summary>
		public static Color GoldCoinColor { get; } = new Color( 224, 201, 92 );

		/// <summary></summary>
		public static Color SilverCoinColor { get; } = new Color( 181, 192, 193 );

		/// <summary></summary>
		public static Color CopperCoinColor { get; } = new Color( 246, 138, 96 );



		////////////////

		/// <summary>
		/// Generates an English-formatted string indicating an amount of money.
		/// </summary>
		/// <param name="money"></param>
		/// <returns></returns>
		public static string RenderMoney( int money ) {
			string render = "";
			string labelCopper = Language.GetTextValue( "CopperCoin" );    //Lang.inter[18];
			string labelSilver = Language.GetTextValue( "SilverCoin" );    //Lang.inter[17];
			string labelGold = Language.GetTextValue( "GoldCoin" );  //Lang.inter[16];
			string labelPlat = Language.GetTextValue( "PlatinumCoin" );  //Lang.inter[15];

			int plat = 0;
			int gold = 0;
			int silver = 0;
			int copper = 0;

			if( money < 0 ) { money = 0; }

			if( money >= 1000000 ) {
				plat = money / 1000000;
				money -= plat * 1000000;
			}
			if( money >= 10000 ) {
				gold = money / 10000;
				money -= gold * 10000;
			}
			if( money >= 100 ) {
				silver = money / 100;
				money -= silver * 100;
			}
			if( money >= 1 ) {
				copper = money;
			}

			if( plat > 0 ) { render += plat + " " + labelPlat; }
			if( render.Length > 0 ) { render += " "; }
			if( gold > 0 ) { render += gold + " " + labelGold; }
			if( render.Length > 0 ) { render += " "; }
			if( silver > 0 ) { render += silver + " " + labelSilver; }
			if( render.Length > 0 ) { render += " "; }
			if( copper > 0 ) { render += copper + " " + labelCopper; }

			return render;
		}


		////

		/// <summary>
		/// Renders a given game tick value as a string.
		/// </summary>
		/// <param name="ticks"></param>
		/// <returns></returns>
		public static string RenderTickDuration( int ticks ) {
			int seconds = ticks / 60;
			int minutes = seconds / 60;
			int hours = minutes / 60;

			if( ticks < 60 ) {
				return "<1 second";
			}

			seconds -= minutes * 60;
			minutes -= hours * 60;

			string output = seconds + " seconds";
			if( minutes > 0 ) {
				output = minutes + " minutes " + output;
			}
			if( hours > 0 ) {
				output = hours + " hours " + output;
			}

			return output;
		}

		/// <summary>
		/// Renders a given game tick value as a condensed string.
		/// </summary>
		/// <param name="ticks"></param>
		/// <returns></returns>
		public static string RenderCondensedTickDuration( int ticks ) {
			int seconds = ticks / 60;
			int minutes = seconds / 60;
			int hours = minutes / 60;

			if( ticks < 60 ) {
				return "<1s";
			}

			seconds -= minutes * 60;
			minutes -= hours * 60;

			string output = seconds + "s";
			if( minutes > 0 ) {
				output = minutes + "m " + output;
			}
			if( hours > 0 ) {
				output = hours + "h " + output;
			}

			return output;
		}


		////

		/// <summary>
		/// Renders a color as a hex code string.
		/// </summary>
		/// <param name="color"></param>
		/// <returns></returns>
		public static string RenderColorHex( Color color ) {
			string r = ((int)color.R).ToString( "X2" );
			string g = ((int)color.G).ToString( "X2" );
			string b = ((int)color.B).ToString( "X2" );
			return r + g + b;
		}
	}
}
