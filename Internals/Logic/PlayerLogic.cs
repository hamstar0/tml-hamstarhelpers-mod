using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader.IO;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Internals.UI;
using HamstarHelpers.Services.Cheats;


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

		public CheatModeType ActiveCheats { get; private set; }



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
				if( tags.ContainsKey("uid") ) {
					this.OldPrivateUID = tags.GetString( "uid" );
				}
				if( tags.ContainsKey("perma_buffs") ) {
					IList<int> permaBuffs = tags.GetList<int>( "perma_buffs" );
					this.PermaBuffsById = new HashSet<int>( permaBuffs );
				}
				if( tags.ContainsKey("cheats") ) {
					this.ActiveCheats = (CheatModeType)tags.GetInt( "cheats" );
				}
			} catch( Exception e ) {
				LogHelpers.Warn( e.ToString() );
			}

			this.HasLoadedOldUID = true;
		}

		public void Save( TagCompound tags ) {
			int[] permaBuffs = this.PermaBuffsById.ToArray();

			tags["cheat"] = this.OldPrivateUID;
			tags["uid"] = this.OldPrivateUID;
			tags["perma_buffs"] = permaBuffs;
			tags["cheats"] = (int)this.ActiveCheats;
		}


		////////////////

		public void ToggleCheats( CheatModeType cheat ) {
			if( (this.ActiveCheats & cheat) == cheat ) {
				this.ActiveCheats = (CheatModeType)((int)this.ActiveCheats - (int)cheat);
			} else {
				this.ActiveCheats = (CheatModeType)((int)this.ActiveCheats + (int)cheat);
			}
		}
	}
}
