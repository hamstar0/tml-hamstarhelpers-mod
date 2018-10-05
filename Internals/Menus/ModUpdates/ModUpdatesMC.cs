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
	partial class ModUpdatesMenuContext : MenuContextBase {
		public static void Initialize() {
			new ModUpdatesMenuContext();
		}



		////////////////

		public override string UIName => "UIMods";
		public override string ContextName => "Mod Updates";



		////////////////

		private ModUpdatesMenuContext() : base( false, false ) {
			Action<UIState, IDictionary<string, Tuple<string, Version>>> loader = ( ui, mod_info ) => {
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
					
					this.CheckVersion( mod_info, list, tmod.name, tmod.version );
				}
			};

			MenuContextService.AddMenuLoader( this.UIName, "ModHelpers: " + this.ContextName + " Load Mods",
				ui => {
					Timers.SetTimer( "ModHelpersUpdatesLoaderPause", 5, () => {
						Promises.AddValidatedPromise<ModVersionPromiseArguments>( GetModVersion.ModVersionPromiseValidator, ( args ) => {
							loader( ui, args.Info );
							return false;
						} );
						return false;
					} );
				},
				ui => { }
			);
		}


		////////////////

		public void CheckVersion( IDictionary<string, Tuple<string, Version>> mod_info, UIList mod_list, string name, Version vers ) {
//LogHelpers.Log( "mod_info.Count:"+mod_info.Count+ ", name:"+name+", vers:"+vers);
			if( !mod_info.ContainsKey( name ) ) { return; }
			if( mod_info[ name ].Item2 == vers ) { return; }

			UIPanel ui_mod_item = null;

			foreach( UIElement mod_item in mod_list._items ) {
				object mod;
				TmodFile tmod;

				ReflectionHelpers.Get<object>( mod_item, "mod", BindingFlags.Instance | BindingFlags.NonPublic, out mod );
				ReflectionHelpers.Get<TmodFile>( mod, "modFile", out tmod );

				if( tmod.name == name ) {
					ui_mod_item = (UIPanel)mod_item;
					break;
				}
			}

			if( ui_mod_item != null ) {
//LogHelpers.Log( " name: "+name+", ui_mod_item: " + ui_mod_item.GetOuterDimensions().ToRectangle() );
				var txt = new UIText( mod_info[ name ].Item2+" Available", 0.8f, true );
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
