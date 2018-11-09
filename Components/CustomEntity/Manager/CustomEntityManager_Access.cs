using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;


namespace HamstarHelpers.Components.CustomEntity {
	public partial class CustomEntityManager {
		public static CustomEntity GetEntityByWho( int who ) {
			CustomEntityManager mngr = ModHelpersMod.Instance.CustomEntMngr;

			CustomEntity ent = null;
			mngr.EntitiesByIndexes.TryGetValue( who, out ent );
			return ent;
		}


		public static void SetEntityByWho( int who, CustomEntity ent ) {
			if( ent == null ) { throw new HamstarException( "!ModHelpers.CustomEntityManager.SetEntityByWho - Null ent not allowed." ); }
			if( !ent.IsInitialized ) { throw new HamstarException("!ModHelpers.CustomEntityManager.SetEntityByWho - Initialized ents only."); }

			CustomEntityManager mngr = ModHelpersMod.Instance.CustomEntMngr;

			Type comp_type;
			Type base_type = typeof( CustomEntityComponent );

			// Map entity to each of its components
			foreach( CustomEntityComponent component in ent.Components ) {
				comp_type = component.GetType();
				lock( CustomEntityManager.MyLock ) {
					do {
						if( !mngr.EntitiesByComponentType.ContainsKey( comp_type ) ) {
							mngr.EntitiesByComponentType[comp_type] = new HashSet<int>();
						}

						mngr.EntitiesByComponentType[comp_type].Add( who );

						comp_type = comp_type.BaseType;
					} while( comp_type != base_type );
				}
			}

			ent.Core.whoAmI = who;
			mngr.EntitiesByIndexes[ who ] = ent;
			
			var save_comp = ent.GetComponentByType<SaveableEntityComponent>();
			if( save_comp != null ) {
				save_comp.InternalOnLoad( ent );
			}

			if( ModHelpersMod.Instance.Config.DebugModeCustomEntityInfo ) {
				LogHelpers.Log( "ModHelpers.CustomEntity.SetEntityByWho - Set " + ent.ToString() );
			}
		}



		////////////////

		public static ISet<CustomEntity> GetEntitiesByComponent<T>() where T : CustomEntityComponent {
			CustomEntityManager mngr = ModHelpersMod.Instance.CustomEntMngr;

			ISet<int> ent_idxs = new HashSet<int>();
			Type curr_type = typeof( T );

			lock( CustomEntityManager.MyLock ) {
				if( !mngr.EntitiesByComponentType.TryGetValue( curr_type, out ent_idxs ) ) {
					foreach( var kv in mngr.EntitiesByComponentType ) {
						if( kv.Key.IsSubclassOf( curr_type ) ) {
							ent_idxs.UnionWith( kv.Value );
						}
					}

					if( ent_idxs == null ) {
						return new HashSet<CustomEntity>();
					}
				}

				return new HashSet<CustomEntity>(
					ent_idxs.Select( i => (CustomEntity)mngr.EntitiesByIndexes[i] )
				);
			}
		}
	}
}
