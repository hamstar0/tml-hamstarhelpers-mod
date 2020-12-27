using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using HamstarHelpers.Helpers.Players;


namespace HamstarHelpers.Helpers.Items {
	/// <summary>
	/// Assorted static "helper" functions pertaining to money items.
	/// </summary>
	public partial class ItemMoneyHelpers {
		/// @private
		[Obsolete("use alt", true)]
		public static string RenderMoney( int money ) {
			return string.Join( " ", ItemMoneyHelpers.RenderMoneyDenominations(money, true, false) );
		}
	}
}
