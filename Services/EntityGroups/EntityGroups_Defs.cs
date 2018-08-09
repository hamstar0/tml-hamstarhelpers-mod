using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Services.EntityGroups {
	public partial class EntityGroups {
		private IList<KeyValuePair<string, Func<Item, bool>>> DefineItemGroups() {
			var matchers = new List<KeyValuePair<string, Func<Item, bool>>>();

			Action<string, Func<Item, bool>> add_item_grp_def = ( name, matcher ) => {
				matchers.Add( new KeyValuePair<string, Func<Item, bool>>( name, matcher ) );
			};
			
			this.DefineItemEquipmentGroups1( add_item_grp_def );
			this.DefineItemAccessoriesGroups1( add_item_grp_def );
			this.DefineItemWeaponGroups1( add_item_grp_def );
			this.DefineItemPlaceablesGroups1( add_item_grp_def );

			this.DefineItemEquipmentGroups2( add_item_grp_def );
			this.DefineItemWeaponGroups2( add_item_grp_def );
			this.DefineItemPlaceablesGroups2( add_item_grp_def );

			this.DefineItemEquipmentGroups3( add_item_grp_def );

			this.DefineItemMiscGroups4( add_item_grp_def );
			
			return matchers;
		}


		private IList<KeyValuePair<string, Func<NPC, bool>>> DefineNPCGroups() {
			var matchers = new List<KeyValuePair<string, Func<NPC, bool>>>();

			Action<string, Func<NPC, bool>> add_item_grp_def = ( name, matcher ) => {
				matchers.Add( new KeyValuePair<string, Func<NPC, bool>>( name, matcher ) );
			};

			this.DefineNPCGroups1( add_item_grp_def );

			return matchers;
		}


		private IList<KeyValuePair<string, Func<Projectile, bool>>> DefineProjectileGroups() {
			var matchers = new List<KeyValuePair<string, Func<Projectile, bool>>>();
			return matchers;
		}
	}
}
