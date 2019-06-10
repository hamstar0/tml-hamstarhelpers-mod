using HamstarHelpers.Components.UI.Menu.UI;
using HamstarHelpers.Components.UI.Menus;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Reflection;
using HamstarHelpers.Services.Menus;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;


namespace HamstarHelpers.Components.UI.Menu {
	abstract partial class SessionMenuContext : MenuContext {
		public UIInfoDisplay InfoDisplay { get; private set; }

		public UIState MyUI { get; protected set; }

		private bool DisplayInfo;
		private bool OccludesLogo;
		private Vector2 OldOverhaulLogoPos = default( Vector2 );

		private Texture2D OldLogo1;
		private Texture2D OldLogo2;



		////////////////

		protected SessionMenuContext( bool displayInfo, bool occludesLogo ) {
			this.DisplayInfo = displayInfo;
			this.OccludesLogo = occludesLogo;
			this.OldLogo1 = Main.logoTexture;
			this.OldLogo2 = Main.logo2Texture;
			this.InfoDisplay = new UIInfoDisplay();
		}

		public override void OnContexualize( string uiClassName, string contextName ) {
			if( this.DisplayInfo ) {
				WidgetMenuContext widgetCtx;

				if( MenuContextService.GetMenuContext( uiClassName, "ModHelpers: Info Display" ) == null ) {
					widgetCtx = new WidgetMenuContext( this.InfoDisplay, false );
					MenuContextService.AddMenuContext( uiClassName, "ModHelpers: Info Display", widgetCtx );
				} else {
					widgetCtx = (WidgetMenuContext)MenuContextService.GetMenuContext( uiClassName, "ModHelpers: Info Display" );
					this.InfoDisplay = (UIInfoDisplay)widgetCtx.MyElement;
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
			if( Main.screenWidth < ( 800 + 128 ) ) {
				this.SetOverhaulMenuDataOffset( new Vector2( -384, -384 ) );
			}
		}

		protected void ResetMenuObjects() {
			if( this.OldOverhaulLogoPos != default( Vector2 ) ) {
				this.SetOverhaulMenuDataOffset( this.OldOverhaulLogoPos );
			}
		}


		////////////////

		private bool SetOverhaulMenuDataOffset( Vector2 newValue ) {
			Mod ohMod = ModLoader.GetMod( "OverhaulMod" );
			if( ohMod == null ) { return false; }
			
			Type ohModType = ohMod.GetType();
			var ohLogoPosField = ohModType.GetField( "mainMenuDataOffset", ReflectionHelpers.MostAccess );

			if( ohLogoPosField != null ) {
				if( this.OldOverhaulLogoPos != default( Vector2 ) ) {
					this.OldOverhaulLogoPos = (Vector2)ohLogoPosField.GetValue( ohMod );
				}
				ohLogoPosField.SetValue( ohMod, newValue );
			} else {	// For version 3.3
				Type classType = ReflectionHelpers.GetClassFromAssembly( ohModType.AssemblyQualifiedName, "TerrariaOverhaul.UI.OverhaulUI" );
				if( classType == null ) { return false; }

				ReflectionHelpers.Set( classType, null, "mainMenuDataOffset", newValue );
			}

			return true;
		}
	}
}
