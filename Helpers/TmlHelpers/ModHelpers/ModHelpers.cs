using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using HamstarHelpers.Helpers.TmlHelpers.Menus;
using HamstarHelpers.Services.Menus;
using HamstarHelpers.Services.Timers;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;


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
				if( Encoding.UTF8.GetString( h_reader.ReadBytes( 4 ) ) != "TMOD" ) {
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


		////////////////

		public static void PromptModDownloads( string pack_title, List<string> mod_names ) {
			Type interface_type = Assembly.GetAssembly( typeof(ModLoader) ).GetType( "Terraria.ModLoader.Interface" );

			int mod_browser_menu_mode;
			if( !ReflectionHelpers.GetField<int>( interface_type, null, "modBrowserID", BindingFlags.Static | BindingFlags.NonPublic, out mod_browser_menu_mode ) ) {
				LogHelpers.Log( "Could not switch to mod browser menu context." );
				return;
			}

			Main.PlaySound( SoundID.MenuTick );
			Main.menuMode = mod_browser_menu_mode;

			UIState mod_browser_ui;
			if( !ReflectionHelpers.GetField<UIState>( interface_type, null, "modBrowser", BindingFlags.Static | BindingFlags.NonPublic, out mod_browser_ui ) ) {
				LogHelpers.Log( "Could not acquire mod browser UI." );
				return;
			}

			Timers.SetTimer( "ModHelpersModDownloadPrompt", 5, () => {
				if( MenuContextService.GetCurrentMenuUI()?.GetType().Name != "UIModBrowser" ) {
					return false;
				}

				bool is_loading;
				if( !ReflectionHelpers.GetField<bool>( mod_browser_ui, "loading", out is_loading ) ) {
					return false;
				}

				if( is_loading ) {
					return true;
				}
				
				MenuModHelper.ApplyModBrowserFilter( pack_title, true, mod_names );
				return false;
			} );

			/*Assembly tml_asm = typeof( ModLoader ).Assembly;
			Type interface_type = tml_asm.GetType( "Interface" );

			Type ui_mod_dl_type = tml_asm.GetType( "UIModDownloadItem" );
			object ui_mod_dl = Activator.CreateInstance( ui_mod_dl_type, "ModName", "0.0.0", "hamstar", "", ModSide.Both, "", "http://javid.ddns.net/tModLoader/download.php?Down=mods/HamstarHelpers.tmod", 0, 0, "", false, false, null );
			//UIModDownloadItem modItem = new UIModDownloadItem( displayname, name, version, author, modreferences, modside, modIconURL, download, downloads, hot, timeStamp, update, updateIsDowngrade, installed );
			items.Add( modItem );
			
			Interface.downloadMods.SetDownloading( pack_title );
			Interface.downloadMods.SetModsToDownload( mod_filter, items );
			Interface.modBrowser.updateNeeded = true;

			int menu_mode;
			if( !ReflectionHelpers.GetField<int>( interface_type, null, "downloadModsID", out menu_mode ) ) {
				LogHelpers.Log( "Could not switch to downloads menu." );
				return;
			}
			Main.PlaySound( SoundID.MenuTick );
			Main.menuMode = menu_mode;*/
		}
	}
}
