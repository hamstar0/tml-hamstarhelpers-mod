using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Items.Attributes;
using HamstarHelpers.Helpers.Recipes;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;


namespace HamstarHelpers.Services.EntityGroups.Definitions {
	/// <summary></summary>
	public partial class ItemGroupIDs {
		/// <summary></summary>
		public const string AnyCactusEquipment = "Any Cactus Equipment";
		//"Any Tiki Equipment",
		//"Any Plain Wood Equipment",
		//"Any Boreal Wood Equipment",
		//"Any Palm Wood Equipment",
		//"Any Rich Mahogany Equipment",
		//"Any Ebonwood Equipment",
		//"Any Shadewood Equipment",
		//"Any Pearlwood Equipment",
		//"Any Dynasty Wood Equipment",
		//"Any Spooky Wood Equipment",
		//"Any Tin Equipment",
		//"Any Copper Equipment",
		//"Any Iron Equipment",
		//"Any Lead Equipment",
		//"Any Silver Equipment",
		//"Any Tungsten Equipment",
		//"Any Gold Equipment",
		//"Any Platinum Equipment",
		//"Any Demonite Equipment",
		//"Any Crimtane Equipment",
		/// <summary></summary>
		public const string AnyMeteorEquipment = "Any Meteor Equipment";
		/// <summary></summary>
		public const string AnyJungleEquipment = "Any Jungle Equipment";
		/// <summary></summary>
		public const string AnyBeeEquipment = "Any Bee Equipment";
		/// <summary></summary>
		public const string AnyBoneEquipment = "Any Bone Equipment";
		/// <summary></summary>
		public const string AnyHellstoneEquipment = "Any Hellstone Equipment";
		//"Any Cobalt Equipment",
		//"Any Palladium Equipment",
		//"Any Mythril Equipment",
		//"Any Orichalcum Equipment",
		//"Any Adamantite Equipment",
		//"Any Titanium Equipment",
		/// <summary></summary>
		public const string AnyFrostCoreEquipment = "Any Frost Core Equipment";
		/// <summary></summary>
		public const string AnyForbiddenEquipment = "Any Forbidden Equipment";
		/// <summary></summary>
		public const string AnyHallowEquipment = "Any Hallow Equipment";
		//"Any Chlorophyte Equipment",
		//"Any Shroomite Equipment",
		//"Any Spectre Equipment",
		/// <summary></summary>
		public const string AnyShellEquipment = "Any Shell Equipment";
		//"Any Nebula Equipment",
		//"Any Vortex Equipment",
		//"Any Solar Equipment",
		//"Any Stardust Equipment",
		/// <summary></summary>
		public const string AnyLuminiteOreEquipment = "Any Luminite Ore Equipment";
	}




	partial class EntityGroupDefs {
		internal static void DefineItemEquipmentGroups3( IList<EntityGroupMatcherDefinition<Item>> defs ) {
			// Equipment Tiers

			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Tiki Equipment",
				new string[] { ItemGroupIDs.AnyEquipment },
				new ItemGroupMatcher( ( item, grps ) => {
					bool isEquip = grps[ItemGroupIDs.AnyEquipment].Contains( item.type );
					string name = ItemAttributeHelpers.GetQualifiedName( item );

					if( !isEquip || !name.Contains( "Tiki" ) ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );

			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Cactus Equipment",
				new string[] { ItemGroupIDs.AnyEquipment, },
				new ItemGroupMatcher( ( item, grps ) => {
					bool isEquip = grps[ItemGroupIDs.AnyEquipment].Contains( item.type );
					bool has = RecipeHelpers.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.Cactus, 1} } );

					if( !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );

			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Plain Wood Equipment",
				new string[] { ItemGroupIDs.AnyEquipment },
				new ItemGroupMatcher( ( item, grps ) => {
					bool isEquip = grps[ItemGroupIDs.AnyEquipment].Contains( item.type );
					bool has = RecipeHelpers.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.Wood, 2} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Boreal Wood Equipment",
				new string[] { ItemGroupIDs.AnyEquipment },
				new ItemGroupMatcher( ( item, grps ) => {
					bool isEquip = grps[ItemGroupIDs.AnyEquipment].Contains( item.type );
					bool has = RecipeHelpers.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.BorealWood, 2} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Palm Wood Equipment",
				new string[] { ItemGroupIDs.AnyEquipment },
				new ItemGroupMatcher( ( item, grps ) => {
					bool isEquip = grps[ItemGroupIDs.AnyEquipment].Contains( item.type );
					bool has = RecipeHelpers.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.PalmWood, 2} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Rich Mahogany Equipment",
				new string[] { ItemGroupIDs.AnyEquipment },
				new ItemGroupMatcher( ( item, grps ) => {
					bool isEquip = grps[ItemGroupIDs.AnyEquipment].Contains( item.type );
					bool has = RecipeHelpers.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.RichMahogany, 2} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Ebonwood Equipment",
				new string[] { ItemGroupIDs.AnyEquipment },
				new ItemGroupMatcher( ( item, grps ) => {
					bool isEquip = grps[ItemGroupIDs.AnyEquipment].Contains( item.type );
					bool has = RecipeHelpers.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.Ebonwood, 2} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				 "Any Shadewood Equipment",
				new string[] { ItemGroupIDs.AnyEquipment },
				new ItemGroupMatcher( ( item, grps ) => {
					bool isEquip = grps[ItemGroupIDs.AnyEquipment].Contains( item.type );
					bool has = RecipeHelpers.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.Shadewood, 2} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Pearlwood Equipment",
				new string[] { ItemGroupIDs.AnyEquipment },
				new ItemGroupMatcher( ( item, grps ) => {
					bool isEquip = grps[ItemGroupIDs.AnyEquipment].Contains( item.type );
					bool has = RecipeHelpers.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.Pearlwood, 2} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Spooky Wood Equipment",
				new string[] { ItemGroupIDs.AnyEquipment },
				new ItemGroupMatcher( ( item, grps ) => {
					bool isEquip = grps[ItemGroupIDs.AnyEquipment].Contains( item.type );
					bool has = RecipeHelpers.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.SpookyWood, 2} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );

			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Tin Equipment",
				new string[] { ItemGroupIDs.AnyEquipment },
				new ItemGroupMatcher( ( item, grps ) => {
					bool isEquip = grps[ItemGroupIDs.AnyEquipment].Contains( item.type );
					bool has = RecipeHelpers.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.TinBar, 1} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Copper Equipment",
				new string[] { ItemGroupIDs.AnyEquipment },
				new ItemGroupMatcher( ( item, grps ) => {
					bool isEquip = grps[ItemGroupIDs.AnyEquipment].Contains( item.type );
					bool has = RecipeHelpers.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.CopperBar, 1} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );

			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Iron Equipment",
				new string[] { ItemGroupIDs.AnyEquipment },
				new ItemGroupMatcher( ( item, grps ) => {
					bool isEquip = grps[ItemGroupIDs.AnyEquipment].Contains( item.type );
					bool has = RecipeHelpers.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.IronBar, 1} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Lead Equipment",
				new string[] { ItemGroupIDs.AnyEquipment },
				new ItemGroupMatcher( ( item, grps ) => {
					bool isEquip = grps[ItemGroupIDs.AnyEquipment].Contains( item.type );
					bool has = RecipeHelpers.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.LeadBar, 1} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );

			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Silver Equipment",
				new string[] { ItemGroupIDs.AnyEquipment },
				new ItemGroupMatcher( ( item, grps ) => {
					bool isEquip = grps[ItemGroupIDs.AnyEquipment].Contains( item.type );
					bool has = RecipeHelpers.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.SilverBar, 1} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Tungsten Equipment",
				new string[] { ItemGroupIDs.AnyEquipment },
				new ItemGroupMatcher( ( item, grps ) => {
					bool isEquip = grps[ItemGroupIDs.AnyEquipment].Contains( item.type );
					bool has = RecipeHelpers.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.TungstenBar, 1} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );

			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Gold Equipment",
				new string[] { ItemGroupIDs.AnyEquipment },
				new ItemGroupMatcher( ( item, grps ) => {
					bool isEquip = grps[ItemGroupIDs.AnyEquipment].Contains( item.type );
					bool has = RecipeHelpers.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.GoldBar, 1} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Platinum Equipment",
				new string[] { ItemGroupIDs.AnyEquipment },
				new ItemGroupMatcher( ( item, grps ) => {
					bool isEquip = grps[ItemGroupIDs.AnyEquipment].Contains( item.type );
					bool has = RecipeHelpers.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.PlatinumBar, 1} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );

			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Meteor Equipment",
				new string[] { ItemGroupIDs.AnyEquipment },
				new ItemGroupMatcher( ( item, grps ) => {
					bool isEquip = grps[ItemGroupIDs.AnyEquipment].Contains( item.type );
					bool has = RecipeHelpers.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.MeteoriteBar, 1} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Demonite Equipment",
				new string[] { ItemGroupIDs.AnyEquipment },
				new ItemGroupMatcher( ( item, grps ) => {
					bool isEquip = grps[ItemGroupIDs.AnyEquipment].Contains( item.type );
					bool has = RecipeHelpers.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.DemoniteBar, 1} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Crimtane Equipment",
				new string[] { ItemGroupIDs.AnyEquipment },
				new ItemGroupMatcher( ( item, grps ) => {
					bool isEquip = grps[ItemGroupIDs.AnyEquipment].Contains( item.type );
					bool has = RecipeHelpers.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.CrimtaneBar, 1} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Jungle Equipment",
				new string[] { ItemGroupIDs.AnyEquipment },
				new ItemGroupMatcher( ( item, grps ) => {
					bool isEquip = grps[ItemGroupIDs.AnyEquipment].Contains( item.type );
					bool has = RecipeHelpers.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.JungleSpores, 1} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Bee Equipment",
				new string[] { ItemGroupIDs.AnyEquipment },
				new ItemGroupMatcher( ( item, grps ) => {
					bool isEquip = grps[ItemGroupIDs.AnyEquipment].Contains( item.type );
					bool has = RecipeHelpers.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.BeeWax, 1} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Bone Equipment",
				new string[] { ItemGroupIDs.AnyEquipment },
				new ItemGroupMatcher( ( item, grps ) => {
					bool isEquip = grps[ItemGroupIDs.AnyEquipment].Contains( item.type );
					bool has = RecipeHelpers.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.Bone, 1} } );
					
					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Hellstone Equipment",
				new string[] { ItemGroupIDs.AnyEquipment },
				new ItemGroupMatcher( ( item, grps ) => {
					bool isEquip = grps[ItemGroupIDs.AnyEquipment].Contains( item.type );
					bool has = RecipeHelpers.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.HellstoneBar, 1} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );

			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Cobalt Equipment",
				new string[] { ItemGroupIDs.AnyEquipment },
				new ItemGroupMatcher( ( item, grps ) => {
					bool isEquip = grps[ItemGroupIDs.AnyEquipment].Contains( item.type );
					bool has = RecipeHelpers.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.CobaltBar, 1} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Palladium Equipment",
				new string[] { ItemGroupIDs.AnyEquipment },
				new ItemGroupMatcher( ( item, grps ) => {
					bool isEquip = grps[ItemGroupIDs.AnyEquipment].Contains( item.type );
					bool has = RecipeHelpers.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.PalladiumBar, 1} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Mythril Equipment",
				new string[] { ItemGroupIDs.AnyEquipment },
				new ItemGroupMatcher( ( item, grps ) => {
					bool isEquip = grps[ItemGroupIDs.AnyEquipment].Contains( item.type );
					bool has = RecipeHelpers.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.MythrilBar, 1} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Orichalcum Equipment",
				new string[] { ItemGroupIDs.AnyEquipment },
				new ItemGroupMatcher( ( item, grps ) => {
					bool isEquip = grps[ItemGroupIDs.AnyEquipment].Contains( item.type );
					bool has = RecipeHelpers.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.OrichalcumBar, 1} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Adamantite Equipment",
				new string[] { ItemGroupIDs.AnyEquipment },
				new ItemGroupMatcher( ( item, grps ) => {
					bool isEquip = grps[ItemGroupIDs.AnyEquipment].Contains( item.type );
					bool has = RecipeHelpers.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.AdamantiteBar, 1} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Titanium Equipment",
				new string[] { ItemGroupIDs.AnyEquipment },
				new ItemGroupMatcher( ( item, grps ) => {
					bool isEquip = grps[ItemGroupIDs.AnyEquipment].Contains( item.type );
					bool has = RecipeHelpers.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.TitaniumBar, 1} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );

			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Frost Core Equipment",
				new string[] { ItemGroupIDs.AnyEquipment },
				new ItemGroupMatcher( ( item, grps ) => {
					bool isEquip = grps[ItemGroupIDs.AnyEquipment].Contains( item.type );
					bool has = RecipeHelpers.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.FrostCore, 1} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Forbidden Equipment",
				new string[] { ItemGroupIDs.AnyEquipment },
				new ItemGroupMatcher( ( item, grps ) => {
					bool isEquip = grps[ItemGroupIDs.AnyEquipment].Contains( item.type );
					bool has = RecipeHelpers.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.AncientBattleArmorMaterial, 1} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Hallow Equipment",
				new string[] { ItemGroupIDs.AnyEquipment },
				new ItemGroupMatcher( ( item, grps ) => {
					bool isEquip = grps[ItemGroupIDs.AnyEquipment].Contains( item.type );
					bool has = RecipeHelpers.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.HallowedBar, 1} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Chlorophyte Equipment",
				new string[] { ItemGroupIDs.AnyEquipment },
				new ItemGroupMatcher( ( item, grps ) => {
					bool isEquip = grps[ItemGroupIDs.AnyEquipment].Contains( item.type );
					bool has = RecipeHelpers.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.ChlorophyteBar, 1} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Shroomite Equipment",
				new string[] { ItemGroupIDs.AnyEquipment },
				new ItemGroupMatcher( ( item, grps ) => {
					bool isEquip = grps[ItemGroupIDs.AnyEquipment].Contains( item.type );
					bool has = RecipeHelpers.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.ShroomiteBar, 1} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Spectre Equipment",
				new string[] { ItemGroupIDs.AnyEquipment },
				new ItemGroupMatcher( ( item, grps ) => {
					bool isEquip = grps[ItemGroupIDs.AnyEquipment].Contains( item.type );
					bool has = RecipeHelpers.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.SpectreBar, 1} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Shell Equipment",
				new string[] { ItemGroupIDs.AnyEquipment },
				new ItemGroupMatcher( ( item, grps ) => {
					bool isEquip = grps[ItemGroupIDs.AnyEquipment].Contains( item.type );
					bool has1 = RecipeHelpers.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.BeetleShell, 1} } );
					bool has2 = RecipeHelpers.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.TurtleShell, 1} } );

					if( !isEquip || (!has1 && !has2) ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );

			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Nebula Equipment",
				new string[] { ItemGroupIDs.AnyEquipment },
				new ItemGroupMatcher( ( item, grps ) => {
					bool isEquip = grps[ItemGroupIDs.AnyEquipment].Contains( item.type );
					bool has = RecipeHelpers.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.FragmentNebula, 1} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Vortex Equipment",
				new string[] { ItemGroupIDs.AnyEquipment },
				new ItemGroupMatcher( ( item, grps ) => {
					bool isEquip = grps[ItemGroupIDs.AnyEquipment].Contains( item.type );
					bool has = RecipeHelpers.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.FragmentVortex, 1} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Solar Equipment",
				new string[] { ItemGroupIDs.AnyEquipment },
				new ItemGroupMatcher( ( item, grps ) => {
					bool isEquip = grps[ItemGroupIDs.AnyEquipment].Contains( item.type );
					bool isCraftedWithSolar = RecipeHelpers.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.FragmentSolar, 1} } );

					if( !isEquip || !isCraftedWithSolar ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Stardust Equipment",
				new string[] { ItemGroupIDs.AnyEquipment },
				new ItemGroupMatcher( ( item, grps ) => {
					bool isEquip = grps[ItemGroupIDs.AnyEquipment].Contains( item.type );
					bool isCraftedWithStardust = RecipeHelpers.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.FragmentStardust, 1} } );

					if( !isEquip || !isCraftedWithStardust ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Luminite Ore Equipment",
				new string[] { ItemGroupIDs.AnyEquipment },
				new ItemGroupMatcher( ( item, grps ) => {
					bool isEquip = grps[ItemGroupIDs.AnyEquipment].Contains( item.type );
					bool has = RecipeHelpers.ItemHasIngredients( item.type, new Dictionary<int, int> { { ItemID.LunarBar, 1} } );

					if( isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
		}
	}
}
