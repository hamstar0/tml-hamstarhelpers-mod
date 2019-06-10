using HamstarHelpers.Helpers.Debug;
using System;
using Terraria.ID;

using Matcher = System.Func<Terraria.Item, System.Collections.Generic.IDictionary<string, System.Collections.Generic.ISet<int>>, bool>;


namespace HamstarHelpers.Services.EntityGroups.Defs {
	partial class EntityGroupDefs {
		internal static void DefineItemPlaceablesGroups2( Action<string, string[], Matcher> addDef ) {
			// Materials
			addDef( "Any Wood", null,
				( item, grps ) => {
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
				} );

			// Stations
			
			addDef( "Any Workbench", null,
				( item, grps ) => {
					return item.createTile == 18;
				} );
			addDef( "Any Anvil", null,
				( item, grps ) => {
					return item.createTile == 16 || item.createTile == 134;
				} );
			addDef( "Any Forge", null,
				( item, grps ) => {
					return item.createTile == 18;
				} );
			addDef( "Any Table", null,
				( item, grps ) => {
					return item.createTile == 14;
				} );
			addDef( "Any Alchemy Station", null,
				( item, grps ) => {
					return item.createTile == 13 || item.createTile == 355;
				} );
			addDef( "Any Hardmode Crafting Station", null,
				( item, grps ) => {
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
				} );
			addDef( "Any Vanilla Themed Crafting Station", null,
				( item, grps ) => {
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
				} );
			addDef( "Any Misc Crafting Station", null,
				( item, grps ) => {
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
				} );

			// Chests

			addDef( "Any Chest", null,
				( item, grps ) => {
					return item.createTile == 21;
				} );

			// Wire

			addDef( "Any Wire Component", null,
				( item, grps ) => {
					if( item.createTile == -1 ) { return false; }
					return item.mech;
				} );
			addDef( "Any Trap Chest", null,
				( item, grps ) => {
					return item.createTile == 441;
				} );
			addDef( "Any Vanilla Wire Trap", null,
				( item, grps ) => {
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
				} );
			addDef( "Any Vanilla Wire Switch", null,
				( item, grps ) => {
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
				} );

			// Lights

			addDef( "Any Light", null,
				( item, grps ) => {
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
				} );
			addDef( "Any Candle", null,
				( item, grps ) => {
					switch( item.createTile ) {
					case 33:    // Candle
					case 174:   // Platinum Candle (?)
					case 372:   // Peace Candle
					case 49:    // Water Candle
						return true;
					}
					return false;
				} );
			addDef( "Any Wall Torch", null,
				( item, grps ) => {
					return item.createTile == 4;
				} );
			addDef( "Any Campfire", null,
				( item, grps ) => {
					return item.createTile == 215 || item.createTile == 405;
				} );

			// Misc

			addDef( "Any Statue", null,
				( item, grps ) => {
					return item.createTile == 105;
				} );
		}
	}
}
