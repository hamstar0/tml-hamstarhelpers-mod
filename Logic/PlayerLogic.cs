using HamstarHelpers.NetProtocol;
using HamstarHelpers.TmlHelpers;
using HamstarHelpers.Utilities.Messages;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace HamstarHelpers.Logic {
	class PlayerLogic {
		public string UID { get; private set; }
		public bool HasUID { get; private set; }
		public ISet<int> PermaBuffsById { get; private set; }
		internal string ControlPanelNewSince = "1.0.0";

		private ISet<int> HasBuffIds = new HashSet<int>();
		private IDictionary<int, int> EquipSlotsToItemTypes = new Dictionary<int, int>();

		public bool HasSyncedModSettings { get; private set; }
		public bool HasSyncedModData { get; private set; }



		////////////////

		public PlayerLogic() {
			this.UID = Guid.NewGuid().ToString( "D" );
			this.HasUID = false;
			this.HasSyncedModSettings = false;
			this.HasSyncedModData = false;
			this.PermaBuffsById = new HashSet<int>();
		}

		////////////////
		
		public void SendClientChanges( HamstarHelpersMod mymod, ModPlayer client_player ) {
			var myclient = (HamstarHelpersPlayer)client_player;
			var logic = myclient.Logic;
			bool uid_mismatch = !string.IsNullOrEmpty( logic.UID ) && ( Main.netMode == 2 && !logic.UID.Equals( this.UID ) );
			
			if( uid_mismatch || !logic.PermaBuffsById.SetEquals( this.PermaBuffsById ) ) {
//LogHelpers.Log( "SendClientChanges to: " + ( Main.netMode == 2 && !myclient.UID.Equals( this.UID ) ) + "|"+ myclient.PermaBuffsById.SetEquals( this.PermaBuffsById ) + ", client: "+ client_player.player.whoAmI+ ", whoAmI: "+this.player.whoAmI );
				ClientPacketHandlers.SendPlayerData( mymod, -1 );
			}
		}

		public void OnEnterWorld( HamstarHelpersMod mymod, Player player ) {
//LogHelpers.Log( "OnEnterWorld player: " + player.whoAmI+ ", me: "+this.player.whoAmI );
			if( Main.netMode == 0 ) {   // Single player only
				if( !mymod.JsonConfig.LoadFile() ) {
					mymod.JsonConfig.SaveFile();
				}
			}

			// Sync mod (world) data; must be called after world is loaded
			if( Main.netMode == 1 ) {
				ClientPacketHandlers.SendPlayerData( mymod, -1 );
				ClientPacketHandlers.SendRequestPlayerData( mymod, -1 );
				ClientPacketHandlers.SendRequestModSettings( mymod );
				ClientPacketHandlers.SendRequestModData( mymod );
			}

			if( Main.netMode != 1 ) {   // NOT client; clients won't receive their own data back from server
				this.FinishModSettingsSync();
				this.FinishModDataSync();
			}
		}

		////////////////

		public bool IsSynced() {
			return this.HasSyncedModSettings && this.HasSyncedModData;
		}

		public void FinishModSettingsSync() {
			this.HasSyncedModSettings = true;
		}
		public void FinishModDataSync() {
			this.HasSyncedModData = true;
		}


		////////////////

		public void NetSend( BinaryWriter writer, bool include_uid = true ) {
			if( include_uid ) {
				writer.Write( (string)this.UID );
			}

			writer.Write( (int)this.PermaBuffsById.Count );

			foreach( int buff_id in this.PermaBuffsById ) {
				writer.Write( (int)buff_id );
			}
		}

		public void NetReceive( BinaryReader reader, bool include_uid = true ) {
			this.PermaBuffsById = new HashSet<int>();

			if( include_uid ) {
				this.UID = reader.ReadString();
			}

			int perma_buff_id_count = reader.ReadInt32();

			for( int i = 0; i < perma_buff_id_count; i++ ) {
				this.PermaBuffsById.Add( reader.ReadInt32() );
			}
		}


		////////////////

		public void Load( TagCompound tags ) {
			if( tags.ContainsKey( "uid" ) ) {
				this.UID = tags.GetString( "uid" );
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
				{ "uid", this.UID },
				{ "cp_new_since", this.ControlPanelNewSince },
				{ "perma_buffs", perma_buffs }
			};
			return tags;
		}


		////////////////

		public void Update( HamstarHelpersMod mymod, Player player ) {
			if( player.whoAmI == Main.myPlayer ) { // Current player
				mymod.PlayerMessages.UpdatePlayerLabels();
				SimpleMessage.UpdateMessage();
			}
			
			var modworld = mymod.GetModWorld<HamstarHelpersWorld>();

			if( Main.netMode == 1 ) {   // Client only
				if( player.whoAmI == Main.myPlayer ) { // Current player only
					modworld.WorldLogic.Update( mymod );
				}
			} else if( Main.netMode == 2 ) {    // Server
				modworld.WorldLogic.IsServerPlaying = true; // Needed?
			}

			foreach( int buff_id in this.PermaBuffsById ) {
				player.AddBuff( buff_id, 3 );
			}

			this.CheckBuffHooks( player );
			this.CheckArmorEquipHooks( player );
		}


		////////////////

		public void ProcessTriggers( HamstarHelpersMod mymod, TriggersSet triggers_set ) {
			if( mymod.ControlPanelHotkey.JustPressed ) {
				if( mymod.Config.DisableControlPanel ) {
					Main.NewText( "Control panel disabled.", Color.Red );
				} else {
					mymod.ControlPanel.Open();
				}
			}
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
