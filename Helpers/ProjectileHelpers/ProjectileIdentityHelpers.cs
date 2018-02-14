using HamstarHelpers.Helpers.DotNetHelpers.DataStructures;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.ProjectileHelpers {
	public class ProjectileIdentityHelpers {
		// TODO: GetUniqueId()

		// TODO: GetVanillaSnapshotHash()


		////////////////

		public static ReadOnlyDictionaryOfSets<string, int> NamesToIds {
			get { return HamstarHelpersMod.Instance.ProjectileIdentityHelpers._NamesToIds; }
		}



		////////////////
		
		private ReadOnlyDictionaryOfSets<string, int> _NamesToIds = null;


		////////////////

		internal void PopulateNames() {
			var dict = new Dictionary<string, ISet<int>>();

			for( int i = 1; i < ProjectileLoader.ProjectileCount; i++ ) {
				string name = Lang.GetProjectileName( i ).Value;

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
