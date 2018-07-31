﻿using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.UI;
using HamstarHelpers.Internals.NetProtocols;
using HamstarHelpers.Services.Promises;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Internals.Logic {
	internal class PlayerLogicPromiseValidator : PromiseValidator {
		internal readonly static object MyValidatorKey;
		internal readonly static PlayerLogicPromiseValidator ConnectValidator;

		////////////////

		static PlayerLogicPromiseValidator() {
			PlayerLogicPromiseValidator.ConnectValidator = new PlayerLogicPromiseValidator();
		}

		////////////////

		public Player MyPlayer;

		////////////////

		public PlayerLogicPromiseValidator() : base( PlayerLogicPromiseValidator.MyValidatorKey ) { }
	}




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
			}
			PlayerDataProtocol.SyncToEveryone( this.PermaBuffsById, this.HasBuffIds, this.EquipSlotsToItemTypes );
			
			PacketProtocol.QuickRequestToServer<ModSettingsProtocol>();
			PacketProtocol.QuickRequestToServer<WorldDataProtocol>();

			mymod.ControlPanel.LoadModListAsync();
		}

		public void OnEnterWorldServer( HamstarHelpersMod mymod, Player player ) {
			this.FinishModSettingsSync();
			this.FinishWorldDataSync();

			PlayerLogicPromiseValidator.ConnectValidator.MyPlayer = player;
			Promises.TriggerValidatedPromise( PlayerLogicPromiseValidator.ConnectValidator, PlayerLogicPromiseValidator.MyValidatorKey );
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
