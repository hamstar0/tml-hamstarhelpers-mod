using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Helpers.Items.Attributes {
	/// <summary>
	/// Assorted static "helper" functions pertaining to gameplay attributes of items.
	/// </summary>
	public partial class ItemAttributeHelpers {
		private IDictionary<long, ISet<int>> PurchasableItems = new Dictionary<long, ISet<int>>();



		////////////////

		internal ItemAttributeHelpers() { }
	}
}
