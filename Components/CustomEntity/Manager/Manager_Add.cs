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
			int who = mngr.EntitiesByIndexes.Count + 1;

			if( !ent.SyncClientServer.Item1 && !ent.SyncClientServer.Item2 ) {
				who = -who;
			}

			CustomEntityManager.AddToWorld( who, ent );
		}


		public static CustomEntity AddToWorld( int who, CustomEntity ent, bool skipSync = false ) {
			if( ent == null ) { throw new HamstarException( "!ModHelpers.CustomEntityManager.AddToWorld - Null ent not allowed." ); }
			if( !ent.IsInitialized ) { throw new HamstarException( "!ModHelpers.CustomEntityManager.AddToWorld - Initialized ents only." ); }

			CustomEntityManager mngr = ModHelpersMod.Instance.CustomEntMngr;
			CustomEntity realEnt = ent;

			if( mngr.EntitiesByIndexes.ContainsKey(who) ) {
				throw new HamstarException( "!ModHelpers.CustomEntityManager.AddToWorld - "
					+ "Attempting to add "+ent.ToString()+" to slot "+who+" occupied by "+mngr.EntitiesByIndexes[who].ToString() );
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
						if( !mngr.EntitiesByComponentType.ContainsKey( compType ) ) {
							mngr.EntitiesByComponentType[compType] = new HashSet<int>();
						}

						mngr.EntitiesByComponentType[compType].Add( who );

						compType = compType.BaseType;
					} while( compType != baseType );
				}
			}

			realEnt.Core.whoAmI = who;
			mngr.EntitiesByIndexes[ who ] = realEnt;

			var saveComp = realEnt.GetComponentByType<SaveableEntityComponent>();
			if( saveComp != null ) {
				saveComp.InternalOnLoad( realEnt );
			}

			// Sync also
			if( !skipSync ) {
				if( Main.netMode == 1 ) {
					if( ent.SyncClientServer.Item1 ) {
						Promises.AddValidatedPromise( SaveableEntityComponent.LoadAllValidator, () => {
							ent.SyncToAll();
							return false;
						} );
					}
				} else if( Main.netMode == 2 ) {
					if( ent.SyncClientServer.Item2 ) {
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
	}
}
