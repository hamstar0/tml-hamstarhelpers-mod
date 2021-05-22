using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Libraries.Debug;


namespace HamstarHelpers.Libraries.Items.Attributes {
	/// <summary>
	/// Assorted static "helper" functions pertaining to information attributes of items (e.g. tooltips).
	/// </summary>
	public partial class ItemInformationAttributeLibraries {
		/// <summary>
		/// Convenience function (meant to be called in any ModifyTooltips hook) to show a tooltip for an item at
		/// the end of its list. Adapts to item context.
		/// </summary>
		/// <param name="tooltips"></param>
		/// <param name="tip"></param>
		/// <returns>`true` if tooltip added.</returns>
		public static bool AppendTooltipToFurthest( IList<TooltipLine> tooltips, TooltipLine tip ) {
			return ItemInformationAttributeLibraries.ApplyTooltipAtFurthest(
				tooltips,
				tip,
				new HashSet<VanillaTooltipName> {
					VanillaTooltipName.Tooltip,
					VanillaTooltipName.Material,
					VanillaTooltipName.Consumable,
					VanillaTooltipName.HammerPower,
					VanillaTooltipName.AxePower,
					VanillaTooltipName.PickPower
				},
				true
			);
		}


		////

		/// <summary>
		/// Convenience function (meant to be called in any ModifyTooltips hook) to show a tooltip for an item at a specific
		/// list position. Adapts to item context.
		/// </summary>
		/// <param name="tooltips"></param>
		/// <param name="tip"></param>
		/// <param name="insertAt">Vanilla-standard tooltip type. Does not accept bit flags.</param>
		/// <param name="insertAfter"></param>
		/// <returns>`true` if tooltip added.</returns>
		public static bool ApplyTooltipAt(
					IList<TooltipLine> tooltips,
					TooltipLine tip,
					VanillaTooltipName insertAt = VanillaTooltipName.Tooltip,
					bool insertAfter = true ) {
			for( int i = tooltips.Count - 1; i >= 0; i-- ) {
				string name = tooltips[i].Name;
				VanillaTooltipName nameType;

				if( !Enum.TryParse(name, out nameType) ) {
					if( name.StartsWith("Tooltip") ) {
						nameType = VanillaTooltipName.Tooltip;
					} else {
						continue;
					}
				}

				if( insertAt == nameType ) {
					if( insertAfter ) {
						tooltips.Insert( i+1, tip );
					} else {
						tooltips.Insert( i, tip );
					}
					return true;
				}

				//if( tooltips[i].Name == "Vanity" ) {
				//	return -1;
				//}
				//switch( tooltips[i].Name ) {
				//case "SpecialPrice":
				//case "Price":
				//	return i;
				//}
			}

			return false;
		}


		/// <summary>
		/// Convenience function (meant to be called in any ModifyTooltips hook) to show a tooltip for an item at a specific
		/// list position. Adapts to item context.
		/// </summary>
		/// <param name="tooltips"></param>
		/// <param name="tip"></param>
		/// <param name="insertAt"></param>
		/// <param name="insertAfter"></param>
		/// <returns>`true` if tooltip added.</returns>
		public static bool ApplyTooltipAt(
					IList<TooltipLine> tooltips,
					TooltipLine tip,
					string insertAt,	// = "Tooltip"
					bool insertAfter = true ) {
			for( int i = tooltips.Count - 1; i >= 0; i-- ) {
				string name = tooltips[i].Name;

				if( name.StartsWith( "Tooltip" ) ) {
					name = "Tooltip";
				}

				if( insertAt == name ) {
					if( insertAfter ) {
						tooltips.Insert( i+1, tip );
					} else {
						tooltips.Insert( i, tip );
					}
					return true;
				}
			}

			return false;
		}


		/// <summary>
		/// Convenience function (meant to be called in any ModifyTooltips hook) to show a tooltip for an item at
		/// a specific list position. Adapts to item context.
		/// </summary>
		/// <param name="tooltips"></param>
		/// <param name="tip"></param>
		/// <param name="insertAts">A set of vanilla-standard tooltip types.</param>
		/// <param name="insertAfter">If `true`, inserts after the highest indexed tooltip. Otherwise the lowest.</param>
		/// <returns>`true` if any specified insertion points were found, and the tip was then inserted.</returns>
		public static bool ApplyTooltipAtFurthest(
					IList<TooltipLine> tooltips,
					TooltipLine tip,
					ISet<VanillaTooltipName> insertAts,
					bool insertAfter = true ) {
			int maxIdx = tooltips.Count - 1;
			int highestIdx = 0;
			int lowestIdx = maxIdx;

			bool foundSpecificInertionPoint = false;

			for( int i = maxIdx; i >= 0; i-- ) {
				string name = tooltips[i].Name;
				VanillaTooltipName nameType;

				if( !Enum.TryParse( name, out nameType ) ) {
					if( name.StartsWith( "Tooltip" ) ) {
						nameType = VanillaTooltipName.Tooltip;
					} else {
						continue;
					}
				}

				if( insertAts.Contains( nameType ) ) {
					foundSpecificInertionPoint = true;

					if( i > highestIdx ) {
						highestIdx = i;
					}
					if( i < lowestIdx ) {
						lowestIdx = i;
					}
				}
			}

			if( foundSpecificInertionPoint ) {
				if( insertAfter ) {
					tooltips.Insert( highestIdx + 1, tip );
				} else {
					tooltips.Insert( lowestIdx, tip );
				}
			}

			return foundSpecificInertionPoint;
		}
	}
}
