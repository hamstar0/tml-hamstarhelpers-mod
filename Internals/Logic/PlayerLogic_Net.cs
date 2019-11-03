using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Internals.ControlPanel.ModControlPanel;
using HamstarHelpers.Internals.NetProtocols;
using HamstarHelpers.Services.Hooks.LoadHooks;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Internals.Logic {
	/// @private
	partial class PlayerLogic {
		public void OnCurrentClientConnect( Player player ) {
			PlayerNewIdProtocol.QuickSendToServer();
		}

		public void OnServerConnect( Player player ) {
			this.HasSyncedWorldData = true;
			this.IsSynced = true;	// Technically this should only be set upon sync receipt of player's 'old' uid...

			CustomLoadHooks.TriggerHook( PlayerLogic.ServerConnectHookValidator, PlayerLogic.MyValidatorKey, player.whoAmI );

			PlayerOldIdProtocol.QuickRequestToClient( player.whoAmI );
			PlayerNewIdProtocol.QuickRequestToClient( player.whoAmI );
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
			PlayerDataProtocol.SyncToEveryone( this.PermaBuffsById, this.HasBuffIds, this.EquipSlotsToItemTypes );

			// Receive
			WorldDataProtocol.QuickRequest();
		}


		////////////////

		public void FinishWorldDataSyncOnLocal() {
			this.HasSyncedWorldData = true;
			if( ModHelpersMod.Config.DebugModeNetInfo ) {
				LogHelpers.Alert();
			}
			if( this.HasSyncedState() ) { this.FinishSyncOnClient(); }
		}

		////

		public bool HasSyncedState() {
			if( ModHelpersMod.Config.DebugModeNetInfo ) {
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

			if( ModHelpersMod.Config.DebugModeNetInfo ) {
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
