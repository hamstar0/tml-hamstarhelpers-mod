﻿using HamstarHelpers.Helpers.DebugHelpers;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;


namespace HamstarHelpers.Components.UI.Elements {
	public class UITextPanelButton : UITextPanel<string> {
		private readonly UITheme Theme;

		public bool IsEnabled { get; private set; }

		//public string HoverText = "";


		////////////////

		public UITextPanelButton( UITheme theme, string label, float scale = 1f, bool large = false ) : base( label, scale, large ) {
			this.Theme = theme;
			this.IsEnabled = true;

			this.SetPadding( 5f );

			var self = this;

			theme.ApplyButton( this );
			this.OnMouseOver += delegate ( UIMouseEvent evt, UIElement from_elem ) {
				if( !self.IsEnabled ) { return; }
				theme.ApplyButtonLit( self );
			};
			this.OnMouseOut += delegate ( UIMouseEvent evt, UIElement from_elem ) {
				if( !self.IsEnabled ) { return; }
				theme.ApplyButton( self );
			};

			this.RefreshTheme();
		}


		////////////////

		public void Enable() {
			this.IsEnabled = true;
			this.RefreshTheme();
		}

		public void Disable() {
			this.IsEnabled = false;
			this.RefreshTheme();
		}


		////////////////

		public virtual void RefreshTheme() {
			if( this.IsEnabled ) {
				this.Theme.ApplyButton( this );
			} else {
				this.Theme.ApplyButtonDisable( this );
			}
		}


		////////////////

		/*private void DrawHoverTextIfHover( GameTime game_time ) {
			if( string.IsNullOrEmpty( this.HoverText ) ) { return; }

			if( this.GetOuterDimensions().ToRectangle().Contains( Main.mouseX, Main.mouseY ) ) {
				var pos = new Vector2( Main.mouseX, Main.mouseY );

				Main.spriteBatch.DrawString( Main.fontMouseText, this.HoverText, pos, Color.White );
			}
		}*/
	}
}
