using HamstarHelpers.Services.Menus;
using Terraria.UI;


namespace HamstarHelpers.Components.UI.Menus {
	public class WidgetMenuContext : MenuContext {
		public readonly bool IsInner;
		public readonly UIElement MyElement;



		////////////////

		public WidgetMenuContext( UIElement my_elem, bool is_inner ) {
			this.MyElement = my_elem;
			this.IsInner = is_inner;
		}

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
				UIElement ui_outer_container = MenuContextService.GetMenuContainerOuter( ui );
				UIElement ui_inner_container = MenuContextService.GetMenuContainerInner( ui_outer_container );

				return MenuContextService.GetMenuContainerInsertPoint( ui_inner_container );
			} else {
				return ui;//ui_outer_container;
			}
		}
	}
}
