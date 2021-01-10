using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.UI;
using Terraria.UI.Chat;
using Terraria.GameContent.UI.Elements;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Helpers.DotNET.Reflection;


namespace HamstarHelpers.Classes.UI.Elements {
	/// <summary>
	/// Theme-able UIText.
	/// </summary>
	public class UIThemedText : UIText, IThemeable {
		/// <summary>
		/// Appearance style.
		/// </summary>
		public UITheme Theme { get; protected set; }

		/// <summary></summary>
		public bool IsHidden { get; protected set; }


		////////////////

		private float ScaleCopy;
		private bool LargeCopy;
		private Vector2 SizeCopy;



		////////////////

		/// <summary></summary>
		/// <param name="theme"></param>
		/// <param name="skipThemeRefreshNow"></param>
		/// <param name="text"></param>
		/// <param name="textScale"></param>
		/// <param name="large"></param>
		public UIThemedText( UITheme theme,
					bool skipThemeRefreshNow,
					string text,
					float textScale=1,
					bool large=false )
				: base( text, textScale, large ) {
			this.Theme = theme;

			this.ScaleCopy = textScale;
			this.LargeCopy = large;

			DynamicSpriteFont font = large ? Main.fontDeathText : Main.fontMouseText;
			this.SizeCopy = new Vector2(
				font.MeasureString( text.ToString() ).X,
				large ? 32f : 16f
			) * textScale;

			if( !skipThemeRefreshNow ) {
				theme.ApplyText( this );
			}
		}


		////////////////

		/// <summary>
		/// An alternative to the normal `Append` method to apply theming to appended element.
		/// </summary>
		/// <param name="element"></param>
		public void AppendThemed( UIElement element ) {
			base.Append( element );
			this.RefreshThemeForChild( element );
		}


		////////////////

		/// <summary>
		/// Re-applies the current theme styles (including child elements).
		/// </summary>
		public virtual void RefreshTheme() {
			this.Theme.ApplyText( this );

			foreach( UIElement elem in this.Elements ) {
				this.RefreshThemeForChild( elem );
			}
		}

		/// <summary>
		/// Applies the current theme's styles to a given element (presumably a child element).
		/// </summary>
		/// <param name="element"></param>
		public virtual void RefreshThemeForChild( UIElement element ) {
			if( !this.Theme.Apply( element ) ) {
				this.Theme.ApplyByType( element );
			}
		}

		////////////////

		/// <summary>
		/// Sets the current theme.
		/// </summary>
		/// <param name="theme"></param>
		public virtual void SetTheme( UITheme theme ) {
			this.Theme = theme;
			this.RefreshTheme();
		}


		////////////////

		/// <summary></summary>
		public virtual void Hide() {
			this.IsHidden = true;
		}

		/// <summary></summary>
		public virtual void Show() {
			this.IsHidden = false;
		}


		////////////////

		/// @private
		public override void Draw( SpriteBatch spriteBatch ) {
			if( !this.IsHidden ) {
				base.Draw( spriteBatch );
			}
		}

		/// @private
		protected override void DrawSelf( SpriteBatch sb ) {
			CalculatedStyle innerDimensions = base.GetInnerDimensions();
			DynamicSpriteFont font = this.LargeCopy ? Main.fontDeathText : Main.fontMouseText;
			Vector2 pos = innerDimensions.Position();

			pos.X += ( innerDimensions.Width - this.SizeCopy.X ) * 0.5f;
			if( this.LargeCopy ) {
				pos.Y -= 10f * this.ScaleCopy;
			} else {
				pos.Y -= 2f * this.ScaleCopy;
			}

			string[] textLines = this.Text.Split( '\n' );
			float largestSnippetScaleOfLine = 0f;

			for( int i=0; i<textLines.Length; i++ ) {
				TextSnippet[] snippets = ChatManager.ParseMessage( textLines[i], this.TextColor ).ToArray();

				for( int j=0; j<snippets.Length; j++ ) {
					string[] sublines = snippets[j].Text.Split( '\n' );
					if( sublines.Length > 1 ) {
						string rejoined = string.Join( "", sublines );

						snippets[j] = snippets[j].CopyMorph( rejoined );
					}

					if( largestSnippetScaleOfLine < snippets[j].Scale ) {
						largestSnippetScaleOfLine = snippets[j].Scale;
					}
				}

				ChatManager.DrawColorCodedStringWithShadow(
					spriteBatch: sb,
					font: font,
					snippets: snippets,
					position: pos,
					rotation: 0f,
					origin: Vector2.Zero,
					baseScale: new Vector2( this.ScaleCopy ),
					hoveredSnippet: out _,
					maxWidth: -1f,
					spread: 2f
				);

				pos.Y += (float)font.LineSpacing * largestSnippetScaleOfLine * this.ScaleCopy;

				largestSnippetScaleOfLine = 0f;
			}
		}
	}
}
