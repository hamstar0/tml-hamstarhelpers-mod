using System;
using Terraria;
using Terraria.ID;


namespace HamstarHelpers.Utilities.EntityGroups {
	public partial class EntityGroups {
		private void DefineItemPlaceablesGroups1( Action<string, Func<Item, bool>> add_def ) {
			add_def( "Any Placeable", ( item ) => {
				return item.createTile != -1 || item.createWall != -1;
			} );
			add_def( "Any Tile", ( item ) => {
				return item.createTile != -1;
			} );
			add_def( "Any Wall", ( item ) => {
				return item.createWall != -1;
			} );
		}


		private void DefineItemPlaceablesGroups2( Action<string, Func<Item, bool>> add_def ) {
			// Materials

			add_def( "Any Wood", ( item ) => {

			} );

			// Stations

			add_def( "Any Vanilla Workbench", ( item ) => {
			} );
			add_def( "Any Vanilla Anvil", ( item ) => {
			} );
			add_def( "Any Vanilla Ore Furnace", ( item ) => {
				switch( item.type ) {
				case ItemID.Furnace:
				case ItemID.Hellforge:
				case ItemID.AdamantiteForge:
				case ItemID.TitaniumForge:
					return true;
				default:
					return false;
				}
			} );
			add_def( "Any Vanilla Table", ( item ) => {
			} );
			add_def( "Any Vanilla Misc Crafting Station", ( item ) => {
			} );
			add_def( "Any Vanilla Chest", ( item ) => {
			} );

			// Wire

			add_def( "Any Vanilla Wire Chest", ( item ) => {
			} );
			add_def( "Any Vanilla Wire Switch", ( item ) => {
			} );
			add_def( "Any Vanilla Wire Light", ( item ) => {
			} );
			add_def( "Any Vanilla Wire Trap", ( item ) => {
			} );
			add_def( "Any Vanilla Wire Misc", ( item ) => {
			} );

			// General Stations

			add_def( "Any Vanilla Crafting Station", ( item ) => {
			} );
			add_def( "Any Vanilla Wire Component", ( item ) => {
			} );
		}


		private void DefineItemPlaceablesGroups3( Action<string, Func<Item, bool>> add_def ) {
		}
	}
}
