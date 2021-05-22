using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Libraries.Debug;
using HamstarHelpers.Libraries.Items.Attributes;


namespace HamstarHelpers.Services.EntityGroups.Definitions {
	/// <summary></summary>
	public partial class ItemGroupIDs {
		/// <summary></summary>
		public const string AnyItem = "Any Item";
		/// <summary></summary>
		public const string AnyRainbow2Tier = "Any Rainbow 2 Tier";
		/// <summary></summary>
		public const string AnyRainbowTier = "Any Rainbow Tier";
		/// <summary></summary>
		public const string AnyAmberTier = "Any Amber Tier";
		/// <summary></summary>
		public const string AnyGreyTier = "Any Grey Tier";
		/// <summary></summary>
		public const string AnyWhiteTier = "Any White Tier";
		/// <summary></summary>
		public const string AnyBlueTier = "Any Blue Tier";
		/// <summary></summary>
		public const string AnyGreenTier = "Any Green Tier";
		/// <summary></summary>
		public const string AnyOrangeTier = "Any Orange Tier";
		/// <summary></summary>
		public const string AnyLightRedTier = "Any Light Red Tier";
		/// <summary></summary>
		public const string AnyPinkTier = "Any Pink Tier";
		/// <summary></summary>
		public const string AnyLightPurpleTier = "Any Light Purple Tier";
		/// <summary></summary>
		public const string AnyLimeTier = "Any Lime Tier";
		/// <summary></summary>
		public const string AnyYellowTier = "Any Yellow Tier";
		/// <summary></summary>
		public const string AnyCyanTier = "Any Cyan Tier";
		/// <summary></summary>
		public const string AnyRedTier = "Any Red Tier";
		/// <summary></summary>
		public const string AnyPurpleTier = "Any Purple Tier";
		/// <summary></summary>
		public const string AnyDye = "Any Dye";
		/// <summary></summary>
		public const string AnyFood = "Any Food";
	}




	partial class EntityGroupDefs {
		internal static void DefineItemMiscGroups1( IList<EntityGroupMatcherDefinition<Item>> defs ) {
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: ItemGroupIDs.AnyItem,
				grpDeps: null,
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					return true;
				} )
			) );

			for( int i = -12; i <= ItemRarityAttributeLibraries.HighestVanillaRarity; i++ ) {
				if( i >= -10 && i <= -3 ) { i = -2; }

				int tier = i;
				defs.Add( new EntityGroupMatcherDefinition<Item>(
					grpName: "Any " + ItemRarityAttributeLibraries.RarityColorText[i] + " Tier",
					grpDeps: null,
					matcher: new ItemGroupMatcher( ( item, grps ) => {
						return item.rare == tier;
					} )
				) );
			}

			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: ItemGroupIDs.AnyDye,
				grpDeps: null,
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					return item.dye != 0 || item.hairDye != -1;
				} )
			) );

			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: ItemGroupIDs.AnyFood,
				grpDeps: null,
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					return item.buffType == BuffID.WellFed;
				} )
			) );
		}
	}
}
