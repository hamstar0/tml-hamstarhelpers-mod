using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Libraries.Debug;
using HamstarHelpers.Libraries.Items.Attributes;
using HamstarHelpers.Libraries.Recipes;


namespace HamstarHelpers.Services.EntityGroups.Definitions {
	/// <summary></summary>
	public partial class ItemGroupIDs {
		/// <summary></summary>
		public const string AnyOreBarEquipment = "Any Ore Bar Equipment";
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
				grpName: ItemGroupIDs.AnyOreBarEquipment,
				grpDeps: new string[] { ItemGroupIDs.AnyEquipment, ItemGroupIDs.AnyOreBar },
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					ISet<int> equipGrp = grps[ItemGroupIDs.AnyEquipment];
					ISet<int> oreBarGrp = grps[ItemGroupIDs.AnyEquipment];
					bool isEquip = equipGrp.Contains( item.type );
					bool has = RecipeLibraries.ItemHasIngredients( item.type, oreBarGrp.ToDictionary(i=>i, i=>1) );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );

			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: "Any Tiki Equipment",
				grpDeps: new string[] { ItemGroupIDs.AnyEquipment },
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					ISet<int> equipGrp = grps[ItemGroupIDs.AnyEquipment];
					bool isEquip = equipGrp.Contains( item.type );
					string name = ItemAttributeLibraries.GetQualifiedName( item );

					if( !isEquip || !name.Contains( "Tiki" ) ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );

			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: "Any Cactus Equipment",
				grpDeps: new string[] { ItemGroupIDs.AnyEquipment, },
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					ISet<int> equipGrp = grps[ItemGroupIDs.AnyEquipment];
					bool isEquip = equipGrp.Contains( item.type );
					bool has = RecipeLibraries.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.Cactus, 1} } );

					if( !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );

			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: "Any Plain Wood Equipment",
				grpDeps: new string[] { ItemGroupIDs.AnyEquipment },
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					ISet<int> equipGrp = grps[ItemGroupIDs.AnyEquipment];
					bool isEquip = equipGrp.Contains( item.type );
					bool has = RecipeLibraries.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.Wood, 2} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: "Any Boreal Wood Equipment",
				grpDeps: new string[] { ItemGroupIDs.AnyEquipment },
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					ISet<int> equipGrp = grps[ItemGroupIDs.AnyEquipment];
					bool isEquip = equipGrp.Contains( item.type );
					bool has = RecipeLibraries.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.BorealWood, 2} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: "Any Palm Wood Equipment",
				grpDeps: new string[] { ItemGroupIDs.AnyEquipment },
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					ISet<int> equipGrp = grps[ItemGroupIDs.AnyEquipment];
					bool isEquip = equipGrp.Contains( item.type );
					bool has = RecipeLibraries.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.PalmWood, 2} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: "Any Rich Mahogany Equipment",
				grpDeps: new string[] { ItemGroupIDs.AnyEquipment },
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					ISet<int> equipGrp = grps[ItemGroupIDs.AnyEquipment];
					bool isEquip = equipGrp.Contains( item.type );
					bool has = RecipeLibraries.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.RichMahogany, 2} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: "Any Ebonwood Equipment",
				grpDeps: new string[] { ItemGroupIDs.AnyEquipment },
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					ISet<int> equipGrp = grps[ItemGroupIDs.AnyEquipment];
					bool isEquip = equipGrp.Contains( item.type );
					bool has = RecipeLibraries.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.Ebonwood, 2} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: "Any Shadewood Equipment",
				grpDeps: new string[] { ItemGroupIDs.AnyEquipment },
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					ISet<int> equipGrp = grps[ItemGroupIDs.AnyEquipment];
					bool isEquip = equipGrp.Contains( item.type );
					bool has = RecipeLibraries.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.Shadewood, 2} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: "Any Pearlwood Equipment",
				grpDeps: new string[] { ItemGroupIDs.AnyEquipment },
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					ISet<int> equipGrp = grps[ItemGroupIDs.AnyEquipment];
					bool isEquip = equipGrp.Contains( item.type );
					bool has = RecipeLibraries.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.Pearlwood, 2} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: "Any Spooky Wood Equipment",
				grpDeps: new string[] { ItemGroupIDs.AnyEquipment },
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					ISet<int> equipGrp = grps[ItemGroupIDs.AnyEquipment];
					bool isEquip = equipGrp.Contains( item.type );
					bool has = RecipeLibraries.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.SpookyWood, 2} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );

			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: "Any Tin Equipment",
				grpDeps: new string[] { ItemGroupIDs.AnyEquipment },
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					ISet<int> equipGrp = grps[ItemGroupIDs.AnyEquipment];
					bool isEquip = equipGrp.Contains( item.type );
					bool has = RecipeLibraries.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.TinBar, 1} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: "Any Copper Equipment",
				grpDeps: new string[] { ItemGroupIDs.AnyEquipment },
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					ISet<int> equipGrp = grps[ItemGroupIDs.AnyEquipment];
					bool isEquip = equipGrp.Contains( item.type );
					bool has = RecipeLibraries.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.CopperBar, 1} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );

			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: "Any Iron Equipment",
				grpDeps: new string[] { ItemGroupIDs.AnyEquipment },
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					ISet<int> equipGrp = grps[ItemGroupIDs.AnyEquipment];
					bool isEquip = equipGrp.Contains( item.type );
					bool has = RecipeLibraries.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.IronBar, 1} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: "Any Lead Equipment",
				grpDeps: new string[] { ItemGroupIDs.AnyEquipment },
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					ISet<int> equipGrp = grps[ItemGroupIDs.AnyEquipment];
					bool isEquip = equipGrp.Contains( item.type );
					bool has = RecipeLibraries.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.LeadBar, 1} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );

			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: "Any Silver Equipment",
				grpDeps: new string[] { ItemGroupIDs.AnyEquipment },
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					ISet<int> equipGrp = grps[ItemGroupIDs.AnyEquipment];
					bool isEquip = equipGrp.Contains( item.type );
					bool has = RecipeLibraries.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.SilverBar, 1} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: "Any Tungsten Equipment",
				grpDeps: new string[] { ItemGroupIDs.AnyEquipment },
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					ISet<int> equipGrp = grps[ItemGroupIDs.AnyEquipment];
					bool isEquip = equipGrp.Contains( item.type );
					bool has = RecipeLibraries.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.TungstenBar, 1} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );

			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: "Any Gold Equipment",
				grpDeps: new string[] { ItemGroupIDs.AnyEquipment },
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					ISet<int> equipGrp = grps[ItemGroupIDs.AnyEquipment];
					bool isEquip = equipGrp.Contains( item.type );
					bool has = RecipeLibraries.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.GoldBar, 1} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: "Any Platinum Equipment",
				grpDeps: new string[] { ItemGroupIDs.AnyEquipment },
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					ISet<int> equipGrp = grps[ItemGroupIDs.AnyEquipment];
					bool isEquip = equipGrp.Contains( item.type );
					bool has = RecipeLibraries.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.PlatinumBar, 1} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );

			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: "Any Meteor Equipment",
				grpDeps: new string[] { ItemGroupIDs.AnyEquipment },
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					ISet<int> equipGrp = grps[ItemGroupIDs.AnyEquipment];
					bool isEquip = equipGrp.Contains( item.type );
					bool has = RecipeLibraries.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.MeteoriteBar, 1} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: "Any Demonite Equipment",
				grpDeps: new string[] { ItemGroupIDs.AnyEquipment },
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					ISet<int> equipGrp = grps[ItemGroupIDs.AnyEquipment];
					bool isEquip = equipGrp.Contains( item.type );
					bool has = RecipeLibraries.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.DemoniteBar, 1} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: "Any Crimtane Equipment",
				grpDeps: new string[] { ItemGroupIDs.AnyEquipment },
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					ISet<int> equipGrp = grps[ItemGroupIDs.AnyEquipment];
					bool isEquip = equipGrp.Contains( item.type );
					bool has = RecipeLibraries.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.CrimtaneBar, 1} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: "Any Jungle Equipment",
				grpDeps: new string[] { ItemGroupIDs.AnyEquipment },
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					ISet<int> equipGrp = grps[ItemGroupIDs.AnyEquipment];
					bool isEquip = equipGrp.Contains( item.type );
					bool has = RecipeLibraries.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.JungleSpores, 1} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: "Any Bee Equipment",
				grpDeps: new string[] { ItemGroupIDs.AnyEquipment },
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					ISet<int> equipGrp = grps[ItemGroupIDs.AnyEquipment];
					bool isEquip = equipGrp.Contains( item.type );
					bool has = RecipeLibraries.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.BeeWax, 1} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: "Any Bone Equipment",
				grpDeps: new string[] { ItemGroupIDs.AnyEquipment },
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					ISet<int> equipGrp = grps[ItemGroupIDs.AnyEquipment];
					bool isEquip = equipGrp.Contains( item.type );
					bool has = RecipeLibraries.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.Bone, 1} } );
					
					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: "Any Hellstone Equipment",
				grpDeps: new string[] { ItemGroupIDs.AnyEquipment },
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					ISet<int> equipGrp = grps[ItemGroupIDs.AnyEquipment];
					bool isEquip = equipGrp.Contains( item.type );
					bool has = RecipeLibraries.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.HellstoneBar, 1} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );

			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: "Any Cobalt Equipment",
				grpDeps: new string[] { ItemGroupIDs.AnyEquipment },
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					ISet<int> equipGrp = grps[ItemGroupIDs.AnyEquipment];
					bool isEquip = equipGrp.Contains( item.type );
					bool has = RecipeLibraries.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.CobaltBar, 1} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: "Any Palladium Equipment",
				grpDeps: new string[] { ItemGroupIDs.AnyEquipment },
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					ISet<int> equipGrp = grps[ItemGroupIDs.AnyEquipment];
					bool isEquip = equipGrp.Contains( item.type );
					bool has = RecipeLibraries.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.PalladiumBar, 1} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: "Any Mythril Equipment",
				grpDeps: new string[] { ItemGroupIDs.AnyEquipment },
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					ISet<int> equipGrp = grps[ItemGroupIDs.AnyEquipment];
					bool isEquip = equipGrp.Contains( item.type );
					bool has = RecipeLibraries.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.MythrilBar, 1} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: "Any Orichalcum Equipment",
				grpDeps: new string[] { ItemGroupIDs.AnyEquipment },
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					ISet<int> equipGrp = grps[ItemGroupIDs.AnyEquipment];
					bool isEquip = equipGrp.Contains( item.type );
					bool has = RecipeLibraries.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.OrichalcumBar, 1} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: "Any Adamantite Equipment",
				grpDeps: new string[] { ItemGroupIDs.AnyEquipment },
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					ISet<int> equipGrp = grps[ItemGroupIDs.AnyEquipment];
					bool isEquip = equipGrp.Contains( item.type );
					bool has = RecipeLibraries.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.AdamantiteBar, 1} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: "Any Titanium Equipment",
				grpDeps: new string[] { ItemGroupIDs.AnyEquipment },
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					ISet<int> equipGrp = grps[ItemGroupIDs.AnyEquipment];
					bool isEquip = equipGrp.Contains( item.type );
					bool has = RecipeLibraries.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.TitaniumBar, 1} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );

			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: "Any Frost Core Equipment",
				grpDeps: new string[] { ItemGroupIDs.AnyEquipment },
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					ISet<int> equipGrp = grps[ItemGroupIDs.AnyEquipment];
					bool isEquip = equipGrp.Contains( item.type );
					bool has = RecipeLibraries.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.FrostCore, 1} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: "Any Forbidden Equipment",
				grpDeps: new string[] { ItemGroupIDs.AnyEquipment },
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					ISet<int> equipGrp = grps[ItemGroupIDs.AnyEquipment];
					bool isEquip = equipGrp.Contains( item.type );
					bool has = RecipeLibraries.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.AncientBattleArmorMaterial, 1} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: "Any Hallow Equipment",
				grpDeps: new string[] { ItemGroupIDs.AnyEquipment },
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					ISet<int> equipGrp = grps[ItemGroupIDs.AnyEquipment];
					bool isEquip = equipGrp.Contains( item.type );
					bool has = RecipeLibraries.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.HallowedBar, 1} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: "Any Chlorophyte Equipment",
				grpDeps: new string[] { ItemGroupIDs.AnyEquipment },
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					ISet<int> equipGrp = grps[ItemGroupIDs.AnyEquipment];
					bool isEquip = equipGrp.Contains( item.type );
					bool has = RecipeLibraries.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.ChlorophyteBar, 1} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: "Any Shroomite Equipment",
				grpDeps: new string[] { ItemGroupIDs.AnyEquipment },
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					ISet<int> equipGrp = grps[ItemGroupIDs.AnyEquipment];
					bool isEquip = equipGrp.Contains( item.type );
					bool has = RecipeLibraries.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.ShroomiteBar, 1} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: "Any Spectre Equipment",
				grpDeps: new string[] { ItemGroupIDs.AnyEquipment },
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					ISet<int> equipGrp = grps[ItemGroupIDs.AnyEquipment];
					bool isEquip = equipGrp.Contains( item.type );
					bool has = RecipeLibraries.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.SpectreBar, 1} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: "Any Shell Equipment",
				grpDeps: new string[] { ItemGroupIDs.AnyEquipment },
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					ISet<int> equipGrp = grps[ItemGroupIDs.AnyEquipment];
					bool isEquip = equipGrp.Contains( item.type );
					bool has1 = RecipeLibraries.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.BeetleShell, 1} } );
					bool has2 = RecipeLibraries.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.TurtleShell, 1} } );

					if( !isEquip || (!has1 && !has2) ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );

			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: "Any Nebula Equipment",
				grpDeps: new string[] { ItemGroupIDs.AnyEquipment },
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					ISet<int> equipGrp = grps[ItemGroupIDs.AnyEquipment];
					bool isEquip = equipGrp.Contains( item.type );
					bool has = RecipeLibraries.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.FragmentNebula, 1} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: "Any Vortex Equipment",
				grpDeps: new string[] { ItemGroupIDs.AnyEquipment },
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					ISet<int> equipGrp = grps[ItemGroupIDs.AnyEquipment];
					bool isEquip = equipGrp.Contains( item.type );
					bool has = RecipeLibraries.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.FragmentVortex, 1} } );

					if( !isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: "Any Solar Equipment",
				grpDeps: new string[] { ItemGroupIDs.AnyEquipment },
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					ISet<int> equipGrp = grps[ItemGroupIDs.AnyEquipment];
					bool isEquip = equipGrp.Contains( item.type );
					bool isCraftedWithSolar = RecipeLibraries.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.FragmentSolar, 1} } );

					if( !isEquip || !isCraftedWithSolar ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: "Any Stardust Equipment",
				grpDeps: new string[] { ItemGroupIDs.AnyEquipment },
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					ISet<int> equipGrp = grps[ItemGroupIDs.AnyEquipment];
					bool isEquip = equipGrp.Contains( item.type );
					bool isCraftedWithStardust = RecipeLibraries.ItemHasIngredients( item.type, new Dictionary<int, int> { {ItemID.FragmentStardust, 1} } );

					if( !isEquip || !isCraftedWithStardust ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: "Any Luminite Ore Equipment",
				grpDeps: new string[] { ItemGroupIDs.AnyEquipment },
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					ISet<int> equipGrp = grps[ItemGroupIDs.AnyEquipment];
					bool isEquip = equipGrp.Contains( item.type );
					bool has = RecipeLibraries.ItemHasIngredients( item.type, new Dictionary<int, int> { { ItemID.LunarBar, 1} } );

					if( isEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} )
			) );
		}
	}
}
