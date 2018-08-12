using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.UI;
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
		internal readonly static object MyValidatorKey;
		internal readonly static PromiseValidator ServerConnectValidator;


		////////////////

		static PlayerLogic() {
			PlayerLogic.MyValidatorKey = new object();
			PlayerLogic.ServerConnectValidator = new PromiseValidator( PlayerLogic.MyValidatorKey );
		}



		////////////////

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
		
		public void OnEnterWorldSingle( HamstarHelpersMod mymod, Player player ) {
			if( !mymod.ConfigJson.LoadFile() ) {
				mymod.ConfigJson.SaveFile();
			}

			this.FinishModSettingsSync();
			this.FinishWorldDataSync();

			mymod.ControlPanel.LoadModListAsync();
		}

		public void OnEnterWorldClient( HamstarHelpersMod mymod, Player player ) {
			if( this.HasUID ) {
				PacketProtocol.QuickSendToServer<PlayerIdProtocol>();
			} else {
				LogHelpers.Log( "ModHelpers.PlayerLogic.OnEnterWorldClient - No UID for "+player.name+" ("+player.whoAmI+") to send to server" );
			}
			PlayerDataProtocol.SyncToEveryone( this.PermaBuffsById, this.HasBuffIds, this.EquipSlotsToItemTypes );
			
			PacketProtocol.QuickRequestToServer<ModSettingsProtocol>();
			PacketProtocol.QuickRequestToServer<WorldDataProtocol>();

			mymod.ControlPanel.LoadModListAsync();
		}

		public void OnEnterWorldServer( HamstarHelpersMod mymod, Player player ) {
			this.FinishModSettingsSync();
			this.FinishWorldDataSync();

			var args = new PlayerLogicPromiseArguments { Who = player.whoAmI };

			Promises.TriggerValidatedPromise( PlayerLogic.ServerConnectValidator, PlayerLogic.MyValidatorKey, args );
		}


		////////////////

		public bool IsSynced() {
			return this.HasSyncedModSettings && this.HasSyncedModData;
		}

		public void FinishModSettingsSync() {
			this.HasSyncedModSettings = true;
			if( this.IsSynced() ) { this.FinishSync(); }
		}
		public void FinishWorldDataSync() {
			this.HasSyncedModData = true;
			if( this.IsSynced() ) { this.FinishSync(); }
		}

		private void FinishSync() {
			if( this.IsFinishedSyncing ) { return; }
			this.IsFinishedSyncing = true;
		}
	}
}
