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

			Action<string, string[], ItemMatcher> addItemGrpDef = ( name, grps, matcher ) => {
				matchers.Add( new Tuple<string, string[], ItemMatcher>( name, grps, matcher ) );
			};
			
			this.DefineItemEquipmentGroups1( addItemGrpDef );
			this.DefineItemAccessoriesGroups1( addItemGrpDef );
			this.DefineItemWeaponGroups1( addItemGrpDef );
			this.DefineItemPlaceablesGroups1( addItemGrpDef );
			this.DefineItemMiscGroups1( addItemGrpDef );

			this.DefineItemEquipmentGroups2( addItemGrpDef );
			//this.DefineItemAccessoriesGroups2( addItemGrpDef );
			this.DefineItemWeaponGroups2( addItemGrpDef );
			this.DefineItemPlaceablesGroups2( addItemGrpDef );
			//this.DefineItemMiscGroups2( addItemGrpDef );

			this.DefineItemEquipmentGroups3( addItemGrpDef );
			//this.DefineItemAccessoriesGroups3( addItemGrpDef );
			//this.DefineItemWeaponGroups3( addItemGrpDef );
			//this.DefineItemPlaceablesGroups3( addItemGrpDef );
			this.DefineItemMiscGroups3( addItemGrpDef );

			this.DefineItemEquipmentGroups4( addItemGrpDef );
			//this.DefineItemAccessoriesGroups4( addItemGrpDef );
			//this.DefineItemWeaponGroups4( addItemGrpDef );
			//this.DefineItemPlaceablesGroups4( addItemGrpDef );
			this.DefineItemMiscGroups4( addItemGrpDef );
			
			return matchers;
		}


		private IList<Tuple<string, string[], NPCMatcher>> DefineNPCGroups() {
			var matchers = new List<Tuple<string, string[], NPCMatcher>>();

			Action<string, string[], NPCMatcher> addNpcGrpDef = ( name, dependencies, matcher ) => {
				matchers.Add( new Tuple<string, string[], NPCMatcher>( name, dependencies, matcher ) );
			};

			this.DefineNPCGroups1( addNpcGrpDef );

			return matchers;
		}


		private IList<Tuple<string, string[], ProjMatcher>> DefineProjectileGroups() {
			var matchers = new List<Tuple<string, string[], ProjMatcher>>();
			return matchers;
		}
	}
}
