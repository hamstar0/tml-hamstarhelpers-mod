using HamstarHelpers.Helpers.TModLoader.Menus;
using Terraria.UI;


namespace HamstarHelpers.Classes.UI.Menus {
	/// <summary>
	/// Defines the interface of a class that associates with a given menu (via. MenuContextServices).
	/// </summary>
	abstract public class MenuContext {
		/// <summary>
		/// When our menu context first becomes "contextualized" with a given menu.
		/// </summary>
		/// <param name="menuDef"></param>
		/// <param name="contextName"></param>
		public abstract void OnContexualize( TModLoaderMenuDefinition menuDef, string contextName );
		/// <summary>
		/// When a menu bound to the current context is shown.
		/// </summary>
		/// <param name="ui"></param>
		public abstract void Show( UIState ui );
		/// <summary>
		/// When a menu bound to the current context is hidden.
		/// </summary>
		/// <param name="ui"></param>
		public abstract void Hide( UIState ui );
	}
}
