using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader.IO;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Extensions;
using HamstarHelpers.Services.Cheats;
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

		public CheatModeType ActiveCheats { get; private set; }

		public bool HasSyncedWorldData { get; private set; }
		public bool IsSynced { get; private set; }



		////////////////

		public PlayerLogic() {
			this.OldPrivateUID = Guid.NewGuid().ToString( "D" );
			this.HasLoadedOldUID = false;
			this.HasSyncedWorldData = false;
			this.IsSynced = false;
		}

		internal PlayerLogic Clone() {
			var clone = new PlayerLogic();
			clone.PermaBuffsById = new HashSet<int>( this.PermaBuffsById );
			clone.HasBuffIds = new HashSet<int>( this.HasBuffIds );
			clone.EquipSlotsToItemTypes = new Dictionary<int, int>( this.EquipSlotsToItemTypes );
			clone.TestPing = this.TestPing;
			clone.DialogManager = this.DialogManager;
			clone.OldPrivateUID = this.OldPrivateUID;
			clone.HasLoadedOldUID = this.HasLoadedOldUID;
			clone.HasSyncedWorldData = this.HasSyncedWorldData;
			clone.IsSynced = this.IsSynced;
			clone.ActiveCheats = this.ActiveCheats;
			return clone;
		}

		////

		public bool Equals( PlayerLogic clone ) {
			if( !clone.PermaBuffsById.SetEquals(this.PermaBuffsById) ) {
				return false;
			}
			if( !clone.HasBuffIds.SetEquals(this.HasBuffIds ) ) {
				return false;
			}
			if( !clone.EquipSlotsToItemTypes.Compare(this.EquipSlotsToItemTypes) ) {
				return false;
			}
			if( clone.ActiveCheats != this.ActiveCheats ) {
				return false;
			}
			return true;
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

		internal void SetCheats( CheatModeType cheat ) {
			this.ActiveCheats = cheat;
		}
	}
}
