using Terraria.Localization;


namespace HamstarHelpers.Helpers.MiscHelpers {
	public static partial class MiscHelpers {
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
	}
}
