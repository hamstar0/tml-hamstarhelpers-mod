using HamstarHelpers.Components.UI.Menu;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using HamstarHelpers.Internals.WebRequests;
using HamstarHelpers.Services.Menus;
using HamstarHelpers.Services.Promises;
using HamstarHelpers.Services.Timers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
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
				Promises.AddValidatedPromise<ModVersionPromiseArguments>( GetModVersion.ModVersionPromiseValidator, ( args ) => {
					this.DisplayModListVersions( ui, args.Info );
					return false;
				} );
				return false;
			} );
		}


		////////////////

		private void DisplayModListVersions( UIState ui, IDictionary<string, Tuple<string, Version>> modVersionInfo ) {
			object items;
			if( !ReflectionHelpers.Get( ui, "items", BindingFlags.Instance | BindingFlags.NonPublic, out items ) ) {
				LogHelpers.Log( "!ModHelpers.ModUpdatesMenuContext._ctor - No 'items' field in ui " + ui );
				return;
			}

			UIList list;
			if( !ReflectionHelpers.Get<UIList>( ui, "modList", BindingFlags.Instance | BindingFlags.NonPublic, out list ) ) {
				LogHelpers.Log( "!ModHelpers.ModUpdatesMenuContext._ctor - No 'modList' field in ui " + ui );
				return;
			}

			var itemsArr = (Array)items.GetType()
				.GetMethod( "ToArray" )
				.Invoke( items, new object[] { } );

			for( int i = 0; i < itemsArr.Length; i++ ) {
				object item = itemsArr.GetValue( i );

				object mod;
				ReflectionHelpers.Get<object>( item, "mod", BindingFlags.Instance | BindingFlags.NonPublic, out mod );

				TmodFile tmod;
				ReflectionHelpers.Get<TmodFile>( mod, "modFile", out tmod );

				this.CheckVersion( modVersionInfo, list, tmod.name, tmod.version );
			}
		}


		////////////////

		public void CheckVersion( IDictionary<string, Tuple<string, Version>> modInfo, UIList modList, string modName, Version modVersion ) {
//LogHelpers.Log( "modInfo.Count:"+modInfo.Count+ ", name:"+name+", vers:"+vers);
			if( !modInfo.ContainsKey( modName ) ) { return; }
			if( modInfo[ modName ].Item2 == modVersion ) { return; }

			UIPanel uiModItem = null;

			foreach( UIElement modItem in modList._items ) {
				object mod;
				TmodFile tmod;

				ReflectionHelpers.Get<object>( modItem, "mod", BindingFlags.Instance | BindingFlags.NonPublic, out mod );
				ReflectionHelpers.Get<TmodFile>( mod, "modFile", out tmod );

				if( tmod.name == modName ) {
					uiModItem = (UIPanel)modItem;
					break;
				}
			}

			if( uiModItem != null ) {
				string newModVersion = modInfo[modName].Item2.ToString();

//LogHelpers.Log( " name: "+name+", uiModItem: " + uiModItem.GetOuterDimensions().ToRectangle() );
				var txt = new UIText( newModVersion + " On Mod Browser", 0.8f, true );
				txt.Top.Set( 24f, 0f );
				txt.Left.Set( -184f, 0.5f );
				txt.TextColor = Color.Gold;

				uiModItem.Append( txt );
				uiModItem.Recalculate();
				uiModItem.Parent?.Recalculate();
				uiModItem.Parent?.Parent?.Recalculate();
				uiModItem.Parent?.Parent?.Parent?.Recalculate();
			}
		}
	}
}
