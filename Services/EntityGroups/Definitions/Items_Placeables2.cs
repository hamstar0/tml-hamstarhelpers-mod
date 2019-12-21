using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TileHelpers;
using HamstarHelpers.Helpers.Tiles;
using HamstarHelpers.Helpers.Tiles.Attributes;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace HamstarHelpers.Services.EntityGroups.Definitions {
	/// <summary></summary>
	public partial class ItemGroupIDs {
		/// <summary></summary>
		public const string AnyWood = "Any Wood";
		/// <summary></summary>
		public const string AnyOreBar = "Any Ore Bar";
		/// <summary></summary>
		public const string AnyWorkbench = "Any Workbench";
		/// <summary></summary>
		public const string AnyAnvil = "Any Anvil";
		/// <summary></summary>
		public const string AnyForge = "Any Forge";
		/// <summary></summary>
		public const string AnyTable = "Any Table";
		/// <summary></summary>
		public const string AnyAlchemyStation = "Any Alchemy Station";
		/// <summary></summary>
		public const string AnyHardmodeCraftingStation = "Any Hardmode Crafting Station";
		/// <summary></summary>
		public const string AnyVanillaThemedCraftingStation = "Any Vanilla Themed Crafting Station";
		/// <summary></summary>
		public const string AnyMiscCraftingStation = "Any Misc Crafting Station";
		/// <summary></summary>
		public const string AnyChest = "Any Chest";
		/// <summary></summary>
		public const string AnyWireComponent = "Any Wire Component";
		/// <summary></summary>
		public const string AnyTrapChest = "Any Trap Chest";
		/// <summary></summary>
		public const string AnyVanillaWireTrap = "Any Vanilla Wire Trap";
		/// <summary></summary>
		public const string AnyVanillaWireSwitch = "Any Vanilla Wire Switch";
		/// <summary></summary>
		public const string AnyLight = "Any Light";
		/// <summary></summary>
		public const string AnyCandle = "Any Candle";
		/// <summary></summary>
		public const string AnyWallTorch = "Any Wall Torch";
		/// <summary></summary>
		public const string AnyCampfire = "Any Campfire";
		/// <summary></summary>
		public const string AnyStatue = "Any Statue";
	}




	partial class EntityGroupDefs {
		internal static void DefineItemPlaceablesGroups2( IList<EntityGroupMatcherDefinition<Item>> defs ) {
			// Materials
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				ItemGroupIDs.AnyWood,
				null,
				new ItemGroupMatcher( ( item, grps ) => {
					switch( item.type ) {
					case ItemID.Wood:
					case ItemID.RichMahogany:
					case ItemID.Ebonwood:
					case ItemID.Shadewood:
					case ItemID.Pearlwood:
					case ItemID.BorealWood:
					case ItemID.PalmWood:
					case ItemID.DynastyWood:
					case ItemID.SpookyWood:
						return true;
					}
					return false;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				ItemGroupIDs.AnyOreBar,
				new string[] { ItemGroupIDs.AnyTile },
				new ItemGroupMatcher( ( item, grps ) => {
					if( !grps[ItemGroupIDs.AnyTile].Contains( item.type ) ) { return false; }
					if( item.createTile != 239 ) {
						if( item.createTile <= 0 ) { return false; }
						if( !Main.tileSolid[item.createTile] ) { return false; }
						if( !Main.tileSolidTop[item.createTile] ) { return false; }
						if( Main.tileShine[item.createTile] == 0 ) { return false; }

						if( item.modItem == null ) { return false; }
						if( !item.modItem.Name.EndsWith(" Bar") ) { return false; }
					}
					return true;
				} )
			) );

			// Stations

			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Workbench", null,
				new ItemGroupMatcher( ( item, grps ) => {
					return item.createTile == 18;
				} )
			) );

			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Anvil", null,
				new ItemGroupMatcher( ( item, grps ) => {
					return item.createTile == 16 || item.createTile == 134;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Forge", null,
				new ItemGroupMatcher( ( item, grps ) => {
					return item.createTile == 18;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Table", null,
				new ItemGroupMatcher( ( item, grps ) => {
					return item.createTile == 14;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Alchemy Station", null,
				new ItemGroupMatcher( ( item, grps ) => {
					return item.createTile == 13 || item.createTile == 355;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Hardmode Crafting Station", null,
				new ItemGroupMatcher( ( item, grps ) => {
					switch( item.createTile ) {
					case 133:   // Hardmode Forge
					case 134:   // Hardmode Anvil
					case 101:   // Bookshelf
					case 125:	// Crystal Ball
					case 217:   // Blend-O-Matic
					case 218:   // Meat Grinder
					case 247:   // Autohammer
					case 412:	// Ancient Manipulator
						return true;
					}
					return false;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Vanilla Themed Crafting Station", null,
				new ItemGroupMatcher( ( item, grps ) => {
					switch( item.type ) {
					case ItemID.Solidifier:
					case ItemID.GlassKiln:
					case ItemID.BoneWelder:
					case ItemID.FleshCloningVaat:
					case ItemID.LihzahrdFurnace:
					case ItemID.SkyMill:
					case ItemID.HoneyDispenser:
					case ItemID.LivingLoom:
					case ItemID.IceMachine:
					case ItemID.SteampunkBoiler:
						return true;
					}
					return false;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Misc Crafting Station", null,
				new ItemGroupMatcher( ( item, grps ) => {
					switch( item.createTile ) {
					case 86:    // Loom
					case 94:    // Keg
					case 96:    // Cooking Pot
					case 106:   // Sawmill
					case 114:   // Tinkerer's Workshop
					case 228:   // Dye Vat
					case 243:   // Imbuing Station
					case 283:   // Heavy Work Bench
						return true;
					}
					return false;
				} )
			) );

			// Chests

			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Chest", null,
				new ItemGroupMatcher( ( item, grps ) => {
					return item.createTile == 21;
				} )
			) );

			// Wire

			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Wire Component", null,
				new ItemGroupMatcher( ( item, grps ) => {
					if( item.createTile == -1 ) { return false; }
					return item.mech;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Trap Chest", null,
				new ItemGroupMatcher( ( item, grps ) => {
					return item.createTile == 441;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Vanilla Wire Trap", null,
				new ItemGroupMatcher( ( item, grps ) => {
					switch( item.type ) {
					case ItemID.DartTrap:
					case ItemID.SuperDartTrap:
					case ItemID.SpikyBallTrap:
					case ItemID.SpearTrap:
					case ItemID.FlameTrap:
					case ItemID.GeyserTrap:
					case ItemID.Explosives:
					case ItemID.LandMine:
					case ItemID.Cannon:
					case ItemID.BunnyCannon:
					case ItemID.RedRocket:
					case ItemID.BlueRocket:
					case ItemID.GreenRocket:
					case ItemID.YellowRocket:
					//case ItemID.PortalGunStation:
					//case ItemID.Teleporter:
						return true;
					}
					return false;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Vanilla Wire Switch", null,
				new ItemGroupMatcher( ( item, grps ) => {
					switch( item.type ) {
					case ItemID.Switch:
					case ItemID.Lever:
					case ItemID.BluePressurePlate:
					case ItemID.BrownPressurePlate:
					case ItemID.GrayPressurePlate:
					case ItemID.GreenPressurePlate:
					case ItemID.RedPressurePlate:
					case ItemID.YellowPressurePlate:
					case ItemID.LihzahrdPressurePlate:
					case ItemID.ProjectilePressurePad:
					case ItemID.WeightedPressurePlateCyan:
					case ItemID.WeightedPressurePlateOrange:
					case ItemID.WeightedPressurePlatePink:
					case ItemID.WeightedPressurePlatePurple:
					case ItemID.PressureTrack:
					case ItemID.Detonator:
					case ItemID.GemLockAmber:
					case ItemID.GemLockAmethyst:
					case ItemID.GemLockDiamond:
					case ItemID.GemLockEmerald:
					case ItemID.GemLockRuby:
					case ItemID.GemLockSapphire:
					case ItemID.GemLockTopaz:
					case ItemID.Timer1Second:
					case ItemID.Timer3Second:
					case ItemID.Timer5Second:
						return true;
					}
					return false;
				} )
			) );

			// Lights

			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Light", null,
				new ItemGroupMatcher( ( item, grps ) => {
					switch( item.createTile ) {
					case 4:		// Torch
					case 33:	// Candle
					case 174:   // Platinum Candle (?)
					case 372:   // Peace Candle
					case 49:    // Water Candle
					case 98:    // Skull Lantern
					case 100:   // Candelabra
					case 173:   // Platinum Candelabra (?)
					case 215:   // Campfire
					case 93:    // Lamp, Tiki Torch
					case 42:    // Lantern
					case 34:    // Large Dynasty Lantern
					case 95:    // Chinese Lantern
					case 270:   // Firefly in a Bottle
					case 271:   // Lightning Bug in a Bottle
					case 35:    // Jack O' Lantern
					case 92:    // Lamp Post
					case 405:   // Fireplace
					case 50:    // Christmas Light
					case 262:   // Gemspark Block
					case 429:   // Wire Bulb
					case 18:    // Furnace/Forge
					case 126:	// Disco Ball
						return true;
					}
					return false;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Candle", null,
				new ItemGroupMatcher( ( item, grps ) => {
					switch( item.createTile ) {
					case 33:    // Candle
					case 174:   // Platinum Candle (?)
					case 372:   // Peace Candle
					case 49:    // Water Candle
						return true;
					}
					return false;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Wall Torch", null,
				new ItemGroupMatcher( ( item, grps ) => {
					return item.createTile == 4;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Campfire", null,
				new ItemGroupMatcher( ( item, grps ) => {
					return item.createTile == 215 || item.createTile == 405;
				} )
			) );

			// Misc

			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Statue", null,
				new ItemGroupMatcher( ( item, grps ) => {
					return item.createTile == 105;
				} )
			) );
		}
	}
}
