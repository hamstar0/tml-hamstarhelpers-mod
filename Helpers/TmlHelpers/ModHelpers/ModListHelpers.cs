using HamstarHelpers.Components.DataStructures;
using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers.Reflection;
using HamstarHelpers.Helpers.TmlHelpers.Menus;
using HamstarHelpers.Services.Menus;
using HamstarHelpers.Services.Timers;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;


namespace HamstarHelpers.Helpers.TmlHelpers.ModHelpers {
	public static class ModListHelpers {
		public static IEnumerable<Mod> GetAllLoadedModsPreferredOrder() {
			var mymod = ModHelpersMod.Instance;
			var self = mymod.ModMetaDataMngr;
			var mods = new LinkedList<Mod>();
			var modSet = new HashSet<string>();

			// Set Mod Helpers to front of list
			mods.AddLast( mymod );
			modSet.Add( mymod.Name );

			// Order mods with configs first
			foreach( var kv in self.ConfigMods ) {
				if( kv.Key == mymod.Name || kv.Value.File == null ) { continue; }
				mods.AddLast( kv.Value );
				modSet.Add( kv.Value.Name );
			}

			// Add remaining mods
			foreach( var mod in ModLoader.LoadedMods ) {
				if( modSet.Contains( mod.Name ) || mod.File == null ) { continue; }
				mods.AddLast( mod );
			}

			return mods;
		}


		public static IDictionary<string, ISet<Mod>> GetModsByAuthor() {
			var mods = new Dictionary<string, ISet<Mod>>();

			foreach( Mod mod in ModLoader.LoadedMods ) {
				if( mod.Name == "tModLoader" ) { continue; }
				if( mod.File == null ) {
					LogHelpers.Warn( "Mod " + mod.DisplayName + " has no file data." );
					continue;
				}
				var editor = Services.Tml.BuildPropertiesEditor.GetBuildPropertiesForModFile( mod.File );

				mods.Append2D( editor.Author, mod );
			}

			return mods;
		}


		public static IDictionary<Services.Tml.BuildPropertiesEditor, Mod> GetModsByBuildInfo() {
			var mods = new Dictionary<Services.Tml.BuildPropertiesEditor, Mod>();

			foreach( Mod mod in ModLoader.LoadedMods ) {
				if( mod.Name == "tModLoader" ) { continue; }
				if( mod.File == null ) {
					LogHelpers.Warn( "Mod " + mod.DisplayName + " has no file data." );
					continue;
				}
				var editor = Services.Tml.BuildPropertiesEditor.GetBuildPropertiesForModFile( mod.File );

				mods[editor] = mod;
			}

			return mods;
		}


		////////////////

		public static void PromptModDownloads( string packTitle, List<string> modNames ) {
			Type interfaceType = Assembly.GetAssembly( typeof(ModLoader) ).GetType( "Terraria.ModLoader.Interface" );

			int modBrowserMenuMode;
			if( !ReflectionHelpers.Get( interfaceType, null, "modBrowserID", out modBrowserMenuMode ) ) {
				LogHelpers.Warn( "Could not switch to mod browser menu context." );
				return;
			}

			Main.PlaySound( SoundID.MenuTick );
			Main.menuMode = modBrowserMenuMode;

			UIState modBrowserUi;
			if( !ReflectionHelpers.Get( interfaceType, null, "modBrowser", out modBrowserUi ) ) {
				LogHelpers.Warn( "Could not acquire mod browser UI." );
				return;
			}

			Timers.SetTimer( "ModHelpersModDownloadPrompt", 5, () => {
				if( MenuContextService.GetCurrentMenuUI()?.GetType().Name != "UIModBrowser" ) {
					return false;
				}

				bool isLoading;
				if( !ReflectionHelpers.Get( modBrowserUi, "loading", out isLoading ) ) {
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
