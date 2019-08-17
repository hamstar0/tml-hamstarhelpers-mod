using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.UI.Elements.Menu;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.HUD;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;


namespace HamstarHelpers.Internals.ModTags.Base.UI.Buttons {
	/// @private
	internal class UICategoryMenuButton : UIMenuButton {
		public static IDictionary<string, UICategoryMenuButton> CreateButtons( UITheme theme, ModTagsManager manager ) {
			IDictionary<string, UICategoryMenuButton> buttons = null;

			buttons = manager.MyTagCategoryDescriptions.ToDictionary( kv => kv.Key, kv => {
				string category = kv.Key;
				string desc = kv.Value;
				var button = new UICategoryMenuButton( theme, manager, category, desc );

				return button;
			} );

			return buttons;
		}



		////////////////

		public static float ButtonWidth { get; private set; } = 102f;
		public static float ButtonHeight { get; private set; } = 16f;



		////////////////

		private ModTagsManager Manager;

		private UIText PositiveTagCount;
		private UIText NegativeTagCount;


		////////////////

		public event MouseEvent OnSelect;
		public event MouseEvent OnUnselect;


		////////////////

		public string Description { get; private set; }
		public bool IsSelected { get; private set; }



		////////////////

		private UICategoryMenuButton( UITheme theme,
					ModTagsManager manager,
					string category,
					string description )
				: base( theme,
					category,
					UICategoryMenuButton.ButtonWidth,
					UICategoryMenuButton.ButtonHeight,
					0,
					0,
					0.6f,
					false ) {
			this.Manager = manager;
			this.IsSelected = false;
			this.DrawPanel = false;
			this.Description = description;

			this.PositiveTagCount = new UIText( "+0", 0.7f );
			this.PositiveTagCount.Left.Set( 0f, 0f );
			this.PositiveTagCount.TextColor = Color.Green;
			this.Append( this.PositiveTagCount );

			if( this.Manager.CanExcludeTags ) {
				this.NegativeTagCount = new UIText( "-0", 0.7f );
				this.NegativeTagCount.Left.Set( -12f, 1f );
				this.NegativeTagCount.TextColor = Color.Red;
				this.Append( this.NegativeTagCount );
			}

			this.OnClick += ( _, __ ) => {
				if( !this.IsEnabled ) { return; }
				this.Select();
			};
			this.OnMouseOver += ( _, __ ) => {
				this.Manager.SetInfoText( description );
				//context.InfoDisplay?.SetText( desc );
				this.RefreshTheme();
			};
			this.OnMouseOut += ( _, __ ) => {
				if( this.Manager.GetInfoText() == description ) {
					this.Manager.SetInfoText( "" );
				}
				this.RefreshTheme();
			};

			this.RecalculatePosition();
			this.RefreshTheme();
		}


		////////////////

		public void Select() {
			if( this.IsSelected ) { return; }
			this.IsSelected = true;

			this.Manager.TagsUI.SetCategory( this.Text );

			this.OnSelect?.Invoke(
				new UIMouseEvent( this, new Vector2(Main.mouseX, Main.mouseY) ),
				this
			);

			this.RefreshTheme();
		}
		
		public void Unselect() {
			if( !this.IsSelected ) { return; }
			this.IsSelected = false;
			
			this.OnUnselect?.Invoke(
				new UIMouseEvent( this, new Vector2(Main.mouseX, Main.mouseY) ),
				this
			);

			this.RefreshTheme();
		}


		////////////////

		public override void RefreshTheme() {
			base.RefreshTheme();

			if( !this.IsSelected ) {
				this.TextColor = Color.White;
			} else {
				this.TextColor = Color.Lime;
			}
		}


		////////////////

		public override void Draw( SpriteBatch sb ) {
			if( !this.IsHidden ) {
				Rectangle rect = this.GetOuterDimensions().ToRectangle();
				rect.X += 4;
				rect.Y += 4;
				rect.Width -= 4;
				rect.Height -= 5;

				Color bgColor = this.IsMouseHovering ? this.Theme.ButtonBgLitColor : this.Theme.ButtonBgColor;
				Color edgeColor = this.IsMouseHovering ? this.Theme.ButtonEdgeLitColor : this.Theme.ButtonEdgeColor;

				HUDHelpers.DrawBorderedRect( sb, bgColor, edgeColor, rect, 2 );
			}

			this.UpdateTagCounts();

			base.Draw( sb );
		}


		////

		public void UpdateTagCounts() {
			int positives = this.Manager.GetTagsWithGivenState( 1, this.Text ).Count;

			this.PositiveTagCount.SetText( "+" + positives );
			if( positives == 0 ) {
				this.PositiveTagCount.TextColor = Color.Green * 0.65f;
			} else {
				this.PositiveTagCount.TextColor = Color.Green;
			}

			if( this.Manager.CanExcludeTags ) {
				int negatives = this.Manager.GetTagsWithGivenState( -1, this.Text ).Count;

				this.NegativeTagCount.SetText( "-" + negatives );
				if( negatives == 0 ) {
					this.PositiveTagCount.TextColor = Color.Red * 0.65f;
				} else {
					this.PositiveTagCount.TextColor = Color.Red;
				}
			}
		}
	}
}
