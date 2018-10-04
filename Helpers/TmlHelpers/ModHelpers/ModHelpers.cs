using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace HamstarHelpers.Helpers.TmlHelpers.ModHelpers {
	public static class ModHelpers {
		[Obsolete( "use GetAllPlayableModsPreferredOrder()", true )]
		public static IEnumerable<Mod> GetAllMods() {
			return ModHelpers.GetAllPlayableModsPreferredOrder();
		}

		public static IEnumerable<Mod> GetAllPlayableModsPreferredOrder() {
			var mymod = ModHelpersMod.Instance;
			var self = mymod.ModMetaDataManager;
			var mods = new LinkedList<Mod>();
			var mod_set = new HashSet<string>();

			mods.AddLast( mymod );
			mod_set.Add( mymod.Name );

			foreach( var kv in self.ConfigMods ) {
				if( kv.Key == mymod.Name || kv.Value.File == null ) { continue; }
				mods.AddLast( kv.Value );
				mod_set.Add( kv.Value.Name );
			}

			foreach( var mod in ModLoader.LoadedMods ) {
				if( mod_set.Contains( mod.Name ) || mod.File == null ) { continue; }
				mods.AddLast( mod );
			}
			return mods;
		}


		////////////////

		public static byte[] UnsafeLoadFileFromMod( TmodFile tmod, string file_name ) {
			using( var file_stream = File.OpenRead( tmod.path ) )
			using( var h_reader = new BinaryReader( file_stream ) ) {
				if( Encoding.ASCII.GetString( h_reader.ReadBytes( 4 ) ) != "TMOD" ) {
					throw new Exception( "Magic Header != \"TMOD\"" );
				}

				var _tml_vers = new Version( h_reader.ReadString() );
				byte[] _hash = h_reader.ReadBytes( 20 );
				byte[] _signature = h_reader.ReadBytes( 256 );
				int _datalen = h_reader.ReadInt32();

				using( var deflate_stream = new DeflateStream( file_stream, CompressionMode.Decompress ) )
				using( var reader = new BinaryReader( deflate_stream ) ) {
					string name = reader.ReadString();
					var version = new Version( reader.ReadString() );

					int count = reader.ReadInt32();
					for( int i = 0; i < count; i++ ) {
						string inner_file_name = reader.ReadString();
						int len = reader.ReadInt32();
						byte[] content = reader.ReadBytes( len );

						if( inner_file_name == file_name ) {
							return content;
						}
					}
				}
			}

			return null;
		}
	}
}
