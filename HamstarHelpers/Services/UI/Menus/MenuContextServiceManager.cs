using HamstarHelpers.Classes.UI.Menus;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET;
using HamstarHelpers.Helpers.TModLoader.Menus;
using HamstarHelpers.Services.Hooks.LoadHooks;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Services.UI.Menus {
	partial class MenuContextServiceManager {
		private IDictionary<MenuUIDefinition, IDictionary<string, MenuContext>> Contexts
				= new Dictionary<MenuUIDefinition, IDictionary<string, MenuContext>>();

		internal MenuUIDefinition CurrentMenuUI { get; private set; }
		internal MenuUIDefinition PreviousMenuUI { get; private set; }



		////////////////

		public MenuContextServiceManager() {
			if( Main.dedServ ) { return; }

			Main.OnPostDraw += MenuContextServiceManager._Update;

			LoadHooks.AddModUnloadHook( this.ModsUnloading );
		}


		private void ModsUnloading() {
			try {
				Main.OnPostDraw -= MenuContextServiceManager._Update;
				this.HideAllForCurrentMenuUI();

				foreach( MenuContext context in this.Contexts.Values.SafeSelectMany(kv=>kv.Values) ) {
					context.ModsUnloading();
				}

				this.Contexts.Clear();
			} catch( Exception e ) {
				LogHelpers.Warn( "Could not finish unloading menu contexts: "+e.ToString() );
			}
		}


		////////////////

		private void HideAllForCurrentMenuUI() {
			if( this.CurrentMenuUI == 0 ) {
				return;
			}

			MenuUIDefinition menuDef = this.CurrentMenuUI;
			if( !this.Contexts.ContainsKey( menuDef ) ) {
				LogHelpers.Warn( "Missing menu context " + menuDef );
				return;
			}

			IDictionary<string, MenuContext> contexts = this.Contexts[ menuDef ];

			foreach( MenuContext context in contexts.Values ) {
				context.Hide( MainMenuHelpers.GetMenuUI(this.CurrentMenuUI) );
			}
		}


		////////////////

		public IDictionary<string, MenuContext> GetContexts( MenuUIDefinition menuDef ) {
			if( !this.Contexts.ContainsKey( menuDef ) ) {
				this.Contexts[menuDef] = new Dictionary<string, MenuContext>();
			}
			return this.Contexts[menuDef];
		}
	}
}
