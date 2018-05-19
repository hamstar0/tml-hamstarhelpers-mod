using HamstarHelpers.NetProtocols;
using HamstarHelpers.UIHelpers.Elements;
using HamstarHelpers.Utilities.Network;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Logic {
	partial class PlayerLogic {
		public string PrivateUID { get; private set; }
		public bool HasUID { get; private set; }

		private ISet<int> PermaBuffsById = new HashSet<int>();
		private ISet<int> HasBuffIds = new HashSet<int>();
		private IDictionary<int, int> EquipSlotsToItemTypes = new Dictionary<int, int>();

		private uint TestPing = 0;

		public DialogManager DialogManager = new DialogManager();

		public bool HasSyncedModSettings { get; private set; }
		public bool HasSyncedModData { get; private set; }
		private bool IsFinishedSyncing = false;



		////////////////

		public void ClientClone( HamstarHelpersPlayer client_clone ) {
			client_clone.Logic.PermaBuffsById = this.PermaBuffsById;
			client_clone.Logic.HasBuffIds = this.HasBuffIds;
			client_clone.Logic.EquipSlotsToItemTypes = this.EquipSlotsToItemTypes;
		}

		public void SendClientChanges( HamstarHelpersMod mymod, Player me, ModPlayer client_player ) {
			var myclient = (HamstarHelpersPlayer)client_player;
			var clone = myclient.Logic;

			bool send = this.HasUID != clone.HasUID || this.PrivateUID != clone.PrivateUID;
			
			if( !send && !clone.PermaBuffsById.SetEquals( this.PermaBuffsById ) ) { send = true; }
			if( !send && !clone.HasBuffIds.SetEquals( this.HasBuffIds ) ) { send = true; }
			if( !send ) {
				var dict1 = clone.EquipSlotsToItemTypes;
				var dict2 = this.EquipSlotsToItemTypes;
				if( dict1.Count == dict2.Count && !dict1.Except(dict2).Any() ) { send = true; }
			}
			if( !send && clone.HasSyncedModSettings != this.HasSyncedModSettings ) { send = true; }
			if( !send && clone.HasSyncedModData != this.HasSyncedModData ) { send = true; }
			if( !send && clone.IsFinishedSyncing != this.IsFinishedSyncing ) { send = true; }

			if( send ) {
				if( Main.netMode == 1 ) {
					HHPlayerDataProtocol.SendStateToServer( me.whoAmI, this.HasUID, this.PrivateUID, this.PermaBuffsById,
						this.HasBuffIds, this.EquipSlotsToItemTypes );
				} else if( Main.netMode == 2 ) {
					HHPlayerDataProtocol.SendStateToClient( me.whoAmI, me.whoAmI, this.HasUID, this.PermaBuffsById,
						this.HasBuffIds, this.EquipSlotsToItemTypes );
				}
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
			HHPlayerDataProtocol.SyncToEveryone( player.whoAmI, this.HasUID, this.PrivateUID, this.PermaBuffsById,
				this.HasBuffIds, this.EquipSlotsToItemTypes );
			
			PacketProtocol.QuickRequestToServer<HHModSettingsProtocol>();
			PacketProtocol.QuickRequestToServer<HHModDataProtocol>();

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
		}
	}
}
