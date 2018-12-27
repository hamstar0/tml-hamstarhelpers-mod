using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Services.DataStore;
using HamstarHelpers.Services.Promises;
using System;
using Terraria;


namespace HamstarHelpers.Components.CustomEntity.Components {
	public partial class SaveableEntityComponent : CustomEntityComponent {
		internal static void PostLoadAll() {
			SaveableEntityComponent.HaveAllEntitiesLoaded = true;
			Promises.TriggerValidatedPromise( SaveableEntityComponent.LoadAllValidator, SaveableEntityComponent.MyValidatorKey, null );
		}

		internal static void PostUnloadAll() {
			SaveableEntityComponent.HaveAllEntitiesLoaded = false;
		}


		////////////////

		public static bool HaveAllEntitiesLoaded {
			get {
				bool output;
				bool success = DataStore.Get( SaveableEntityComponent.LoadAllDataKey, out output );
				return success && output;
			}
			internal set {
				DataStore.Set( SaveableEntityComponent.LoadAllDataKey, value );
			}
		}


		////////////////

		protected class MyStaticInitializer : StaticInitializer {
			protected override void StaticInitialize() {
				// Load all entities upon world load (single, server)
				if( Main.netMode != 1 ) {
					Promises.AddValidatedPromise<PromiseArguments>( ModHelpersWorld.LoadValidator, ( _ ) => {
						MyStaticInitializer.LoadAll();
						SaveableEntityComponent.PostLoadAll();
						return true;
					} );
				}

				// Save all entities upon world save (single, server)
				if( Main.netMode != 1 ) {
					Promises.AddValidatedPromise<PromiseArguments>( ModHelpersWorld.SaveValidator, ( _ ) => {
						MyStaticInitializer.SaveAll();
						return true;
					} );
				}

				// Unload entities after world closes
				Promises.AddPostWorldUnloadEachPromise( () => {
					SaveableEntityComponent.PostUnloadAll();
				} );
			}


			private static void LoadAll() {
				var mymod = ModHelpersMod.Instance;

				if( mymod.Config.DebugModeResetCustomEntities ) {
					return;
				}

				try {
					if( !SaveableEntityComponent.LoadAll( true ) ) {
						if( mymod.Config.DebugModeNetInfo ) {
							LogHelpers.Log( "!ModHelpers.SaveableEntityComponent.StaticInitialize.LoadAll - Load (json) failed." );
						}
					}
					if( !SaveableEntityComponent.LoadAll( false ) ) {
						if( mymod.Config.DebugModeNetInfo ) {
							LogHelpers.Log( "!ModHelpers.SaveableEntityComponent.StaticInitialize.LoadAll - Load (no json) failed." );
						}
					}
				} catch( Exception e ) {
					LogHelpers.Log( "!ModHelpers.SaveableEntityComponent.StaticInitialize.LoadAll - " + e.ToString() );
				}
			}

			private static void SaveAll() {
				SaveableEntityComponent.SaveAll( true );
				SaveableEntityComponent.SaveAll( false );
			}
		}



		////////////////
		
		protected virtual void OnLoadSingle( CustomEntity ent ) { }
		protected virtual void OnLoadClient( CustomEntity ent ) { }
		protected virtual void OnLoadServer( CustomEntity ent ) { }

		protected sealed override void OnAddToWorld( CustomEntity ent ) {
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
