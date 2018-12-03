using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Collections.Generic;


namespace HamstarHelpers.Components.CustomEntity {
	public partial class CustomEntityManager {
		public static void RemoveEntity( CustomEntity ent ) {
			if( ent == null ) { throw new HamstarException( "Null ent not allowed." ); }

			CustomEntityManager.RemoveEntityByWho( ent.Core.whoAmI );
		}

		public static void RemoveEntityByWho( int who ) {
			CustomEntityManager mngr = ModHelpersMod.Instance.CustomEntMngr;

			if( !mngr.EntitiesByIndexes.ContainsKey( who ) ) { return; }

			Type compType;
			Type baseType = typeof( CustomEntityComponent );

			lock( CustomEntityManager.MyLock ) {
				IList<CustomEntityComponent> entComponents = mngr.EntitiesByIndexes[who].Components;

				foreach( CustomEntityComponent component in entComponents ) {
					compType = component.GetType();
					do {
						if( mngr.EntitiesByComponentType.ContainsKey( compType ) ) {
							mngr.EntitiesByComponentType[compType].Remove( who );
						}

						compType = compType.BaseType;
					} while( compType != baseType );
				}

				mngr.EntitiesByIndexes.Remove( who );
			}
		}


		public static void ClearAllEntities() {
			CustomEntityManager mngr = ModHelpersMod.Instance.CustomEntMngr;

			lock( CustomEntityManager.MyLock ) {
				mngr.EntitiesByIndexes.Clear();
				mngr.EntitiesByComponentType.Clear();
			}
		}
	}
}
