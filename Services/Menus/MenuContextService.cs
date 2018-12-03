﻿using HamstarHelpers.Components.UI.Menus;
using HamstarHelpers.Helpers.DebugHelpers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.UI;


namespace HamstarHelpers.Services.Menus {
	class MenuContextServiceManager {
		internal IDictionary<string, IDictionary<string, MenuContext>> Contexts = new Dictionary<string, IDictionary<string, MenuContext>>();

		internal Tuple<string, UIState> CurrentMenuUI = null;
		internal Tuple<string, UIState> PreviousMenuUI = null;
		


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
				foreach( MenuContext loader in this.Contexts[ this.CurrentMenuUI.Item1 ].Values ) {
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

			string prevUiName = this.CurrentMenuUI?.Item1;
			string currUiName = ui?.GetType().Name;

			if( prevUiName == currUiName ) {
				return;
			}

			this.LoadUI( ui );
		}


		private void LoadUI( UIState ui ) {
			string prevUiName = this.CurrentMenuUI?.Item1;
			string currUiName = ui?.GetType().Name;

			this.PreviousMenuUI = this.CurrentMenuUI;

			if( prevUiName != null && this.Contexts.ContainsKey(prevUiName) ) {
				var contexts = this.Contexts[ prevUiName ].Values;
				
				foreach( MenuContext ctx in contexts ) {
					ctx.Hide( this.CurrentMenuUI.Item2 );
				}
				//this.Unloaders.Remove( prev_ui_name );
			}
			
			if( ui == null ) {
				this.CurrentMenuUI = null;
				return;
			}

			if( this.Contexts.ContainsKey( currUiName ) ) {
				foreach( MenuContext ctx in this.Contexts[currUiName].Values ) {
					ctx.Show( ui );
				}
			}

			this.CurrentMenuUI = Tuple.Create( currUiName, ui );
		}
	}
}
