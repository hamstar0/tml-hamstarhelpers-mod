using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Reflection;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;


namespace HamstarHelpers.Services.UI.Menus {
	/// <summary>
	/// Provides a way to interface with the main menu and its UI by way of "contexts".
	/// </summary>
	public partial class MenuContextService {
		/// <summary>
		/// Gets the "outer" container element (the element as positioned on the screen) of a menu's UI class.
		/// </summary>
		/// <param name="ui"></param>
		/// <returns></returns>
		public static UIElement GetMenuContainerOuter( UIState ui ) {
			UIElement elem;
			if( !ReflectionHelpers.Get( ui, "uIElement", out elem ) || elem == null ) {
				LogHelpers.AlertOnce();
				return null;
			}

			return elem;

			//Type uiType = ui.GetType();
			//FieldInfo uiOuterBoxField = uiType.GetField( "uIElement", BindingFlags.Instance | BindingFlags.NonPublic );
			//UIElement uiOuterBox = (UIElement)uiOuterBoxField.GetValue( ui );
			//
			//return uiOuterBox;
		}

		/// <summary>
		/// Gets the "inner" container element (contains the UI's components) of a menu's UI class's "outer" container
		/// element.
		/// </summary>
		/// <param name="uiOuterBox"></param>
		/// <returns></returns>
		public static UIElement GetMenuContainerInner( UIElement uiOuterBox ) {
			List<UIElement> uiOuterBoxElems;
			if( !ReflectionHelpers.Get( uiOuterBox, "Elements", out uiOuterBoxElems )
					|| uiOuterBoxElems == null
					|| uiOuterBoxElems.Count == 0 ) {
				LogHelpers.AlertOnce();
				return null;
			}

			return uiOuterBoxElems[0];

			//Type uiOuterBoxType = uiOuterBox.GetType();
			//FieldInfo uiOuterBoxElemsField = uiOuterBoxType.GetField( "Elements", BindingFlags.Instance | BindingFlags.NonPublic );
			//List<UIElement> uiOuterBoxElems = (List<UIElement>)uiOuterBoxElemsField.GetValue( uiOuterBox );
			//
			//return uiOuterBoxElems[0];
		}

		/// <summary>
		/// Gets the recommended element to add content to within an "inner" container element of a given menu UI.
		/// </summary>
		/// <param name="uiInnerContainer"></param>
		/// <returns></returns>
		public static UIElement GetMenuContainerInsertPoint( UIElement uiInnerContainer ) {
			List<UIElement> uiContainerElems;
			if( !ReflectionHelpers.Get( uiInnerContainer, "Elements", out uiContainerElems )
					|| uiContainerElems == null ) {
				LogHelpers.AlertOnce();
				return null;
			}

			//Type uiContainerType = uiInnerContainer.GetType();
			//FieldInfo uiContainerElemsField = uiContainerType.GetField( "Elements", BindingFlags.Instance | BindingFlags.NonPublic );
			//List<UIElement> uiContainerElems = (List<UIElement>)uiContainerElemsField.GetValue( uiInnerContainer );

			for( int i = 0; i < uiContainerElems.Count; i++ ) {
				if( uiContainerElems[i] is UIElement
						&& !( uiContainerElems[i] is UIList )
						&& !( uiContainerElems[i] is UIScrollbar ) ) {
					return uiContainerElems[i];
				}
			}

			LogHelpers.AlertOnce();
			return null;
		}

		/// <summary>
		/// Gets the recommended element to add content to (within an "inner" container element) of a given menu UI.
		/// </summary>
		/// <param name="ui"></param>
		/// <returns></returns>
		public static UIElement GetMenuContainerInsertPoint( UIState ui ) {
			var uiOuterContainer = MenuContextService.GetMenuContainerOuter( ui );
			var uiInnerContainer = MenuContextService.GetMenuContainerInner( uiOuterContainer );
			return MenuContextService.GetMenuContainerInsertPoint( uiInnerContainer );
		}


		////////////////

		/// <summary>
		/// Gets the current active main menu UI object (if any).
		/// </summary>
		/// <returns></returns>
		public static UIState GetCurrentMenuUI() {
			return ModHelpersMod.Instance.MenuContextMngr.CurrentMenuUI?.Item2;
		}

		/// <summary>
		/// Gets the previous active main menu UI object (if any).
		/// </summary>
		/// <returns></returns>
		public static UIState GetPreviousMenuUI() {
			return ModHelpersMod.Instance.MenuContextMngr.PreviousMenuUI?.Item2;
		}
	}
}
