using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.ModTagDefinitions;
using HamstarHelpers.Classes.UI.Elements.Menu;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Draw;
using HamstarHelpers.Helpers.TModLoader.Menus;
using HamstarHelpers.Internals.ModTags.Base.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Internals.ModTags.Base.UI.Buttons {
	/// @private
	internal class UITagMenuButton : UIMenuButton {
		public static float ButtonWidth { get; private set; } = 102f;
		public static float ButtonHeight { get; private set; } = 16f;



		////////////////

		public static IDictionary<string, UITagMenuButton> CreateButtons( UITheme theme, ModTagsManager manager ) {
			var buttons = new Dictionary<string, UITagMenuButton>();
			IReadOnlyList<ModTagDefinition> tags = manager.MyTags;

			for( int i = 0; i < tags.Count; i++ ) {
				string tag = tags[i].Tag;
				string desc = tags[i].Description;

				buttons[tag] = new UITagMenuButton( theme, manager, tag, desc, manager.CanExcludeTags );
			}

			return buttons;
		}



		////////////////

		private ModTagsManager Manager;

		public event Action<int> OnStateChange;


		////////////////

		public string Description { get; private set; }
		public int TagState { get; private set; }



		////////////////

		private UITagMenuButton( UITheme theme,
					ModTagsManager manager,
					string label,
					string desc,
					bool canNegateTags )
				: base( theme, label, UITagMenuButton.ButtonWidth, UITagMenuButton.ButtonHeight, 0, 0, 0.6f, false ) {
			this.Manager = manager;
			this.TagState = 0;
			this.DrawPanel = false;
			this.Description = desc;

			this.OnClick += ( _, __ ) => {
				if( !this.IsEnabled ) { return; }
				this.TogglePositiveTag();
			};
			this.OnRightClick += ( _, __ ) => {
				if( !this.IsEnabled || !canNegateTags ) { return; }
				this.ToggleNegativeTag();
			};
			this.OnMouseOver += ( _, __ ) => {
				if( !this.IsEnabled ) { return; }
				this.Manager.SetInfoText( desc );
				this.RefreshTheme();
			};
			this.OnMouseOut += ( _, __ ) => {
				if( this.Manager.GetInfoText() == desc ) {
					this.Manager.SetInfoText( "" );
				}
				this.RefreshTheme();
			};

			this.Disable();
			this.RecalculatePosition();
			this.RefreshTheme();
		}


		////////////////

		public void TakeOut() {
			/*this.Enable();
			this.Show();
			this.SetMenuSpacePosition( this.PositionXCenterOffset, this.PositionY );*/

			if( Main.MenuUI.CurrentState == null ) {
				LogHelpers.Warn( "No menu UI loaded." );
				return;
			}
			if( this.Parent != null ) {
				return;
			}

			string currentMenuContextUi = Main.MenuUI.CurrentState.GetType().Name;
			string expectedMenuContextUi = Enum.GetName( typeof(MenuUIDefinition), this.Manager.MenuDefinition );

			if( currentMenuContextUi == expectedMenuContextUi ) {
				Main.MenuUI.CurrentState?.Append( this );
			} else {
				LogHelpers.Warn( "Found " + currentMenuContextUi + ", expected " + expectedMenuContextUi );
			}
		}

		public void PutAway() {
			/*this.Disable();
			this.Hide();
			this.Left.Set( -(UITagMenuButton.ButtonWidth + 1), 0f );
			this.Top.Set( -(UITagMenuButton.ButtonHeight + 1), 0f );*/

			this.Remove();
		}


		////////////////

		public void SetTagState( int state ) {
			if( state < -1 || state > 1 ) { throw new ModHelpersException( "Invalid state." ); }
			if( this.TagState == state ) { return; }
			this.TagState = state;

			this.Manager.OnTagButtonStateChange( this.Text, this.TagState );
			this.RefreshTheme();

			this.OnStateChange?.Invoke( state );
		}

		public void TogglePositiveTag() {
			this.TagState = this.TagState <= 0 ? 1 : 0;

			this.Manager.OnTagButtonStateChange( this.Text, this.TagState );
			this.RefreshTheme();

			this.OnStateChange?.Invoke( this.TagState );
		}

		public void ToggleNegativeTag() {
			this.TagState = this.TagState >= 0 ? -1 : 0;

			this.Manager.OnTagButtonStateChange( this.Text, this.TagState );
			this.RefreshTheme();

			this.OnStateChange?.Invoke( this.TagState );
		}


		////////////////

		public override void RefreshTheme() {
			base.RefreshTheme();

			if( this.TagState > 0 ) {
				this.TextColor = Color.Lime;
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
			
			/*if( this.Description.Contains("Mechanics:") ) {
				bgColor = Color.Lerp( bgColor, Color.Gold, 0.3f );
			} else if( this.Description.Contains("Theme:") ) {
				bgColor = Color.Lerp( bgColor, Color.DarkTurquoise, 0.4f );
			} else if( this.Description.Contains( "Content:" ) ) {
				bgColor = Color.Lerp( bgColor, Color.Red, 0.3f );	//DarkRed
			//} else if( this.Desc.Contains( "Where:" ) ) {
			//	bgColor = Color.Lerp( bgColor, Color.Green, 0.4f );
			} else if( this.Description.Contains( "When:" ) ) {
				bgColor = Color.Lerp( bgColor, Color.Green, 0.4f );
			} else if( this.Description.Contains( "State:" ) ) {
				bgColor = Color.Lerp( bgColor, Color.DarkViolet, 0.4f );
			} else if( this.Description.Contains( "Judgmental:" ) ) {
				bgColor = Color.Lerp( bgColor, Color.DimGray, 0.4f );
			} else if( this.Description.Contains( "Priviledge:" ) ) {
				bgColor = Color.Lerp( bgColor, Color.Black, 0.4f );
			}
			bgColor.A = a;*/

			return bgColor;
		}

		public Color GetEdgeColor() {
			Color edgeColor = !this.IsEnabled ?
				this.Theme.ButtonEdgeDisabledColor :
				this.IsMouseHovering ?
					this.Theme.ButtonEdgeLitColor :
					this.Theme.ButtonEdgeColor;
			byte a = edgeColor.A;
			
			/*if( this.Description.Contains( "Mechanics:" ) ) {
				edgeColor = Color.Lerp( edgeColor, Color.Goldenrod, 0.35f );
			} else if( this.Description.Contains( "Theme:" ) ) {
				edgeColor = Color.Lerp( edgeColor, Color.Aquamarine, 0.25f );
			} else if( this.Description.Contains( "Content:" ) ) {
				edgeColor = Color.Lerp( edgeColor, Color.Red, 0.25f );
			//} else if( this.Desc.Contains( "Where:" ) ) {
			//	edgeColor = Color.Lerp( edgeColor, Color.Green, 0.25f );
			} else if( this.Description.Contains( "When:" ) ) {
				edgeColor = Color.Lerp( edgeColor, Color.Green, 0.25f );
			} else if( this.Description.Contains( "State:" ) ) {
				edgeColor = Color.Lerp( edgeColor, Color.DarkViolet, 0.4f );
			} else if( this.Description.Contains( "Judgmental:" ) ) {
				edgeColor = Color.Lerp( edgeColor, Color.DimGray, 0.4f );
			} else if( this.Description.Contains( "Priviledge:" ) ) {
				edgeColor = Color.Lerp( edgeColor, Color.Black, 0.4f );
			}
			edgeColor.A = a;*/

			return edgeColor;
		}


		////////////////

		public override void Draw( SpriteBatch sb ) {
			if( !this.IsHidden ) {
				Rectangle rect = this.GetOuterDimensions().ToRectangle();
				rect.X += 4;
				rect.Y += 4;
				rect.Width -= 4;
				rect.Height -= 5;

				DrawHelpers.DrawBorderedRect( sb, this.GetBgColor(), this.GetEdgeColor(), rect, 2 );
			}

			base.Draw( sb );
		}
	}
}
