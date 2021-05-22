using HamstarHelpers.Classes.UI.Elements.Menu;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Libraries.Debug;
using Microsoft.Xna.Framework;
using Terraria.GameContent.UI.Elements;


namespace HamstarHelpers.Classes.UI.Menu.UI {
	/// <summary>
	/// An information display panel specialized for menu contexts.
	/// </summary>
	public class UIInfoDisplay : UIMenuPanel {
		private readonly UIText TextElem;

		private string DefaultText = "";
		private readonly Color DefaultColor = new Color( 128, 128, 128 );



		////////////////

		/// <summary></summary>
		public UIInfoDisplay() : base( UITheme.Vanilla, 800f, 40f, -400f, 2f ) {
			this.TextElem = new UIText( "" );
			this.TextElem.Width.Set( 0f, 1f );
			this.TextElem.Height.Set( 0f, 1f );
			this.Append( this.TextElem );

			//this.RefreshTheme();
			this.Recalculate();
		}


		////////////////

		/// <summary></summary>
		/// <param name="text"></param>
		public void SetDefaultText( string text ) {
			this.DefaultText = text;

			// If the field is empty or already as a default, set to our new default
			if( this.TextElem.Text == "" || this.TextElem.TextColor == this.DefaultColor ) {
				this.SetText( "" );
			}
		}

		////////////////

		/// <summary></summary>
		/// <param name="text"></param>
		/// <param name="color"></param>
		public void SetText( string text, Color? color=null ) {
			if( string.IsNullOrEmpty(text) ) {
				text = this.DefaultText;
				color = this.DefaultColor;
			}

			this.TextElem.TextColor = color ?? Color.White;
			this.TextElem.SetText( text );
		}

		/// <summary></summary>
		/// <returns></returns>
		public string GetText() {
			return this.TextElem.Text;
		}


		////////////////

		/*public override void Draw( SpriteBatch sb ) {
			Rectangle rect = this.GetOuterDimensions().ToRectangle();
			rect.X += 4;
			rect.Y += 4;
			rect.Width -= 4;
			rect.Height -= 5;

			HudHelpers.DrawBorderedRect( sb, this.GetBgColor(), this.GetEdgeColor(), rect, 2 );

			base.Draw( sb );
		}*/
	}
}
