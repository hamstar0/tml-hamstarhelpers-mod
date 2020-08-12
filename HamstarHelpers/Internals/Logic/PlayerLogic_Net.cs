using System.Collections.Generic;
using Terraria;
using HamstarHelpers.Internals.ControlPanel.ModControlPanel;
using HamstarHelpers.Internals.NetProtocols;
using HamstarHelpers.Helpers.Debug;


namespace HamstarHelpers.Internals.Logic {
	/// @private
	partial class PlayerLogic {
		public void OnCurrentClientConnect() {
			PlayerNewIdProtocol.QuickSendToServer();
		}

		public void OnServerConnect( Player player ) {
			this.HasSyncedWorldData = true;
			this.IsSynced = true;	// Technically this should only be set upon sync receipt of player's 'old' uid...

			PlayerOldIdRequestProtocol.QuickRequestToClient( player.whoAmI );
			PlayerNewIdRequestProtocol.QuickRequestToClient( player.whoAmI );
		}

		////

		public void OnSingleEnterWorld( Player player ) {
			if( !this.HasLoadedOldUID ) {
				LogHelpers.Warn( "No (old) UID for " + player.name + " (" + player.whoAmI + ")" );
				this.HasLoadedOldUID = true;	// Ugly failsafe; don't really know why data from ModPlayer.Load isn't available here
			}
			
			this.FinishWorldDataSyncOnLocal();
		}


		public void OnCurrentClientEnterWorld( Player player ) {
			if( !this.HasLoadedOldUID ) {
				LogHelpers.Alert( "No (old) UID for " + player.name + " (" + player.whoAmI + ") to send to server" );
				this.HasLoadedOldUID = true;	// Ugly failsafe; don't really know why data from ModPlayer.Load isn't available here
			}

			// Send
			PlayerOldIdProtocol.QuickSendToServer();
			PlayerDataProtocol.BroadcastToAll(
				(HashSet<int>)this.PermaBuffsById,
				(HashSet<int>)this.HasBuffIds,
				(Dictionary<int, int>)this.EquipSlotsToItemTypes
			);

			// Receive
			WorldDataRequestProtocol.QuickRequest();
		}


		////////////////

		public void FinishWorldDataSyncOnLocal() {
			this.HasSyncedWorldData = true;
			if( ModHelpersConfig.Instance.DebugModeNetInfo ) {
				LogHelpers.Alert();
			}
			if( this.HasSyncedState() ) { this.FinishSyncOnClient(); }
		}

		////

		public bool HasSyncedState() {
			if( ModHelpersConfig.Instance.DebugModeNetInfo ) {
				LogHelpers.AlertOnce(
					"HasSyncedWorldData: " + this.HasSyncedWorldData +
					", HasLoadedOldUID: " + this.HasLoadedOldUID
				);
			}

			return this.HasSyncedWorldData && this.HasLoadedOldUID;
		}

		private void FinishSyncOnClient() {
			if( this.IsSynced ) { return; }

			var mymod = ModHelpersMod.Instance;

			if( ModHelpersConfig.Instance.DebugModeNetInfo ) {
				LogHelpers.Alert();
			}

			this.IsSynced = true;

			UIModControlPanelTab uiModCtrlPanel = mymod.ControlPanel.DefaultTab;
			uiModCtrlPanel.LoadModListAsync();
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

		public void NetReceiveUIDOnServer( bool hasUid, string uid ) {
			this.HasLoadedOldUID = hasUid;
			this.OldPrivateUID = uid;
			
			if( this.HasSyncedState() ) { this.FinishSyncOnServer(); }
		}
	}
}
