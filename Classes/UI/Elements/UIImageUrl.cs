using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Microsoft.Xna.Framework.Graphics;
using HamstarHelpers.Helpers.DotNET;
using HamstarHelpers.Helpers.UI;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Classes.UI.Theme;


namespace HamstarHelpers.Classes.UI.Elements {
	/// <summary>
	/// Defines a visitable web URL (hyperlink) as an image UI element.
	/// </summary>
	public class UIImageUrl : UIThemedElement {
		/// <summary>
		/// Element holding the display image.
		/// </summary>
		public UIImageButton ImageElement { get; private set; }

		/// <summary>
		/// URL to browse to when element is clicked.
		/// </summary>
		public string Url { get; private set; }
		/// <summary>
		/// Indicates if the current element handles its own mouse hover URL display behavior.
		/// </summary>
		public bool WillDrawOwnHoverUrl { get; private set; }

		/// <summary>
		/// Indicates if the link has been visited (e.g. clicked on).
		/// </summary>
		public bool IsVisited { get; private set; }



		////////////////

		/// <param name="theme">Appearance style.</param>
		/// <param name="image">Display image.</param>
		/// <param name="url">URL.</param>
		/// <param name="hoverUrl">Indicates if the current element handles its own mouse hover URL display behavior.</param>
		public UIImageUrl( UITheme theme, Texture2D image, string url, bool hoverUrl = true )
				: base( theme, true ) {
			this.IsVisited = false;
			this.Url = url;
			this.WillDrawOwnHoverUrl = hoverUrl;

			this.ImageElement = new UIImageButton( image );
			this.ImageElement.SetVisibility( 1f, 0.75f );
			this.Append( this.ImageElement );

			this.Width.Pixels = this.ImageElement.Width.Pixels;
			this.Width.Percent = this.ImageElement.Width.Percent;
			this.Height.Pixels = this.ImageElement.Height.Pixels;
			this.Height.Percent = this.ImageElement.Height.Percent;

			this.OnMouseOver += ( evt, __ ) => {
				if( !this.ImageElement.IsMouseHovering ) {
					this.ImageElement.MouseOver( evt );
				}
			};
			this.OnMouseOut += ( evt, __ ) => {
				if( this.ImageElement.IsMouseHovering ) {
					this.ImageElement.MouseOut( evt );
				}
			};
			this.OnClick += ( _, __ ) => {
				try {
					SystemHelpers.OpenUrl( this.Url );
					//System.Diagnostics.Process.Start( this.Url );

					this.IsVisited = true;
				} catch( Exception e ) {
					Main.NewText( e.Message );
				}
			};

			this.RefreshTheme();
		}


		////////////////

		/// <summary>
		/// Draws the element. Handles mouse hover behavior.
		/// </summary>
		/// <param name="sb">SpriteBatch to draw to. Usually `Main.spriteBatch`.</param>
		public override void Draw( SpriteBatch sb ) {
			base.Draw( sb );
			
			if( this.ImageElement.IsMouseHovering || this.IsMouseHovering ) {
				if( this.WillDrawOwnHoverUrl ) {
					this.DrawHoverEffects( sb );
				}
			}
		}

		/// <summary>
		/// Default mouse hover URL drawing.
		/// </summary>
		/// <param name="sb">SpriteBatch to draw to. Usually `Main.spriteBatch`.</param>
		public void DrawHoverEffects( SpriteBatch sb ) {
			if( !string.IsNullOrEmpty(this.Url) ) {
				Vector2 pos = UIHelpers.GetHoverTipPosition( this.Url );
				
				Utils.DrawBorderStringFourWay( sb, Main.fontMouseText, this.Url, pos.X, pos.Y, Color.White, Color.Black, default(Vector2) );
				//sb.DrawString( Main.fontMouseText, this.Url, UIHelpers.GetHoverTipPosition( this.Url ), Color.White );
			}
		}
	}
}
