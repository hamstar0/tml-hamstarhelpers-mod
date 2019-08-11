using HamstarHelpers.Classes.UI.Menus;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TModLoader.Menus;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.UI;


namespace HamstarHelpers.Services.UI.Menus {
	/// <summary>
	/// Provides a way to interface with the main menu and its UI by way of "contexts".
	/// </summary>
	public partial class MenuContextService {
		/// <summary>
		/// Indicates if a menu "context" (menu page) has custom content.
		/// </summary>
		/// <param name="menuDefinition"></param>
		/// <returns></returns>
		public static bool ContainsMenuContexts( MenuUIDefinition menuDefinition ) {
			var mymod = ModHelpersMod.Instance;
			if( mymod == null || mymod.MenuContextMngr == null ) { return false; }

			IDictionary<string, MenuContext> contexts = mymod.MenuContextMngr.GetContexts( menuDefinition );

			return contexts != null && contexts.Count > 0;
		}


		////////////////

		/// <summary>
		/// Gets a specific piece of added content for a menu "context" (menu page) by name.
		/// </summary>
		/// <param name="menuDefinition"></param>
		/// <param name="contextName"></param>
		/// <returns></returns>
		public static MenuContext GetMenuContext( MenuUIDefinition menuDefinition, string contextName ) {
			var mymod = ModHelpersMod.Instance;
			if( mymod == null || mymod.MenuContextMngr == null ) { return null; }

			IDictionary<string, MenuContext> contexts = mymod.MenuContextMngr.GetContexts( menuDefinition );

			if( contexts.ContainsKey(contextName) ) {
				return contexts[ contextName ];
			}
			return null;
		}


		////////////////

		/// <summary>
		/// Adds a piece of menu content to a menu "context" (menu page) by name.
		/// </summary>
		/// <param name="context"></param>
		public static void AddMenuContext( MenuContext context ) {
			var mymod = ModHelpersMod.Instance;
			MenuUIDefinition menuDef = context.MenuDefinitionOfContext;

			IDictionary<string, MenuContext> contexts = mymod.MenuContextMngr.GetContexts( menuDef );
			contexts[ context.ContextName ] = context;

			context.OnContexualize();

			UIState ui = Main.MenuUI.CurrentState;
			string currUiName = ui?.GetType().Name;

			if( Enum.GetName(typeof(MenuUIDefinition), menuDef ) == currUiName ) {
				context.Show( ui );
			}
		}
	}
}
