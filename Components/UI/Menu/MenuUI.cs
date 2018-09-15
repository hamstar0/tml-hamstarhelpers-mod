using HamstarHelpers.Helpers.DebugHelpers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;


namespace HamstarHelpers.Components.UI.Menu {
	public class MenuUI {
		public static void AddMenuLoader( string ui_class_name, string elem_name, Action<UIState> on_load, Action<UIState> on_unload ) {
			var mymod = ModHelpersMod.Instance;

			if( !mymod.MenuUIMngr.Loaders.ContainsKey( ui_class_name ) ) {
				mymod.MenuUIMngr.Loaders[ ui_class_name ] = new Dictionary<string, Action<UIState>>();
				mymod.MenuUIMngr.Unloaders[ ui_class_name ] = new Dictionary<string, Action<UIState>>();
			}
			mymod.MenuUIMngr.Loaders[ ui_class_name ][ elem_name ] = on_load;
			mymod.MenuUIMngr.Unloaders[ ui_class_name ][ elem_name ] = on_unload;
		}


		public static void AddMenuLoader( string ui_class_name, string elem_name, UIElement myelem, bool inner=true ) {
			bool myinner = inner;

			Func<UIState, UIElement> get_insert_point = ( UIState ui ) => {
				if( myinner ) {
					UIElement ui_outer_container = MenuUI.GetMenuContainerOuter( ui );
					UIElement ui_inner_container = MenuUI.GetMenuContainerInner( ui_outer_container );

					return MenuUI.GetMenuContainerInsertPoint( ui_inner_container );
				} else {
					return ui;//ui_outer_container;
				}
			};

			Action<UIState> on_load = ( UIState ui ) => {
				UIElement elem = get_insert_point( ui );
				elem.Append( myelem );
			};

			Action<UIState> on_unload = ( UIState ui ) => {
				UIElement elem = get_insert_point( ui );
				elem.RemoveChild( myelem );
			};

			MenuUI.AddMenuLoader( ui_class_name, elem_name, on_load, on_unload );
		}


		////////////////

		private static UIElement GetMenuContainerOuter( UIState ui ) {
			Type ui_type = ui.GetType();
			FieldInfo ui_outer_box_field = ui_type.GetField( "uIElement", BindingFlags.Instance | BindingFlags.NonPublic );
			UIElement ui_outer_box = (UIElement)ui_outer_box_field.GetValue( ui );

			return ui_outer_box;
		}

		private static UIElement GetMenuContainerInner( UIElement ui_outer_box ) {
			Type ui_outer_box_type = ui_outer_box.GetType();
			FieldInfo ui_outer_box_elems_field = ui_outer_box_type.GetField( "Elements", BindingFlags.Instance | BindingFlags.NonPublic );
			List<UIElement> ui_outer_box_elems = (List<UIElement>)ui_outer_box_elems_field.GetValue( ui_outer_box );

			return ui_outer_box_elems[0];
		}

		private static UIElement GetMenuContainerInsertPoint( UIElement ui_container ) {
			Type ui_container_type = ui_container.GetType();
			FieldInfo ui_container_elems_field = ui_container_type.GetField( "Elements", BindingFlags.Instance | BindingFlags.NonPublic );
			List<UIElement> ui_container_elems = (List<UIElement>)ui_container_elems_field.GetValue( ui_container );
			
			for( int i=0; i<ui_container_elems.Count; i++ ) {
				if( ui_container_elems[i] is UIElement && !(ui_container_elems[i] is UIList) && !(ui_container_elems[i] is UIScrollbar) ) {
					return ui_container_elems[i];
				}
			}

			return null;
		}
	}




	class MenuUIManager {
		internal IDictionary<string, IDictionary<string, Action<UIState>>> Loaders = new Dictionary<string, IDictionary<string, Action<UIState>>>();
		internal IDictionary<string, IDictionary<string, Action<UIState>>> Unloaders = new Dictionary<string, IDictionary<string, Action<UIState>>>();

		private readonly IDictionary<string, UIState> CachedMenus = new Dictionary<string, UIState>();


		////////////////

		public MenuUIManager() {
			if( Main.dedServ ) { return; }

			Main.OnPostDraw += MenuUIManager._Update;
		}

		~MenuUIManager() {
			if( Main.dedServ ) { return; }

			try {
				this.Unload();
				Main.OnPostDraw -= MenuUIManager._Update;
			} catch { }
		}


		private void Unload() {
			foreach( var kv in this.CachedMenus ) {
				string ui_name = kv.Key;
				UIState ui = kv.Value;

				foreach( Action<UIState> unloader in this.Unloaders[ ui_name ].Values ) {
					unloader( ui );
				}
			}
		}


		////////////////

		private static void _Update( GameTime gametime ) {   // <- Just in case references are doing something funky...
			ModHelpersMod mymod = ModHelpersMod.Instance;
			if( mymod == null ) { return; }

			if( mymod.MenuUIMngr == null ) { return; }
			mymod.MenuUIMngr.Update();
		}

		private void Update() {
			UIState ui = Main.MenuUI.CurrentState;
			if( ui == null ) { return; }

			string ui_name = ui.GetType().Name;
			if( !this.Loaders.ContainsKey( ui_name ) ) { return; }
			
			foreach( Action<UIState> loader in this.Loaders[ ui_name ].Values ) {
				loader( ui );
			}

			this.CachedMenus[ ui_name ] = ui;
			this.Loaders.Remove( ui_name );
		}
	}
}
