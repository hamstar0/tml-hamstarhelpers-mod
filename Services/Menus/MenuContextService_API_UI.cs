using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;


namespace HamstarHelpers.Services.Menus {
	public class MenuContextService {
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

		public static UIState GetCurrentMenuUI() {
			return ModHelpersMod.Instance.MenuContextMngr.CurrentMenuUI?.Item2;
		}

		public static UIState GetPreviousMenuUI() {
			return ModHelpersMod.Instance.MenuContextMngr.PreviousMenuUI?.Item2;
		}
	}
}
