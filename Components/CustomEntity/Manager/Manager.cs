using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.TmlHelpers;
using System;
using System.Collections.Generic;
using System.Linq;


namespace HamstarHelpers.Components.CustomEntity {
	public partial class CustomEntityManager {
		public readonly static object MyLock = new object();


		
		////////////////

		private int LatestId = 1;
		private IDictionary<string, int> EntTypeIds = new Dictionary<string, int>();
		private IDictionary<int, Type> TypeIdEnts = new Dictionary<int, Type>();

		private readonly IDictionary<int, CustomEntity> EntitiesByIndexes = new Dictionary<int, CustomEntity>();
		private readonly IDictionary<Type, ISet<int>> EntitiesByComponentType = new Dictionary<Type, ISet<int>>();

		private Func<bool> OnTickGet;



		////////////////

		private void CacheTypeIdInfo( Type ent_type ) {
			if( this.EntTypeIds.ContainsKey(ent_type.Name) ) {
				int other_id = this.EntTypeIds[ ent_type.Name ];
				Type other_ent_type = this.TypeIdEnts[ other_id ];
				throw new HamstarException( "CustomEntity "+ent_type.Name+" name conflict ("+ent_type.FullName+" vs "+other_ent_type.FullName+")" );
			}

			int id = this.LatestId++;

			this.TypeIdEnts[ id ] = ent_type;
			this.EntTypeIds[ ent_type.Name ] = id;
		}


		////////////////

		private static void _Update() { // <- Just in case references are doing something funky...
			var mymod = ModHelpersMod.Instance;
			if( mymod == null || mymod.CustomEntMngr == null ) { return; }

			if( mymod.CustomEntMngr.OnTickGet() ) {
				mymod.CustomEntMngr.Update();
			}
		}

		internal void Update() {
			if( !LoadHelpers.IsWorldBeingPlayed() ) { return; }

			lock( CustomEntityManager.MyLock ) {
				foreach( CustomEntity ent in this.EntitiesByIndexes.Values.ToArray() ) {
					ent.Update();
				}
			}
		}
	}
}
