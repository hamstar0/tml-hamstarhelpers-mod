using HamstarHelpers.Components.DataStructures;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.NPCs {
	public partial class NPCIdentityHelpers {
		private ReadOnlyDictionaryOfSets<string, int> _NamesToIds = null;


		////////////////

		internal void PopulateNames() {
			var dict = new Dictionary<string, ISet<int>>();

			for( int i = 1; i < NPCLoader.NPCCount; i++ ) {
				string name = Lang.GetNPCNameValue( i );
				
				if( dict.ContainsKey(name) ) {
					dict[name].Add( i );
				} else {
					dict[name] = new HashSet<int>() { i };
				}
			}

			this._NamesToIds = new ReadOnlyDictionaryOfSets<string, int>( dict );
		}
	}
}
