using System;
using Terraria;


namespace HamstarHelpers.Helpers.Items {
	/// <summary>
	/// Assorted static "helper" functions pertaining to money items.
	/// </summary>
	public partial class ItemMoneyHelpers {
		/// @private
		[Obsolete( "use RenderMoneyDenominations(long, bool, bool)", true)]
		public static string RenderMoney( int money ) {
			return string.Join( " ", ItemMoneyHelpers.RenderMoneyDenominations(money, true, false) );
		}
	}
}
