using HamstarHelpers.Helpers.DebugHelpers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
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
			}
			if( !mymod.MenuUIMngr.Unloaders.ContainsKey( ui_class_name ) ) {
				mymod.MenuUIMngr.Unloaders[ui_class_name] = new Dictionary<string, Action<UIState>>();
			}
			mymod.MenuUIMngr.Loaders[ ui_class_name ][ elem_name ] = on_load;
			mymod.MenuUIMngr.Unloaders[ ui_class_name ][ elem_name ] = on_unload;
		}


		public static void AddMenuLoader( string ui_class_name, string elem_name, UIElement myelem, bool inner ) {
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
				myelem.Remove();

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

		private Tuple<string, UIState> CurrentMenu = null;


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
			if( this.CurrentMenu != null ) {
				foreach( Action<UIState> unloader in this.Unloaders[ this.CurrentMenu.Item1 ].Values ) {
					unloader( this.CurrentMenu.Item2 );
				}
			}
			
			this.Loaders.Clear();
			this.Unloaders.Clear();
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

			string prev_ui_name = this.CurrentMenu?.Item1;
			string curr_ui_name = ui?.GetType().Name;

			if( prev_ui_name == curr_ui_name ) {
				return;
			}

			this.LoadUI( ui );
		}


		private void LoadUI( UIState ui ) {
			string prev_ui_name = this.CurrentMenu?.Item1;
			string curr_ui_name = ui?.GetType().Name;

			if( prev_ui_name != null && this.Unloaders.ContainsKey(prev_ui_name) ) {
				var unloaders = this.Unloaders[ prev_ui_name ].Values;
				
				foreach( Action<UIState> unloader in unloaders ) {
					unloader( this.CurrentMenu.Item2 );
				}
				//this.Unloaders.Remove( prev_ui_name );
			}
			
			if( ui == null ) {
				this.CurrentMenu = null;
				return;
			}

			if( this.Loaders.ContainsKey( curr_ui_name ) ) {
				KeyValuePair<string, Action<UIState>>[] loaders = this.Loaders[curr_ui_name].ToArray();

				foreach( var kv in loaders ) {
					kv.Value( ui );
				}
			}

			this.CurrentMenu = Tuple.Create( curr_ui_name, ui );
		}
	}
}
