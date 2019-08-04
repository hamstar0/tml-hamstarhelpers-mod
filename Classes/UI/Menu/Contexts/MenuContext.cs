using Terraria.UI;


namespace HamstarHelpers.Components.UI.Menus {
	abstract public class MenuContext {
		public abstract void OnContexualize( string uiClassName, string contextName );
		public abstract void Show( UIState ui );
		public abstract void Hide( UIState ui );
	}
}
