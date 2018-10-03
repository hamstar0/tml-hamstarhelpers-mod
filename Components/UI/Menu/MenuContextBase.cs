using HamstarHelpers.Components.UI.Menu.UI;
using HamstarHelpers.Helpers.DebugHelpers;
using Microsoft.Xna.Framework;
using System;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;


namespace HamstarHelpers.Components.UI.Menu {
	abstract partial class MenuContextBase {
		public abstract string UIName { get; }
		public abstract string ContextName { get; }

		////////////////

		protected UIState MyUI = null;
		internal UIInfoDisplay InfoDisplay;

		protected Vector2 OldOverhaulLogoPos = default( Vector2 );



		////////////////

		protected MenuContextBase( bool occludes_logo ) {
			this.InitializeBase( occludes_logo );
			this.InitializeInfoDisplay();
		}


		////////////////

		protected void RecalculateMenuObjects() {
			if( Main.screenWidth < (800 + 128) || Main.screenHeight < (640 + 128) ) {
				Mod oh_mod = ModLoader.GetMod( "OverhaulMod" );

				if( oh_mod != null ) {
					Type oh_mod_type = oh_mod.GetType();
					var oh_logo_pos_field = oh_mod_type.GetField( "mainMenuDataOffset", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static );

					if( oh_logo_pos_field != null ) {
						if( this.OldOverhaulLogoPos != default( Vector2 ) ) {
							this.OldOverhaulLogoPos = (Vector2)oh_logo_pos_field.GetValue( oh_mod );
						}

						oh_logo_pos_field.SetValue( oh_mod, new Vector2( -384, -384 ) );
					}
				}
			}
		}

		protected void ResetMenuObjects() {
			if( this.OldOverhaulLogoPos != default( Vector2 ) ) {
				Mod oh_mod = ModLoader.GetMod( "OverhaulMod" );

				if( oh_mod != null ) {
					Type overhaul_mod_type = oh_mod.GetType();
					var menu_data_pos_field = overhaul_mod_type.GetField( "mainMenuDataOffset", BindingFlags.Public | BindingFlags.Static );

					if( menu_data_pos_field != null ) {
						menu_data_pos_field.SetValue( oh_mod, this.OldOverhaulLogoPos );
					}
				}
			}
		}
	}
}
