using HamstarHelpers.Components.UI.Menus;
using HamstarHelpers.Helpers.Debug;
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
		/// <param name="uiClassName"></param>
		/// <returns></returns>
		public static bool ContainsMenuContexts( string uiClassName ) {
			var mymod = ModHelpersMod.Instance;
			if( mymod == null || mymod.MenuContextMngr == null ) { return false; }
			var loaders = mymod.MenuContextMngr.Contexts;

			return loaders.ContainsKey( uiClassName );
		}


		////////////////

		/// <summary>
		/// Gets the given menu "context" (container of added menu content) by name of a given UI by its class name
		/// (the UIState of a given main menu UI).
		/// </summary>
		/// <param name="uiClassName"></param>
		/// <param name="contextName"></param>
		/// <returns></returns>
		public static MenuContext GetMenuContext( string uiClassName, string contextName ) {
			var mymod = ModHelpersMod.Instance;
			if( mymod == null || mymod.MenuContextMngr == null ) { return null; }
			var loaders = mymod.MenuContextMngr.Contexts;

			MenuContext ctx = null;

			if( loaders.ContainsKey( uiClassName ) ) {
				loaders[uiClassName].TryGetValue( contextName, out ctx );
			}
			return ctx;
		}


		////////////////

		/// <summary>
		/// Adds a menu "context" (container of added menu content) for a given given UI by its class name
		/// (the UIState of a given main menu UI).
		/// </summary>
		/// <param name="uiClassName"></param>
		/// <param name="contextName"></param>
		/// <param name="context"></param>
		public static void AddMenuContext( string uiClassName, string contextName, MenuContext context ) {
			var mymod = ModHelpersMod.Instance;

			if( !mymod.MenuContextMngr.Contexts.ContainsKey( uiClassName ) ) {
				mymod.MenuContextMngr.Contexts[uiClassName] = new Dictionary<string, MenuContext>();
			}
			mymod.MenuContextMngr.Contexts[uiClassName][contextName] = context;

			context.OnContexualize( uiClassName, contextName );

			UIState ui = Main.MenuUI.CurrentState;
			string currUiName = ui?.GetType().Name;

			if( uiClassName == currUiName ) {
				context.Show( ui );
			}
		}
	}
}
