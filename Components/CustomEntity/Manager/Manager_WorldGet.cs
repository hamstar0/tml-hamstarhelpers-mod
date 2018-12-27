using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Components.CustomEntity {
	public partial class CustomEntityManager {
		public static bool IsInWorld( CustomEntity myent ) {
			CustomEntityManager mngr = ModHelpersMod.Instance.CustomEntMngr;

			CustomEntity ent = null;
			mngr.WorldEntitiesByIndexes.TryGetValue( myent.Core.WhoAmI, out ent );

			return ent == myent;
		}


		////////////////

		public static CustomEntity GetEntityByWho( int whoAmI ) {
			CustomEntityManager mngr = ModHelpersMod.Instance.CustomEntMngr;

			CustomEntity ent = null;
			mngr.WorldEntitiesByIndexes.TryGetValue( whoAmI, out ent );
			return ent;
		}

		////

		public static ISet<CustomEntity> GetEntitiesByComponent<T>() where T : CustomEntityComponent {
			CustomEntityManager mngr = ModHelpersMod.Instance.CustomEntMngr;

			ISet<int> entIdxs = new HashSet<int>();
			Type currType = typeof( T );

			lock( CustomEntityManager.MyLock ) {
				if( !mngr.WorldEntitiesByComponentType.TryGetValue( currType, out entIdxs ) ) {
					foreach( var kv in mngr.WorldEntitiesByComponentType ) {
						if( kv.Key.IsSubclassOf( currType ) ) {
							entIdxs.UnionWith( kv.Value );
						}
					}

					if( entIdxs == null ) {
						return new HashSet<CustomEntity>();
					}
				}

				return new HashSet<CustomEntity>(
					entIdxs.Select( i => (CustomEntity)mngr.WorldEntitiesByIndexes[i] )
				);
			}
		}

		////

		public static ISet<T> GetEntitiesForPlayer<T>( Player player ) where T : CustomEntity {
			CustomEntityManager mngr = ModHelpersMod.Instance.CustomEntMngr;
			var ents = new HashSet<T>();

			foreach( CustomEntity ent in mngr.WorldEntitiesByIndexes.Values ) {
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
