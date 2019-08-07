using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.UI.Elements.Menu;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.HUD;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;


namespace HamstarHelpers.Internals.ModTags.UI {
	/// @private
	internal class UITagButton : UIMenuButton {
		public const float ButtonWidth = 102f;
		public const float ButtonHeight = 16f;



		////////////////

		private UITagsPanel Container;


		////////////////
		
		public string Desc { get; private set; }
		public int TagState { get; private set; }



		////////////////

		public UITagButton( UITheme theme, UITagsPanel container, string label, string desc, bool canNegateTags )
				: base( theme, label, UITagButton.ButtonWidth, UITagButton.ButtonHeight, -308f, 40, 0.6f, false ) {
			this.Container = container;
			this.TagState = 0;
			this.DrawPanel = false;
			this.Desc = desc;
			
			this.OnClick += ( UIMouseEvent evt, UIElement listeningElement ) => {
				if( !this.IsEnabled ) { return; }
				this.TogglePositiveTag();
			};
			this.OnRightClick += ( UIMouseEvent evt, UIElement listeningElement ) => {
				if( !this.IsEnabled || !canNegateTags ) { return; }
				this.ToggleNegativeTag();
			};
			this.OnMouseOver += ( UIMouseEvent evt, UIElement listeningElement ) => {
				this.Container.SetInfoText( desc );
				//context.InfoDisplay?.SetText( desc );
				this.RefreshTheme();
			};
			this.OnMouseOut += ( UIMouseEvent evt, UIElement listeningElement ) => {
				if( this.Container.GetInfoText() == desc ) {
					this.Container.SetInfoText( "" );
				}
				this.RefreshTheme();
			};

			this.Disable();
			this.RecalculatePos();
			this.RefreshTheme();
		}


		////////////////

		public void SetTagState( int state ) {
			if( state < -1 || state > 1 ) { throw new ModHelpersException( "Invalid state." ); }
			if( this.TagState == state ) { return; }
			this.TagState = state;

			this.Container.OnTagStateChange( this );
			this.RefreshTheme();
		}

		public void TogglePositiveTag() {
			this.TagState = this.TagState <= 0 ? 1 : 0;

			this.Container.OnTagStateChange( this );
			this.RefreshTheme();
		}

		public void ToggleNegativeTag() {
			this.TagState = this.TagState >= 0 ? -1 : 0;

			this.Container.OnTagStateChange( this );
			this.RefreshTheme();
		}


		////////////////

		public override void RefreshTheme() {
			base.RefreshTheme();

			if( this.TagState > 0 ) {
				this.TextColor = Color.LimeGreen;
			} else if( this.TagState < 0 ) {
				this.TextColor = Color.Red;
			}
		}


		////////////////

		public Color GetBgColor() {
			Color bgColor = !this.IsEnabled ?
				this.Theme.ButtonBgDisabledColor :
				this.IsMouseHovering ?
					this.Theme.ButtonBgLitColor :
					this.Theme.ButtonBgColor;
			byte a = bgColor.A;
			
			if( this.Desc.Contains("Mechanics:") ) {
				bgColor = Color.Lerp( bgColor, Color.Gold, 0.3f );
			} else if( this.Desc.Contains("Theme:") ) {
				bgColor = Color.Lerp( bgColor, Color.DarkTurquoise, 0.4f );
			} else if( this.Desc.Contains( "Content:" ) ) {
				bgColor = Color.Lerp( bgColor, Color.Red, 0.3f );	//DarkRed
			//} else if( this.Desc.Contains( "Where:" ) ) {
			//	bgColor = Color.Lerp( bgColor, Color.Green, 0.4f );
			} else if( this.Desc.Contains( "When:" ) ) {
				bgColor = Color.Lerp( bgColor, Color.Green, 0.4f );
			} else if( this.Desc.Contains( "State:" ) ) {
				bgColor = Color.Lerp( bgColor, Color.DarkViolet, 0.4f );
			} else if( this.Desc.Contains( "Judgmental:" ) ) {
				bgColor = Color.Lerp( bgColor, Color.DimGray, 0.4f );
			} else if( this.Desc.Contains( "Priviledge:" ) ) {
				bgColor = Color.Lerp( bgColor, Color.Black, 0.4f );
			}
			bgColor.A = a;

			return bgColor;
		}

		public Color GetEdgeColor() {
			Color edgeColor = !this.IsEnabled ?
				this.Theme.ButtonEdgeDisabledColor :
				this.IsMouseHovering ?
					this.Theme.ButtonEdgeLitColor :
					this.Theme.ButtonEdgeColor;
			byte a = edgeColor.A;
			
			if( this.Desc.Contains( "Mechanics:" ) ) {
				edgeColor = Color.Lerp( edgeColor, Color.Goldenrod, 0.35f );
			} else if( this.Desc.Contains( "Theme:" ) ) {
				edgeColor = Color.Lerp( edgeColor, Color.Aquamarine, 0.25f );
			} else if( this.Desc.Contains( "Content:" ) ) {
				edgeColor = Color.Lerp( edgeColor, Color.Red, 0.25f );
			//} else if( this.Desc.Contains( "Where:" ) ) {
			//	edgeColor = Color.Lerp( edgeColor, Color.Green, 0.25f );
			} else if( this.Desc.Contains( "When:" ) ) {
				edgeColor = Color.Lerp( edgeColor, Color.Green, 0.25f );
			} else if( this.Desc.Contains( "State:" ) ) {
				edgeColor = Color.Lerp( edgeColor, Color.DarkViolet, 0.4f );
			} else if( this.Desc.Contains( "Judgmental:" ) ) {
				edgeColor = Color.Lerp( edgeColor, Color.DimGray, 0.4f );
			} else if( this.Desc.Contains( "Priviledge:" ) ) {
				edgeColor = Color.Lerp( edgeColor, Color.Black, 0.4f );
			}
			edgeColor.A = a;

			return edgeColor;
		}


		////////////////

		public override void Draw( SpriteBatch sb ) {
			Rectangle rect = this.GetOuterDimensions().ToRectangle();
			rect.X += 4;
			rect.Y += 4;
			rect.Width -= 4;
			rect.Height -= 5;

			HUDHelpers.DrawBorderedRect( sb, this.GetBgColor(), this.GetEdgeColor(), rect, 2 );

			base.Draw( sb );
		}
	}
}
