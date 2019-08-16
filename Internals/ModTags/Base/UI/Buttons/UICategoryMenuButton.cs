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

			base.Draw( sb );
		}
	}
}
