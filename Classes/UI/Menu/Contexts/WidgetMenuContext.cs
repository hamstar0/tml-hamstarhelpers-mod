using HamstarHelpers.Helpers.TModLoader.Menus;
using HamstarHelpers.Services.UI.Menus;
using Terraria.UI;


namespace HamstarHelpers.Classes.UI.Menus {
	/// <summary>
	/// A simplified "menu context" for binding an arbitrary UI element to a given menu UI.
	/// </summary>
	public class WidgetMenuContext : MenuContext {
		/// <summary>
		/// Indicates a dedicated "inner" UI element container is being used to hold our widget UI element, separate
		/// from the menu context's "outer" `UIState` UI element.
		/// </summary>
		public readonly bool IsInner;
		/// <summary>
		/// Widget's UI element.
		/// </summary>
		public readonly UIElement MyElement;



		////////////////

		/// <summary></summary>
		/// <param name="menuDefinition">Menu context to bind to.</param>
		/// <param name="contextName"></param>
		/// <param name="myElement">UI element of the widget.</param>
		/// <param name="isInner">See `IsInner` property.</param>
		public WidgetMenuContext(
					MenuUIDefinition menuDefinition,
					string contextName,
					UIElement myElement,
					bool isInner )
					: base( menuDefinition, contextName ) {
			this.MyElement = myElement;
			this.IsInner = isInner;
		}

		/// @private
		public override void OnModsUnloading() { }


		////////////////

		/// @private
		public override void OnActivation( UIState ui ) {
			UIElement elem = this.GetInsertElem( ui );
			elem.Append( this.MyElement );
		}

		/// @private
		public override void OnDeactivation() {
			UIState ui = MainMenuHelpers.GetMenuUI( this.MenuDefinitionOfContext );

			this.MyElement.Remove();

			UIElement elem = this.GetInsertElem( ui );
			elem.RemoveChild( this.MyElement );
		}


		////////////////

		/// @private
		public override void Show( UIState ui ) { }

		/// @private
		public override void Hide( UIState ui ) { }


		////////////////

		private UIElement GetInsertElem( UIState ui ) {
			if( this.IsInner ) {
				UIElement uiOuterContainer = MenuContextService.GetMenuContainerOuter( ui );
				UIElement uiInnerContainer = MenuContextService.GetMenuContainerInner( uiOuterContainer );

				return MenuContextService.GetMenuContainerInsertPoint( uiInnerContainer );
			} else {
				return ui;//uiOuterContainer;
			}
		}
	}
}
