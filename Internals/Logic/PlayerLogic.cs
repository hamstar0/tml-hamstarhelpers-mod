﻿using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader.IO;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Internals.UI;


namespace HamstarHelpers.Internals.Logic {
	/// @private
	partial class PlayerLogic {
		private ISet<int> PermaBuffsById = new HashSet<int>();
		private ISet<int> HasBuffIds = new HashSet<int>();
		private IDictionary<int, int> EquipSlotsToItemTypes = new Dictionary<int, int>();

		private uint TestPing = 0;

		public DialogManager DialogManager = new DialogManager();


		////////////////

		public string OldPrivateUID { get; private set; }
		public bool HasLoadedOldUID { get; private set; }

		public bool HasSyncedWorldData { get; private set; }
		public bool IsSynced { get; private set; }



		////////////////

		public PlayerLogic() {
			this.OldPrivateUID = Guid.NewGuid().ToString( "D" );
			this.HasLoadedOldUID = false;
			this.HasSyncedWorldData = false;
			this.IsSynced = false;
		}


		////////////////

		public void Load( TagCompound tags ) {
			try {
				if( tags.ContainsKey( "uid" ) ) {
					this.OldPrivateUID = tags.GetString( "uid" );
				}
				if( tags.ContainsKey( "perma_buffs" ) ) {
					var permaBuffs = tags.GetList<int>( "perma_buffs" );
					this.PermaBuffsById = new HashSet<int>( permaBuffs );
				}
			} catch( Exception e ) {
				LogHelpers.Warn( e.ToString() );
			}

			this.HasLoadedOldUID = true;
		}

		public void Save( TagCompound tags ) {
			var permaBuffs = this.PermaBuffsById.ToArray();

			tags["uid"] = this.OldPrivateUID;
			tags["perma_buffs"] = permaBuffs;
		}
	}
}
