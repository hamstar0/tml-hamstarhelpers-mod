using HamstarHelpers.Helpers.TModLoader.Menus;
using Terraria.UI;


namespace HamstarHelpers.Classes.UI.Menus {
	/// <summary>
	/// Defines the interface of a class that associates with a given menu (via. MenuContextServices).
	/// </summary>
	abstract public class MenuContext {
		/// <summary></summary>
		public MenuUIDefinition MenuDefinitionOfContext { get; private set; }
		/// <summary>Name associated with this context (unique identifier).</summary>
		public string ContextName { get; private set; }



		////////////////

		/// <summary></summary>
		/// <param name="menuDefinitionOfContext"></param>
		/// <param name="contextName"></param>
		protected MenuContext( MenuUIDefinition menuDefinitionOfContext, string contextName ) {
			this.MenuDefinitionOfContext = menuDefinitionOfContext;
			this.ContextName = contextName;
		}


		/// <summary></summary>
		public abstract void OnModUnload();


		////

		/// <summary>
		/// When our menu context first becomes "contextualized" with a given menu.
		/// </summary>
		public abstract void OnContexualize();
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
