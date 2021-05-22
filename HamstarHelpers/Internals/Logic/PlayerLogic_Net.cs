using System.Collections.Generic;
using Terraria;
using HamstarHelpers.Internals.ControlPanel.ModControlPanel;
using HamstarHelpers.Internals.NetProtocols;
using HamstarHelpers.Libraries.Debug;


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
				LogLibraries.Warn( "No (old) UID for " + player.name + " (" + player.whoAmI + ")" );
				this.HasLoadedOldUID = true;	// Ugly failsafe; don't really know why data from ModPlayer.Load isn't available here
			}
			
			this.FinishWorldDataSyncOnLocal();
		}


		public void OnCurrentClientEnterWorld( Player player ) {
			if( !this.HasLoadedOldUID ) {
				LogLibraries.Alert( "No (old) UID for " + player.name + " (" + player.whoAmI + ") to send to server" );
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
				LogLibraries.Alert();
			}
			if( this.HasAllSyncedState() ) {
				this.FinishAllSyncOnLocal();
			}
		}

		////

		public bool HasAllSyncedState() {
			if( ModHelpersConfig.Instance.DebugModeNetInfo ) {
				LogLibraries.AlertOnce(
					"HasSyncedWorldData: " + this.HasSyncedWorldData +
					", HasLoadedOldUID: " + this.HasLoadedOldUID
				);
			}

			return this.HasSyncedWorldData && this.HasLoadedOldUID;
		}

		private void FinishAllSyncOnLocal() {
			if( this.IsSynced ) { return; }

			var mymod = ModHelpersMod.Instance;

			if( ModHelpersConfig.Instance.DebugModeNetInfo ) {
				LogLibraries.Alert();
			}

			this.IsSynced = true;

			UIModControlPanelTab uiModCtrlPanel = mymod.ControlPanelUI.DefaultTab;
			uiModCtrlPanel.LoadModListAsync();
		}

		private void FinishAllSyncOnServer() {
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
			
			if( this.HasAllSyncedState() ) { this.FinishAllSyncOnServer(); }
		}
	}
}
