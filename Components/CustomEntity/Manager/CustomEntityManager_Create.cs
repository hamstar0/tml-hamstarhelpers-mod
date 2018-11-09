using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Collections.Generic;


namespace HamstarHelpers.Components.CustomEntity {
	public partial class CustomEntityManager {
		public static int GetId( Type ent_type ) {
			CustomEntityManager mngr = ModHelpersMod.Instance.CustomEntMngr;

			if( mngr.EntTypeIds.ContainsKey(ent_type.Name) ) {
				return mngr.EntTypeIds[ ent_type.Name ];
			}

			int id = mngr.LatestId++;

			mngr.TypeIdEnts[ id ] = ent_type;
			return mngr.EntTypeIds[ ent_type.Name ] = mngr.LatestId++;
		}


		public static Type GetTypeById( int type_id ) {
			CustomEntityManager mngr = ModHelpersMod.Instance.CustomEntMngr;

			if( mngr.TypeIdEnts.ContainsKey( type_id ) ) {
				throw new HamstarException( "!ModHelpers.CustomEntityManager.GetTypeById - No CustomEntity for type id "+type_id );
			}
			return mngr.TypeIdEnts[type_id];
		}


		////////////////

		internal static void LoadAs( string type_name, CustomEntity ent ) {
			Type ent_type = Type.GetType( type_name );
			if( ent_type == null ) {
				throw new HamstarException( "!ModHelpers.CustomEntityManager.LoadAs - Invalid custom entity of type "+type_name );
			}

			CustomEntity typed_ent = ent.CloneAsType( ent_type );
		}


		////////////////

		public static void RemoveEntity( CustomEntity ent ) {
			if( ent == null ) { throw new HamstarException( "Null ent not allowed." ); }

			CustomEntityManager.RemoveEntityByWho( ent.Core.whoAmI );
		}

		public static void RemoveEntityByWho( int who ) {
			CustomEntityManager mngr = ModHelpersMod.Instance.CustomEntMngr;

			if( !mngr.EntitiesByIndexes.ContainsKey( who ) ) { return; }

			Type comp_type;
			Type base_type = typeof( CustomEntityComponent );

			lock( CustomEntityManager.MyLock ) {
				IList<CustomEntityComponent> ent_components = mngr.EntitiesByIndexes[who].Components;

				foreach( CustomEntityComponent component in ent_components ) {
					comp_type = component.GetType();
					do {
						if( mngr.EntitiesByComponentType.ContainsKey( comp_type ) ) {
							mngr.EntitiesByComponentType[comp_type].Remove( who );
						}

						comp_type = comp_type.BaseType;
					} while( comp_type != base_type );
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
