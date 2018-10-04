using HamstarHelpers.Components.UI.Menu.UI;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Services.Menus;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;


namespace HamstarHelpers.Components.UI.Menu {
	abstract partial class MenuContextBase {
		protected void InitializeBase( bool occludes_logo ) {
			Texture2D old_logo1 = Main.logoTexture;
			Texture2D old_logo2 = Main.logo2Texture;
			
			MenuContextService.AddMenuLoader( this.UIName, "ModHelpers: " + this.ContextName + " Set UI",
				ui => {
					if( occludes_logo ) {
						Main.instance.LoadProjectile( ProjectileID.ShadowBeamHostile );

						Main.logoTexture = Main.projectileTexture[ProjectileID.ShadowBeamHostile];
						Main.logo2Texture = Main.projectileTexture[ProjectileID.ShadowBeamHostile];
					}
					this.MyUI = ui;
				},
				ui => {
					if( occludes_logo ) {
						Main.logoTexture = old_logo1;
						Main.logo2Texture = old_logo2;
					}
					this.MyUI = null;
				}
			);
		}


		protected void InitializeInfoDisplay() {
			if( !MenuContextService.ContainsMenuLoader( this.UIName, "ModHelpers: Info Display" ) ) {
				MenuContextBase.InfoDisplay = new UIInfoDisplay( this );
			}

			MenuContextService.AddMenuLoader( this.UIName, "ModHelpers: Info Display", MenuContextBase.InfoDisplay, false );
		}
	}
}
