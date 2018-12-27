using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Services.Promises;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Components.CustomEntity {
	public partial class CustomEntityManager {
		public static void AddToWorld( CustomEntity ent ) {
			CustomEntityManager mngr = ModHelpersMod.Instance.CustomEntMngr;
			int who = mngr.WorldEntitiesByIndexes.Count + 1;

			if( !ent.SyncFromClientServer.Item1 && !ent.SyncFromClientServer.Item2 ) {
				who = -who;
			}

			CustomEntityManager.AddToWorld( who, ent );
		}


		public static CustomEntity AddToWorld( int who, CustomEntity ent, bool skipSync = false ) {
			if( ent == null ) { throw new HamstarException( "!ModHelpers.CustomEntityManager.AddToWorld - Null ent not allowed." ); }
			if( !ent.IsInitialized ) { throw new HamstarException( "!ModHelpers.CustomEntityManager.AddToWorld - Initialized ents only." ); }

			CustomEntityManager mngr = ModHelpersMod.Instance.CustomEntMngr;
			CustomEntity realEnt = ent;

			if( mngr.WorldEntitiesByIndexes.ContainsKey(who) ) {
				throw new HamstarException( "!ModHelpers.CustomEntityManager.AddToWorld - "
					+ "Attempting to add "+ent.ToString()+" to slot "+who+" occupied by "+mngr.WorldEntitiesByIndexes[who].ToString() );
			}

			if( ent is SerializableCustomEntity ) {
				realEnt = ( (SerializableCustomEntity)ent ).Convert();
			}

			Type compType;
			Type baseType = typeof( CustomEntityComponent );

			// Map entity to each of its components
			foreach( CustomEntityComponent component in realEnt.InternalComponents ) {
				compType = component.GetType();
				lock( CustomEntityManager.MyLock ) {
					do {
						if( !mngr.WorldEntitiesByComponentType.ContainsKey( compType ) ) {
							mngr.WorldEntitiesByComponentType[compType] = new HashSet<int>();
						}

						mngr.WorldEntitiesByComponentType[compType].Add( who );

						compType = compType.BaseType;
					} while( compType != baseType );
				}
			}

			realEnt.Core.whoAmI = who;
			mngr.WorldEntitiesByIndexes[ who ] = realEnt;

			realEnt.InternalWorldInitialize();

			// Sync also
			if( !skipSync ) {
				if( Main.netMode == 1 ) {
					if( ent.SyncFromClientServer.Item1 ) {
						Promises.AddValidatedPromise( SaveableEntityComponent.LoadAllValidator, () => {
							ent.SyncToAll();
							return false;
						} );
					}
				} else if( Main.netMode == 2 ) {
					if( ent.SyncFromClientServer.Item2 ) {
						Promises.AddValidatedPromise( SaveableEntityComponent.LoadAllValidator, () => {
							ent.SyncToAll();
							return false;
						} );
					}
				}
			}

			if( ModHelpersMod.Instance.Config.DebugModeCustomEntityInfo ) {
				LogHelpers.Log( "ModHelpers.CustomEntity.AddToWorld - Set " + realEnt.ToString() );
			}

			return realEnt;
		}


		////////////////

		public static void RemoveEntity( CustomEntity ent ) {
			if( ent == null ) { throw new HamstarException( "Null ent not allowed." ); }

			CustomEntityManager.RemoveEntityByWho( ent.Core.whoAmI );
		}

		public static void RemoveEntityByWho( int who ) {
			CustomEntityManager mngr = ModHelpersMod.Instance.CustomEntMngr;

			if( !mngr.WorldEntitiesByIndexes.ContainsKey( who ) ) { return; }

			Type compType;
			Type baseType = typeof( CustomEntityComponent );

			lock( CustomEntityManager.MyLock ) {
				IList<CustomEntityComponent> entComponents = mngr.WorldEntitiesByIndexes[who].InternalComponents;

				foreach( CustomEntityComponent component in entComponents ) {
					compType = component.GetType();
					do {
						if( mngr.WorldEntitiesByComponentType.ContainsKey( compType ) ) {
							mngr.WorldEntitiesByComponentType[compType].Remove( who );
						}

						compType = compType.BaseType;
					} while( compType != baseType );
				}

				mngr.WorldEntitiesByIndexes.Remove( who );
			}
		}


		public static void ClearAllEntities() {
			CustomEntityManager mngr = ModHelpersMod.Instance.CustomEntMngr;

			lock( CustomEntityManager.MyLock ) {
				mngr.WorldEntitiesByIndexes.Clear();
				mngr.WorldEntitiesByComponentType.Clear();
			}
		}
	}
}
