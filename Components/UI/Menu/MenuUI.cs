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
		public static void AddMenuLoader( string ui_class_name, string elem_name, Action<UIState> on_show, Action<UIState> on_hide ) {
			var mymod = ModHelpersMod.Instance;

			if( !mymod.MenuUIMngr.Show.ContainsKey( ui_class_name ) ) {
				mymod.MenuUIMngr.Show[ ui_class_name ] = new Dictionary<string, Action<UIState>>();
			}
			if( !mymod.MenuUIMngr.Hide.ContainsKey( ui_class_name ) ) {
				mymod.MenuUIMngr.Hide[ ui_class_name ] = new Dictionary<string, Action<UIState>>();
			}
			mymod.MenuUIMngr.Show[ ui_class_name ][ elem_name ] = on_show;
			mymod.MenuUIMngr.Hide[ ui_class_name ][ elem_name ] = on_hide;

			UIState ui = Main.MenuUI.CurrentState;
			string curr_ui_name = ui?.GetType().Name;

			if( ui_class_name == curr_ui_name ) {
				on_show( ui );
			}
		}


		public static void AddMenuLoader( string ui_class_name, string elem_name, UIElement myelem, bool inner ) {
			bool myinner = inner;

			Func<UIState, UIElement> get_insert_elem = ( UIState ui ) => {
				if( myinner ) {
					UIElement ui_outer_container = MenuUI.GetMenuContainerOuter( ui );
					UIElement ui_inner_container = MenuUI.GetMenuContainerInner( ui_outer_container );

					return MenuUI.GetMenuContainerInsertPoint( ui_inner_container );
				} else {
					return ui;//ui_outer_container;
				}
			};
			
			Action<UIState> on_show = ( UIState ui ) => {
				UIElement elem = get_insert_elem( ui );
				elem.Append( myelem );
			};

			Action<UIState> on_hide = ( UIState ui ) => {
				myelem.Remove();

				UIElement elem = get_insert_elem( ui );
				elem.RemoveChild( myelem );
			};

			MenuUI.AddMenuLoader( ui_class_name, elem_name, on_show, on_hide );
		}


		////////////////

		public static UIElement GetMenuContainerOuter( UIState ui ) {
			Type ui_type = ui.GetType();
			FieldInfo ui_outer_box_field = ui_type.GetField( "uIElement", BindingFlags.Instance | BindingFlags.NonPublic );
			UIElement ui_outer_box = (UIElement)ui_outer_box_field.GetValue( ui );

			return ui_outer_box;
		}

		public static UIElement GetMenuContainerInner( UIElement ui_outer_box ) {
			Type ui_outer_box_type = ui_outer_box.GetType();
			FieldInfo ui_outer_box_elems_field = ui_outer_box_type.GetField( "Elements", BindingFlags.Instance | BindingFlags.NonPublic );
			List<UIElement> ui_outer_box_elems = (List<UIElement>)ui_outer_box_elems_field.GetValue( ui_outer_box );

			return ui_outer_box_elems[0];
		}

		public static UIElement GetMenuContainerInsertPoint( UIElement ui_inner_container ) {
			Type ui_container_type = ui_inner_container.GetType();
			FieldInfo ui_container_elems_field = ui_container_type.GetField( "Elements", BindingFlags.Instance | BindingFlags.NonPublic );
			List<UIElement> ui_container_elems = (List<UIElement>)ui_container_elems_field.GetValue( ui_inner_container );
			
			for( int i=0; i<ui_container_elems.Count; i++ ) {
				if( ui_container_elems[i] is UIElement && !(ui_container_elems[i] is UIList) && !(ui_container_elems[i] is UIScrollbar) ) {
					return ui_container_elems[i];
				}
			}

			return null;
		}

		public static UIElement GetMenuContainerInsertPoint( UIState ui ) {
			var ui_outer_container = MenuUI.GetMenuContainerOuter( ui );
			var ui_inner_container = MenuUI.GetMenuContainerInner( ui_outer_container );
			return MenuUI.GetMenuContainerInsertPoint( ui_inner_container );
		}


		////////////////

		public static UIState GetCurrentMenu() {
			return ModHelpersMod.Instance.MenuUIMngr.CurrentMenuUI?.Item2;
		}

		public static UIState GetPreviousMenu() {
			return ModHelpersMod.Instance.MenuUIMngr.PreviousMenuUI?.Item2;
		}



		////////////////

		private IDictionary<string, IDictionary<string, Action<UIState>>> Show = new Dictionary<string, IDictionary<string, Action<UIState>>>();
		private IDictionary<string, IDictionary<string, Action<UIState>>> Hide = new Dictionary<string, IDictionary<string, Action<UIState>>>();

		private Tuple<string, UIState> CurrentMenuUI = null;
		private Tuple<string, UIState> PreviousMenuUI = null;
		


		////////////////
		
		public MenuUI() {
			if( Main.dedServ ) { return; }

			Main.OnPostDraw += MenuUI._Update;
		}

		~MenuUI() {
			if( Main.dedServ ) { return; }

			try {
				Main.OnPostDraw -= MenuUI._Update;
				this.HideAll();

				this.Show.Clear();
				this.Hide.Clear();
			} catch { }
		}


		////////////////

		private void HideAll() {
			if( this.CurrentMenuUI != null ) {
				foreach( Action<UIState> hide in this.Hide[ this.CurrentMenuUI.Item1 ].Values ) {
					hide( this.CurrentMenuUI.Item2 );
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

			string prev_ui_name = this.CurrentMenuUI?.Item1;
			string curr_ui_name = ui?.GetType().Name;

			if( prev_ui_name == curr_ui_name ) {
				return;
			}

			this.LoadUI( ui );
		}


		private void LoadUI( UIState ui ) {
			string prev_ui_name = this.CurrentMenuUI?.Item1;
			string curr_ui_name = ui?.GetType().Name;

			this.PreviousMenuUI = this.CurrentMenuUI;

			if( prev_ui_name != null && this.Hide.ContainsKey(prev_ui_name) ) {
				var hiders = this.Hide[ prev_ui_name ].Values;
				
				foreach( Action<UIState> hide in hiders ) {
					hide( this.CurrentMenuUI.Item2 );
				}
				//this.Unloaders.Remove( prev_ui_name );
			}
			
			if( ui == null ) {
				this.CurrentMenuUI = null;
				return;
			}

			if( this.Show.ContainsKey( curr_ui_name ) ) {
				KeyValuePair<string, Action<UIState>>[] loaders = this.Show[curr_ui_name].ToArray();

				foreach( var kv in loaders ) {
					kv.Value( ui );
				}
			}

			this.CurrentMenuUI = Tuple.Create( curr_ui_name, ui );
		}
	}
}
