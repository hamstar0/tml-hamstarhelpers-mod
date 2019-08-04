using HamstarHelpers.Classes.DataStructures;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Helpers.Buffs {
	/// @private
	public partial class BuffIdentityHelpers {
		private ReadOnlyDictionaryOfSets<string, int> _NamesToIds = null;


		////////////////
		
		internal void PopulateNames() {
			var dict = new Dictionary<string, ISet<int>>();

			for( int i = 1; i < Main.buffTexture.Length; i++ ) {
				string name = BuffAttributesHelpers.GetBuffDisplayName( i );

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
