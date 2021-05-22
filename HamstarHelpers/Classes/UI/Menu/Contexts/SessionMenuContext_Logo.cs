using HamstarHelpers.Classes.UI.Menus;
using HamstarHelpers.Libraries.Debug;
using HamstarHelpers.Libraries.DotNET.Reflection;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace HamstarHelpers.Classes.UI.Menu {
	/// <summary>
	/// Defines a class for menu contexts meaning to extensively modify or add to a given menu UI.
	/// </summary>
	abstract public partial class SessionMenuContext : MenuContext {
		private void AccommodateLogo() {
			if( this.OccludesLogo ) {
				Main.instance.LoadProjectile( ProjectileID.ShadowBeamHostile );

				Main.logoTexture = Main.projectileTexture[ProjectileID.ShadowBeamHostile];
				Main.logo2Texture = Main.projectileTexture[ProjectileID.ShadowBeamHostile];
			}

			this.RecalculateMenuObjects();
		}

		private void RevertLogo() {
			if( this.OccludesLogo ) {
				Main.logoTexture = this.OldLogo1;
				Main.logo2Texture = this.OldLogo2;
			}

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
			var ohLogoPosField = ohModType.GetField( "mainMenuDataOffset", ReflectionLibraries.MostAccess );

			if( ohLogoPosField != null ) {
				if( this.OldOverhaulLogoPos != default( Vector2 ) ) {
					this.OldOverhaulLogoPos = (Vector2)ohLogoPosField.GetValue( ohMod );
				}
				ohLogoPosField.SetValue( ohMod, newValue );
			} else {	// For version 3.3
				Type classType = ReflectionLibraries.GetTypeFromAssembly( ohModType.AssemblyQualifiedName, "TerrariaOverhaul.UI.OverhaulUI" );
				if( classType == null ) { return false; }

				ReflectionLibraries.Set( classType, null, "mainMenuDataOffset", newValue );
			}

			return true;
		}
	}
}
