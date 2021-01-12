using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using HamstarHelpers.Helpers.Players;
using HamstarHelpers.Helpers.Misc;


namespace HamstarHelpers.Helpers.Items {
	/// <summary>
	/// Assorted static "helper" functions pertaining to money items.
	/// </summary>
	public partial class ItemMoneyHelpers {
		/// <summary></summary>
		public static Color PlatinumCoinColor { get; } = new Color( 220, 220, 198 );

		/// <summary></summary>
		public static Color GoldCoinColor { get; } = new Color( 224, 201, 92 );

		/// <summary></summary>
		public static Color SilverCoinColor { get; } = new Color( 181, 192, 193 );

		/// <summary></summary>
		public static Color CopperCoinColor { get; } = new Color( 246, 138, 96 );



		////////////////
		
		/// <summary></summary>
		/// <param name="money"></param>
		/// <returns></returns>
		public static (long platinum, long gold, long silver, long copper) GetMoneyDenominations( long money ) {
			long plat = 0;
			long gold = 0;
			long silver = 0;
			long copper = 0;
			long absMoney = Math.Abs( money );

			if( absMoney >= 1000000 ) {
				plat = money / 1000000;
				money -= plat * 1000000;
				absMoney -= Math.Abs(plat) * 1000000;
			}
			if( absMoney >= 10000 ) {
				gold = money / 10000;
				money -= gold * 10000;
				absMoney -= Math.Abs(gold) * 10000;
			}
			if( absMoney >= 100 ) {
				silver = money / 100;
				money -= silver * 100;
				absMoney -= Math.Abs(silver) * 100;
			}
			if( absMoney >= 1 ) {
				copper = absMoney;
			}

			return (plat, gold, silver, copper);
		}


		/// <summary>
		/// Generates an English-formatted string indicating an amount of money.
		/// </summary>
		/// <param name="money"></param>
		/// <param name="addDenom"></param>
		/// <param name="addColors"></param>
		/// <returns></returns>
		public static string[] RenderMoneyDenominations( long money, bool addDenom, bool addColors ) {
			var denoms = GetMoneyDenominations( money );
			var rendered = new List<string>( 4 );

			if( denoms.copper != 0 ) {
				string render = denoms.copper.ToString();
				if( addDenom ) {
					render += " " + Language.GetTextValue( "Currency.Copper" );    //Lang.inter[18];
				}
				if( addColors ) {
					string colorHex = MiscHelpers.RenderColorHex( ItemMoneyHelpers.CopperCoinColor );
					render = "[c/"+colorHex+":"+render+"]";
				}
				rendered.Add( render );
			}
			if( denoms.silver != 0 ) {
				string render = denoms.silver.ToString();
				if( addDenom ) {
					render += " " + Language.GetTextValue( "Currency.Silver" );    //Lang.inter[17];
				}
				if( addColors ) {
					string colorHex = MiscHelpers.RenderColorHex( ItemMoneyHelpers.SilverCoinColor );
					render = "[c/"+colorHex+":"+render+"]";
				}
				rendered.Add( render );
			}
			if( denoms.gold != 0 ) {
				string render = denoms.gold.ToString();
				if( addDenom ) {
					render += " " + Language.GetTextValue( "Currency.Gold" );    //Lang.inter[16];
				}
				if( addColors ) {
					string colorHex = MiscHelpers.RenderColorHex( ItemMoneyHelpers.GoldCoinColor );
					render = "[c/"+colorHex+":"+render+"]";
				}
				rendered.Add( render );
			}
			if( denoms.platinum != 0 ) {
				string render = denoms.platinum.ToString();
				if( addDenom ) {
					render += " " + Language.GetTextValue( "Currency.Platinum" );    //Lang.inter[15];
				}
				if( addColors ) {
					string colorHex = MiscHelpers.RenderColorHex( ItemMoneyHelpers.PlatinumCoinColor );
					render = "[c/"+colorHex+":"+render+"]";
				}
				rendered.Add( render );
			}

			return rendered.ToArray();
		}


		////////////////

		/// <summary>
		/// Totals up player's money.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="includeBanks"></param>
		/// <returns></returns>
		public static long CountMoney( Player player, bool includeBanks )
			=> PlayerItemHelpers.CountMoney( player, includeBanks );
	}
}
