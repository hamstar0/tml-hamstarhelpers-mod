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

		public DialogManager DialogManager = new DialogManager();

		public bool HasSyncedModSettings { get; private set; }
		public bool HasSyncedModData { get; private set; }



		////////////////

		public void SendClientChanges( HamstarHelpersMod mymod, Player me, ModPlayer client_player ) {
			var myclient = (HamstarHelpersPlayer)client_player;
			var logic = myclient.Logic;
			bool uid_changed = this.HasUID != logic.HasUID || this.PrivateUID != logic.PrivateUID;
			
			if( !logic.PermaBuffsById.SetEquals( this.PermaBuffsById ) || uid_changed ) {
				this.NetSend( mymod , me, -1, -1, true );
			}
		}

		public void OnEnterWorld( HamstarHelpersMod mymod, Player player ) {
			if( Main.netMode == 0 ) {   // Single player only
				if( !mymod.JsonConfig.LoadFile() ) {
					mymod.JsonConfig.SaveFile();
				}
			}

			// Sync mod (world) data; must be called after world is loaded
			if( Main.netMode == 1 ) {
				var player_data = new PlayerDataProtocol( player.whoAmI, this.HasUID, this.PrivateUID, this.PermaBuffsById );
				player_data.SendData( -1, -1 );
				player_data.SendRequest( -1, -1 );
				PacketProtocol.QuickSendRequest<ModSettingsProtocol>( -1, -1 );
				PacketProtocol.QuickSendRequest<ModDataProtocol>( -1, -1 );
			}

			if( Main.netMode != 1 ) {   // NOT client; clients won't receive their own data back from server
				this.FinishModSettingsSync();
				this.FinishModDataSync();
			}
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

		public void Update( HamstarHelpersMod mymod, Player player ) {
			if( player.whoAmI == Main.myPlayer ) { // Current player
				var modworld = mymod.GetModWorld<HamstarHelpersWorld>();

				SimpleMessage.UpdateMessage();
				mymod.PlayerMessages.Update();
				this.DialogManager.Update( mymod );
			
				if( Main.netMode == 1 ) {   // Client only
					modworld.WorldLogic.Update( mymod );
				} else if( Main.netMode == 2 ) {    // Server
					modworld.WorldLogic.IsServerPlaying = true;
				}
			}

			foreach( int buff_id in this.PermaBuffsById ) {
				player.AddBuff( buff_id, 3 );
			}

			this.UpdateTml( mymod, player );
		}


		////////////////

		public void ProcessTriggers( HamstarHelpersMod mymod, TriggersSet triggers_set ) {
			if( mymod.ControlPanelHotkey.JustPressed ) {
				if( mymod.Config.DisableControlPanel ) {
					Main.NewText( "Control panel disabled.", Color.Red );
				} else {
					mymod.ControlPanel.Open();
				}
			}
		}
	}
}
