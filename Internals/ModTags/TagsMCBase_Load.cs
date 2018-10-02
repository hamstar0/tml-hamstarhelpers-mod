using HamstarHelpers.Components.UI.Menu;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Internals.ModTags.UI;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;


namespace HamstarHelpers.Internals.ModTags {
	abstract partial class TagsMenuContextBase {
		protected void InitializeBase() {
			Texture2D old_logo1 = Main.logoTexture;
			Texture2D old_logo2 = Main.logo2Texture;
			
			MenuUI.AddMenuLoader( this.UIName, "ModHelpers: " + this.ContextName + " Set UI",
				ui => {
					Main.instance.LoadProjectile( ProjectileID.ShadowBeamHostile );

					Main.logoTexture = Main.projectileTexture[ ProjectileID.ShadowBeamHostile ];
					Main.logo2Texture = Main.projectileTexture[ ProjectileID.ShadowBeamHostile ];
					this.MyUI = ui;
				},
				ui => {
					Main.logoTexture = old_logo1;
					Main.logo2Texture = old_logo2;
					this.MyUI = null;
				}
			);
		}


		protected abstract void InitializeContext();


		protected abstract void InitializeControls();


		protected void InitializeInfoDisplay() {
			this.InfoDisplay = new UIInfoDisplay( this );

			MenuUI.AddMenuLoader( this.UIName, "ModHelpers: " + this.ContextName + " Info Display", this.InfoDisplay, false );
		}


		protected void InitializeTagButtons( bool can_disable_tags ) {
			int i = 0;

			foreach( var kv in TagsMenuContextBase.Tags ) {
				string tag_text = kv.Key;
				string tag_desc = kv.Value;

				var button = new UITagButton( this, i, tag_text, tag_desc, can_disable_tags );

				MenuUI.AddMenuLoader( this.UIName, "ModHelpers: " + this.ContextName + " Tag " + i, button, false );
				this.TagButtons[tag_text] = button;

				i++;
			}
		}
	}
}
