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
		/// Indicates if a "context" (added menu content) exists for the given main menu UI by itsclass name
		/// (the UIState of a given main menu UI).
		/// </summary>
		/// <param name="menuDefinition"></param>
		/// <returns></returns>
		public static bool ContainsMenuContexts( MenuUIDefinition menuDefinition ) {
			var mymod = ModHelpersMod.Instance;
			if( mymod == null || mymod.MenuContextMngr == null ) { return false; }
			var loaders = mymod.MenuContextMngr.Contexts;

			return loaders.ContainsKey( menuDefinition );
		}


		////////////////

		/// <summary>
		/// Gets the given menu "context" (container of added menu content) by name of a given UI by its class name
		/// (the UIState of a given main menu UI).
		/// </summary>
		/// <param name="menuDefinition"></param>
		/// <param name="contextName"></param>
		/// <returns></returns>
		public static MenuContext GetMenuContext( MenuUIDefinition menuDefinition, string contextName ) {
			var mymod = ModHelpersMod.Instance;
			if( mymod == null || mymod.MenuContextMngr == null ) { return null; }
			var loaders = mymod.MenuContextMngr.Contexts;

			MenuContext ctx = null;

			if( loaders.ContainsKey( menuDefinition ) ) {
				loaders[menuDefinition].TryGetValue( contextName, out ctx );
			}
			return ctx;
		}


		////////////////

		/// <summary>
		/// Adds a menu "context" (container of added menu content) for a given given UI by its class name
		/// (the UIState of a given main menu UI).
		/// </summary>
		/// <param name="menuDefinition"></param>
		/// <param name="contextName"></param>
		/// <param name="context"></param>
		public static void AddMenuContext( MenuUIDefinition menuDefinition, string contextName, MenuContext context ) {
			var mymod = ModHelpersMod.Instance;

			if( !mymod.MenuContextMngr.Contexts.ContainsKey( menuDefinition ) ) {
				mymod.MenuContextMngr.Contexts[menuDefinition] = new Dictionary<string, MenuContext>();
			}
			mymod.MenuContextMngr.Contexts[menuDefinition][contextName] = context;

			context.OnContexualize( menuDefinition, contextName );

			UIState ui = Main.MenuUI.CurrentState;
			string currUiName = ui?.GetType().Name;

			if( uiClassName == currUiName ) {
				context.Show( ui );
			}
		}
	}
}
