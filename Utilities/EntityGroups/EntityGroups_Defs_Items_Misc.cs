using HamstarHelpers.DebugHelpers;
using System;
using Terraria;


namespace HamstarHelpers.Utilities.EntityGroups {
	public partial class EntityGroups {
		private void DefineItemMiscGroups3( Action<string, Func<Item, bool>> add_def ) {
			add_def( "Any Plain Material", ( Item item ) => {
				return item.material &&
					//!EntityGroups.ItemGroups["Any Placeable"].Contains( item.type ) &&
					!EntityGroups.ItemGroups["Any Equipment"].Contains( item.type );
			} );
		}
	}
}
