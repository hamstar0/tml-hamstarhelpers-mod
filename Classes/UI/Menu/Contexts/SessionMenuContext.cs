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
		protected SessionMenuContext( MenuUIDefinition menuDefinitionOfContext,
				string contextName,
				bool displayInfo,
				bool occludesLogo )
				: base( menuDefinitionOfContext, contextName ) {
			this.DisplayInfo = displayInfo;
			this.OccludesLogo = occludesLogo;
			this.OldLogo1 = Main.logoTexture;
			this.OldLogo2 = Main.logo2Texture;
			this.InfoDisplay = new UIInfoDisplay();
		}


		////////////////

		/// @private
		public sealed override void OnActivation( UIState ui ) {
			var menuDef = this.MenuDefinitionOfContext;

			if( this.DisplayInfo ) {
				WidgetMenuContext widgetCtx;

				if( MenuContextService.GetMenuContext( menuDef, "ModHelpers: Info Display" ) == null ) {
					widgetCtx = new WidgetMenuContext( menuDef, "ModHelpers: Info Display", this.InfoDisplay, false );
					MenuContextService.AddMenuContext( widgetCtx );
				} else {
					widgetCtx = (WidgetMenuContext)MenuContextService.GetMenuContext( menuDef, "ModHelpers: Info Display" );
					this.InfoDisplay = (UIInfoDisplay)widgetCtx.MyElement;
				}
			}

			this.OnActivationForSession( ui );
		}

		/// <summary>
		/// When our menu context first becomes activated with a given menu UI (runs when menu opens).
		/// </summary>
		/// <param name="ui"></param>
		public abstract void OnActivationForSession( UIState ui );


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

			this.RecalculateMenuObjects();
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

			this.ResetMenuObjects();
		}


		////////////////

		/// <summary>
		/// Re-aligns menu objects for the context.
		/// </summary>
		protected virtual void RecalculateMenuObjects() {
			if( Main.screenWidth < ( 800 + 128 ) ) {
				this.SetOverhaulMenuDataOffset( new Vector2( -384, -384 ) );
			}
		}
		
		/// <summary>
		/// Reverts menu objects when the context changes.
		/// </summary>
		protected virtual void ResetMenuObjects() {
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
				Type classType = ReflectionHelpers.GetTypeFromAssembly( ohModType.AssemblyQualifiedName, "TerrariaOverhaul.UI.OverhaulUI" );
				if( classType == null ) { return false; }

				ReflectionHelpers.Set( classType, null, "mainMenuDataOffset", newValue );
			}

			return true;
		}
	}
}
