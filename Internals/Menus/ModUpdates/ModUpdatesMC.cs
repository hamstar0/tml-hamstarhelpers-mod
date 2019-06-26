using HamstarHelpers.Components.UI.Menu;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Reflection;
using HamstarHelpers.Internals.WebRequests;
using HamstarHelpers.Services.Menus;
using HamstarHelpers.Services.Promises;
using HamstarHelpers.Services.Timers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader.Core;
using Terraria.ModLoader.IO;
using Terraria.UI;


namespace HamstarHelpers.Internals.Menus.ModUpdates {
	/** @private */
	partial class ModUpdatesMenuContext : SessionMenuContext {
		public static void Initialize() {
			if( ModHelpersMod.Instance.Config.DisableModMenuUpdates ) { return; }
			
			var ctx = new ModUpdatesMenuContext();
			MenuContextService.AddMenuContext( "UIMods", "ModHelpers: Mod Updates", ctx );
		}



		////////////////

		private ModUpdatesMenuContext() : base( false, false ) {
		}
		
		public override void OnContexualize( string uiClassName, string contextName ) {
			base.OnContexualize( uiClassName, contextName );
		}


		public override void Show( UIState ui ) {
			base.Show( ui );
			
			Timers.SetTimer( "ModHelpersUpdatesLoaderPause", 5, () => {
				Promises.AddValidatedPromise<ModInfoListPromiseArguments>( GetModInfo.ModInfoListPromiseValidator, ( args ) => {
					if( args != null ) {
						this.DisplayModListVersions( ui, args.ModInfo );
					}
					return false;
				} );
				return false;
			} );
		}


		////////////////

		private void DisplayModListVersions( UIState ui, IDictionary<string, BasicModInfoEntry> modInfo ) {
			object items;
			if( !ReflectionHelpers.Get( ui, "items", out items ) ) {
				LogHelpers.Warn( "No 'items' field in ui " + ui );
				return;
			}

			UIList list;
			if( !ReflectionHelpers.Get( ui, "modList", out list ) ) {
				LogHelpers.Warn( "No 'modList' field in ui " + ui );
				return;
			}

			var itemsArr = (Array)items.GetType()
				.GetMethod( "ToArray" )
				.Invoke( items, new object[] { } );

			for( int i = 0; i < itemsArr.Length; i++ ) {
				object item = itemsArr.GetValue( i );

				object mod;
				if( !ReflectionHelpers.Get( item, "mod", out mod ) || mod == null ) {
					LogHelpers.Warn( "Could not get Mod from list item " + item.ToString() );
					continue;
				}

				TmodFile modFile;
				if( !ReflectionHelpers.Get(mod, "modFile", out modFile) || modFile == null ) {
					LogHelpers.Warn( "Could not get modFile from list item " + item.ToString()+"'s mod "+mod.ToString() );
					continue;
				}

				if( modInfo.ContainsKey(modFile.name) ) {
					this.CheckVersion( modFile.name, modInfo[modFile.name], list, modFile.version );
				}
			}
		}


		////////////////

		public void CheckVersion( string modName, BasicModInfoEntry modInfo, UIList modList, Version modVersion ) {
//LogHelpers.Log( "modInfo.Count:"+modInfo.Count+ ", name:"+name+", vers:"+vers);
			if( modInfo.Version == modVersion ) { return; }

			UIPanel uiModItem = null;

			foreach( UIElement modItem in modList._items ) {
				object mod;
				TmodFile modFile;

				if( !ReflectionHelpers.Get(modItem, "mod", out mod) || mod == null ) {
					LogHelpers.Warn( "Could not get mod for version check from mod list item "+modItem.ToString() );
					continue;
				}
				if( !ReflectionHelpers.Get(mod, "modFile", out modFile) || modFile == null ) {
					LogHelpers.Warn( "Could not get mod file for version check from mod "+mod.ToString()+"'s list item " + modItem.ToString() );
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
