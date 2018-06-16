using HamstarHelpers.UIHelpers;
using Microsoft.Xna.Framework;
using Terraria.UI;


namespace HamstarHelpers.Utilities.UI {
	[System.Obsolete( "use UIHelpers.UI.UITextArea", true )]
	public class UITextArea : UIElement {
		private UIHelpers.Elements.UITextArea TrueElement;

		public string Text { get { return this.TrueElement.Text; } }
		public string Hint { get { return this.TrueElement.Hint; } }

		public Color TextColor {
			get { return this.TrueElement.TextColor; }
			set { this.TrueElement.TextColor = value; }
		}
		public Color HintColor {
			get { return this.TrueElement.HintColor; }
			set { this.TrueElement.HintColor = value; }
		}

		public int CursorPos { get { return this.TrueElement.CursorPos; } }
		public int CursorAnimation { get { return this.TrueElement.CursorAnimation; } }

		public bool HasFocus { get { return this.TrueElement.HasFocus; } }
		public bool IsEnabled { get { return this.TrueElement.IsEnabled; } }


		////////////////

		public UITextArea( UITheme theme, string hint ) : base() {
			this.TrueElement = new UIHelpers.Elements.UITextArea( theme, hint );
			this.Append( this.TrueElement );

			CalculatedStyle dim = this.TrueElement.GetDimensions();
			this.Width.Set( dim.Width, 0f );
			this.Height.Set( dim.Height, 0f );
		}


		////////////////

		public void SetText( string text, bool allow_overflow ) {
			this.TrueElement.SetText( text );
		}

		////////////////

		public void Focus() {
			this.TrueElement.Focus();
		}

		public void Unfocus() {
			this.TrueElement.Unfocus();
		}


		////////////////

		public void Disable() {
			this.TrueElement.Disable();
		}

		public void Enable() {
			this.TrueElement.Enable();
		}
	}
}
