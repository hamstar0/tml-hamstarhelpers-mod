using HamstarHelpers.Services.Menus;
using Terraria.UI;


namespace HamstarHelpers.Components.UI.Menus {
	abstract public class MenuContextBase {
		public abstract void Show( UIState ui );
		public abstract void Hide( UIState ui );
	}
}
