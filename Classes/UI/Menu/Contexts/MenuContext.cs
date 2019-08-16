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
		/// <summary>Context has become active with its associated menu's UI having loaded.</summary>
		public bool IsActive { get; internal set; } = false;



		////////////////

		/// <summary></summary>
		/// <param name="menuDefinitionOfContext"></param>
		/// <param name="contextName"></param>
		protected MenuContext( MenuUIDefinition menuDefinitionOfContext, string contextName ) {
			this.MenuDefinitionOfContext = menuDefinitionOfContext;
			this.ContextName = contextName;
		}

		////////////////

		internal void ModsUnloading() {
			if( this.IsActive ) {
				this.IsActive = false;
				this.OnDeactivation();
			}
			this.OnModsUnloading();
		}

		/// <summary>
		/// Called when mods are unloading. Runs after `OnDeactivation()`.
		/// </summary>
		public abstract void OnModsUnloading();


		////////////////

		internal void ActivateIfInactive( UIState _ui ) {
			if( !this.IsActive ) {
				this.IsActive = true;
				this.OnActivation( _ui );
			}
		}

		/// <summary>
		/// When our menu context first becomes active with a given menu UI (occurs when that menu is opened).
		/// </summary>
		/// <param name="_ui"></param>
		public abstract void OnActivation( UIState _ui );
		
		/// <summary>
		/// When our menu context deactivates after activating.
		/// </summary>
		public abstract void OnDeactivation();


		////////////////

		/// <summary>
		/// When a menu bound to the current context is shown.
		/// </summary>
		/// <param name="_ui"></param>
		public abstract void Show( UIState _ui );
		/// <summary>
		/// When a menu bound to the current context is hidden.
		/// </summary>
		/// <param name="_ui"></param>
		public abstract void Hide( UIState _ui );
	}
}
