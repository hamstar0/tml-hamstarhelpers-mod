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
	/// Defines a visitable web URL (hyperlink) as a UI element.
	/// </summary>
	public class UIWebUrl : UIThemedElement {
		/// <summary>
		/// Indicated number of underscore characters that fit within a given string's length.
		/// </summary>
		/// <param name="label"></param>
		/// <param name="lengthOffset"></param>
		/// <returns></returns>
		public static int GetUnderlineLength( string label, int lengthOffset ) {
			float underscoreLen = Main.fontMouseText.MeasureString( "_" ).X;
			float textLen = Main.fontMouseText.MeasureString( label ).X;

			return (int)Math.Max( 1f, Math.Round( textLen / ( underscoreLen - 2f ) ) ) + lengthOffset;
		}


		/// <summary>
		/// Generates a UIText element of the URL's underline (a set of underscores). Used to render together with URL text.
		/// </summary>
		/// <param name="label">Label to generate underline based on.</param>
		/// <param name="scale">Size multipler of text label.</param>
		/// <param name="large">Label as 'large' text.</param>
		/// <returns>UIText underline element.</returns>
		public static UIText GetLineElement( string label, float scale, bool large ) {
			int lineLen = UIWebUrl.GetUnderlineLength( label, 0 );

			return new UIText( new String('_', lineLen), scale, large );
		}

		private static UIText GetLineElement( string label, float scale, bool large, int lengthOffset ) {
			int lineLen = UIWebUrl.GetUnderlineLength( label, lengthOffset );

			return new UIText( new String('_', lineLen), scale, large );
		}




		////////////////

		/// <summary>
		/// Element holding the display text.
		/// </summary>
		public UIText TextElem { get; private set; }
		/// <summary>
		/// Element holding the display text's underline.
		/// </summary>
		public UIText LineElem { get; private set; }

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

		/// <summary>
		/// Size multiplier of display text.
		/// </summary>
		public float Scale { get; private set; }
		/// <summary>
		/// 'Large' state of display text.
		/// </summary>
		public bool Large { get; private set; }



		////////////////

		/// <param name="theme">Appearance style.</param>
		/// <param name="label">Display text.</param>
		/// <param name="url">URL.</param>
		/// <param name="hoverUrl">Indicates if the current element handles its own mouse hover URL display behavior.</param>
		/// <param name="scale">Size multiplier of display text.</param>
		/// <param name="large">'Large' state of display text.</param>
		public UIWebUrl( UITheme theme, string label, string url, bool hoverUrl = true, float scale = 0.85f,
				bool large = false )
				: base( theme, true ) {
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

			this.RefreshTheme();
		}


		////////////////

		/// <summary>
		/// Sets the display text.
		/// </summary>
		/// <param name="text">New display text.</param>
		public void SetText( string text ) {
			this.TextElem.SetText( text, this.Scale, this.Large );

			this.RemoveChild( this.LineElem );
			this.LineElem.Remove();

			this.LineElem = UIWebUrl.GetLineElement( text, this.Scale, this.Large );
			this.Append( this.LineElem );

			this.Recalculate();
		}

		/// <summary>
		/// Adjusts the length of this URL's underscore character line by a given offset amount.
		/// </summary>
		/// <param name="offset">Amount of underscores to add or remove.</param>
		public void ApplyUnderlineOffset( int offset ) {
			this.LineElem.Remove();

			this.LineElem = UIWebUrl.GetLineElement( this.TextElem.Text, this.Scale, this.Large, offset );
			this.LineElem.TextColor = this.Theme.UrlColor;
			this.Append( this.LineElem );
		}


		////////////////

		/// <summary>
		/// Draws the element. Handles mouse hover behavior.
		/// </summary>
		/// <param name="sb">SpriteBatch to draw to. Usually `Main.spriteBatch`.</param>
		public override void Draw( SpriteBatch sb ) {
			base.Draw( sb );
			
			if( this.TextElem.IsMouseHovering || this.IsMouseHovering ) {
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


		////////////////

		/// <summary>
		/// Refreshes visual theming.
		/// </summary>
		public override void RefreshTheme() {
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
