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
		
		public override void OnContexualize( string ui_class_name, string context_name ) {
			base.OnContexualize( ui_class_name, context_name );
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

		private void DisplayModListVersions( UIState ui, IDictionary<string, Tuple<string, Version>> mod_version_info ) {
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

			var items_arr = (Array)items.GetType()
				.GetMethod( "ToArray" )
				.Invoke( items, new object[] { } );

			for( int i = 0; i < items_arr.Length; i++ ) {
				object item = items_arr.GetValue( i );

				object mod;
				ReflectionHelpers.Get<object>( item, "mod", BindingFlags.Instance | BindingFlags.NonPublic, out mod );

				TmodFile tmod;
				ReflectionHelpers.Get<TmodFile>( mod, "modFile", out tmod );

				this.CheckVersion( mod_version_info, list, tmod.name, tmod.version );
			}
		}


		////////////////

		public void CheckVersion( IDictionary<string, Tuple<string, Version>> mod_info, UIList mod_list, string mod_name, Version mod_version ) {
//LogHelpers.Log( "mod_info.Count:"+mod_info.Count+ ", name:"+name+", vers:"+vers);
			if( !mod_info.ContainsKey( mod_name ) ) { return; }
			if( mod_info[ mod_name ].Item2 == mod_version ) { return; }

			UIPanel ui_mod_item = null;

			foreach( UIElement mod_item in mod_list._items ) {
				object mod;
				TmodFile tmod;

				ReflectionHelpers.Get<object>( mod_item, "mod", BindingFlags.Instance | BindingFlags.NonPublic, out mod );
				ReflectionHelpers.Get<TmodFile>( mod, "modFile", out tmod );

				if( tmod.name == mod_name ) {
					ui_mod_item = (UIPanel)mod_item;
					break;
				}
			}

			if( ui_mod_item != null ) {
				string new_mod_version = mod_info[mod_name].Item2.ToString();

//LogHelpers.Log( " name: "+name+", ui_mod_item: " + ui_mod_item.GetOuterDimensions().ToRectangle() );
				var txt = new UIText( new_mod_version + " On Mod Browser", 0.8f, true );
				txt.Top.Set( 24f, 0f );
				txt.Left.Set( -184f, 0.5f );
				txt.TextColor = Color.Gold;

				ui_mod_item.Append( txt );
				ui_mod_item.Recalculate();
				ui_mod_item.Parent?.Recalculate();
				ui_mod_item.Parent?.Parent?.Recalculate();
				ui_mod_item.Parent?.Parent?.Parent?.Recalculate();
			}
		}
	}
}
