using HamstarHelpers.TmlHelpers;
using HamstarHelpers.UIHelpers;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using System.IO;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;


namespace HamstarHelpers.Utilities.UI {
	public class UIModData : UIPanel {
		public Mod Mod { get; private set; }
		public string Author { get; private set; }
		public string HomepageUrl { get; private set; }

		public UIImage IconElem { get; private set; }
		public UIElement TitleElem { get; private set; }
		public UIElement AuthorElem { get; private set; }
		public UITextPanel<string> ConfigButton { get; private set; }

		public bool HasIconLoaded { get; private set; }
		public bool WillDrawOwnHoverElements { get; private set; }


		////////////////

		public UIModData( UITheme theme, Mod mod, bool will_draw_own_hover_elements=true ) {
			TmodFile modfile = mod.File;

			this.Mod = mod;
			this.WillDrawOwnHoverElements = will_draw_own_hover_elements;

			this.Author = null;
			this.HomepageUrl = null;
			this.HasIconLoaded = false;

			BuildPropertiesInterface props = modfile != null ?
				BuildPropertiesInterface.GetBuildPropertiesForModFile( modfile ) :
				(BuildPropertiesInterface)null;
			if( props != null ) {
				this.Author = (string)props.GetField( "author" );
				this.HomepageUrl = (string)props.GetField( "homepage" );
			}
			
			this.SetPadding( 4f );
			this.Width.Set( 0f, 1f );
			this.Height.Set( 64, 0f );

			string mod_title = this.Mod.DisplayName + " " + this.Mod.Version.ToString();
			
			if( this.HomepageUrl != null ) {
				this.TitleElem = new UIWebUrl( mod_title, this.HomepageUrl, false );
			} else {
				this.TitleElem = new UIText( mod_title );
			}
			this.TitleElem.Left.Set( 72f, 0f );
			this.Append( (UIElement)this.TitleElem );

			if( this.Author != null ) {
				this.AuthorElem = new UIText( "By: "+this.Author, 0.7f );
				this.AuthorElem.Top.Set( 20f, 0f );
				this.AuthorElem.Left.Set( 72f, 0f );
				this.Append( (UIElement)this.AuthorElem );
			}

			if( modfile != null && modfile.HasFile( "icon.png" ) ) {
				var stream = new MemoryStream( modfile.GetFile( "icon.png" ) );
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

			if( ExtendedModManager.HasConfig(mod) ) {
				var config_button = new UITextPanelButton( theme, "Open Config File" );
				config_button.HAlign = 1f;
				config_button.VAlign = 1f;
				this.Append( config_button );

				this.ConfigButton = config_button;
					
				this.ConfigButton.OnClick += delegate ( UIMouseEvent evt, UIElement from_elem ) {
					string path = ExtendedModManager.GetConfigRelativePath( mod );
					Process.Start( Main.SavePath + Path.DirectorySeparatorChar + path );
				};
			}
		}


		////////////////

		public override void Draw( SpriteBatch sb ) {
			base.Draw( sb );

			if( this.IsMouseHovering && this.WillDrawOwnHoverElements ) {
				this.DrawHoverEffects( sb );
			}
		}


		public void DrawHoverEffects( SpriteBatch sb ) {
			if( this.TitleElem.IsMouseHovering ) {
				if( this.TitleElem is UIWebUrl ) {
					var title_url = (UIWebUrl)this.TitleElem;

					if( !title_url.WillDrawOwnHoverUrl ) {
						title_url.DrawHoverEffects( sb );
					}
				}
			}
		}
	}
}
