using HamstarHelpers.DebugHelpers;
using HamstarHelpers.NetProtocols;
using HamstarHelpers.UIHelpers.Elements;
using HamstarHelpers.Utilities.Network;
using System.Collections.Generic;
using Terraria;


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
		
		public void OnEnterWorldForSingle( HamstarHelpersMod mymod, Player player ) {
			if( !mymod.JsonConfig.LoadFile() ) {
				mymod.JsonConfig.SaveFile();
			}

			this.FinishModSettingsSync();
			this.FinishModDataSync();

			mymod.ControlPanel.LoadModListAsync();
		}

		public void OnEnterWorldForClient( HamstarHelpersMod mymod, Player player ) {
			HHPlayerDataProtocol.SyncToEveryone( this.HasUID, this.PrivateUID, this.PermaBuffsById, this.HasBuffIds,
				this.EquipSlotsToItemTypes );
			
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
