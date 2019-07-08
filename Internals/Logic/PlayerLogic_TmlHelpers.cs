using HamstarHelpers.Internals.NetProtocols;
using HamstarHelpers.Services.Hooks.ExtendedHooks;
using HamstarHelpers.Services.Timers;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Internals.Logic {
	/// @private
	partial class PlayerLogic {
		private bool CanUpdateData = true;


		////////////////

		public void UpdateTml( Player player ) {
			this.CheckBuffHooks( player );
			this.CheckArmorEquipHooks( player );
		}


		////////////////

		private void CheckBuffHooks( Player player ) {
			bool buffChange = false;

			// Add new buffs
			for( int i = 0; i < player.buffTime.Length; i++ ) {
				if( player.buffTime[i] > 0 ) {
					int buffId = player.buffType[i];

					if( !this.HasBuffIds.Contains( buffId ) ) {
						this.HasBuffIds.Add( buffId );
						buffChange = true;
					}
				}
			}

			// Remove old buffs + fire hooks
			foreach( int buffId in this.HasBuffIds.ToArray() ) {
				if( player.FindBuffIndex( buffId ) == -1 ) {
					this.HasBuffIds.Remove( buffId );
					buffChange = true;

					ExtendedPlayerHooks.OnBuffExpire( player, buffId );
				}
			}

			if( buffChange ) {
				if( Main.netMode == 1 ) {
					if( this.CanUpdateData ) {
						this.CanUpdateData = false;

						Timers.SetTimer( "ModHelpersPlayerDataAntiHammer", 60 * 3, () => {
							this.CanUpdateData = true;
							return false;
						} );

						PlayerDataProtocol.SyncToEveryone( this.PermaBuffsById, this.HasBuffIds, this.EquipSlotsToItemTypes );
					}
				}
			}
		}

		private void CheckArmorEquipHooks( Player player ) {
			bool equipChange = false;

			for( int i = 0; i < player.armor.Length; i++ ) {
				Item item = player.armor[i];

				if( item != null && !item.IsAir ) {
					bool hadAnEquip = this.EquipSlotsToItemTypes.ContainsKey( i );

					if( hadAnEquip ) {
						if( item.type != this.EquipSlotsToItemTypes[i] ) {
							ExtendedPlayerHooks.OnArmorUnequip( player, i, this.EquipSlotsToItemTypes[i] );
							ExtendedPlayerHooks.OnArmorEquip( player, i, item );
							this.EquipSlotsToItemTypes[i] = item.type;
							equipChange = true;
						}
					} else {
						this.EquipSlotsToItemTypes[i] = item.type;
						ExtendedPlayerHooks.OnArmorEquip( player, i, item );
						equipChange = true;
					}
				} else {
					if( this.EquipSlotsToItemTypes.ContainsKey( i ) ) {
						ExtendedPlayerHooks.OnArmorUnequip( player, i, this.EquipSlotsToItemTypes[i] );
						this.EquipSlotsToItemTypes.Remove( i );
						equipChange = true;
					}
				}
			}

			if( equipChange ) {
				if( Main.netMode == 1 ) {
					if( this.CanUpdateData ) {
						this.CanUpdateData = false;

						Timers.SetTimer( "ModHelpersPlayerDataAntiHammer", 60 * 3, () => {
							this.CanUpdateData = true;
							return false;
						} );

						PlayerDataProtocol.SyncToEveryone( this.PermaBuffsById, this.HasBuffIds, this.EquipSlotsToItemTypes );
					}
				}
			}
		}


		////////////////

		public void AddPermaBuff( int buffId ) {
			if( this.PermaBuffsById.Add( buffId ) ) {
				PlayerDataProtocol.SyncToEveryone( this.PermaBuffsById, this.HasBuffIds, this.EquipSlotsToItemTypes );
			}
		}

		public void RemovePermaBuff( int buffId ) {
			if( !this.PermaBuffsById.Contains( buffId ) ) { return; }

			if( this.PermaBuffsById.Remove( buffId ) ) {
				PlayerDataProtocol.SyncToEveryone( this.PermaBuffsById, this.HasBuffIds, this.EquipSlotsToItemTypes );
			}
		}
	}
}
