using HamstarHelpers.Components.DataStructures;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.ItemHelpers {
	public partial class ItemIdentityHelpers {
		public static ReadOnlyDictionaryOfSets<string, int> NamesToIds {
			get {
				return ModHelpersMod.Instance.ItemIdentityHelpers._NamesToIds;
			}
		}
		
		private ReadOnlyDictionaryOfSets<string, int> _NamesToIds = null;

		

		////////////////

		internal void PopulateNames() {
			var dict = new Dictionary<string, ISet<int>>();

			for( int i = 1; i < ItemLoader.ItemCount; i++ ) {
				string name = Lang.GetItemNameValue( i );

				if( dict.ContainsKey( name ) ) {
					dict[name].Add( i );
				} else {
					dict[name] = new HashSet<int>() { i };
				}
			}

			this._NamesToIds = new ReadOnlyDictionaryOfSets<string, int>( dict );
		}
	}
}
