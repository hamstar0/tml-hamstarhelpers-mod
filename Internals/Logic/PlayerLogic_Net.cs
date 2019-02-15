using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Internals.NetProtocols;
using HamstarHelpers.Services.Promises;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Internals.Logic {
	class PlayerLogicPromiseArguments : PromiseArguments {
		public int Who;
	}




	partial class PlayerLogic {
		public void OnCurrentClientConnect( Player player ) {
			PacketProtocolSentToEither.QuickSendToServer<PlayerNewIdProtocol>();
		}

		public void OnServerConnect( Player player ) {
			var mymod = ModHelpersMod.Instance;

			this.HasSyncedModSettings = true;
			this.HasSyncedWorldData = true;
			this.IsSynced = true;

			var args = new PlayerLogicPromiseArguments { Who = player.whoAmI };
			Promises.TriggerValidatedPromise( PlayerLogic.ServerConnectValidator, PlayerLogic.MyValidatorKey, args );

			PacketProtocolSentToEither.QuickRequestToClient<PlayerNewIdProtocol>( player.whoAmI, -1, -1 );
		}


		public void OnSingleEnterWorld( Player player ) {
			var mymod = ModHelpersMod.Instance;

			if( !this.HasLoadedUID ) {
				LogHelpers.Warn( "No UID for " + player.name + " (" + player.whoAmI + ")" );
				this.HasLoadedUID = true; // Ugly failsafe
			}

			if( !mymod.ConfigJson.LoadFile() ) {
				mymod.ConfigJson.SaveFile();
			}
			
			this.FinishModSettingsSyncFromServer();
			this.FinishWorldDataSyncFromServer();
		}


		public void OnCurrentClientEnterWorld( Player player ) {
			var mymod = ModHelpersMod.Instance;

			if( !this.HasLoadedUID ) {
				LogHelpers.Warn( "No UID for " + player.name + " (" + player.whoAmI + ") to send to server" );
				this.HasLoadedUID = true;	// Ugly failsafe
			}

			// Send
			PacketProtocolSendToServer.QuickSend<PlayerOldIdProtocol>();
			PlayerDataProtocol.SyncToEveryone( this.PermaBuffsById, this.HasBuffIds, this.EquipSlotsToItemTypes );

			// Receive
			PacketProtocolRequestToServer.QuickRequest<ModSettingsProtocol>( -1 );
			PacketProtocolRequestToServer.QuickRequest<WorldDataProtocol>( -1 );
		}


		////////////////

		public void FinishModSettingsSyncFromServer() {
			this.HasSyncedModSettings = true;
			if( ModHelpersMod.Instance.Config.DebugModeNetInfo ) {
				LogHelpers.Alert();
			}
			if( this.HasSyncedState() ) { this.FinishSyncFromServer(); }
		}

		public void FinishWorldDataSyncFromServer() {
			this.HasSyncedWorldData = true;
			if( ModHelpersMod.Instance.Config.DebugModeNetInfo ) {
				LogHelpers.Alert();
			}
			if( this.HasSyncedState() ) { this.FinishSyncFromServer(); }
		}


		public bool HasSyncedState() {
			return this.HasSyncedModSettings && this.HasSyncedWorldData && this.HasLoadedUID;
		}


		private void FinishSyncFromServer() {
			if( this.IsSynced ) { return; }

			var mymod = ModHelpersMod.Instance;

			if( mymod.Config.DebugModeNetInfo ) {
				LogHelpers.Alert();
			}

			mymod.ControlPanel.LoadModListAsync();

			this.IsSynced = true;
		}


		////////////////

		public void NetReceiveDataClient( ISet<int> permaBuffIds, ISet<int> hasBuffIds, IDictionary<int, int> equipSlotsToItemTypes ) {
			this.PermaBuffsById = permaBuffIds;
			this.HasBuffIds = hasBuffIds;
			this.EquipSlotsToItemTypes = equipSlotsToItemTypes;
		}

		public void NetReceiveDataServer( ISet<int> permaBuffIds, ISet<int> hasBuffIds, IDictionary<int, int> equipSlotsToItemTypes ) {
			this.PermaBuffsById = permaBuffIds;
			this.HasBuffIds = hasBuffIds;
			this.EquipSlotsToItemTypes = equipSlotsToItemTypes;
		}


		public void NetReceiveIdServer( bool hasUid, string uid ) {
			this.HasLoadedUID = hasUid;
			this.PrivateUID = uid;
		}
	}
}
