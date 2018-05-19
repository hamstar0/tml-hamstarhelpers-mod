using HamstarHelpers.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader.IO;


namespace HamstarHelpers.Logic {
	partial class PlayerLogic {
		public PlayerLogic() {
			this.PrivateUID = Guid.NewGuid().ToString( "D" );
			this.HasUID = false;
			this.HasSyncedModSettings = false;
			this.HasSyncedModData = false;
		}

		
		////////////////

		public void Load( TagCompound tags ) {
			if( tags.ContainsKey( "uid" ) ) {
				this.PrivateUID = tags.GetString( "uid" );
			}
			if( tags.ContainsKey( "perma_buffs" ) ) {
				var perma_buffs = tags.GetList<int>( "perma_buffs" );
				this.PermaBuffsById = new HashSet<int>( perma_buffs.ToArray() );
			}
			
			this.HasUID = true;
		}

		public TagCompound Save() {
			var perma_buffs = this.PermaBuffsById.ToArray();

			TagCompound tags = new TagCompound {
				{ "uid", this.PrivateUID },
				{ "perma_buffs", perma_buffs }
			};
			return tags;
		}


		////////////////
		
		public void NetReceiveClient( ISet<int> perma_buff_ids, ISet<int> has_buff_ids,
				IDictionary<int, int> equip_slots_to_item_types ) {
			this.PermaBuffsById = perma_buff_ids;
			this.HasBuffIds = has_buff_ids;
			this.EquipSlotsToItemTypes = equip_slots_to_item_types;
		}

		public void NetReceiveServer( bool has_uid, string uid, ISet<int> perma_buff_ids,
				ISet<int> has_buff_ids, IDictionary<int, int> equip_slots_to_item_types ) {
			this.HasUID = has_uid;
			this.PrivateUID = uid;
			this.PermaBuffsById = perma_buff_ids;
			this.HasBuffIds = has_buff_ids;
			this.EquipSlotsToItemTypes = equip_slots_to_item_types;
		}
	}
}
