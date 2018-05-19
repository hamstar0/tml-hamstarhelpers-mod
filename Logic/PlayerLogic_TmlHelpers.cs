using HamstarHelpers.NetProtocols;
using HamstarHelpers.TmlHelpers;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Logic {
	partial class PlayerLogic {
		public void UpdateTml( HamstarHelpersMod mymod, Player player ) {
			this.CheckBuffHooks( player );
			this.CheckArmorEquipHooks( player );
		}


		////////////////

		private void CheckBuffHooks( Player player ) {
			bool buff_change = false;

			// Add new buffs
			for( int i = 0; i < player.buffTime.Length; i++ ) {
				if( player.buffTime[i] > 0 ) {
					int buff_id = player.buffType[i];
					if( !this.HasBuffIds.Contains( buff_id ) ) {
						this.HasBuffIds.Add( buff_id );
						buff_change = true;
					}
				}
			}

			// Remove old buffs + fire hooks
			foreach( int buff_id in this.HasBuffIds.ToArray() ) {
				if( player.FindBuffIndex( buff_id ) == -1 ) {
					this.HasBuffIds.Remove( buff_id );
					TmlPlayerHelpers.OnBuffExpire( player, buff_id );
					buff_change = true;
				}
			}

			if( buff_change ) {
				if( Main.netMode == 1 ) {
					HHPlayerDataProtocol.SendStateToServer( this.PermaBuffsById, this.HasBuffIds, this.EquipSlotsToItemTypes );
				}
			}
		}

		private void CheckArmorEquipHooks( Player player ) {
			bool equip_change = false;

			for( int i = 0; i < player.armor.Length; i++ ) {
				Item item = player.armor[i];

				if( item != null && !item.IsAir ) {
					bool found = this.EquipSlotsToItemTypes.ContainsKey( i );

					if( found && item.type != this.EquipSlotsToItemTypes[i] ) {
						TmlPlayerHelpers.OnArmorUnequip( player, i, this.EquipSlotsToItemTypes[i] );
					}

					if( !found || item.type != this.EquipSlotsToItemTypes[i] ) {
						this.EquipSlotsToItemTypes[i] = item.type;
						TmlPlayerHelpers.OnArmorEquip( player, i, item );
						equip_change = true;
					}
				} else {
					if( this.EquipSlotsToItemTypes.ContainsKey( i ) ) {
						TmlPlayerHelpers.OnArmorUnequip( player, i, this.EquipSlotsToItemTypes[i] );
						this.EquipSlotsToItemTypes.Remove( i );
						equip_change = true;
					}
				}
			}

			if( equip_change ) {
				if( Main.netMode == 1 ) {
					HHPlayerDataProtocol.SendStateToServer( this.PermaBuffsById, this.HasBuffIds, this.EquipSlotsToItemTypes );
				}
			}
		}


		////////////////

		public void AddPermaBuff( int buff_id ) {
			this.PermaBuffsById.Add( buff_id );

			HHPlayerDataProtocol.SendStateToServer( this.PermaBuffsById, this.HasBuffIds, this.EquipSlotsToItemTypes );
		}

		public void RemovePermaBuff( int buff_id ) {
			if( !this.PermaBuffsById.Contains( buff_id ) ) { return; }

			this.PermaBuffsById.Remove( buff_id );

			HHPlayerDataProtocol.SendStateToServer( this.PermaBuffsById, this.HasBuffIds, this.EquipSlotsToItemTypes );
		}
	}
}
