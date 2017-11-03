using HamstarHelpers.TmlHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;


namespace HamstarHelpers.Utilities.UI {
	class UIModData : UIPanel {
		public Mod Mod { get; private set; }
		public string Author { get; private set; }
		public string Homepage { get; private set; }

		public UIImage IconElem { get; private set; }
		public UIText TitleElem { get; private set; }

		public bool HasIconLoaded { get; private set; }


		////////////////

		public UIModData( Mod mod ) {
			this.Mod = mod;
			this.Author = null;
			this.Homepage = null;
			this.HasIconLoaded = false;

			BuildPropertiesInterface props = this.Mod.File != null ?
				BuildPropertiesInterface.GetBuildPropertiesForModFile( this.Mod.File ) :
				(BuildPropertiesInterface)null;
			if( props != null ) {
				this.Author = (string)props.GetField( "author" );
				this.Homepage = (string)props.GetField( "homepage" );
			}

			this.SetPadding( 4f );
			this.Width.Set( 0f, 1f );
			this.Height.Set( 64, 0f );

			string mod_title = this.Mod.DisplayName + " " + this.Mod.Version.ToString();
			//if( this.Author != null ) {
			//	mod_title += " - " + this.Author;
			//}
		
			this.TitleElem = new UIText( mod_title );
			this.TitleElem.Left.Set( 96, 0f );
			this.Append( (UIElement)this.TitleElem );
			
			TmodFile modfile = this.Mod.File;

			if( modfile != null && modfile.HasFile( "icon.png" ) ) {
				var stream = new MemoryStream( this.Mod.File.GetFile( "icon.png" ) );
				var icon_tex = Texture2D.FromStream( Main.graphics.GraphicsDevice, stream );

				if( icon_tex.Width == 80 && icon_tex.Height == 80 ) {
					this.IconElem = new UIImage( icon_tex );
					this.IconElem.Top.Set( -4f, 0f );
					this.IconElem.Left.Set( -4f, 0f );
					this.IconElem.MarginTop = -8f;
					this.IconElem.MarginLeft = -4f;
					this.IconElem.ImageScale = 0.7f;
					this.Append( this.IconElem );
				}
			}
		}


		public override void Draw( SpriteBatch sb ) {
			base.Draw( sb );

			if( this.TitleElem.IsMouseHovering && this.Author != null ) {
				sb.DrawString( Main.fontMouseText, "By " + this.Author, new Vector2(Main.mouseX, Main.mouseY), Color.White );
			}
		}
	}
}
