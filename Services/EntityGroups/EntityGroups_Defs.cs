using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Collections.Generic;

using ItemMatcher = System.Func<Terraria.Item, System.Collections.Generic.IDictionary<string, System.Collections.Generic.ISet<int>>, bool>;
using NPCMatcher = System.Func<Terraria.NPC, System.Collections.Generic.IDictionary<string, System.Collections.Generic.ISet<int>>, bool>;
using ProjMatcher = System.Func<Terraria.Projectile, System.Collections.Generic.IDictionary<string, System.Collections.Generic.ISet<int>>, bool>;


namespace HamstarHelpers.Services.EntityGroups {
	public partial class EntityGroups {
		private IList<Tuple<string, string[], ItemMatcher>> DefineItemGroups() {
			var matchers = new List<Tuple<string, string[], ItemMatcher>>();

			Action<string, string[], ItemMatcher> add_item_grp_def = ( name, grps, matcher ) => {
				matchers.Add( new Tuple<string, string[], ItemMatcher>( name, grps, matcher ) );
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


		private IList<Tuple<string, string[], NPCMatcher>> DefineNPCGroups() {
			var matchers = new List<Tuple<string, string[], NPCMatcher>>();

			Action<string, string[], NPCMatcher> add_npc_grp_def = ( name, dependencies, matcher ) => {
				matchers.Add( new Tuple<string, string[], NPCMatcher>( name, dependencies, matcher ) );
			};

			this.DefineNPCGroups1( add_npc_grp_def );

			return matchers;
		}


		private IList<Tuple<string, string[], ProjMatcher>> DefineProjectileGroups() {
			var matchers = new List<Tuple<string, string[], ProjMatcher>>();
			return matchers;
		}
	}
}
