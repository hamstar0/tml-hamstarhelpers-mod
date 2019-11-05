using HamstarHelpers.Helpers.Recipes;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Services.EntityGroups.Definitions {
	/// <summary></summary>
	public partial class ItemGroupIDs {
		/// <summary></summary>
		public static readonly string AnyWoodEquipment = "Any Wood Equipment";
		/// <summary></summary>
		public static readonly string AnyCopperOrTinEquipment = "Any Copper Or Tin Equipment";
		/// <summary></summary>
		public static readonly string AnyIronOrLeadEquipment = "Any Iron Or Lead Equipment";
		/// <summary></summary>
		public static readonly string AnySilverOrTungstenEquipment = "Any Silver Or Tungsten Equipment";
		/// <summary></summary>
		public static readonly string AnyGoldOrPlatinumEquipment = "Any Gold Or Platinum Equipment";
		/// <summary></summary>
		public static readonly string AnyDemoniteOrCrimtaneEquipment = "Any Demonite Or Crimtane Equipment";
		/// <summary></summary>
		public static readonly string AnyMeteorOrJungleOrBoneOrBeeEquipment = "Any Meteor Or Jungle Or Bone Or Bee Equipment";
		/// <summary></summary>
		public static readonly string AnyCobaltOrPalladiumEquipment = "Any Cobalt Or Palladium Equipment";
		/// <summary></summary>
		public static readonly string AnyMythrilOrOrichalcumEquipment = "Any Mythril Or Orichalcum Equipment";
		/// <summary></summary>
		public static readonly string AnyAdamantiteOrTitaniumEquipment = "Any Adamantite Or Titanium Equipment";
		/// <summary></summary>
		public static readonly string AnyFrostCoreOrForbiddenEquipment = "Any Frost Core Or Forbidden Equipment";
		/// <summary></summary>
		public static readonly string AnyChlorophyteOrShroomiteOrSpectreEquipment = "Any Chlorophyte Or Shroomite Or Spectre Equipment";
		/// <summary></summary>
		public static readonly string AnyCelestialEquipment = "Any Celestial Equipment";
	}
	



	partial class EntityGroupDefs {
		internal static void DefineItemEquipmentGroups4( IList<EntityGroupMatcherDefinition<Item>> defs ) {
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				ItemGroupIDs.AnyWoodEquipment,
				new string[] { ItemGroupIDs.AnyEquipment, "Any Wood" },
				new ItemGroupMatcher( ( item, grps ) => {
					IDictionary<int, int> anyEquipGrp = grps[ItemGroupIDs.AnyEquipment].ToDictionary( id=>id, id=>1 );
					IDictionary<int, int> anyWoodGrp = grps["Any Wood"].ToDictionary( id => id, id=>1 );

					bool hasWood = RecipeHelpers.ItemHasIngredients( item.type, anyWoodGrp );
					if( !anyEquipGrp.ContainsKey(item.type) || !hasWood ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Copper Or Tin Equipment",
				new string[] { "Any Copper Equipment", "Any Tin Equipment" },
				new ItemGroupMatcher( ( item, grps ) => {
					return grps["Any Copper Equipment"].Contains( item.type ) ||
						grps["Any Tin Equipment"].Contains( item.type );
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Iron Or Lead Equipment",
				new string[] { "Any Iron Equipment", "Any Lead Equipment" },
				new ItemGroupMatcher( ( item, grps ) => {
					return grps["Any Iron Equipment"].Contains( item.type ) ||
						grps["Any Lead Equipment"].Contains( item.type );
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Silver Or Tungsten Equipment",
				new string[] { "Any Silver Equipment", "Any Tungsten Equipment" },
				new ItemGroupMatcher( ( item, grps ) => {
					return grps["Any Silver Equipment"].Contains( item.type ) ||
						grps["Any Tungsten Equipment"].Contains( item.type );
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Gold Or Platinum Equipment",
				new string[] { "Any Gold Equipment", "Any Platinum Equipment" },
				new ItemGroupMatcher( ( item, grps ) => {
					return grps["Any Gold Equipment"].Contains( item.type ) ||
						grps["Any Platinum Equipment"].Contains( item.type );
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Demonite Or Crimtane Equipment",
				new string[] { "Any Demonite Equipment", "Any Crimtane Equipment" },
				new ItemGroupMatcher( ( item, grps ) => {
					return grps["Any Demonite Equipment"].Contains( item.type ) ||
						grps["Any Crimtane Equipment"].Contains( item.type );
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Meteor Or Jungle Or Bone Or Bee Equipment",
				new string[] { "Any Meteor Equipment", "Any Jungle Equipment", "Any Bone Equipment", "Any Bee Equipment" },
				new ItemGroupMatcher( ( item, grps ) => {
					return grps["Any Meteor Equipment"].Contains( item.type ) ||
						grps["Any Jungle Equipment"].Contains( item.type ) ||
						grps["Any Bone Equipment"].Contains( item.type ) ||
						grps["Any Bee Equipment"].Contains( item.type );
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Cobalt Or Palladium Equipment",
				new string[] { "Any Cobalt Equipment", "Any Palladium Equipment" },
				new ItemGroupMatcher( ( item, grps ) => {
					return grps["Any Cobalt Equipment"].Contains( item.type ) ||
						grps["Any Palladium Equipment"].Contains( item.type );
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Mythril Or Orichalcum Equipment",
				new string[] { "Any Mythril Equipment", "Any Orichalcum Equipment" },
				new ItemGroupMatcher( ( item, grps ) => {
					return grps["Any Mythril Equipment"].Contains( item.type ) ||
						grps["Any Orichalcum Equipment"].Contains( item.type );
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Adamantite Or Titanium Equipment",
				new string[] { "Any Adamantite Equipment", "Any Titanium Equipment" },
				new ItemGroupMatcher( ( item, grps ) => {
					return grps["Any Adamantite Equipment"].Contains( item.type ) ||
						grps["Any Titanium Equipment"].Contains( item.type );
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Frost Core Or Forbidden Equipment",
				new string[] { "Any Frost Core Equipment", "Any Forbidden Equipment" },
				new ItemGroupMatcher( ( item, grps ) => {
					return grps["Any Frost Core Equipment"].Contains( item.type ) ||
						grps["Any Forbidden Equipment"].Contains( item.type );
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Chlorophyte Or Shroomite Or Spectre Equipment",
				new string[] { "Any Chlorophyte Equipment", "Any Shroomite Equipment", "Any Spectre Equipment" },
				new ItemGroupMatcher( ( item, grps ) => {
					return grps["Any Chlorophyte Equipment"].Contains( item.type ) ||
						grps["Any Shroomite Equipment"].Contains( item.type ) ||
						grps["Any Spectre Equipment"].Contains( item.type );
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Celestial Equipment",
				new string[] { "Any Nebula Equipment", "Any Vortex Equipment", "Any Solar Equipment", "Any Stardust Equipment" },
				new ItemGroupMatcher( ( item, grps ) => {
					return grps["Any Nebula Equipment"].Contains( item.type ) ||
						grps["Any Vortex Equipment"].Contains( item.type ) ||
						grps["Any Solar Equipment"].Contains( item.type ) ||
						grps["Any Stardust Equipment"].Contains( item.type );
				} )
			) );
		}
	}
}
