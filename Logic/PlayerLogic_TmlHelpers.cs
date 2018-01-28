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
			// Add new buffs
			for( int i = 0; i < player.buffTime.Length; i++ ) {
				if( player.buffTime[i] > 0 ) {
					int buff_id = player.buffType[i];
					if( !this.HasBuffIds.Contains( buff_id ) ) {
						this.HasBuffIds.Add( buff_id );
					}
				}
			}

			// Remove old buffs + fire hooks
			foreach( int buff_id in this.HasBuffIds.ToArray() ) {
				if( player.FindBuffIndex( buff_id ) == -1 ) {
					this.HasBuffIds.Remove( buff_id );
					TmlPlayerHelpers.OnBuffExpire( player, buff_id );
				}
			}
		}

		private void CheckArmorEquipHooks( Player player ) {
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
					}
				} else {
					if( this.EquipSlotsToItemTypes.ContainsKey( i ) ) {
						TmlPlayerHelpers.OnArmorUnequip( player, i, this.EquipSlotsToItemTypes[i] );
						this.EquipSlotsToItemTypes.Remove( i );
					}
				}
			}
		}
	}
}
