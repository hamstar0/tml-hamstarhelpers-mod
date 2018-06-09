using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;


namespace HamstarHelpers.Utilities.EntityGroups {
	public partial class EntityGroups {
		private void DefineGroups( IList<KeyValuePair<string, Func<Item, bool>>> matchers ) {
			void add_item_grp_def( string name, Func<Item, bool> matcher ) {
				matchers.Add( new KeyValuePair<string, Func<Item, bool>>( name, matcher ) );
			}

			this.DefineItemEquipmentGroups1( add_item_grp_def );
			this.DefineItemPlaceablesGroups1( add_item_grp_def );

			this.DefineItemEquipmentGroups2( add_item_grp_def );
			this.DefineItemPlaceablesGroups2( add_item_grp_def );

			this.DefineItemEquipmentGroups3( add_item_grp_def );
			this.DefineItemPlaceablesGroups3( add_item_grp_def );
		}
	}
}
