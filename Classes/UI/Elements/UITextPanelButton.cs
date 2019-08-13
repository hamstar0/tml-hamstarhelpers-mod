using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Helpers.Debug;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;


namespace HamstarHelpers.Classes.UI.Elements {
	/// <summary>
	/// Defines a common text panel button element.
	/// </summary>
	public class UITextPanelButton : UIThemedTextPanel {
		/// <summary>
		/// Indicates if button is visible. Does not effect interactivity (use Enable and Disable).
		/// </summary>
		public bool IsHidden { get; private set; }

		/// <summary>
		/// Indicates if button accepts inputs.
		/// </summary>
		public bool IsEnabled { get; private set; }

		//public string HoverText = "";


		////////////////

		/// <param name="theme">Appearance style.</param>
		/// <param name="label">Button's label.</param>
		/// <param name="scale">Multiplier of label text size.</param>
		/// <param name="large">Sets label text 'large'.</param>
		public UITextPanelButton( UITheme theme, string label, float scale = 1f, bool large = false )
				: base( theme, true, label, scale, large ) {
			this.Theme = theme;
			this.IsEnabled = true;

			this.SetPadding( 5f );

			theme.ApplyButton( this );

			var self = this;
			this.OnMouseOver += delegate ( UIMouseEvent evt, UIElement fromElem ) {
				if( !self.IsEnabled ) { return; }
				theme.ApplyButtonLit( self );
			};
			this.OnMouseOut += delegate ( UIMouseEvent evt, UIElement fromElem ) {
				if( !self.IsEnabled ) { return; }
				theme.ApplyButton( self );
			};

			this.RefreshTheme();
		}


		////////////////

		/// <summary>
		/// Enables the button.
		/// </summary>
		public void Enable() {
			this.IsEnabled = true;
			this.RefreshTheme();
		}

		/// <summary>
		/// Disables the button.
		/// </summary>
		public void Disable() {
			this.IsEnabled = false;
			this.RefreshTheme();
		}


		////////////////

		/// <summary>
		/// Refreshes visual theming.
		/// </summary>
		public override void RefreshTheme() {
			if( this.IsEnabled ) {
				this.Theme.ApplyButton( this );
			} else {
				this.Theme.ApplyButtonDisable( this );
			}
		}


		////////////////

		public virtual void Show() {
			this.IsHidden = false;
		}

		public virtual void Hide() {
			this.IsHidden = true;
		}

		////////////////

		public override void Draw( SpriteBatch spriteBatch ) {
			if( !this.IsHidden ) {
				base.Draw( spriteBatch );
			}
		}


		////////////////

		/*private void DrawHoverTextIfHover( GameTime gameTime ) {
			if( string.IsNullOrEmpty( this.HoverText ) ) { return; }

			if( this.GetOuterDimensions().ToRectangle().Contains( Main.mouseX, Main.mouseY ) ) {
				var pos = new Vector2( Main.mouseX, Main.mouseY );

				Main.spriteBatch.DrawString( Main.fontMouseText, this.HoverText, pos, Color.White );
			}
		}*/
	}
}
