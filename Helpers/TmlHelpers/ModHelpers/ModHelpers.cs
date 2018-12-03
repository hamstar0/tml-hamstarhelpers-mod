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
			var modSet = new HashSet<string>();

			mods.AddLast( mymod );
			modSet.Add( mymod.Name );

			foreach( var kv in self.ConfigMods ) {
				if( kv.Key == mymod.Name || kv.Value.File == null ) { continue; }
				mods.AddLast( kv.Value );
				modSet.Add( kv.Value.Name );
			}

			foreach( var mod in ModLoader.LoadedMods ) {
				if( modSet.Contains( mod.Name ) || mod.File == null ) { continue; }
				mods.AddLast( mod );
			}
			return mods;
		}


		////////////////

		public static byte[] UnsafeLoadFileFromMod( TmodFile tmod, string fileName ) {
			using( var fileStream = File.OpenRead( tmod.path ) )
			using( var hReader = new BinaryReader( fileStream ) ) {
				if( Encoding.UTF8.GetString( hReader.ReadBytes( 4 ) ) != "TMOD" ) {
					throw new Exception( "Magic Header != \"TMOD\"" );
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


		////////////////

		public static void PromptModDownloads( string packTitle, List<string> modNames ) {
			Type interfaceType = Assembly.GetAssembly( typeof(ModLoader) ).GetType( "Terraria.ModLoader.Interface" );

			int modBrowserMenuMode;
			if( !ReflectionHelpers.GetField<int>( interfaceType, null, "modBrowserID", BindingFlags.Static | BindingFlags.NonPublic, out modBrowserMenuMode ) ) {
				LogHelpers.Log( "Could not switch to mod browser menu context." );
				return;
			}

			Main.PlaySound( SoundID.MenuTick );
			Main.menuMode = modBrowserMenuMode;

			UIState modBrowserUi;
			if( !ReflectionHelpers.GetField<UIState>( interfaceType, null, "modBrowser", BindingFlags.Static | BindingFlags.NonPublic, out modBrowserUi ) ) {
				LogHelpers.Log( "Could not acquire mod browser UI." );
				return;
			}

			Timers.SetTimer( "ModHelpersModDownloadPrompt", 5, () => {
				if( MenuContextService.GetCurrentMenuUI()?.GetType().Name != "UIModBrowser" ) {
					return false;
				}

				bool isLoading;
				if( !ReflectionHelpers.GetField<bool>( modBrowserUi, "loading", out isLoading ) ) {
					return false;
				}

				if( isLoading ) {
					return true;
				}
				
				MenuModHelper.ApplyModBrowserFilter( packTitle, true, modNames );
				return false;
			} );

			/*Assembly tmlAsm = typeof( ModLoader ).Assembly;
			Type interfaceType = tmlAsm.GetType( "Interface" );

			Type uiModDlType = tmlAsm.GetType( "UIModDownloadItem" );
			object uiModDl = Activator.CreateInstance( uiModDlType, "ModName", "0.0.0", "hamstar", "", ModSide.Both, "", "http://javid.ddns.net/tModLoader/download.php?Down=mods/HamstarHelpers.tmod", 0, 0, "", false, false, null );
			//UIModDownloadItem modItem = new UIModDownloadItem( displayname, name, version, author, modreferences, modside, modIconURL, download, downloads, hot, timeStamp, update, updateIsDowngrade, installed );
			items.Add( modItem );
			
			Interface.downloadMods.SetDownloading( packTitle );
			Interface.downloadMods.SetModsToDownload( modFilter, items );
			Interface.modBrowser.updateNeeded = true;

			int menuMode;
			if( !ReflectionHelpers.GetField<int>( interfaceType, null, "downloadModsID", out menuMode ) ) {
				LogHelpers.Log( "Could not switch to downloads menu." );
				return;
			}
			Main.PlaySound( SoundID.MenuTick );
			Main.menuMode = menuMode;*/
		}
	}
}
