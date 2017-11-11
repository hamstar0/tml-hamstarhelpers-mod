using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;


namespace HamstarHelpers.UIHelpers.Elements {
	public class UIWebUrl : UIElement {
		public static Color DefaultColor = new Color( 32, 32, 255 );
		public static Color DefaultLitColor = new Color( 80, 80, 255 );
		public static Color DefaultVisitColor = new Color( 192, 0, 255 );



		////////////////

		public UIText TextElem { get; private set; }
		public UIText LineElem { get; private set; }

		public string Url { get; private set; }
		public bool WillDrawOwnHoverUrl { get; private set; }


		////////////////

		public UIWebUrl( string label, string url, bool hover_url=true, float scale=0.85f, bool large=false ) : base() {
			this.WillDrawOwnHoverUrl = hover_url;
			this.Url = url;

			this.TextElem = new UIText( label, scale, large );
			this.TextElem.TextColor = UIWebUrl.DefaultColor;
			this.Append( this.TextElem );
				
			float underscore_len = Main.fontMouseText.MeasureString("_").X * scale;
			CalculatedStyle dim = this.TextElem.GetDimensions();
			int line_len = Math.Max( 1, (int)((dim.Width + 1) / underscore_len) + 1 );

			this.LineElem = new UIText( new String('_', line_len), scale, large );
			this.LineElem.TextColor = UIWebUrl.DefaultColor;
			this.Append( this.LineElem );

			this.Width.Set( dim.Width, 0f );
			this.Height.Set( dim.Height, 0f );

			UIText text_elem = this.TextElem;
			UIText line_elem = this.LineElem;

			this.OnMouseOver += delegate ( UIMouseEvent evt, UIElement from_elem ) {
				if( text_elem.TextColor != UIWebUrl.DefaultVisitColor ) {
					text_elem.TextColor = UIWebUrl.DefaultLitColor;
					text_elem.TextColor = UIWebUrl.DefaultLitColor;
				}
			};
			this.OnMouseOut += delegate ( UIMouseEvent evt, UIElement from_elem ) {
				if( text_elem.TextColor != UIWebUrl.DefaultVisitColor ) {
					text_elem.TextColor = UIWebUrl.DefaultColor;
					text_elem.TextColor = UIWebUrl.DefaultColor;
				}
			};

			this.OnClick += delegate ( UIMouseEvent evt, UIElement from_elem ) {
				try {
					System.Diagnostics.Process.Start( this.Url );

					text_elem.TextColor = UIWebUrl.DefaultVisitColor;
					line_elem.TextColor = UIWebUrl.DefaultVisitColor;
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
				sb.DrawString( Main.fontMouseText, this.Url, UIHelpers.GetHoverTipPosition( this.Url ), Color.White );
			}
		}
	}
}
