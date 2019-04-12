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
		public static UIText GetLineElement( string label, float scale, bool large ) {
			float underscoreLen = Main.fontMouseText.MeasureString("_").X;
			float textLen = Main.fontMouseText.MeasureString( label ).X;
			int lineLen = (int)Math.Max( 1f, Math.Round(textLen / (underscoreLen - 2)) );

			return new UIText( new String('_', lineLen), scale, large );
		}



		////////////////

		public UITheme Theme { get; protected set; }
		public UIText TextElem { get; private set; }
		public UIText LineElem { get; private set; }

		public string Url { get; private set; }
		public bool WillDrawOwnHoverUrl { get; private set; }

		public bool IsVisited { get; private set; }

		public float Scale { get; private set; }
		public bool Large { get; private set; }



		////////////////

		public UIWebUrl( UITheme theme, string label, string url, bool hoverUrl = true, float scale = 0.85f, bool large = false ) : base() {
			this.Theme = theme;
			this.IsVisited = false;
			this.Url = url;
			this.WillDrawOwnHoverUrl = hoverUrl;
			this.Scale = scale;
			this.Large = large;

			this.TextElem = new UIText( label, scale, large );
			this.TextElem.TextColor = theme.UrlColor;
			this.Append( this.TextElem );

			this.LineElem = UIWebUrl.GetLineElement( label, scale, large );
			this.LineElem.TextColor = theme.UrlColor;
			this.Append( this.LineElem );

			CalculatedStyle labelSize = this.TextElem.GetDimensions();
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

		public void SetText( string text ) {
			this.TextElem.SetText( text, this.Scale, this.Large );

			this.RemoveChild( this.LineElem );
			this.LineElem.Remove();

			this.LineElem = UIWebUrl.GetLineElement( text, this.Scale, this.Large );
			this.Append( this.LineElem );

			this.Recalculate();
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
