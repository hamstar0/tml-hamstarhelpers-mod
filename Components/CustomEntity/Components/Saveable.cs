﻿using HamstarHelpers.Components.Network;
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
		internal readonly static object MyValidatorKey;
		public readonly static PromiseValidator LoadAllValidator;

		private readonly static object LoadAllDataKey = new object();


		////////////////

		static SaveableEntityComponent() {
			SaveableEntityComponent.MyValidatorKey = new object();
			SaveableEntityComponent.LoadAllValidator = new PromiseValidator( SaveableEntityComponent.MyValidatorKey );
			SaveableEntityComponent.LoadAllDataKey = new object();
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
				var mymod = HamstarHelpersMod.Instance;
				var myworld = mymod.GetModWorld<HamstarHelpersWorld>();
				var wld_save_json = new SaveableEntityComponent( true );
				var wld_save_nojson = new SaveableEntityComponent( false );

				Promises.AddValidatedPromise( HamstarHelpersWorld.LoadValidator, () => {
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

					DataStore.Set( SaveableEntityComponent.LoadAllDataKey, true );

					Promises.TriggerValidatedPromise( SaveableEntityComponent.LoadAllValidator, SaveableEntityComponent.MyValidatorKey );

					return true;
				} );

				Promises.AddValidatedPromise( HamstarHelpersWorld.SaveValidator, () => {
					wld_save_json.SaveAll();
					wld_save_nojson.SaveAll();

					return true;
				} );

				Promises.AddPostWorldUnloadEachPromise( () => {
					DataStore.Remove( SaveableEntityComponent.LoadAllDataKey );
				} );

				Promises.AddValidatedPromise( PlayerLogicPromiseValidator.ServerConnectValidator, () => {
					PacketProtocol.QuickSendToClient<CustomEntityAllProtocol>( PlayerLogicPromiseValidator.ServerConnectValidator.MyPlayer.whoAmI, -1 );
					return true;
				} );
			}
		}



		////////////////

		public bool AsJson;



		////////////////

		public SaveableEntityComponent() { }

		public SaveableEntityComponent( bool as_json ) {
			this.AsJson = as_json;
			
			this.ConfirmLoad();
		}


		////////////////
		
		public override CustomEntityComponent Clone() {
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
					if( ent != null ) {
						CustomEntityManager.Instance.Add( ent );
					}
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
