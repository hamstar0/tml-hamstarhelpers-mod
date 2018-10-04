using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;


namespace HamstarHelpers.Services.Menus {
	public class MenuContextService {
		public static bool ContainsMenuLoader( string ui_class_name, string context ) {
			var mymod = ModHelpersMod.Instance;

			if( mymod.MenuContextMngr.Show.ContainsKey( ui_class_name ) ) {
				return mymod.MenuContextMngr.Show[ui_class_name].ContainsKey( context );
			}
			return false;
		}


		////////////////

		public static void AddMenuLoader( string ui_class_name, string context, Action<UIState> on_show, Action<UIState> on_hide ) {
			var mymod = ModHelpersMod.Instance;

			if( !mymod.MenuContextMngr.Show.ContainsKey( ui_class_name ) ) {
				mymod.MenuContextMngr.Show[ui_class_name] = new Dictionary<string, Action<UIState>>();
			}
			if( !mymod.MenuContextMngr.Hide.ContainsKey( ui_class_name ) ) {
				mymod.MenuContextMngr.Hide[ui_class_name] = new Dictionary<string, Action<UIState>>();
			}
			mymod.MenuContextMngr.Show[ui_class_name][context] = on_show;
			mymod.MenuContextMngr.Hide[ui_class_name][context] = on_hide;

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
					UIElement ui_outer_container = MenuContextService.GetMenuContainerOuter( ui );
					UIElement ui_inner_container = MenuContextService.GetMenuContainerInner( ui_outer_container );

					return MenuContextService.GetMenuContainerInsertPoint( ui_inner_container );
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

			MenuContextService.AddMenuLoader( ui_class_name, elem_name, on_show, on_hide );
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

			for( int i = 0; i < ui_container_elems.Count; i++ ) {
				if( ui_container_elems[i] is UIElement && !( ui_container_elems[i] is UIList ) && !( ui_container_elems[i] is UIScrollbar ) ) {
					return ui_container_elems[i];
				}
			}

			return null;
		}

		public static UIElement GetMenuContainerInsertPoint( UIState ui ) {
			var ui_outer_container = MenuContextService.GetMenuContainerOuter( ui );
			var ui_inner_container = MenuContextService.GetMenuContainerInner( ui_outer_container );
			return MenuContextService.GetMenuContainerInsertPoint( ui_inner_container );
		}


		////////////////

		public static UIState GetCurrentMenu() {
			return ModHelpersMod.Instance.MenuContextMngr.CurrentMenuUI?.Item2;
		}

		public static UIState GetPreviousMenu() {
			return ModHelpersMod.Instance.MenuContextMngr.PreviousMenuUI?.Item2;
		}
	}
}
