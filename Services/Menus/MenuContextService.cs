using HamstarHelpers.Components.UI.Menus;
using HamstarHelpers.Helpers.Debug;
using System.Collections.Generic;
using Terraria;
using Terraria.UI;


namespace HamstarHelpers.Services.Menus {
	public partial class MenuContextService {
		public static bool ContainsMenuContexts( string uiClassName ) {
			var mymod = ModHelpersMod.Instance;
			if( mymod == null || mymod.MenuContextMngr == null ) { return false; }
			var loaders = mymod.MenuContextMngr.Contexts;

			return loaders.ContainsKey(uiClassName) && loaders.Count > 0;
		}


		////////////////

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
