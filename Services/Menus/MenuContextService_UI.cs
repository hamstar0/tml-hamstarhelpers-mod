using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;


namespace HamstarHelpers.Services.Menus {
	public partial class MenuContextService {
		public static UIElement GetMenuContainerOuter( UIState ui ) {
			Type uiType = ui.GetType();
			FieldInfo uiOuterBoxField = uiType.GetField( "uIElement", BindingFlags.Instance | BindingFlags.NonPublic );
			UIElement uiOuterBox = (UIElement)uiOuterBoxField.GetValue( ui );

			return uiOuterBox;
		}

		public static UIElement GetMenuContainerInner( UIElement uiOuterBox ) {
			Type uiOuterBoxType = uiOuterBox.GetType();
			FieldInfo uiOuterBoxElemsField = uiOuterBoxType.GetField( "Elements", BindingFlags.Instance | BindingFlags.NonPublic );
			List<UIElement> uiOuterBoxElems = (List<UIElement>)uiOuterBoxElemsField.GetValue( uiOuterBox );

			return uiOuterBoxElems[0];
		}

		public static UIElement GetMenuContainerInsertPoint( UIElement uiInnerContainer ) {
			Type uiContainerType = uiInnerContainer.GetType();
			FieldInfo uiContainerElemsField = uiContainerType.GetField( "Elements", BindingFlags.Instance | BindingFlags.NonPublic );
			List<UIElement> uiContainerElems = (List<UIElement>)uiContainerElemsField.GetValue( uiInnerContainer );

			for( int i = 0; i < uiContainerElems.Count; i++ ) {
				if( uiContainerElems[i] is UIElement && !( uiContainerElems[i] is UIList ) && !( uiContainerElems[i] is UIScrollbar ) ) {
					return uiContainerElems[i];
				}
			}

			return null;
		}

		public static UIElement GetMenuContainerInsertPoint( UIState ui ) {
			var uiOuterContainer = MenuContextService.GetMenuContainerOuter( ui );
			var uiInnerContainer = MenuContextService.GetMenuContainerInner( uiOuterContainer );
			return MenuContextService.GetMenuContainerInsertPoint( uiInnerContainer );
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
