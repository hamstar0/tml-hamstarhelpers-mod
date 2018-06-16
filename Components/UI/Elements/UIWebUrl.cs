using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using HamstarHelpers.DotNetHelpers;

namespace HamstarHelpers.Components.UI.Elements {
	public class UIWebUrl : UIElement {
		[System.Obsolete( "use UITheme.UrlColor", true )]
		public static Color DefaultColor = new Color( 80, 80, 255 );
		[System.Obsolete( "use UITheme.UrlLitColor", true )]
		public static Color DefaultLitColor = new Color( 128, 128, 255 );
		[System.Obsolete( "use UITheme.UrlVisitColor", true )]
		public static Color DefaultVisitColor = new Color( 192, 0, 255 );



		////////////////

		public UITheme Theme { get; private set; }
		public UIText TextElem { get; private set; }
		public UIText LineElem { get; private set; }

		public string Url { get; private set; }
		public bool WillDrawOwnHoverUrl { get; private set; }


		////////////////

		[System.Obsolete( "use other constructor", true )]
		public UIWebUrl( string label, string url, bool hover_url = true, float scale = 0.85f, bool large = false )
				: this( new UITheme(), label, url, hover_url, scale, large ) { }

		public UIWebUrl( UITheme theme, string label, string url, bool hover_url = true, float scale = 0.85f, bool large = false ) : base() {
			this.Theme = theme;

			this.WillDrawOwnHoverUrl = hover_url;
			this.Url = url;

			this.TextElem = new UIText( label, scale, large );
			this.TextElem.TextColor = theme.UrlColor;
			this.Append( this.TextElem );

			CalculatedStyle label_size = this.TextElem.GetDimensions();
			float underscore_len = Main.fontMouseText.MeasureString("_").X;
			float text_len = Main.fontMouseText.MeasureString( label ).X;
			int line_len = (int)Math.Max( 1f, Math.Round(text_len / (underscore_len - 2)) );

			this.LineElem = new UIText( new String('_', line_len), scale, large );
			this.LineElem.TextColor = theme.UrlColor;
			this.Append( this.LineElem );

			this.Width.Set( label_size.Width, 0f );
			this.Height.Set( label_size.Height, 0f );

			UIText text_elem = this.TextElem;
			UIText line_elem = this.LineElem;

			this.OnMouseOver += delegate ( UIMouseEvent evt, UIElement from_elem ) {
				if( text_elem.TextColor != theme.UrlVisitColor ) {
					text_elem.TextColor = theme.UrlLitColor;
					text_elem.TextColor = theme.UrlLitColor;
				}
			};
			this.OnMouseOut += delegate ( UIMouseEvent evt, UIElement from_elem ) {
				if( text_elem.TextColor != theme.UrlVisitColor ) {
					text_elem.TextColor = theme.UrlColor;
					text_elem.TextColor = theme.UrlColor;
				}
			};

			this.OnClick += delegate ( UIMouseEvent evt, UIElement from_elem ) {
				try {
					SystemHelpers.OpenUrl( this.Url );
					//System.Diagnostics.Process.Start( this.Url );

					text_elem.TextColor = theme.UrlVisitColor;
					line_elem.TextColor = theme.UrlVisitColor;
				} catch( Exception e ) {
					Main.NewText( e.Message );
				}
			};
		}


		////////////////

		public override void Draw( SpriteBatch sb ) {
			base.Draw( sb );

			if( this.TextElem.IsMouseHovering && this.WillDrawOwnHoverUrl ) {
				this.DrawHoverEffects( sb );
			}
		}

		public void DrawHoverEffects( SpriteBatch sb ) {
			if( !string.IsNullOrEmpty(this.Url) ) {
				sb.DrawString( Main.fontMouseText, this.Url, UIHelpers.UIHelpers.GetHoverTipPosition( this.Url ), Color.White );
			}
		}
	}
}
