using HamstarHelpers.TmlHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
		public UIElement TitleElem { get; private set; }
		public UIElement AuthorElem { get; private set; }

		public Color BgColor { get; private set; }
		public Color BgLitColor { get; private set; }
		public Color EdgeColor { get; private set; }
		public Color EdgeLitColor { get; private set; }

		public bool HasIconLoaded { get; private set; }
		public bool WillDrawHoverElements { get; private set; }


		////////////////

		public UIModData( Mod mod, Color bg_color, Color bg_lit_color, Color edge_color, Color edge_lit_color, bool will_draw_hover_elements=true ) {
			TmodFile modfile = mod.File;

			this.Mod = mod;
			this.BgColor = bg_color;
			this.BgLitColor = bg_lit_color;
			this.EdgeColor = edge_color;
			this.EdgeLitColor = edge_lit_color;
			this.WillDrawHoverElements = will_draw_hover_elements;

			this.Author = null;
			this.Homepage = null;
			this.HasIconLoaded = false;

			BuildPropertiesInterface props = modfile != null ?
				BuildPropertiesInterface.GetBuildPropertiesForModFile( modfile ) :
				(BuildPropertiesInterface)null;
			if( props != null ) {
				this.Author = (string)props.GetField( "author" );
				this.Homepage = (string)props.GetField( "homepage" );
			}

			this.SetPadding( 4f );
			this.Width.Set( 0f, 1f );
			this.Height.Set( 64, 0f );

			string mod_title = this.Mod.DisplayName + " " + this.Mod.Version.ToString();
			
			if( this.Homepage != null ) {
				this.TitleElem = new UIWebUrl( mod_title, this.Homepage, false );
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
		}


		////////////////


		public override void Update( GameTime gameTime ) {
			base.Update( gameTime );

			if( this.IsMouseHovering ) {
				this.BackgroundColor = this.BgLitColor;
				this.BorderColor = this.EdgeLitColor;
			} else {
				this.BackgroundColor = this.BgColor;
				this.BorderColor = this.EdgeColor;
			}
		}

		////////////////

		public override void Draw( SpriteBatch sb ) {
			base.Draw( sb );

			if( this.IsMouseHovering ) {
				this.DrawHoverEffects( sb );
			}
		}

		public void DrawHoverEffects( SpriteBatch sb ) {
			if( this.WillDrawHoverElements && this.Homepage != null ) {
				((UIWebUrl)this.TitleElem).DrawHoverEffects( sb );
			}
		}
	}
}
