using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;


namespace HamstarHelpers.Utilities.EntityGroups {
	public partial class EntityGroups {
		private void DefineItemPlaceablesGroups( IList<KeyValuePair<string, Func<Item, bool>>> matchers ) {
			void add_grp_def( string name, Func<Item, bool> matcher ) {
				matchers.Add( new KeyValuePair<string, Func<Item, bool>>( name, matcher ) );
			}

			add_grp_def( "Any Placeable", ( item ) => {
				return item.createTile != -1 || item.createWall != -1;
			} );
			add_grp_def( "Any Tile", ( item ) => {
				return item.createTile != -1;
			} );
			add_grp_def( "Any Wall", ( item ) => {
				return item.createWall != -1;
			} );
			
			// Stations

			add_grp_def( "Any Vanilla Workbench", ( item ) => {
			} );
			add_grp_def( "Any Vanilla Anvil", ( item ) => {
			} );
			add_grp_def( "Any Vanilla Ore Furnace", ( item ) => {
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
			add_grp_def( "Any Vanilla Table", ( item ) => {
			} );
			add_grp_def( "Any Vanilla Misc Crafting Station", ( item ) => {
			} );
			add_grp_def( "Any Vanilla Chest", ( item ) => {
			} );

			// Wire

			add_grp_def( "Any Vanilla Wire Chest", ( item ) => {
			} );
			add_grp_def( "Any Vanilla Wire Switch", ( item ) => {
			} );
			add_grp_def( "Any Vanilla Wire Light", ( item ) => {
			} );
			add_grp_def( "Any Vanilla Wire Trap", ( item ) => {
			} );
			add_grp_def( "Any Vanilla Wire Misc", ( item ) => {
			} );
			
			// General Stations

			add_grp_def( "Any Vanilla Crafting Station", ( item ) => {
			} );
			add_grp_def( "Any Vanilla Wire Component", ( item ) => {
			} );
		}
	}
}
