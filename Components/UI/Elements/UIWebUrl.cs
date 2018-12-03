using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using HamstarHelpers.Helpers.DotNetHelpers;
using HamstarHelpers.Helpers.UIHelpers;
using HamstarHelpers.Helpers.DebugHelpers;


namespace HamstarHelpers.Components.UI.Elements {
	public class UIWebUrl : UIElement {
		public UITheme Theme { get; protected set; }
		public UIText TextElem { get; private set; }
		public UIText LineElem { get; private set; }

		public string Url { get; private set; }
		public bool WillDrawOwnHoverUrl { get; private set; }

		public bool IsVisited { get; private set; }



		////////////////
		
		public UIWebUrl( UITheme theme, string label, string url, bool hoverUrl = true, float scale = 0.85f, bool large = false ) : base() {
			this.Theme = theme;
			this.IsVisited = false;

			this.WillDrawOwnHoverUrl = hoverUrl;
			this.Url = url;

			this.TextElem = new UIText( label, scale, large );
			this.TextElem.TextColor = theme.UrlColor;
			this.Append( this.TextElem );

			CalculatedStyle labelSize = this.TextElem.GetDimensions();
			float underscoreLen = Main.fontMouseText.MeasureString("_").X;
			float textLen = Main.fontMouseText.MeasureString( label ).X;
			int lineLen = (int)Math.Max( 1f, Math.Round(textLen / (underscoreLen - 2)) );

			this.LineElem = new UIText( new String('_', lineLen), scale, large );
			this.LineElem.TextColor = theme.UrlColor;
			this.Append( this.LineElem );

			this.Width.Set( labelSize.Width, 0f );
			this.Height.Set( labelSize.Height, 0f );

			UIText textElem = this.TextElem;
			UIText lineElem = this.LineElem;

			this.OnMouseOver += delegate ( UIMouseEvent evt, UIElement fromElem ) {
				if( textElem.TextColor != theme.UrlVisitColor ) {
					textElem.TextColor = theme.UrlLitColor;
					textElem.TextColor = theme.UrlLitColor;
				}
			};
			this.OnMouseOut += delegate ( UIMouseEvent evt, UIElement fromElem ) {
				if( textElem.TextColor != theme.UrlVisitColor ) {
					textElem.TextColor = theme.UrlColor;
					textElem.TextColor = theme.UrlColor;
				}
			};

			this.OnClick += delegate ( UIMouseEvent evt, UIElement fromElem ) {
				try {
					SystemHelpers.OpenUrl( this.Url );
					//System.Diagnostics.Process.Start( this.Url );

					this.IsVisited = true;

					textElem.TextColor = theme.UrlVisitColor;
					lineElem.TextColor = theme.UrlVisitColor;
				} catch( Exception e ) {
					Main.NewText( e.Message );
				}
			};
		}


		////////////////

		public override void Draw( SpriteBatch sb ) {
			base.Draw( sb );
			
			if( this.TextElem.IsMouseHovering || this.IsMouseHovering ) {
				if( this.WillDrawOwnHoverUrl ) {
					this.DrawHoverEffects( sb );
				}
			}
		}

		public void DrawHoverEffects( SpriteBatch sb ) {
			if( !string.IsNullOrEmpty(this.Url) ) {
				sb.DrawString( Main.fontMouseText, this.Url, UIHelpers.GetHoverTipPosition( this.Url ), Color.White );
			}
		}


		////////////////

		public virtual void RefreshTheme() {
			if( this.IsVisited ) {
				this.TextElem.TextColor = this.Theme.UrlVisitColor;
				this.LineElem.TextColor = this.Theme.UrlVisitColor;
			} else {
				if( this.IsMouseHovering ) {
					this.TextElem.TextColor = this.Theme.UrlLitColor;
					this.LineElem.TextColor = this.Theme.UrlLitColor;
				} else {
					this.TextElem.TextColor = this.Theme.UrlColor;
					this.LineElem.TextColor = this.Theme.UrlColor;
				}
			}
		}
	}
}
