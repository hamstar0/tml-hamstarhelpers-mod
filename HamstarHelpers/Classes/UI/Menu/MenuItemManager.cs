using HamstarHelpers.Libraries.Debug;
using HamstarHelpers.Services.Hooks.LoadHooks;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Classes.UI.Menu {
	class MenuItemManager {
		internal IDictionary<string, MenuItem> Items = new Dictionary<string, MenuItem>();

		

		////////////////

		public MenuItemManager() {
			if( !Main.dedServ ) {
				Main.OnPostDraw += MenuItemManager._Draw;
			}

			LoadHooks.AddModUnloadHook( () => {
				try {
					if( !Main.dedServ ) {
						Main.OnPostDraw -= MenuItemManager._Draw;
					}
				} catch { }
			} );
		}

		////////////////

		private static void _Draw( GameTime gameTime ) {	// <- Just in case references are doing something funky...
			ModHelpersMod mymod = ModHelpersMod.Instance;
			if( mymod == null || mymod.MenuItemMngr == null ) { return; }

			mymod.MenuItemMngr.Draw( gameTime );
		}

		private void Draw( GameTime gameTime ) {
			foreach( MenuItem item in this.Items.Values.ToArray() ) {
				if( item.MenuContext == Main.menuMode ) {
					item.Draw();
				}
			}
		}
	}
}
