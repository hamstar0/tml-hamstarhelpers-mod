using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Components.Errors;
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

		public static CustomEntity GetEntityByWho( int who ) {
			CustomEntityManager mngr = ModHelpersMod.Instance.CustomEntMngr;

			CustomEntity ent = null;
			mngr.EntitiesByIndexes.TryGetValue( who, out ent );
			return ent;
		}

		////

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

		////

		public static ISet<T> GetEntitiesForPlayer<T>( Player player ) where T : CustomEntity {
			CustomEntityManager mngr = ModHelpersMod.Instance.CustomEntMngr;
			var ents = new HashSet<T>();

			foreach( CustomEntity ent in mngr.EntitiesByIndexes.Values ) {
				if( !(ent is T) ) { continue; }

				if( Main.netMode == 2 ) {
					ent.RefreshOwnerWho();
				}
				if( ent.OwnerPlayerWho == player.whoAmI ) {
					ents.Add( (T)ent );
				}
			}
			return ents;
		}


		////////////////

		public static void AddToWorld( CustomEntity ent ) {
			CustomEntityManager mngr = ModHelpersMod.Instance.CustomEntMngr;
			CustomEntityManager.AddToWorld( mngr.EntitiesByIndexes.Count, ent );
		}
		

		public static void AddToWorld( int who, CustomEntity ent ) {
			if( ent == null ) { throw new HamstarException( "!ModHelpers.CustomEntityManager.AddToWorld - Null ent not allowed." ); }
			if( !ent.IsInitialized ) { throw new HamstarException( "!ModHelpers.CustomEntityManager.AddToWorld - Initialized ents only." ); }

			CustomEntityManager mngr = ModHelpersMod.Instance.CustomEntMngr;

			if( ent is SerializableCustomEntity ) {
				ent = ((SerializableCustomEntity)ent).Convert();
			}

			Type comp_type;
			Type base_type = typeof( CustomEntityComponent );

			// Map entity to each of its components
			foreach( CustomEntityComponent component in ent.Components ) {
				comp_type = component.GetType();
				lock( CustomEntityManager.MyLock ) {
					do {
						if( !mngr.EntitiesByComponentType.ContainsKey( comp_type ) ) {
							mngr.EntitiesByComponentType[ comp_type ] = new HashSet<int>();
						}

						mngr.EntitiesByComponentType[ comp_type ].Add( who );

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
				LogHelpers.Log( "ModHelpers.CustomEntity.AddToWorld - Set " + ent.ToString() );
			}
		}
	}
}
