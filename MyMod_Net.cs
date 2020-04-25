using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Classes.Protocols.Packet;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TModLoader;
using HamstarHelpers.Services.Network;


namespace HamstarHelpers {
	/// @private
	partial class ModHelpersMod : Mod {
		public override void HandlePacket( BinaryReader reader, int playerWho ) {
//Services.DataStore.DataStore.Add( DebugHelpers.GetCurrentContext()+"_A", 1 );
			try {
				int protocolCode = reader.ReadInt32();
				
				if( Main.netMode == 1 ) {
					PacketProtocol.HandlePacketOnClient( protocolCode, reader, playerWho );
				} else if( Main.netMode == 2 ) {
					PacketProtocol.HandlePacketOnServer( protocolCode, reader, playerWho );
				}
			} catch( Exception e ) {
				LogHelpers.Alert( e.ToString() );
			}
//Services.DataStore.DataStore.Add( DebugHelpers.GetCurrentContext()+"_B", 1 );
		}


		////////////////

		public override bool HijackGetData( ref byte messageType, ref BinaryReader reader, int playerNumber ) {
			if( messageType == MessageID.TileSection ) {
				var client = TmlHelpers.SafelyGetInstance<Client>();

				if( client.TileSectionPacketSubs.Count > 0 ) {
					this.HijackTileSectionData( reader, client.TileSectionPacketSubs );
				}
			}

			return base.HijackGetData( ref messageType, ref reader, playerNumber );
		}

		////

		private void HijackTileSectionData( BinaryReader reader, IList<Client.TileSectionPacketSubscriber> subs ) {
			int tileX, tileY;
			short width, height;

			reader.BaseStream.Position -= 3L;
			ushort len = reader.ReadUInt16();
			reader.BaseStream.Position += 1L;

			using( var ms = new MemoryStream() ) {
				reader.BaseStream.CopyTo( ms, len );
				ms.Position = 0L;

				var ms2 = new MemoryStream();

				if( ms.ReadByte() != 0 ) {
					using( var ds = new DeflateStream( ms, CompressionMode.Decompress, true ) ) {
						ds.CopyTo( ms2 );
						ds.Close();
					}

					ms2.Position = 0L;
				} else {
					ms2 = ms;
					ms2.Position = 1L;
				}

				using( var newReader = new BinaryReader(ms2) ) {
					tileX = newReader.ReadInt32();
					tileY = newReader.ReadInt32();
					width = newReader.ReadInt16();
					height = newReader.ReadInt16();

					foreach( Client.TileSectionPacketSubscriber sub in subs ) {
						sub.Invoke( tileX, tileY, width, height, newReader );
						newReader.BaseStream.Position = 11L;
					}
				}
			}
		}
	}
}
