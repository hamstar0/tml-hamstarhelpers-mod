using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;


namespace HamstarHelpers.Helpers.Items.Attributes {
	/// <summary>
	/// Assorted static "helper" functions pertaining to information attributes of items (e.g. tooltips).
	/// </summary>
	public partial class ItemInformationAttributeHelpers {
		/// @private
		[Obsolete( "use `AppendTooltipToFurthest`", true)]
		public static bool AppendTooltip( IList<TooltipLine> tooltips, TooltipLine tip ) {
			if( ItemInformationAttributeHelpers.ApplyTooltipAt(tooltips, tip, VanillaTooltipName.Tooltip, true) ) {
				return true;
			}
			if( ItemInformationAttributeHelpers.ApplyTooltipAt(tooltips, tip, VanillaTooltipName.Material, true) ) {
				return true;
			}
			if( ItemInformationAttributeHelpers.ApplyTooltipAt(tooltips, tip, VanillaTooltipName.Consumable, true) ) {
				return true;
			}
			//if( ItemInformationAttributeHelpers.ApplyTooltipAt(tooltips, tip, VanillaTooltipName.TileBoost, true) ) {
			//	return true;
			//}

			if( ItemInformationAttributeHelpers.ApplyTooltipAt(tooltips, tip, VanillaTooltipName.HammerPower, true) ) {
				return true;
			}
			if( ItemInformationAttributeHelpers.ApplyTooltipAt(tooltips, tip, VanillaTooltipName.AxePower, true) ) {
				return true;
			}
			if( ItemInformationAttributeHelpers.ApplyTooltipAt(tooltips, tip, VanillaTooltipName.PickPower, true) ) {
				return true;
			}

//DebugHelpers.Print( "tooltipmissing", string.Join(", ", tooltips.Select(t=>t.Name)) );
			return false;
		}
	}
}
