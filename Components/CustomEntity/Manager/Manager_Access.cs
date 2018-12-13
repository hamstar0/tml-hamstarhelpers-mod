using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Components.CustomEntity {
	public partial class CustomEntityManager {
		public static Type GetEntityType( string name ) {
			CustomEntityManager mngr = ModHelpersMod.Instance.CustomEntMngr;
			int id;
			Type mytype;

			if( !mngr.EntTypeIds.TryGetValue( name, out id ) ) {
				return null;
			}
			if( !mngr.TypeIdEnts.TryGetValue( id, out mytype ) ) {
				return null;
			}
			return mytype;
		}


		////////////////

		public static bool IsInWorld( CustomEntity myent ) {
			CustomEntityManager mngr = ModHelpersMod.Instance.CustomEntMngr;

			CustomEntity ent = null;
			mngr.EntitiesByIndexes.TryGetValue( myent.Core.WhoAmI, out ent );

			return ent == myent;
		}


		////////////////

		public static CustomEntity GetEntityByWho( int who ) {
			CustomEntityManager mngr = ModHelpersMod.Instance.CustomEntMngr;

			CustomEntity ent = null;
			mngr.EntitiesByIndexes.TryGetValue( who, out ent );
			return ent;
		}

		////

		public static ISet<CustomEntity> GetEntitiesByComponent<T>() where T : CustomEntityComponent {
			CustomEntityManager mngr = ModHelpersMod.Instance.CustomEntMngr;

			ISet<int> entIdxs = new HashSet<int>();
			Type currType = typeof( T );

			lock( CustomEntityManager.MyLock ) {
				if( !mngr.EntitiesByComponentType.TryGetValue( currType, out entIdxs ) ) {
					foreach( var kv in mngr.EntitiesByComponentType ) {
						if( kv.Key.IsSubclassOf( currType ) ) {
							entIdxs.UnionWith( kv.Value );
						}
					}

					if( entIdxs == null ) {
						return new HashSet<CustomEntity>();
					}
				}

				return new HashSet<CustomEntity>(
					entIdxs.Select( i => (CustomEntity)mngr.EntitiesByIndexes[i] )
				);
			}
		}

		////

		public static ISet<T> GetEntitiesForPlayer<T>( Player player ) where T : CustomEntity {
			CustomEntityManager mngr = ModHelpersMod.Instance.CustomEntMngr;
			var ents = new HashSet<T>();

			foreach( CustomEntity ent in mngr.EntitiesByIndexes.Values ) {
				if( !(ent is T) ) { continue; }

				if( Main.netMode == 2 ) {
					ent.RefreshOwnerWho();
				}
				if( ent.MyOwnerPlayerWho == player.whoAmI ) {
					ents.Add( (T)ent );
				}
			}
			return ents;
		}
	}
}
