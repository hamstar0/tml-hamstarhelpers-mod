using HamstarHelpers.Classes.UI.Theme;
using System;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;


namespace HamstarHelpers.Classes.UI.Elements {
	/// <summary>
	/// Theme-able UIPanel.
	/// </summary>
	public class UIThemedPanel : UIPanel, IThemeable {
		/// <summary>
		/// Appearance style.
		/// </summary>
		public UITheme Theme { get; protected set; }



		////////////////

		/// <summary></summary>
		/// <param name="theme"></param>
		public UIThemedPanel( UITheme theme, bool skipThemeRefreshNow ) : base() {
			this.Theme = theme;

			if( !skipThemeRefreshNow ) {
				theme.ApplyPanel( this );
			}
		}


		////////////////

		/// <summary>
		/// Intended to replace `Append(UIElement)` for propagating the current theme to appended elements.
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
			if( !this.Theme.Apply(element) ) {
				if( element is UIPanel ) {
					this.Theme.ApplyPanel( (UIPanel)element );
				}
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
	}
}
