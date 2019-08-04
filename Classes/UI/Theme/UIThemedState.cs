using System;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;


namespace HamstarHelpers.Components.UI.Theme {
	/// <summary>
	/// Theme-able UIState.
	/// </summary>
	public class UIThemedState : UIState, IThemeable {
		public UITheme Theme { get; protected set; }



		////////////////

		public UIThemedState( UITheme theme ) : base() {
			this.Theme = theme;

			this.RefreshTheme();
		}


		////////////////

		public void AppendThemed( UIElement element ) {
			base.Append( element );
			this.RefreshThemeForChild( element );
		}


		////////////////

		public virtual void RefreshTheme() {
			foreach( UIElement elem in this.Elements ) {
				this.RefreshThemeForChild( elem );
			}
		}

		public virtual void RefreshThemeForChild( UIElement element ) {
			if( !this.Theme.Apply( element ) ) {
				if( element is UIPanel ) {
					this.Theme.ApplyPanel( (UIPanel)element );
				}
			}
		}

		////////////////

		public virtual void SetTheme( UITheme theme ) {
			this.Theme = theme;
			this.RefreshTheme();
		}
	}
}
