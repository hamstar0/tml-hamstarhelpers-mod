using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.MiscHelpers;
using HamstarHelpers.Helpers.PlayerHelpers;
using HamstarHelpers.Services.Promises;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Components.CustomEntity.Components {
	class CustomEntityPlayerData {
		public CustomEntity[] Entities;
		
		public CustomEntityPlayerData( CustomEntity[] ents ) {
			this.Entities = ents;
		}
	}



	
	public class PerPlayerSaveEntityComponent : CustomEntityComponent {
		public bool AsJson;
		

		////////////////

		public PerPlayerSaveEntityComponent( bool as_json ) {
			this.AsJson = as_json;
		}

		protected override void StaticInitialize() {
			var mymod = HamstarHelpersMod.Instance;
			var myworld = mymod.GetModWorld<HamstarHelpersWorld>();

			Promises.AddCustomPromiseForObject( HamstarHelpersPlayer.PlayerLoad, () => {
				if( !this.Load( HamstarHelpersPlayer.PlayerLoad.MyPlayer ) ) {
					LogHelpers.Log( "HamstarHelpersMod.PerPlayerSaveEntityComponent.StaticInitialize - Load failed for "+ HamstarHelpersPlayer.PlayerLoad.MyPlayer.name );
				}
				return true;
			} );
			Promises.AddCustomPromiseForObject( HamstarHelpersPlayer.PlayerSave, () => {
				if( !this.Save( HamstarHelpersPlayer.PlayerSave.MyPlayer ) ) {
					LogHelpers.Log( "HamstarHelpersMod.PerPlayerSaveEntityComponent.StaticInitialize - Save failed for " + HamstarHelpersPlayer.PlayerSave.MyPlayer.name );
				}
				return true;
			} );
		}


		////////////////

		public string GetFileNameBase( Player player, out bool success ) {
			return "player_"+PlayerIdentityHelpers.GetUniqueId(player, out success)+"_ents";
		}


		////////////////

		internal bool Load( Player player ) {
			var mymod = HamstarHelpersMod.Instance;
			bool success;
			string file_name = this.GetFileNameBase( player, out success );
			if( !success ) { return false; }

			CustomEntityPlayerData data;

			if( this.AsJson ) {
				data = DataFileHelpers.LoadJson<CustomEntityPlayerData>( mymod, file_name, out success );
			} else {
				data = DataFileHelpers.LoadBinary<CustomEntityPlayerData>( mymod, file_name, false );
				success = data != null;
			}

			if( success ) {
				foreach( var ent in data.Entities ) {
					CustomEntityManager.Instance.Add( ent );
				}
			}

			return success;
		}


		internal bool Save( Player player ) {
			var mymod = HamstarHelpersMod.Instance;

			bool success;
			string file_name = this.GetFileNameBase( player, out success );
			if( !success ) { return false; }
			
			var data = new CustomEntityPlayerData(
				CustomEntityManager.Instance.TakeWhile(
					t => t.GetComponentByType<PerPlayerSaveEntityComponent>() != null
				).ToArray()
			);

			if( this.AsJson ) {
				DataFileHelpers.SaveAsJson<CustomEntityPlayerData>( mymod, file_name + ".json", data );
			} else {
				DataFileHelpers.SaveAsBinary<CustomEntityPlayerData>( mymod, file_name + ".dat", false, data );
			}

			return true;
		}
	}
}
