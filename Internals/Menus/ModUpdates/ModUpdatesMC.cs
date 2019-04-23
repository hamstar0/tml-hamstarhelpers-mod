using HamstarHelpers.Components.UI.Menu;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers.Reflection;
using HamstarHelpers.Internals.WebRequests;
using HamstarHelpers.Services.Menus;
using HamstarHelpers.Services.Promises;
using HamstarHelpers.Services.Timers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader.IO;
using Terraria.UI;


namespace HamstarHelpers.Internals.Menus.ModUpdates {
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
				Promises.AddValidatedPromise<ModInfoPromiseArguments>( GetModInfo.ModVersionPromiseValidator, ( args ) => {
					this.DisplayModListVersions( ui, args.Info );
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
				ReflectionHelpers.Get( item, "mod", out mod );

				TmodFile tmod;
				ReflectionHelpers.Get<TmodFile>( mod, "modFile", out tmod );

				if( modInfo.ContainsKey(tmod.name) ) {
					this.CheckVersion( tmod.name, modInfo[tmod.name], list, tmod.version );
				}
			}
		}


		////////////////

		public void CheckVersion( string modName, BasicModInfoEntry modInfo, UIList modList, Version modVersion ) {
//LogHelpers.Log( "modInfo.Count:"+modInfo.Count+ ", name:"+name+", vers:"+vers);
			if( modInfo.Version == modVersion ) { return; }

			UIPanel uiModItem = null;
			TmodFile tmod = null;

			foreach( UIElement modItem in modList._items ) {
				object mod;

				ReflectionHelpers.Get( modItem, "mod", out mod );
				ReflectionHelpers.Get( mod, "modFile", out tmod );

				if( tmod.name == modName ) {
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
