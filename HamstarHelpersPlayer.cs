using HamstarHelpers.NetProtocol;
using HamstarHelpers.TmlHelpers;
using HamstarHelpers.Utilities.Messages;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace HamstarHelpers {
	class HamstarHelpersPlayer : ModPlayer {
		public bool HasEnteredWorld { get; private set; }
		
		private ISet<int> HasBuffIds = new HashSet<int>();
		private IDictionary<int, int> EquipSlotsToItemTypes = new Dictionary<int, int>();

		internal ISet<int> PermaBuffsById = new HashSet<int>();


		////////////////

		public override void Initialize() {
			this.HasEnteredWorld = false;
		}

		public override void clientClone( ModPlayer client_clone ) {
			var clone = (HamstarHelpersPlayer)client_clone;	// <- This might be misinformed usage
			clone.HasEnteredWorld = this.HasEnteredWorld;
			clone.HasBuffIds = this.HasBuffIds;
			clone.EquipSlotsToItemTypes = this.EquipSlotsToItemTypes;
			clone.PermaBuffsById = this.PermaBuffsById;
		}

		////////////////

		public override void OnEnterWorld( Player player ) {
			if( player.whoAmI == this.player.whoAmI ) {    // Current player
				var mymod = (HamstarHelpersMod)this.mod;
				var modworld = mymod.GetModWorld<HamstarHelpersWorld>();
				
				if( Main.netMode == 1 ) {   // Client
					ClientPacketHandlers.SendRequestModDataFromClient( mymod );
				} else if( Main.netMode == 0 ) {    // Single
					this.PostEnterWorld();
				}

				mymod.HasCurrentPlayerEnteredWorld = true;
			}
		}

		public void PostEnterWorld() {
			var mymod = (HamstarHelpersMod)this.mod;
			var modworld = mymod.GetModWorld<HamstarHelpersWorld>();
			
			this.HasEnteredWorld = true;
		}


		////////////////

		internal string ControlPanelNewSince = "1.0.0";

		public override void Load( TagCompound tag ) {
			if( tag.ContainsKey( "ControlPanelNewSince" ) ) {
				this.ControlPanelNewSince = tag.GetString( "ControlPanelNewSince" );
			}
		}

		public override TagCompound Save() {
			return new TagCompound { { "ControlPanelNewSince", this.ControlPanelNewSince } };
		}


		////////////////

		public override void PreUpdate() {
			if( this.player.whoAmI == Main.myPlayer ) {	// Current player
				PlayerMessage.UpdatePlayerLabels();
				SimpleMessage.UpdateMessage();
			}

			var mymod = (HamstarHelpersMod)this.mod;
			var modworld = mymod.GetModWorld<HamstarHelpersWorld>();

			if( Main.netMode != 2 ) {   // Not server
				if( this.player.whoAmI == Main.myPlayer ) { // Current player only
					if( modworld.HasCorrectID && this.HasEnteredWorld ) {
						modworld.Logic.Update( mymod );
					}
				}
			} else {    // Server
				modworld.Logic.ReadyServer = true;	// Needed?
			}

			foreach( int buff_id in this.PermaBuffsById ) {
				this.player.AddBuff( buff_id, 3 );
			}

			this.CheckBuffHooks();
			this.CheckArmorEquipHooks();
		}

		////////////////

		private void CheckBuffHooks() {
			// Add new buffs
			for( int i = 0; i < this.player.buffTime.Length; i++ ) {
				if( this.player.buffTime[i] > 0 ) {
					int buff_id = this.player.buffType[i];
					if( !this.HasBuffIds.Contains( buff_id ) ) {
						this.HasBuffIds.Add( buff_id );
					}
				}
			}

			// Remove old buffs + fire hooks
			foreach( int buff_id in this.HasBuffIds.ToArray() ) {
				if( this.player.FindBuffIndex( buff_id ) == -1 ) {
					this.HasBuffIds.Remove( buff_id );
					TmlPlayerHelpers.OnBuffExpire( this.player, buff_id );
				}
			}
		}

		private void CheckArmorEquipHooks() {
			for( int i = 0; i < this.player.armor.Length; i++ ) {
				Item item = this.player.armor[i];

				if( item != null && !item.IsAir ) {
					bool found = this.EquipSlotsToItemTypes.ContainsKey( i );

					if( found && item.type != this.EquipSlotsToItemTypes[i] ) {
						TmlPlayerHelpers.OnArmorUnequip( this.player, i, this.EquipSlotsToItemTypes[i] );
					}

					if( !found || item.type != this.EquipSlotsToItemTypes[i] ) {
						this.EquipSlotsToItemTypes[i] = item.type;
						TmlPlayerHelpers.OnArmorEquip( this.player, i, item );
					}
				} else {
					if( this.EquipSlotsToItemTypes.ContainsKey(i) ) {
						TmlPlayerHelpers.OnArmorUnequip( this.player, i, this.EquipSlotsToItemTypes[i] );
						this.EquipSlotsToItemTypes.Remove( i );
					}
				}
			}
		}
	}
}
