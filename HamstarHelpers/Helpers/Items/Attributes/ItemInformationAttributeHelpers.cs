using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.Items.Attributes {
	/// <summary>
	/// A type of common vanilla tooltip.
	/// </summary>
	public enum VanillaTooltipName {
		/// <summary></summary>
		ItemName,
		/// <summary></summary>
		Favorite,
		/// <summary></summary>
		FavoriteDesc,
		/// <summary></summary>
		Social,
		/// <summary></summary>
		SocialDesc,
		/// <summary></summary>
		Damage,
		/// <summary></summary>
		CritChance,
		/// <summary></summary>
		Speed,
		/// <summary></summary>
		Knockback,
		/// <summary></summary>
		FishingPower,
		/// <summary></summary>
		NeedsBait,
		/// <summary></summary>
		BaitPower,
		/// <summary></summary>
		Equipable,
		/// <summary></summary>
		WandConsumes,
		/// <summary></summary>
		Quest,
		/// <summary></summary>
		Vanity,
		/// <summary></summary>
		Defense,
		/// <summary></summary>
		PickPower,
		/// <summary></summary>
		AxePowe,
		/// <summary></summary>
		HammerPower,
		/// <summary></summary>
		TileBoost,
		/// <summary></summary>
		HealLife,
		/// <summary></summary>
		HealMana,
		/// <summary></summary>
		UseMana,
		/// <summary></summary>
		Placeable,
		/// <summary></summary>
		Ammo,
		/// <summary></summary>
		Consumable,
		/// <summary></summary>
		Material,
		/// <summary></summary>
		Tooltip,
		/// <summary></summary>
		EtherianManaWarning,
		/// <summary></summary>
		WellFedExpert,
		/// <summary></summary>
		BuffTime,
		/// <summary></summary>
		OneDropLogo,
		/// <summary></summary>
		PrefixDamage,
		/// <summary></summary>
		PrefixSpeed,
		/// <summary></summary>
		PrefixCritChance,
		/// <summary></summary>
		PrefixUseMana,
		/// <summary></summary>
		PrefixSize,
		/// <summary></summary>
		PrefixShootSpeed,
		/// <summary></summary>
		PrefixKnockback,
		/// <summary></summary>
		PrefixAccDefense,
		/// <summary></summary>
		PrefixAccMaxMana,
		/// <summary></summary>
		PrefixAccCritChance,
		/// <summary></summary>
		PrefixAccDamage,
		/// <summary></summary>
		PrefixAccMoveSpeed,
		/// <summary></summary>
		PrefixAccMeleeSpeed,
		/// <summary></summary>
		SetBonus,
		/// <summary></summary>
		Expert,
		/// <summary></summary>
		SpecialPrice,
		/// <summary></summary>
		Price
	}




	/// <summary>
	/// Assorted static "helper" functions pertaining to information attributes of items (e.g. tooltips).
	/// </summary>
	public class ItemInformationAttributeHelpers {
		/// <summary>
		/// Convenience function (meant to be called in any ModifyTooltips hook) add a tooltip to an item in a specific
		/// position. Adapts to item context.
		/// </summary>
		/// <param name="tooltips"></param>
		/// <param name="tip"></param>
		/// <param name="insertAt"></param>
		/// <param name="insertAfter"></param>
		/// <returns></returns>
		public static bool ApplyTooltipAt(
					IList<TooltipLine> tooltips,
					TooltipLine tip,
					VanillaTooltipName insertAt = VanillaTooltipName.Tooltip,
					bool insertAfter = true ) {
			for( int i = tooltips.Count - 1; i >= 0; i-- ) {
				string name = tooltips[i].Name;
				VanillaTooltipName nameType;

				if( !Enum.TryParse(name, out nameType) ) {
					if( name.StartsWith( "Tooltip" ) ) {
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
		/// Convenience function (meant to be called in any ModifyTooltips hook) add a tooltip to an item in a specific
		/// position. Adapts to item context.
		/// </summary>
		/// <param name="tooltips"></param>
		/// <param name="tip"></param>
		/// <param name="insertAt"></param>
		/// <param name="insertAfter"></param>
		/// <returns></returns>
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
	}
}
