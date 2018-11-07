using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Internals.Logic;
using HamstarHelpers.Internals.NetProtocols;
using HamstarHelpers.Services.DataStore;
using HamstarHelpers.Services.Promises;
using System;
using Terraria;


namespace HamstarHelpers.Components.CustomEntity.Components {
	public partial class SaveableEntityComponent : CustomEntityComponent {
		internal static void PostLoadAll() {
			DataStore.Set( SaveableEntityComponent.LoadAllDataKey, true );
			
			Promises.TriggerValidatedPromise( SaveableEntityComponent.LoadAllValidator, SaveableEntityComponent.MyValidatorKey, null );
		}

		internal static void PostUnloadAll() {
			DataStore.Remove( SaveableEntityComponent.LoadAllDataKey );
		}


		////////////////

		public static bool IsLoaded {
			get {
				bool success;
				object raw_output = DataStore.Get( SaveableEntityComponent.LoadAllDataKey, out success );
				return success && (bool)raw_output;
			}
		}


		////////////////

		protected class MyStaticInitializer : StaticInitializer {
			protected override void StaticInitialize() {
				var mymod = ModHelpersMod.Instance;
				var myworld = mymod.GetModWorld<ModHelpersWorld>();
				
				Promises.AddValidatedPromise<PromiseArguments>( ModHelpersWorld.LoadValidator, (_) => {
					if( Main.netMode != 1 ) {
						if( !ModHelpersMod.Instance.Config.DebugModeResetCustomEntities ) {
							try {
								if( !SaveableEntityComponent.LoadAll( true ) ) {
									if( mymod.Config.DebugModeNetInfo ) {
										LogHelpers.Log( "!ModHelpers.SaveableEntityComponent.StaticInitialize - Load (json) failed." );
									}
								}
								if( !SaveableEntityComponent.LoadAll( false ) ) {
									if( mymod.Config.DebugModeNetInfo ) {
										LogHelpers.Log( "!ModHelpers.SaveableEntityComponent.StaticInitialize - Load (no json) failed." );
									}
								}
							} catch( Exception e ) {
								LogHelpers.Log( "!ModHelpers.SaveableEntityComponent.StaticInitialize - " + e.ToString() );
							}
						}
					}
					
					SaveableEntityComponent.PostLoadAll();

					return true;
				} );

				Promises.AddValidatedPromise<PromiseArguments>( ModHelpersWorld.SaveValidator, (_) => {
					if( Main.netMode != 1 ) {
						SaveableEntityComponent.SaveAll( true );
						SaveableEntityComponent.SaveAll( false );
					}

					return true;
				} );

				Promises.AddPostWorldUnloadEachPromise( () => {
					SaveableEntityComponent.PostUnloadAll();
					DataStore.Remove( SaveableEntityComponent.LoadAllDataKey );
				} );

				Promises.AddValidatedPromise<PlayerLogicPromiseArguments>( PlayerLogic.ServerConnectValidator, ( args ) => {
					if( Main.netMode != 1 ) {
						PacketProtocol.QuickSendToClient<CustomEntityAllProtocol>( args.Who, -1 );
					}
					return true;
				} );
			}
		}



		////////////////
		
		protected virtual void OnLoadSingle( CustomEntity ent ) { }
		protected virtual void OnLoadClient( CustomEntity ent ) { }
		protected virtual void OnLoadServer( CustomEntity ent ) { }

		internal void InternalOnLoad( CustomEntity ent ) {
			if( Main.netMode == 0 ) {
				this.OnLoadSingle( ent );
			} else if( Main.netMode == 1 ) {
				this.OnLoadClient( ent );
			} else {
				this.OnLoadServer( ent );
			}
		}
	}
}
