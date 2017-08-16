﻿using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.Localization;


namespace HamstarHelpers.MiscHelpers {
	public static class MiscHelpers {
		public static string RenderMoney( int money ) {
			string render = "";
			string label_copper = Language.GetTextValue( "CopperCoin" );	//Lang.inter[18];
			string label_silver = Language.GetTextValue( "SilverCoin" );    //Lang.inter[17];
			string label_gold = Language.GetTextValue( "GoldCoin" );  //Lang.inter[16];
			string label_plat = Language.GetTextValue( "PlatinumCoin" );  //Lang.inter[15];

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

			if( plat > 0 ) { render += plat + " " + label_plat; }
			if( render.Length > 0 ) { render += " "; }
			if( gold > 0 ) { render += gold + " " + label_gold; }
			if( render.Length > 0 ) { render += " "; }
			if( silver > 0 ) { render += silver + " " + label_silver; }
			if( render.Length > 0 ) { render += " "; }
			if( copper > 0 ) { render += copper + " " + label_copper; }

			return render;
		}
	}


	[System.Obsolete( "use DotNetHelpers.DotNetHelpers", true )]
	public static class DotNetHelpers {
		public static string DictToString( IDictionary<object, object> dict ) {
			return global::HamstarHelpers.DotNetHelpers.DotNetHelpers.DictToString( dict );
		}
	}


	[System.Obsolete( "use DebugHelpers.DebugHelpers", true )]
	public static class DebugHelpers {
		public static bool Once {
			get { return global::HamstarHelpers.DebugHelpers.DebugHelpers.Once; }
			set { global::HamstarHelpers.DebugHelpers.DebugHelpers.Once = value; }
		}
		public static int OnceInAWhile {
			get { return global::HamstarHelpers.DebugHelpers.DebugHelpers.OnceInAWhile; }
			set { global::HamstarHelpers.DebugHelpers.DebugHelpers.OnceInAWhile = value; }
		}
		public static IDictionary<string, string> Display {
			get { return global::HamstarHelpers.DebugHelpers.DebugHelpers.Display; }
			set { global::HamstarHelpers.DebugHelpers.DebugHelpers.Display = value; }
		}
		public static void MsgOnce( string msg ) {
			global::HamstarHelpers.DebugHelpers.DebugHelpers.MsgOnce( msg );
		}
		public static void MsgOnceInAWhile( string msg ) {
			global::HamstarHelpers.DebugHelpers.DebugHelpers.MsgOnceInAWhile( msg );
		}
		public static void PrintToBatch( SpriteBatch sb ) {
			global::HamstarHelpers.DebugHelpers.DebugHelpers.PrintToBatch( sb );
		}
		public static void Log( string msg ) {
			global::HamstarHelpers.DebugHelpers.DebugHelpers.Log( msg );
		}
	}
}
