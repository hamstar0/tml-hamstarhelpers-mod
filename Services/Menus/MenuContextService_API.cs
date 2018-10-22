using HamstarHelpers.Components.UI.Menus;
using HamstarHelpers.Helpers.DebugHelpers;
using System.Collections.Generic;
using Terraria;
using Terraria.UI;


namespace HamstarHelpers.Services.Menus {
	public class MenuContextService {
		public static bool ContainsMenuContexts( string ui_class_name ) {
			var mymod = ModHelpersMod.Instance;
			if( mymod == null || mymod.MenuContextMngr == null ) { return false; }
			var loaders = mymod.MenuContextMngr.Contexts;

			return loaders.ContainsKey(ui_class_name) && loaders.Count > 0;
		}


		////////////////

		public static MenuContextBase GetMenuContext( string ui_class_name, string context_name ) {
			var mymod = ModHelpersMod.Instance;
			if( mymod == null || mymod.MenuContextMngr == null ) { return null; }
			var loaders = mymod.MenuContextMngr.Contexts;

			MenuContextBase ctx = null;

			if( loaders.ContainsKey( ui_class_name ) ) {
				loaders[ui_class_name].TryGetValue( context_name, out ctx );
			}
			return ctx;
		}


		////////////////

		public static void AddMenuContext( string ui_class_name, string context_name, MenuContextBase context ) {
			var mymod = ModHelpersMod.Instance;

			if( !mymod.MenuContextMngr.Contexts.ContainsKey( ui_class_name ) ) {
				mymod.MenuContextMngr.Contexts[ui_class_name] = new Dictionary<string, MenuContextBase>();
			}
			mymod.MenuContextMngr.Contexts[ui_class_name][context_name] = context;

			UIState ui = Main.MenuUI.CurrentState;
			string curr_ui_name = ui?.GetType().Name;

			if( ui_class_name == curr_ui_name ) {
				context.Show( ui );
			}
		}
	}
}
