using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.MiscHelpers;
using HamstarHelpers.Helpers.PlayerHelpers;
using HamstarHelpers.Services.Promises;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Components.CustomEntity.Components {
	public class PerPlayerSaveEntityComponent : CustomEntityComponent {
		public bool AsJson;



		////////////////
		
		public PerPlayerSaveEntityComponent( bool as_json ) {
			this.AsJson = as_json;
		}

		private class MyStaticInitializer : StaticInitializer {
			protected override void StaticInitialize() {
				var mymod = HamstarHelpersMod.Instance;
				var myworld = mymod.GetModWorld<HamstarHelpersWorld>();
				var plr_save_json = new PerPlayerSaveEntityComponent( true );
				var plr_save_nojson = new PerPlayerSaveEntityComponent( false );

				Promises.AddCustomPromiseForObject( HamstarHelpersPlayer.PlayerLoad, () => {
					if( !plr_save_json.LoadAll( HamstarHelpersPlayer.PlayerLoad.MyPlayer ) ) {
						LogHelpers.Log( "HamstarHelpersMod.PerPlayerSaveEntityComponent.StaticInitialize - Load (json) failed for " + HamstarHelpersPlayer.PlayerLoad.MyPlayer.name );
					}
					if( !plr_save_nojson.LoadAll( HamstarHelpersPlayer.PlayerLoad.MyPlayer ) ) {
						LogHelpers.Log( "HamstarHelpersMod.PerPlayerSaveEntityComponent.StaticInitialize - Load (no json) failed for " + HamstarHelpersPlayer.PlayerLoad.MyPlayer.name );
					}
					return true;
				} );
				Promises.AddCustomPromiseForObject( HamstarHelpersPlayer.PlayerSave, () => {
					if( !plr_save_json.SaveAll( HamstarHelpersPlayer.PlayerSave.MyPlayer ) ) {
						LogHelpers.Log( "HamstarHelpersMod.PerPlayerSaveEntityComponent.StaticInitialize - Save (json) failed for " + HamstarHelpersPlayer.PlayerSave.MyPlayer.name );
					}
					if( !plr_save_nojson.SaveAll( HamstarHelpersPlayer.PlayerSave.MyPlayer ) ) {
						LogHelpers.Log( "HamstarHelpersMod.PerPlayerSaveEntityComponent.StaticInitialize - Save (no json) failed for " + HamstarHelpersPlayer.PlayerSave.MyPlayer.name );
					}
					return true;
				} );
			}
		}


		////////////////

		public string GetFileNameBase( Player player, out bool success ) {
			return "player_"+PlayerIdentityHelpers.GetUniqueId(player, out success)+"_ents";
		}


		////////////////

		private bool LoadAll( Player player ) {
			var mymod = HamstarHelpersMod.Instance;
			bool success;
			string file_name = this.GetFileNameBase( player, out success );
			if( !success ) { return false; }

			ISet<CustomEntity> ents;

			if( this.AsJson ) {
				ents = DataFileHelpers.LoadJson<HashSet<CustomEntity>>( mymod, file_name, out success );
			} else {
				ents = DataFileHelpers.LoadBinary<HashSet<CustomEntity>>( mymod, file_name+".dat", false );
				success = ents != null;
			}

			if( success ) {
				foreach( var ent in ents ) {
					CustomEntityManager.Instance.Add( ent );
				}
			}

			return success;
		}


		private bool SaveAll( Player player ) {
			var mymod = HamstarHelpersMod.Instance;

			bool success;
			string file_name = this.GetFileNameBase( player, out success );
			if( !success ) { return false; }

			ISet<CustomEntity> ents = CustomEntityManager.Instance.GetByComponentType<PerWorldSaveEntityComponent>();

			if( ents.Count > 0 ) {
				if( this.AsJson ) {
					DataFileHelpers.SaveAsJson( mymod, file_name, ents );
				} else {
					DataFileHelpers.SaveAsBinary( mymod, file_name + ".dat", false, ents );
				}
			}

			return true;
		}
	}
}
