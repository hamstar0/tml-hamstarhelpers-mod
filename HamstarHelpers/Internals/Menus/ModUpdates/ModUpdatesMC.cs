using HamstarHelpers.Classes.UI.Menu;
using HamstarHelpers.Libraries.Debug;
using HamstarHelpers.Libraries.DotNET.Reflection;
using HamstarHelpers.Libraries.TModLoader.Menus;
using HamstarHelpers.Libraries.TModLoader.Mods;
using HamstarHelpers.Internals.WebRequests;
using HamstarHelpers.Services.Hooks.LoadHooks;
using HamstarHelpers.Services.Timers;
using HamstarHelpers.Services.UI.Menus;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader.Core;
using Terraria.UI;


namespace HamstarHelpers.Internals.Menus.ModUpdates {
	partial class ModUpdatesMenuContext : SessionMenuContext {
		public static void Initialize() {
			if( ModHelpersConfig.Instance.DisableModMenuUpdates ) { return; }

			var ctx = new ModUpdatesMenuContext( MenuUIDefinition.UIMods, "ModHelpers: Mod Updates" );
			MenuContextService.AddMenuContext( ctx );
		}



		////////////////

		private ModUpdatesMenuContext( MenuUIDefinition menuDef, string contextName )
				: base( menuDef, contextName, false, false ) { }

		public override void OnModsUnloading() { }

		public override void OnActivationForSession( UIState ui ) { }

		public override void OnDeactivation() { }


		////////////////

		public override void Show( UIState ui ) {
			base.Show( ui );

			Timers.SetTimer( "ModHelpersUpdatesLoaderPause", 5, true, () => {
				CustomLoadHooks.AddHook( GetModInfo.ModInfoListLoadHookValidator, ( args ) => {
					BasicModInfoDatabase modDb = args.ModInfo;

					if( args != null ) {
						this.DisplayModListVersions( ui, modDb );
					}
					return false;
				} );
				return false;
			} );
		}


		////////////////

		private void DisplayModListVersions( UIState modsUi, IDictionary<string, BasicModInfo> modInfo ) {
			object items;
			if( !ReflectionLibraries.Get( modsUi, "items", out items ) ) {
				LogLibraries.Warn( "No 'items' field in ui " + modsUi );
				return;
			}

			UIList list;
			if( !ReflectionLibraries.Get( modsUi, "modList", out list ) ) {
				LogLibraries.Warn( "No 'modList' field in ui " + modsUi );
				return;
			}

			var itemsArr = (Array)items.GetType()
				.GetMethod( "ToArray" )
				.Invoke( items, new object[] { } );

			for( int i = 0; i < itemsArr.Length; i++ ) {
				object item = itemsArr.GetValue( i );

				object mod;
				if( !ReflectionLibraries.Get( item, "_mod", out mod ) || mod == null ) {
					LogLibraries.Warn( "Could not get Mod from list item " + item.ToString() );
					continue;
				}

				TmodFile modFile;
				if( !ReflectionLibraries.Get( mod, "modFile", out modFile ) || modFile == null ) {
					LogLibraries.Warn( "Could not get modFile from list item " + item.ToString() + "'s mod " + mod.ToString() );
					continue;
				}

				if( modInfo.ContainsKey( modFile.name ) ) {
					this.CheckVersion( modFile.name, modInfo[modFile.name], list, modFile.version );
				}
			}
		}


		////////////////

		public void CheckVersion( string modName, BasicModInfo modInfo, UIList modsUiModList, Version modVersion ) {
			//LogHelpers.Log( "modInfo.Count:"+modInfo.Count+ ", name:"+name+", vers:"+vers);
			if( modInfo.Version == modVersion ) { return; }

			UIPanel uiModItem = null;

			foreach( UIElement modItem in modsUiModList._items ) {
				object mod;
				TmodFile modFile;

				if( !ReflectionLibraries.Get( modItem, "_mod", out mod ) || mod == null ) {
					LogLibraries.Warn( "Could not get mod for version check from mod list item " + modItem.ToString() );
					continue;
				}
				if( !ReflectionLibraries.Get( mod, "modFile", out modFile ) || modFile == null ) {
					LogLibraries.Warn( "Could not get mod file for version check from mod " + mod.ToString() + "'s list item " + modItem.ToString() );
					continue;
				}

				if( modFile.name == modName ) {
					uiModItem = (UIPanel)modItem;
					break;
				}
			}

			if( uiModItem != null ) {
				Version newModVersion = modInfo.Version;
				string msg = newModVersion.ToString() + " On Mod Browser";

				//LogHelpers.Log( " name: "+name+", uiModItem: " + uiModItem.GetOuterDimensions().ToRectangle() );
				var txt = new UIText( msg, 0.8f, true );
				txt.Top.Set( 24f, 0f );
				txt.Left.Set( -184f, 0.5f );

				if( newModVersion > modVersion ) {
					txt.TextColor = Color.Gold;
				} else {
					txt.SetText( msg, 0.6f, true );
					txt.TextColor = Color.Gray;
				}

				uiModItem.Append( txt );
				uiModItem.Recalculate();
				uiModItem.Parent?.Recalculate();
				uiModItem.Parent?.Parent?.Recalculate();
				uiModItem.Parent?.Parent?.Parent?.Recalculate();
			}
		}
	}
}
