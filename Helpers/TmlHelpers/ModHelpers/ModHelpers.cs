﻿using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.Debug;
using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using Terraria.ModLoader.IO;


namespace HamstarHelpers.Helpers.TModLoader.Mods {
	public static partial class ModHelpers {
		public static byte[] UnsafeLoadFileFromMod( TmodFile tmod, string fileName ) {
			using( var fileStream = File.OpenRead( tmod.path ) )
			using( var hReader = new BinaryReader( fileStream ) ) {
				if( Encoding.UTF8.GetString( hReader.ReadBytes( 4 ) ) != "TMOD" ) {
					throw new HamstarException( "Magic Header != \"TMOD\"" );
				}

				var _tmlVers = new Version( hReader.ReadString() );
				byte[] _hash = hReader.ReadBytes( 20 );
				byte[] _signature = hReader.ReadBytes( 256 );
				int _datalen = hReader.ReadInt32();

				using( var deflateStream = new DeflateStream( fileStream, CompressionMode.Decompress ) )
				using( var reader = new BinaryReader( deflateStream ) ) {
					string name = reader.ReadString();
					var version = new Version( reader.ReadString() );

					int count = reader.ReadInt32();
					for( int i = 0; i < count; i++ ) {
						string innerFileName = reader.ReadString();
						int len = reader.ReadInt32();
						byte[] content = reader.ReadBytes( len );

						if( innerFileName == fileName ) {
							return content;
						}
					}
				}
			}

			return null;
		}
	}
}
