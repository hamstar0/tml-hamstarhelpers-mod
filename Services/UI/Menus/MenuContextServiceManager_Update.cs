using HamstarHelpers.Classes.UI.Menus;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TModLoader.Menus;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.UI;


namespace HamstarHelpers.Services.UI.Menus {
	partial class MenuContextServiceManager {
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
				MenuUIDefinition prevUiDef = this.CurrentMenuUI.Item1;
				prevUiName = Enum.GetName( typeof(MenuUIDefinition), prevUiDef );
			}

			currUiName = ui?.GetType().Name;

			if( prevUiName == currUiName ) {
				return;
			}

			this.LoadUI( ui );
		}


		////////////////

		private void LoadUI( UIState ui ) {
			if( ui == null ) {
				this.CurrentMenuUI = null;
				return;
			}

			MenuUIDefinition openingUiDef;
			MenuUIDefinition closingUiDef = this.CurrentMenuUI?.Item1 ?? 0;

			if( !Enum.TryParse( ui.GetType().Name, out openingUiDef) ) {
				LogHelpers.WarnOnce( "Could not get MenuUIDefinition " + ui.GetType().Name );
				this.CurrentMenuUI = null;
				return;
			}

			// Out with the old
			if( closingUiDef != 0 && this.Contexts.ContainsKey(closingUiDef) ) {
				var contexts = this.Contexts[ closingUiDef ].Values;
				
				foreach( MenuContext ctx in contexts ) {
					ctx.Hide( this.CurrentMenuUI.Item2 );
				}
			}

			// In with the new
			if( this.Contexts.ContainsKey( openingUiDef ) ) {
				foreach( MenuContext ctx in this.Contexts[openingUiDef].Values ) {
					ctx.Show( ui );
				}
			}

			this.PreviousMenuUI = this.CurrentMenuUI;
			this.CurrentMenuUI = Tuple.Create( openingUiDef, ui );
		}
	}
}
