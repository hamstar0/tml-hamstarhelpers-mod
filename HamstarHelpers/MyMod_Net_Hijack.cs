using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using HamstarHelpers.Libraries.Debug;
using HamstarHelpers.Libraries.TModLoader;
using HamstarHelpers.Services.Network;
using HamstarHelpers.Services.Network.Scraper;


namespace HamstarHelpers {
	/// @private
	partial class ModHelpersMod : Mod {
		public override bool HijackSendData(
					int whoAmI,
					int msgType,
					int remoteClient,
					int ignoreClient,
					NetworkText text,
					int number,
					float number2,
					float number3,
					float number4,
					int number5,
					int number6,
					int number7 ) {
			if( Scraper.IsScrapingSentData ) {
				Scraper.AddSentData( new ScrapedSentData(
					whoAmI,
					msgType,
					remoteClient,
					ignoreClient,
					text,
					number,
					number2,
					number3,
					number4,
					number5,
					number6,
					number7 
				) );
			}

			return base.HijackSendData( whoAmI, msgType, remoteClient, ignoreClient, text, number, number2, number3, number4, number5, number6, number7 );
		}

		public override bool HijackGetData( ref byte messageType, ref BinaryReader reader, int playerNumber ) {
			if( Scraper.IsScrapingReceivedData ) {
				Scraper.AddReceiveData( new ScrapedReceivedData(messageType, reader, playerNumber) );
			}

			if( messageType == MessageID.TileSection ) {
				var client = TmlLibraries.SafelyGetInstance<Client>();

				if( client.TileSectionPacketSubs.Count > 0 ) {
					this.HijackTileSectionData( reader, client.TileSectionPacketSubs );
				}
			}

			return base.HijackGetData( ref messageType, ref reader, playerNumber );
		}


		////////////////

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
