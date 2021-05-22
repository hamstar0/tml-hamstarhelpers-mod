using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using HamstarHelpers.Libraries.Players;
using HamstarHelpers.Libraries.Misc;
using HamstarHelpers.Libraries.XNA;


namespace HamstarHelpers.Libraries.Items {
	/// <summary>
	/// Assorted static "helper" functions pertaining to money items.
	/// </summary>
	public partial class ItemMoneyLibraries {
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
		/// <param name="addColorPulse">Adds Terraria's standard text pulsing to rendered colors.</param>
		/// <returns></returns>
		public static string[] RenderMoneyDenominations( long money, bool addDenom, bool addColors, bool addColorPulse=false ) {
			float colorPulse = addColorPulse
				? ( (float)Main.mouseTextColor / 255f )
				: 1f;

			return ItemMoneyLibraries.RenderMoneyDenominations( money, addDenom, addColors, Color.White * colorPulse );
		}

		/// <summary>
		/// Generates an English-formatted string indicating an amount of money.
		/// </summary>
		/// <param name="money"></param>
		/// <param name="addDenom"></param>
		/// <param name="addColors"></param>
		/// <param name="tint"></param>
		/// <returns></returns>
		public static string[] RenderMoneyDenominations( long money, bool addDenom, bool addColors, Color tint ) {
			var denoms = ItemMoneyLibraries.GetMoneyDenominations( money );
			var rendered = new List<string>( 4 );

			if( denoms.platinum != 0 ) {
				string render = denoms.platinum.ToString();
				if( addDenom ) {
					render += " " + Language.GetTextValue( "Currency.Platinum" );    //Lang.inter[15];
				}
				if( addColors ) {
					Color color = XNAColorLibraries.Mul( ItemMoneyLibraries.PlatinumCoinColor, tint );
					string colorHex = MiscLibraries.RenderColorHex( color );
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
					Color color = XNAColorLibraries.Mul( ItemMoneyLibraries.GoldCoinColor, tint );
					string colorHex = MiscLibraries.RenderColorHex( color );
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
					Color color = XNAColorLibraries.Mul( ItemMoneyLibraries.SilverCoinColor, tint );
					string colorHex = MiscLibraries.RenderColorHex( color );
					render = "[c/"+colorHex+":"+render+"]";
				}
				rendered.Add( render );
			}
			if( denoms.copper != 0 ) {
				string render = denoms.copper.ToString();
				if( addDenom ) {
					render += " " + Language.GetTextValue( "Currency.Copper" );    //Lang.inter[18];
				}
				if( addColors ) {
					Color color = XNAColorLibraries.Mul( ItemMoneyLibraries.CopperCoinColor, tint );
					string colorHex = MiscLibraries.RenderColorHex( color );
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
			=> PlayerItemLibraries.CountMoney( player, includeBanks );
	}
}
