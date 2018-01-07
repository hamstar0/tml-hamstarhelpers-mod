using HamstarHelpers.TmlHelpers;
using HamstarHelpers.Utilities.Config;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader.IO;


namespace HamstarHelpers {
	class HamstarHelpersLogic {
		internal bool HasSyncedModData = false;
		internal int StartupDelay = 0;

		public bool IsClientPlaying = false;
		public bool IsServerPlaying = false;

		public bool IsDay { get; private set; }
		public int HalfDaysElapsed { get; private set; }

		internal IDictionary<string, ISet<string>> Admins { get; private set; }



		////////////////

		public HamstarHelpersLogic( HamstarHelpersMod mymod ) {
			var modworld = mymod.GetModWorld<HamstarHelpersWorld>();

			this.Admins = new Dictionary<string, ISet<string>> { { modworld.UID, new HashSet<string>() } };
		}
		
		////////////////

		public bool IsLoaded( HamstarHelpersMod mymod ) {
//DebugHelpers.DebugHelpers.SetDisplay( "load", "HasSyncedModData: "+ this.HasSyncedModData + ", HasSetupContent: "+ mymod.HasSetupContent + ", HasCorrectID: "+ modworld.HasCorrectID+ ", HasSyncedModSettings: "+ myplayer.HasSyncedModSettings+ ", HasSyncedPlayerData: " + myplayer.HasSyncedPlayerData, 30 );
			if( !this.HasSyncedModData ) { return false; }

			if( !mymod.HasSetupContent ) { return false; }

			var modworld = mymod.GetModWorld<HamstarHelpersWorld>();
			if( !modworld.HasCorrectID ) { return false; }

			if( Main.netMode == 0 || Main.netMode == 1 ) {
				var myplayer = Main.LocalPlayer.GetModPlayer<HamstarHelpersPlayer>();
				return myplayer.HasSyncedModSettings && myplayer.HasSyncedPlayerData;
			}
			return false;
		}

		////////////////

		public void Load( HamstarHelpersMod mymod, TagCompound tags ) {
			var modworld = mymod.GetModWorld<HamstarHelpersWorld>();

			if( tags.ContainsKey( "world_id" ) ) {
				this.HalfDaysElapsed = tags.GetInt( "half_days_elapsed_" + modworld.ObsoleteID );
			}

			if( tags.ContainsKey( "admin_world_uid_count" ) ) {
				int world_count = tags.GetInt( "admin_world_uid_count" );

				for( int i = 0; i < world_count; i++ ) {
					string world_uid = tags.GetString( "admin_world_uid_" + i );
					int admin_count = tags.GetInt( "admin_world_uid_" + world_uid + "_count" );

					this.Admins[world_uid] = new HashSet<string>();

					for( int j = 0; j < admin_count; j++ ) {
						string admin = tags.GetString( "admin_world_uid_" + world_uid + "_" + j );
						this.Admins[world_uid].Add( admin );
					}
				}
			}

			this.HasSyncedModData = true;
		}

		public void Save( HamstarHelpersMod mymod, TagCompound tags ) {
			var modworld = mymod.GetModWorld<HamstarHelpersWorld>();

			tags.Set( "half_days_elapsed_" + modworld.ObsoleteID, (int)this.HalfDaysElapsed );
			tags.Set( "admin_world_uid_count", this.Admins.Count );

			int i = 0;
			foreach( var kv in this.Admins ) {
				tags.Set( "admin_world_uid_" + i++, kv.Key );
				tags.Set( "admin_world_uid_" + kv.Key + "_count", kv.Value.Count );

				int j = 0;
				foreach( string plr_uid in kv.Value ) {
					tags.Set( "admin_world_uid_" + kv.Key + "_" + j++, plr_uid );
				}
			}
		}
		
		public void LoadFromNetwork( int half_days ) {
			this.HalfDaysElapsed = half_days;

			this.HasSyncedModData = true;
		}

		////////////////

		public void NetSend( BinaryWriter writer ) {
			writer.Write( (string)JsonConfig<IDictionary<string, ISet<string>>>.Serialize( this.Admins ) );
		}

		public void NetReceive( BinaryReader reader ) {
			string admins = reader.ReadString();

			this.Admins = JsonConfig<IDictionary<string, ISet<string>>>.Deserialize( admins );
		}

		
		////////////////
		
		public bool IsPlaying() {
			if( Main.netMode == 1 && !this.HasSyncedModData ) {  // Client
				return false;
			}
			if( Main.netMode != 2 && !this.IsClientPlaying ) {  // Client or single
				return false;
			}
			if( Main.netMode == 2 && !this.IsServerPlaying ) {  // Server
				return false;
			}
			return true;
		}

		public bool IsFullyReady() {
			if( !this.IsPlaying() ) {
				return false;
			}
			if( this.StartupDelay++ < ( 60 * 2 ) ) {    // Seems needed for day/night tracking (and possibly other things?)
				return false;
			}
			return true;
		}


		////////////////
		
		public void Update( HamstarHelpersMod mymod ) {
			if( !this.IsLoaded( mymod ) ) {
				return;
			}

			if( this.IsPlaying() ) {
				mymod.ControlPanel.UpdateModList();
			}

			// Simply idle until ready (seems needed)
			if( !this.IsFullyReady() ) {
				this.IsDay = Main.dayTime;
				return;
			}

			this.UpdateDay( mymod );

			AltProjectileInfo.UpdateAll();
			AltNPCInfo.UpdateAll();
		}

		////////////////

		private void UpdateDay( HamstarHelpersMod mymod ) {
			if( this.IsDay != Main.dayTime ) {
				this.HalfDaysElapsed++;

				if( !this.IsDay ) {
					foreach( var kv in mymod.WorldHelpers.DayHooks ) { kv.Value(); }
				} else {
					foreach( var kv in mymod.WorldHelpers.NightHooks ) { kv.Value(); }
				}
			}
			this.IsDay = Main.dayTime;
		}


		////////////////

		public bool IsAdmin( Player player ) {
			var modworld = HamstarHelpersMod.Instance.GetModWorld<HamstarHelpersWorld>();
			string plr_id = TmlPlayerHelpers.GetUniqueId( player );

			return this.Admins[ modworld.UID ].Contains( plr_id );
		}

		public void SetAsAdmin( Player player ) {
			var modworld = HamstarHelpersMod.Instance.GetModWorld<HamstarHelpersWorld>();
			string plr_id = TmlPlayerHelpers.GetUniqueId( player );

			this.Admins[ modworld.UID ].Add( plr_id );
		}

		public bool RemoveAsAdmin( Player player ) {
			var modworld = HamstarHelpersMod.Instance.GetModWorld<HamstarHelpersWorld>();
			string plr_id = TmlPlayerHelpers.GetUniqueId( player );

			if( this.Admins[ modworld.UID ].Contains( plr_id ) ) {
				this.Admins[ modworld.UID ].Remove( plr_id );
				return true;
			}
			return false;
		}
	}
}
