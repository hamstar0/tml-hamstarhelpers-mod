using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader.IO;
using HamstarHelpers.Libraries.Debug;
using HamstarHelpers.Libraries.DotNET.Extensions;
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

		public override bool Equals( object obj ) {
			var clone = obj as PlayerLogic;
			if( clone == null ) {
				return false;
			}

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
			if( clone.TestPing != this.TestPing ) {
				return false;
			}
			if( clone.DialogManager != this.DialogManager ) {
				return false;
			}
			if( clone.OldPrivateUID != this.OldPrivateUID ) {
				return false;
			}
			if( clone.HasLoadedOldUID != this.HasLoadedOldUID ) {
				return false;
			}
			if( clone.HasSyncedWorldData != this.HasSyncedWorldData ) {
				return false;
			}
			if( clone.IsSynced != this.IsSynced ) {
				return false;
			}
			if( clone.ActiveCheats != this.ActiveCheats ) {
				return false;
			}
			return true;
		}

		public override int GetHashCode() {
			return this.PermaBuffsById.GetHashCode()
				+ this.HasBuffIds.GetHashCode()
				+ this.EquipSlotsToItemTypes.GetHashCode()
				+ this.ActiveCheats.GetHashCode()
				+ this.TestPing.GetHashCode()
				+ this.DialogManager.GetHashCode()
				+ this.OldPrivateUID.GetHashCode()
				+ this.HasLoadedOldUID.GetHashCode()
				+ this.HasSyncedWorldData.GetHashCode()
				+ this.IsSynced.GetHashCode()
				+ this.ActiveCheats.GetHashCode();
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
				LogLibraries.Warn( e.ToString() );
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
