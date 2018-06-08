using HamstarHelpers.ItemHelpers;


namespace HamstarHelpers.Utilities.EntityGroups {
	public partial class EntityGroups {
		private void DefinePrimaryItemGroups() {
			this.ItemMatchers["Any Placeable"] = ( item ) => {
				return item.createTile != -1 || item.createWall != -1;
			};

			this.ItemMatchers["Any Weapon"] = ( item ) => {
				return item.damage > 0;
			};
			this.ItemMatchers["Any Tool"] = ( item ) => {
				return ItemAttributeHelpers.IsTool( item );
			};

			this.ItemMatchers["Any Accessory"] = ( item ) => {
				return item.accessory && !item.vanity;
			};
			this.ItemMatchers["Any Armor"] = ( item ) => {
				return ItemAttributeHelpers.IsArmor( item );
			};
			this.ItemMatchers["Any Vanity"] = ( item ) => {
				return item.vanity;
			};

			this.ItemMatchers["Any Ranged Weapon"] = ( item ) => {
				return item.ranged;
			};
			this.ItemMatchers["Any Magic Weapon"] = ( item ) => {
				return item.magic;
			};
			this.ItemMatchers["Any Melee Weapon"] = ( item ) => {
				return item.melee;
			};
			this.ItemMatchers["Any Thrown Weapon"] = ( item ) => {
				return item.thrown;
			};
		}


		private void DefineSecondaryItemGroups() {
			// weapon sub types
			// equipment tiers
			// accessory types
			// 
		}
	}
}
