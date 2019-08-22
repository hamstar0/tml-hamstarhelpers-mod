using HamstarHelpers.Helpers.DotNET.Reflection;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;


namespace HamstarHelpers.Classes.UI.Theme {
	/// <summary>
	/// Theme-able UIState.
	/// </summary>
	public class UIThemedState : UIState, IThemeable {
		/// <summary>
		/// Appearance style.
		/// </summary>
		public UITheme Theme { get; protected set; }

		/// <summary></summary>
		public bool IsHidden { get; protected set; }



		////////////////

		/// <summary></summary>
		/// <param name="theme">Appearance style.</param>
		public UIThemedState( UITheme theme ) : base() {
			this.Theme = theme;

			this.RefreshTheme();
		}


		////////////////

		/// <summary>
		/// An alternative to the normal `Append` method to apply theming to appended element.
		/// </summary>
		/// <param name="element"></param>
		public void AppendThemed( UIElement element ) {
			base.Append( element );
			this.RefreshThemeForChild( element, true );
		}


		////////////////

		/// <summary>
		/// Re-applies the current theme styles (including child elements).
		/// </summary>
		public virtual void RefreshTheme() {
			foreach( UIElement elem in this.Elements ) {
				this.RefreshThemeForChild( elem, true );
			}
		}

		/// <summary>
		/// Applies the current theme's styles to a given element (presumably a child element).
		/// </summary>
		/// <param name="element"></param>
		/// <param name="recursive"></param>
		public virtual void RefreshThemeForChild( UIElement element, bool recursive ) {
			if( !this.Theme.Apply( element ) ) {
				this.Theme.ApplyByType( element );
			}

			List<UIElement> children;
			ReflectionHelpers.Get( element, "Elements", out children );

			foreach( IThemeable child in children ) {
				this.RefreshThemeForChild( element, true );
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
