using System.Collections.Generic;
using System.Collections.ObjectModel;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.ProjectileHelpers {
	public class ProjectileIdentityHelpers {
		// TODO: GetUniqueId()

		// TODO: GetVanillaSnapshotHash()


		////////////////

		public static IReadOnlyDictionary<string, int> NamesToIds {
			get { return HamstarHelpersMod.Instance.ProjectileIdentityHelpers._NamesToIds; }
		}



		////////////////

		private IDictionary<string, int> __namesToIds = new Dictionary<string, int>();
		private IReadOnlyDictionary<string, int> _NamesToIds = null;


		////////////////

		internal void OnPostSetupContent() {
			this._NamesToIds = new ReadOnlyDictionary<string, int>( this.__namesToIds );
			
			for( int i = 1; i < ProjectileLoader.ProjectileCount; i++ ) {
				string name = Lang.GetProjectileName( i ).Value;
				this.__namesToIds[name] = i;
			}
		}
	}
}
