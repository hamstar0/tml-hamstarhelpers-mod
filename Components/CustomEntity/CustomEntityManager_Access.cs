using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;


namespace HamstarHelpers.Components.CustomEntity {
	public partial class CustomEntityManager {
		public static CustomEntity Get( int idx ) {
			CustomEntityManager mngr = HamstarHelpersMod.Instance.CustomEntMngr;

			CustomEntity ent = null;
			mngr.EntitiesByIndexes.TryGetValue( idx, out ent );
			return ent;
		}


		public static void Set( int idx, CustomEntity ent ) {
			if( ent == null ) { throw new HamstarException( "Null ent not allowed." ); }

			CustomEntityManager mngr = HamstarHelpersMod.Instance.CustomEntMngr;

			Type comp_type;
			Type base_type = typeof( CustomEntityComponent );

			foreach( CustomEntityComponent component in ent.Components ) {
				if( !component.IsInitialized ) {
					throw new NotImplementedException( component.GetType().Name + " is not initialized." );
				}

				comp_type = component.GetType();
				do {
					if( !mngr.EntitiesByComponentType.ContainsKey( comp_type ) ) {
						mngr.EntitiesByComponentType[comp_type] = new HashSet<int>();
					}
					mngr.EntitiesByComponentType[comp_type].Add( idx );

					comp_type = comp_type.BaseType;
				} while( comp_type != base_type );
			}

			ent.Core.whoAmI = idx;
			mngr.EntitiesByIndexes[idx] = ent;
		}


		////////////////

		public static int Add( CustomEntity ent ) {
			if( ent == null ) { throw new HamstarException( "Null ent not allowed." ); }

			CustomEntityManager mngr = HamstarHelpersMod.Instance.CustomEntMngr;

			int idx = mngr.EntitiesByIndexes.Count;

			CustomEntityManager.Set( idx, ent );

			return idx;
		}


		public static void Remove( CustomEntity ent ) {
			if( ent == null ) { throw new HamstarException( "Null ent not allowed." ); }

			CustomEntityManager.Remove( ent.Core.whoAmI );
		}

		public static void Remove( int idx ) {
			CustomEntityManager mngr = HamstarHelpersMod.Instance.CustomEntMngr;

			if( !mngr.EntitiesByIndexes.ContainsKey( idx ) ) { return; }

			Type comp_type;
			Type base_type = typeof( CustomEntityComponent );

			IList<CustomEntityComponent> ent_components = mngr.EntitiesByIndexes[idx].Components;

			foreach( CustomEntityComponent component in ent_components ) {
				comp_type = component.GetType();
				do {
					if( mngr.EntitiesByComponentType.ContainsKey( comp_type ) ) {
						mngr.EntitiesByComponentType[comp_type].Remove( idx );
					}

					comp_type = comp_type.BaseType;
				} while( comp_type != base_type );
			}

			mngr.EntitiesByIndexes.Remove( idx );
		}


		public static void Clear() {
			CustomEntityManager mngr = HamstarHelpersMod.Instance.CustomEntMngr;

			mngr.EntitiesByIndexes.Clear();
			mngr.EntitiesByComponentType.Clear();
		}



		////////////////

		public static ISet<CustomEntity> GetByComponentType<T>() where T : CustomEntityComponent {
			CustomEntityManager mngr = HamstarHelpersMod.Instance.CustomEntMngr;

			ISet<int> ent_idxs = new HashSet<int>();
			Type curr_type = typeof( T );

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
