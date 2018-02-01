using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria.ModLoader.IO;


namespace HamstarHelpers.Logic {
	partial class PlayerLogic {
		public PlayerLogic() {
			this.PrivateUID = Guid.NewGuid().ToString( "D" );
			this.HasUID = false;
			this.HasSyncedModSettings = false;
			this.HasSyncedModData = false;
			this.PermaBuffsById = new HashSet<int>();
		}


		////////////////
		
		public void NetSend( BinaryWriter writer, bool include_uid ) {
			if( include_uid ) {
				writer.Write( (bool)this.HasUID );
				writer.Write( (string)this.PrivateUID );
			}

			writer.Write( (int)this.PermaBuffsById.Count );

			foreach( int buff_id in this.PermaBuffsById ) {
				writer.Write( (int)buff_id );
			}
		}

		public void NetReceive( BinaryReader reader, bool include_uid ) {
			this.PermaBuffsById = new HashSet<int>();

			if( include_uid ) {
				bool has = reader.ReadBoolean();
				string uid = reader.ReadString();

				if( has ) {
					this.HasUID = has;
					this.PrivateUID = uid;
				}
			}

			int perma_buff_id_count = reader.ReadInt32();

			for( int i = 0; i < perma_buff_id_count; i++ ) {
				this.PermaBuffsById.Add( reader.ReadInt32() );
			}
		}


		////////////////

		public void Load( TagCompound tags ) {
			if( tags.ContainsKey( "uid" ) ) {
				this.PrivateUID = tags.GetString( "uid" );
			}
			if( tags.ContainsKey( "cp_new_since" ) ) {
				this.ControlPanelNewSince = tags.GetString( "cp_new_since" );
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
				{ "cp_new_since", this.ControlPanelNewSince },
				{ "perma_buffs", perma_buffs }
			};
			return tags;
		}
	}
}
