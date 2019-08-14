using HamstarHelpers.Classes.UI.Menus;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET;
using HamstarHelpers.Helpers.TModLoader.Menus;
using HamstarHelpers.Services.Hooks.LoadHooks;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.UI;


namespace HamstarHelpers.Services.UI.Menus {
	partial class MenuContextServiceManager {
		private IDictionary<MenuUIDefinition, IDictionary<string, MenuContext>> Contexts
				= new Dictionary<MenuUIDefinition, IDictionary<string, MenuContext>>();

		internal Tuple<MenuUIDefinition, UIState> CurrentMenuUI { get; private set; }
		internal Tuple<MenuUIDefinition, UIState> PreviousMenuUI { get; private set; }



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
			if( this.CurrentMenuUI == null ) {
				return;
			}

			MenuUIDefinition menuDef = this.CurrentMenuUI.Item1;
			if( !this.Contexts.ContainsKey( menuDef ) ) {
				LogHelpers.Warn( "Missing menu context " + menuDef );
				return;
			}

			IDictionary<string, MenuContext> loaders = this.Contexts[ menuDef ];

			foreach( MenuContext loader in loaders.Values ) {
				loader.Hide( this.CurrentMenuUI.Item2 );
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
