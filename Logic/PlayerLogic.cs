using HamstarHelpers.NetHelpers;
using HamstarHelpers.UIHelpers.Elements;
using HamstarHelpers.Utilities.Messages;
using HamstarHelpers.Utilities.Network;
using HamstarHelpers.WebRequests;
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



		////////////////

		public void SendClientChanges( HamstarHelpersMod mymod, Player me, ModPlayer client_player ) {
			var myclient = (HamstarHelpersPlayer)client_player;
			var logic = myclient.Logic;
			bool uid_changed = this.HasUID != logic.HasUID || this.PrivateUID != logic.PrivateUID;
			
			if( !logic.PermaBuffsById.SetEquals( this.PermaBuffsById ) || uid_changed ) {
				var protocol = new HHPlayerDataProtocol( me.whoAmI, this.HasUID, this.PrivateUID, this.PermaBuffsById );
				protocol.SendData( -1, -1, false );
			}
		}


		public void OnEnterWorldForServer( Player player ) {
			ServerBrowserReport.AnnounceServer();
		}


		public void OnEnterWorld( HamstarHelpersMod mymod, Player player ) {
			if( Main.netMode == 0 ) {   // Single player only
				if( !mymod.JsonConfig.LoadFile() ) {
					mymod.JsonConfig.SaveFile();
				}
			}

			// Sync mod (world) data; must be called after world is loaded
			if( Main.netMode == 1 ) {
				var player_data = new HHPlayerDataProtocol( player.whoAmI, this.HasUID, this.PrivateUID, this.PermaBuffsById );
				player_data.SendData( -1, -1, true );
				player_data.SendRequest( -1, -1 );
				PacketProtocol.QuickSendRequest<HHModSettingsProtocol>( -1, -1 );
				PacketProtocol.QuickSendRequest<HHModDataProtocol>( -1, -1 );
				ServerBrowserReport.AnnounceServerConnect();
			}

			if( Main.netMode != 1 ) {   // NOT client; clients won't receive their own data back from server
				this.FinishModSettingsSync();
				this.FinishModDataSync();
			}

			mymod.ControlPanel.LoadModList();
		}

		////////////////

		public bool IsSynced() {
			return this.HasSyncedModSettings && this.HasSyncedModData;
		}

		public void FinishModSettingsSync() {
			this.HasSyncedModSettings = true;
		}
		public void FinishModDataSync() {
			this.HasSyncedModData = true;
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
				myworld.WorldLogic.PreUpdateNotServer( mymod );
			}

			// Update ping every 10 seconds
			if( this.TestPing % (600) == 0 ) {
				PacketProtocol.QuickSendData<HHPingProtocol>( -1, -1, false );
			}

			// Update server status 60 seconds
			if( this.TestPing % ( 3600 ) == 0 ) {
				ServerBrowserReport.AnnounceServerConnect();
			}

			this.TestPing++;
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
