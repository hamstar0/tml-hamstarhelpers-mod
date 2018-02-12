using HamstarHelpers.Helpers.DotNetHelpers.DataStructures;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.BuffHelpers {
	public class BuffIdentityHelpers {
		public static ReadOnlyDictionaryOfSets<string, int> NamesToIds {
			get { return HamstarHelpersMod.Instance.BuffIdentityHelpers._NamesToIds; }
		}



		////////////////
		
		private ReadOnlyDictionaryOfSets<string, int> _NamesToIds = null;


		////////////////
		
		internal void OnPostSetupContent() {
			var dict = new Dictionary<string, ISet<int>>();

			for( int i = 1; i < Main.buffTexture.Length; i++ ) {
				string name = Lang.GetBuffName( i );

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
