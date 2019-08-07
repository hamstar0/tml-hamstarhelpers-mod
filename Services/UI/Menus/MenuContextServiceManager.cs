using HamstarHelpers.Classes.UI.Menus;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TModLoader.Menus;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.UI;


namespace HamstarHelpers.Services.UI.Menus {
	class MenuContextServiceManager {
		internal IDictionary<TModLoaderMenuDefinition, IDictionary<string, MenuContext>> Contexts
			= new Dictionary<TModLoaderMenuDefinition, IDictionary<string, MenuContext>>();

		internal Tuple<TModLoaderMenuDefinition, UIState> CurrentMenuUI = null;
		internal Tuple<TModLoaderMenuDefinition, UIState> PreviousMenuUI = null;
		


		////////////////
		
		public MenuContextServiceManager() {
			if( Main.dedServ ) { return; }

			Main.OnPostDraw += MenuContextServiceManager._Update;
		}

		~MenuContextServiceManager() {
			if( Main.dedServ ) { return; }

			try {
				Main.OnPostDraw -= MenuContextServiceManager._Update;
				this.HideAll();

				this.Contexts.Clear();
			} catch { }
		}


		////////////////

		private void HideAll() {
			if( this.CurrentMenuUI != null ) {
				TModLoaderMenuDefinition menuDef = this.CurrentMenuUI.Item1;

				if( !this.Contexts.ContainsKey(menuDef) ) {
					LogHelpers.Warn( "Missing menu context " + Enum.GetName(typeof(TModLoaderMenuDefinition), menuDef) );
					return;
				}

				IDictionary<string, MenuContext> loaders = this.Contexts[ menuDef ];

				foreach( MenuContext loader in loaders.Values ) {
					loader.Hide( this.CurrentMenuUI.Item2 );
				}
			}
		}


		////////////////

		private static void _Update( GameTime gametime ) {   // <- Just in case references are doing something funky...
			ModHelpersMod mymod = ModHelpersMod.Instance;
			if( mymod == null ) { return; }

			if( mymod.MenuContextMngr == null ) { return; }
			mymod.MenuContextMngr.Update();
		}

		private void Update() {
			UIState ui = Main.MenuUI.CurrentState;
			string prevUiName, currUiName;

			if( this.CurrentMenuUI == null ) {
				prevUiName = null;
			} else {
				TModLoaderMenuDefinition prevUiDef = this.CurrentMenuUI.Item1;
				prevUiName = "UI" + Enum.GetName( typeof( TModLoaderMenuDefinition ), prevUiDef );
			}

			currUiName = ui?.GetType().Name;

			if( prevUiName == currUiName ) {
				return;
			}

			this.LoadUI( ui );
		}


		private void LoadUI( UIState ui ) {
			if( ui == null ) {
				this.CurrentMenuUI = null;
				return;
			}

			TModLoaderMenuDefinition prevUiDef, currUiDef;

			prevUiDef = this.CurrentMenuUI?.Item1 ?? 0;

			string currUiName = ui.GetType().Name.Substring( 2 );
			if( !Enum.TryParse(currUiName, out currUiDef) ) {
				this.CurrentMenuUI = null;
				return;
			}

			this.PreviousMenuUI = this.CurrentMenuUI;

			if( prevUiDef != 0 && this.Contexts.ContainsKey(prevUiDef) ) {
				var contexts = this.Contexts[ prevUiDef ].Values;
				
				foreach( MenuContext ctx in contexts ) {
					ctx.Hide( this.CurrentMenuUI.Item2 );
				}
				//this.Unloaders.Remove( prev_ui_name );
			}

			if( this.Contexts.ContainsKey( currUiDef ) ) {
				foreach( MenuContext ctx in this.Contexts[currUiDef].Values ) {
					ctx.Show( ui );
				}
			}

			this.CurrentMenuUI = Tuple.Create( currUiDef, ui );
		}
	}
}
