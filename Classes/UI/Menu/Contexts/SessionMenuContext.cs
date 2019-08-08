using HamstarHelpers.Classes.UI.Menu.UI;
using HamstarHelpers.Classes.UI.Menus;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Reflection;
using HamstarHelpers.Helpers.TModLoader.Menus;
using HamstarHelpers.Services.UI.Menus;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;


namespace HamstarHelpers.Classes.UI.Menu {
	/// <summary>
	/// Defines a class for menu contexts meaning to extensively modify or add to a given menu UI.
	/// </summary>
	abstract public partial class SessionMenuContext : MenuContext {
		private bool DisplayInfo;
		private bool OccludesLogo;

		private Texture2D OldLogo1;
		private Texture2D OldLogo2;


		////////////////

		/// <summary>
		/// The existing menu UI that defines our context.
		/// </summary>
		public UIState MyMenuUI { get; protected set; }

		/// <summary>
		/// A dedicated information displaying panel that 
		/// </summary>
		public UIInfoDisplay InfoDisplay { get; private set; }

		private Vector2 OldOverhaulLogoPos = default( Vector2 );



		////////////////

		/// <summary>
		/// </summary>
		/// <param name="displayInfo">Whether an info panel is used/exists.</param>
		/// <param name="occludesLogo">Whether the Terraria logo is removed.</param>
		protected SessionMenuContext( bool displayInfo, bool occludesLogo ) {
			this.DisplayInfo = displayInfo;
			this.OccludesLogo = occludesLogo;
			this.OldLogo1 = Main.logoTexture;
			this.OldLogo2 = Main.logo2Texture;
			this.InfoDisplay = new UIInfoDisplay();
		}

		/// <summary>
		/// When our menu context first becomes "contextualized" with a given menu.
		/// </summary>
		/// <param name="uiClassName"></param>
		/// <param name="contextNammenuDefe"></param>
		public override void OnContexualize( MenuUIDefinition menuDef, string contextName ) {
			if( this.DisplayInfo ) {
				WidgetMenuContext widgetCtx;

				if( MenuContextService.GetMenuContext( menuDef, "ModHelpers: Info Display" ) == null ) {
					widgetCtx = new WidgetMenuContext( this.InfoDisplay, false );
					MenuContextService.AddMenuContext( menuDef, "ModHelpers: Info Display", widgetCtx );
				} else {
					widgetCtx = (WidgetMenuContext)MenuContextService.GetMenuContext( menuDef, "ModHelpers: Info Display" );
					this.InfoDisplay = (UIInfoDisplay)widgetCtx.MyElement;
				}
			}
		}


		////////////////

		/// <summary>
		/// When a menu bound to the current context is shown.
		/// </summary>
		/// <param name="ui"></param>
		public override void Show( UIState ui ) {
			if( this.OccludesLogo ) {
				Main.instance.LoadProjectile( ProjectileID.ShadowBeamHostile );

				Main.logoTexture = Main.projectileTexture[ProjectileID.ShadowBeamHostile];
				Main.logo2Texture = Main.projectileTexture[ProjectileID.ShadowBeamHostile];
			}

			this.MyMenuUI = ui;
		}

		/// <summary>
		/// When a menu bound to the current context is hidden.
		/// </summary>
		/// <param name="ui"></param>
		public override void Hide( UIState ui ) {
			if( this.OccludesLogo ) {
				Main.logoTexture = this.OldLogo1;
				Main.logo2Texture = this.OldLogo2;
			}

			this.MyMenuUI = null;
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
