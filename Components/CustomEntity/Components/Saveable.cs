using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.MiscHelpers;
using HamstarHelpers.Helpers.WorldHelpers;
using HamstarHelpers.Internals.Logic;
using HamstarHelpers.Internals.NetProtocols;
using HamstarHelpers.Services.DataStore;
using HamstarHelpers.Services.Promises;
using System.Collections.Generic;
using System.Linq;


namespace HamstarHelpers.Components.CustomEntity.Components {
	public class SaveableEntityComponent : CustomEntityComponent {
		public readonly static object LoadHook = new object();

		private static object LoadDataKey = new object();


		////////////////

		public static bool IsLoaded {
			get {
				bool success;
				object raw_output = DataStore.Get( SaveableEntityComponent.LoadDataKey, out success );
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
					if( !wld_save_json.LoadAll() ) {
						LogHelpers.Log( "HamstarHelpersMod.PerWorldSaveEntityComponent.StaticInitialize - Load (json) failed." );
					}
					if( !wld_save_nojson.LoadAll() ) {
						LogHelpers.Log( "HamstarHelpersMod.PerWorldSaveEntityComponent.StaticInitialize - Load (no json) failed." );
					}

					DataStore.Set( SaveableEntityComponent.LoadDataKey, true );
					Promises.TriggerCustomPromiseForObject( SaveableEntityComponent.LoadHook );

					return true;
				} );

				Promises.AddCustomPromiseForObject( HamstarHelpersWorld.WorldSave, () => {
					wld_save_json.SaveAll();
					wld_save_nojson.SaveAll();

					return true;
				} );

				Promises.AddPostWorldUnloadEachPromise( () => {
					DataStore.Remove( SaveableEntityComponent.LoadDataKey );
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
