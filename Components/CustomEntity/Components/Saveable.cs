using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.MiscHelpers;
using HamstarHelpers.Helpers.WorldHelpers;
using HamstarHelpers.Internals.Logic;
using HamstarHelpers.Internals.NetProtocols;
using HamstarHelpers.Services.DataStore;
using HamstarHelpers.Services.Promises;
using System;
using System.Collections.Generic;
using System.Linq;


namespace HamstarHelpers.Components.CustomEntity.Components {
	public class SaveableEntityComponent : CustomEntityComponent {
		public readonly static PromiseTrigger LoadAllHook;
		internal readonly static object PromiseTriggerValidator;

		private readonly static object IsAllLoadedKey = new object();


		////////////////

		static SaveableEntityComponent() {
			SaveableEntityComponent.PromiseTriggerValidator = new object();
			SaveableEntityComponent.LoadAllHook = new PromiseTrigger( SaveableEntityComponent.PromiseTriggerValidator );
			SaveableEntityComponent.IsAllLoadedKey = new object();
		}


		////////////////

		public static bool IsLoaded {
			get {
				bool success;
				object raw_output = DataStore.Get( SaveableEntityComponent.IsAllLoadedKey, out success );
				return success && (bool)raw_output;
			}
		}

		////////////////

		protected class MyStaticInitializer : StaticInitializer {
			protected override void StaticInitialize() {
				var mymod = HamstarHelpersMod.Instance;
				var myworld = mymod.GetModWorld<HamstarHelpersWorld>();
				var wld_save_json = new SaveableEntityComponent( true );
				var wld_save_nojson = new SaveableEntityComponent( false );

				Promises.AddCustomPromiseForObject( HamstarHelpersWorld.WorldLoad, () => {
					try {
						if( !wld_save_json.LoadAll() ) {
							if( mymod.Config.DebugModeNetInfo ) {
								LogHelpers.Log( "HamstarHelpers.SaveableEntityComponent.StaticInitialize - Load (json) failed." );
							}
						}
						if( !wld_save_nojson.LoadAll() ) {
							if( mymod.Config.DebugModeNetInfo ) {
								LogHelpers.Log( "HamstarHelpers.SaveableEntityComponent.StaticInitialize - Load (no json) failed." );
							}
						}
					} catch( Exception e ) {
						LogHelpers.Log( "HamstarHelpers.SaveableEntityComponent.StaticInitialize - " + e.ToString() );
					}

					DataStore.Set( SaveableEntityComponent.IsAllLoadedKey, true );

					Promises.TriggerCustomPromiseForObject( SaveableEntityComponent.LoadAllHook, SaveableEntityComponent.PromiseTriggerValidator );

					return true;
				} );

				Promises.AddCustomPromiseForObject( HamstarHelpersWorld.WorldSave, () => {
					wld_save_json.SaveAll();
					wld_save_nojson.SaveAll();

					return true;
				} );

				Promises.AddPostWorldUnloadEachPromise( () => {
					DataStore.Remove( SaveableEntityComponent.IsAllLoadedKey );
				} );

				Promises.AddCustomPromiseForObject( PlayerLogicHook.ConnectServer, () => {
					PacketProtocol.QuickSendToClient<CustomEntityAllProtocol>( PlayerLogicHook.ConnectServer.MyPlayer.whoAmI, -1 );
					return true;
				} );
			}
		}



		////////////////

		public bool AsJson;



		////////////////
		
		public SaveableEntityComponent( bool as_json ) {
			this.AsJson = as_json;
		}

		////////////////

		public override CustomEntityComponent Clone( out bool can_clone ) {
			can_clone = true;
			return (SaveableEntityComponent)this.MemberwiseClone();
		}

		////////////////

		public string GetFileNameBase() {
			return "world_" + WorldHelpers.GetUniqueIdWithSeed() + "_ents";
		}


		////////////////

		private bool LoadAll() {
			var mymod = HamstarHelpersMod.Instance;
			string file_name = this.GetFileNameBase();
			bool success;
			ISet<CustomEntity> ents;

			if( this.AsJson ) {
				ents = DataFileHelpers.LoadJson<HashSet<CustomEntity>>( mymod, file_name, CustomEntity.SerializerSettings, out success );
			} else {
				ents = DataFileHelpers.LoadBinary<HashSet<CustomEntity>>( mymod, file_name+".dat", false, CustomEntity.SerializerSettings );
				success = ents != null;
			}

			if( success ) {
				foreach( var ent in ents ) {
					CustomEntityManager.Instance.Add( ent );
				}
			}

			return success;
		}


		private void SaveAll() {
			var mymod = HamstarHelpersMod.Instance;
			string file_name = this.GetFileNameBase();

			ISet<CustomEntity> ents = CustomEntityManager.Instance.GetByComponentType<SaveableEntityComponent>();
			ents = new HashSet<CustomEntity>(
				ents.Where(
					ent => ent.GetComponentByType<SaveableEntityComponent>().AsJson == this.AsJson
				)
			);

			if( ents.Count > 0 ) {
				if( this.AsJson ) {
					DataFileHelpers.SaveAsJson( mymod, file_name, CustomEntity.SerializerSettings, ents );
				} else {
					DataFileHelpers.SaveAsBinary( mymod, file_name + ".dat", false, CustomEntity.SerializerSettings, ents );
				}
			}
		}
	}
}
