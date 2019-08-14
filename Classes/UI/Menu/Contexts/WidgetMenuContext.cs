using HamstarHelpers.Helpers.TModLoader.Menus;
using HamstarHelpers.Services.UI.Menus;
using Terraria.UI;


namespace HamstarHelpers.Classes.UI.Menus {
	public class WidgetMenuContext : MenuContext {
		public readonly bool IsInner;
		public readonly UIElement MyElement;



		////////////////

		public WidgetMenuContext( MenuUIDefinition menuDef, string contextName, UIElement myElem, bool isInner )
				: base( menuDef, contextName ) {
			this.MyElement = myElem;
			this.IsInner = isInner;
		}

		public override void OnContexualize() { }

		public override void OnModUnload() { }


		////////////////

		public override void Show( UIState ui ) {
			UIElement elem = this.GetInsertElem( ui );
			elem.Append( this.MyElement );
		}
		
		public override void Hide( UIState ui ) {
			this.MyElement.Remove();

			UIElement elem = this.GetInsertElem( ui );
			elem.RemoveChild( this.MyElement );
		}


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
