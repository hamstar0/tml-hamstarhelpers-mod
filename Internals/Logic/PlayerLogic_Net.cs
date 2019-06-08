using HamstarHelpers.Components.Protocol.Packet.Interfaces;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Internals.ControlPanel.ModControlPanel;
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
			PacketProtocolSentToEither.QuickSendToTheServer<PlayerNewIdProtocol>();
		}

		public void OnServerConnect( Player player ) {
			var mymod = ModHelpersMod.Instance;

			this.HasSyncedModSettings = true;
			this.HasSyncedWorldData = true;
			this.IsSynced = true;	// Technically this should only be set upon sync receipt of player's 'old' uid...

			var args = new PlayerLogicPromiseArguments { Who = player.whoAmI };
			Promises.TriggerValidatedPromise( PlayerLogic.ServerConnectValidator, PlayerLogic.MyValidatorKey, args );

			PacketProtocolSentToEither.QuickRequestToClient<PlayerOldIdProtocol>( player.whoAmI, -1, -1 );
			PacketProtocolSentToEither.QuickRequestToClient<PlayerNewIdProtocol>( player.whoAmI, -1, -1 );
		}

		////

		public void OnSingleEnterWorld( Player player ) {
			var mymod = ModHelpersMod.Instance;

			if( !this.HasLoadedOldUID ) {
				LogHelpers.Warn( "No (old) UID for " + player.name + " (" + player.whoAmI + ")" );
				this.HasLoadedOldUID = true;	// Ugly failsafe; don't really know why data from ModPlayer.Load isn't available here
			}

			if( !mymod.ConfigJson.LoadFile() ) {
				mymod.ConfigJson.SaveFile();
			}
			
			this.FinishModSettingsSyncOnClient();
			this.FinishWorldDataSyncOnClient();
		}


		public void OnCurrentClientEnterWorld( Player player ) {
			var mymod = ModHelpersMod.Instance;

			if( !this.HasLoadedOldUID ) {
				LogHelpers.Alert( "No (old) UID for " + player.name + " (" + player.whoAmI + ") to send to server" );
				this.HasLoadedOldUID = true;	// Ugly failsafe; don't really know why data from ModPlayer.Load isn't available here
			}

			// Send
			PacketProtocolSendToServer.QuickSendToServer<PlayerOldIdProtocol>();
			PlayerDataProtocol.SyncToEveryone( this.PermaBuffsById, this.HasBuffIds, this.EquipSlotsToItemTypes );

			// Receive
			PacketProtocolRequestToServer.QuickRequest<ModSettingsProtocol>( -1 );
			PacketProtocolRequestToServer.QuickRequest<WorldDataProtocol>( -1 );
		}


		////////////////

		public void FinishModSettingsSyncOnClient() {
			this.HasSyncedModSettings = true;
			if( ModHelpersMod.Instance.Config.DebugModeNetInfo ) {
				LogHelpers.Alert();
			}
			if( this.HasSyncedState() ) { this.FinishSyncOnClient(); }
		}

		public void FinishWorldDataSyncOnClient() {
			this.HasSyncedWorldData = true;
			if( ModHelpersMod.Instance.Config.DebugModeNetInfo ) {
				LogHelpers.Alert();
			}
			if( this.HasSyncedState() ) { this.FinishSyncOnClient(); }
		}

		////

		public bool HasSyncedState() {
			return this.HasSyncedModSettings && this.HasSyncedWorldData && this.HasLoadedOldUID;
		}

		private void FinishSyncOnClient() {
			if( this.IsSynced ) { return; }

			var mymod = ModHelpersMod.Instance;

			if( mymod.Config.DebugModeNetInfo ) {
				LogHelpers.Alert();
			}

			UIModControlPanelTab uiModCtrlPanel = mymod.ControlPanel.DefaultTab;
			uiModCtrlPanel.LoadModListAsync();

			this.IsSynced = true;
		}

		private void FinishSyncOnServer() {
			if( this.IsSynced ) { return; }

			this.IsSynced = true;
		}


		////////////////

		public void NetReceiveDataOnClient( ISet<int> permaBuffIds, ISet<int> hasBuffIds, IDictionary<int, int> equipSlotsToItemTypes ) {
			this.PermaBuffsById = permaBuffIds;
			this.HasBuffIds = hasBuffIds;
			this.EquipSlotsToItemTypes = equipSlotsToItemTypes;
		}

		public void NetReceiveDataOnServer( ISet<int> permaBuffIds, ISet<int> hasBuffIds, IDictionary<int, int> equipSlotsToItemTypes ) {
			this.PermaBuffsById = permaBuffIds;
			this.HasBuffIds = hasBuffIds;
			this.EquipSlotsToItemTypes = equipSlotsToItemTypes;
		}

		////

		public void NetReceiveIdOnServer( bool hasUid, string uid ) {
			this.HasLoadedOldUID = hasUid;
			this.OldPrivateUID = uid;
			
			if( this.HasSyncedState() ) { this.FinishSyncOnServer(); }
		}
	}
}
