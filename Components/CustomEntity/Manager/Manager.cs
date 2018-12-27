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

		private readonly IDictionary<int, CustomEntity> WorldEntitiesByIndexes = new Dictionary<int, CustomEntity>();
		private readonly IDictionary<Type, ISet<int>> WorldEntitiesByComponentType = new Dictionary<Type, ISet<int>>();

		private Func<bool> OnTickGet;



		////////////////

		private void CacheTypeIdInfo( Type entType ) {
			if( this.EntTypeIds.ContainsKey(entType.Name) ) {
				int otherId = this.EntTypeIds[ entType.Name ];
				Type otherEntType = this.TypeIdEnts[ otherId ];
				throw new HamstarException( "CustomEntity "+entType.Name+" name conflict ("+entType.FullName+" vs "+otherEntType.FullName+")" );
			}

			int id = this.LatestId++;

			this.TypeIdEnts[ id ] = entType;
			this.EntTypeIds[ entType.Name ] = id;
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
				var ents = this.WorldEntitiesByIndexes.Values.ToArray();

				foreach( CustomEntity ent in ents ) {
					ent.Update();
				}
			}
		}
	}
}
