using HamstarHelpers.Classes.UI.Theme;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;


namespace HamstarHelpers.Classes.UI.Elements {
	/// <summary>
	/// Theme-able UITextPanel.
	/// </summary>
	public class UIThemedTextPanel : UITextPanel<string>, IThemeable {
		/// <summary>
		/// Appearance style.
		/// </summary>
		public UITheme Theme { get; protected set; }

		/// <summary></summary>
		public bool IsHidden { get; protected set; }



		////////////////

		/// <summary>
		/// </summary>
		/// <param name="theme"></param>
		/// <param name="skipThemeRefreshNow"></param>
		/// <param name="text"></param>
		/// <param name="textScale"></param>
		/// <param name="large"></param>
		public UIThemedTextPanel( UITheme theme,
					bool skipThemeRefreshNow,
					string text,
					float textScale=1f,
					bool large=false )
				: base( text, textScale, large ) {
			this.Theme = theme;

			if( !skipThemeRefreshNow ) {
				theme.ApplyPanel( this );
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
			this.Theme.ApplyPanel( this );

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
	}
}
