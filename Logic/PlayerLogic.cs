using HamstarHelpers.NetHelpers;
using HamstarHelpers.NetProtocols;
using HamstarHelpers.UIHelpers.Elements;
using HamstarHelpers.Utilities.Messages;
using HamstarHelpers.Utilities.Network;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;


namespace HamstarHelpers.Logic {
	partial class PlayerLogic {
		public string PrivateUID { get; private set; }
		public bool HasUID { get; private set; }
		public ISet<int> PermaBuffsById { get; private set; }
		internal string ControlPanelNewSince = "1.0.0";

		private ISet<int> HasBuffIds = new HashSet<int>();
		private IDictionary<int, int> EquipSlotsToItemTypes = new Dictionary<int, int>();
		private uint TestPing = 0;

		public DialogManager DialogManager = new DialogManager();

		public bool HasSyncedModSettings { get; private set; }
		public bool HasSyncedModData { get; private set; }
		private bool IsFinishedSyncing = false;



		////////////////

		public void SendClientChanges( HamstarHelpersMod mymod, Player me, ModPlayer client_player ) {
			var myclient = (HamstarHelpersPlayer)client_player;
			var clone = myclient.Logic;
			bool uid_changed = this.HasUID != clone.HasUID || this.PrivateUID != clone.PrivateUID;
			
			if( !clone.PermaBuffsById.SetEquals( this.PermaBuffsById ) || uid_changed ) {
				HHPlayerDataProtocol.SendToClient( me.whoAmI, this.HasUID, this.PrivateUID, this.PermaBuffsById );
			}
		}

		////////////////

		public void OnEnterWorldForSingle( HamstarHelpersMod mymod, Player player ) {
			if( !mymod.JsonConfig.LoadFile() ) {
				mymod.JsonConfig.SaveFile();
			}

			this.FinishModSettingsSync();
			this.FinishModDataSync();

			mymod.ControlPanel.LoadModListAsync();
		}

		public void OnEnterWorldForClient( HamstarHelpersMod mymod, Player player ) {
			HHPlayerDataProtocol.SyncToOtherClients( player.whoAmI, this.HasUID, this.PrivateUID, this.PermaBuffsById );

			PacketProtocol.QuickRequestFromServer<HHPlayerDataProtocol>();
			PacketProtocol.QuickRequestFromServer<HHModSettingsProtocol>();
			PacketProtocol.QuickRequestFromServer<HHModDataProtocol>();

			mymod.ControlPanel.LoadModListAsync();
		}

		public void OnEnterWorldForServer( HamstarHelpersMod mymod, Player player ) {
			this.FinishModSettingsSync();
			this.FinishModDataSync();
		}


		////////////////

		public bool IsSynced() {
			return this.HasSyncedModSettings && this.HasSyncedModData;
		}

		public void FinishModSettingsSync() {
			this.HasSyncedModSettings = true;
			if( this.IsSynced() ) { this.FinishSync(); }
		}
		public void FinishModDataSync() {
			this.HasSyncedModData = true;
			if( this.IsSynced() ) { this.FinishSync(); }
		}

		private void FinishSync() {
			if( this.IsFinishedSyncing ) { return; }
			this.IsFinishedSyncing = true;

			/*if( Main.netMode == 1 ) {
				Timers.SetTimer( "server_connect", 60 * 30, delegate () {   // 30 seconds
					if( ServerBrowserReport.CanAddToBrowser() ) {
						ServerBrowserReport.AnnounceServerConnect();
					}
					return false;
				} );
			}*/
		}


		////////////////

		public void PreUpdateSingle( HamstarHelpersMod mymod, Player player ) {
			if( player.whoAmI == Main.myPlayer ) { // Current player
				var modworld = mymod.GetModWorld<HamstarHelpersWorld>();

				SimpleMessage.UpdateMessage();
				mymod.PlayerMessages.Update();
				this.DialogManager.Update( mymod );
			}

			foreach( int buff_id in this.PermaBuffsById ) {
				player.AddBuff( buff_id, 3 );
			}

			this.UpdateTml( mymod, player );
		}

		public void PreUpdateClient( HamstarHelpersMod mymod, Player player ) {
			this.PreUpdateSingle( mymod, player );

			if( player.whoAmI == Main.myPlayer ) { // Current player
				var myworld = mymod.GetModWorld<HamstarHelpersWorld>();
				myworld.WorldLogic.PreUpdateSingle( mymod );
			}

			// Update ping every 15 seconds
			if( this.TestPing++ > (60*15) ) {
				PacketProtocol.QuickSendToServer<HHPingProtocol>();
				this.TestPing = 0;
			}
		}

		public void PreUpdateServer( HamstarHelpersMod mymod, Player player ) {
			if( player.whoAmI == Main.myPlayer ) { // Current player
				var modworld = mymod.GetModWorld<HamstarHelpersWorld>();
				mymod.TmlLoadHelpers.HasServerBegunHavingPlayers = true;
			}

			foreach( int buff_id in this.PermaBuffsById ) {
				player.AddBuff( buff_id, 3 );
			}

			this.UpdateTml( mymod, player );
		}


		////////////////

		public void ProcessTriggers( HamstarHelpersMod mymod, TriggersSet triggers_set ) {
			if( mymod.ControlPanelHotkey.JustPressed ) {
				if( mymod.Config.DisableControlPanelHotkey ) {
					Main.NewText( "Control panel hotkey disabled.", Color.Red );
				} else {
					mymod.ControlPanel.Open();
				}
			}
		}
	}
}
