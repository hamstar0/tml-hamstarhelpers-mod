using HamstarHelpers.Components.UI.Menu.UI;
using HamstarHelpers.Components.UI.Menus;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Services.Menus;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;


namespace HamstarHelpers.Components.UI.Menu {
	abstract partial class SessionMenuContext : MenuContext {
		public UIInfoDisplay InfoDisplay { get; private set; }

		protected UIState MyUI = null;

		private bool DisplayInfo;
		private bool OccludesLogo;
		private Vector2 OldOverhaulLogoPos = default( Vector2 );

		private Texture2D OldLogo1;
		private Texture2D OldLogo2;



		////////////////

		protected SessionMenuContext( bool display_info, bool occludes_logo ) {
			this.DisplayInfo = display_info;
			this.OccludesLogo = occludes_logo;
			this.OldLogo1 = Main.logoTexture;
			this.OldLogo2 = Main.logo2Texture;
			this.InfoDisplay = new UIInfoDisplay();
		}
		
		public override void OnContexualize( string ui_class_name, string context_name ) {
			if( this.DisplayInfo ) {
				WidgetMenuContext widget_ctx;

				if( MenuContextService.GetMenuContext( ui_class_name, "ModHelpers: Info Display" ) == null ) {
					widget_ctx = new WidgetMenuContext( this.InfoDisplay, false );
					MenuContextService.AddMenuContext( ui_class_name, "ModHelpers: Info Display", widget_ctx );
				} else {
					widget_ctx = (WidgetMenuContext)MenuContextService.GetMenuContext( ui_class_name, "ModHelpers: Info Display" );
					this.InfoDisplay = (UIInfoDisplay)widget_ctx.MyElement;
				}
			}
		}


		////////////////

		public override void Show( UIState ui ) {
			if( this.OccludesLogo ) {
				Main.instance.LoadProjectile( ProjectileID.ShadowBeamHostile );

				Main.logoTexture = Main.projectileTexture[ProjectileID.ShadowBeamHostile];
				Main.logo2Texture = Main.projectileTexture[ProjectileID.ShadowBeamHostile];
			}

			this.MyUI = ui;
		}

		public override void Hide( UIState ui ) {
			if( this.OccludesLogo ) {
				Main.logoTexture = this.OldLogo1;
				Main.logo2Texture = this.OldLogo2;
			}

			this.MyUI = null;
		}


		////////////////

		protected void RecalculateMenuObjects() {
			if( Main.screenWidth < (800 + 128) ) {
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
