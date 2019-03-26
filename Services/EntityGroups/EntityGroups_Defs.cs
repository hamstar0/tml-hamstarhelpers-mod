using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Services.EntityGroups.Defs;
using System;
using System.Collections.Generic;

using ItemMatcher = System.Func<Terraria.Item, System.Collections.Generic.IDictionary<string, System.Collections.Generic.ISet<int>>, bool>;
using NPCMatcher = System.Func<Terraria.NPC, System.Collections.Generic.IDictionary<string, System.Collections.Generic.ISet<int>>, bool>;
using ProjMatcher = System.Func<Terraria.Projectile, System.Collections.Generic.IDictionary<string, System.Collections.Generic.ISet<int>>, bool>;


namespace HamstarHelpers.Services.EntityGroups {
	public partial class EntityGroups {
		private static IList<Tuple<string, string[], ItemMatcher>> DefineItemGroups() {
			var matchers = new List<Tuple<string, string[], ItemMatcher>>();

			Action<string, string[], ItemMatcher> addItemGrpDef = ( name, grps, matcher ) => {
				matchers.Add( new Tuple<string, string[], ItemMatcher>( name, grps, matcher ) );
			};
			
			EntityGroupDefs.DefineItemEquipmentGroups1( addItemGrpDef );
			EntityGroupDefs.DefineItemAccessoriesGroups1( addItemGrpDef );
			EntityGroupDefs.DefineItemWeaponGroups1( addItemGrpDef );
			EntityGroupDefs.DefineItemPlaceablesGroups1( addItemGrpDef );
			EntityGroupDefs.DefineItemMiscGroups1( addItemGrpDef );

			EntityGroupDefs.DefineItemEquipmentGroups2( addItemGrpDef );
			//EntityGroupDefs.DefineItemAccessoriesGroups2( addItemGrpDef );
			EntityGroupDefs.DefineItemWeaponGroups2( addItemGrpDef );
			EntityGroupDefs.DefineItemPlaceablesGroups2( addItemGrpDef );
			//EntityGroupDefs.DefineItemMiscGroups2( addItemGrpDef );

			EntityGroupDefs.DefineItemEquipmentGroups3( addItemGrpDef );
			//EntityGroupDefs.DefineItemAccessoriesGroups3( addItemGrpDef );
			//EntityGroupDefs.DefineItemWeaponGroups3( addItemGrpDef );
			//EntityGroupDefs.DefineItemPlaceablesGroups3( addItemGrpDef );
			EntityGroupDefs.DefineItemMiscGroups3( addItemGrpDef );

			EntityGroupDefs.DefineItemEquipmentGroups4( addItemGrpDef );
			//EntityGroupDefs.DefineItemAccessoriesGroups4( addItemGrpDef );
			//EntityGroupDefs.DefineItemWeaponGroups4( addItemGrpDef );
			//EntityGroupDefs.DefineItemPlaceablesGroups4( addItemGrpDef );
			EntityGroupDefs.DefineItemMiscGroups4( addItemGrpDef );
			
			return matchers;
		}


		private static IList<Tuple<string, string[], NPCMatcher>> DefineNPCGroups() {
			var matchers = new List<Tuple<string, string[], NPCMatcher>>();

			Action<string, string[], NPCMatcher> addNpcGrpDef = ( name, dependencies, matcher ) => {
				matchers.Add( new Tuple<string, string[], NPCMatcher>( name, dependencies, matcher ) );
			};

			EntityGroupDefs.DefineNPCGroups1( addNpcGrpDef );

			return matchers;
		}


		private static IList<Tuple<string, string[], ProjMatcher>> DefineProjectileGroups() {
			var matchers = new List<Tuple<string, string[], ProjMatcher>>();
			return matchers;
		}
	}
}
