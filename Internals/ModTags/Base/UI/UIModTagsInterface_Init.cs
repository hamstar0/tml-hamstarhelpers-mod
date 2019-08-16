using HamstarHelpers.Classes.UI.Elements;
using HamstarHelpers.Classes.UI.Elements.Menu;
using HamstarHelpers.Classes.UI.Menus;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TModLoader.Menus;
using HamstarHelpers.Internals.ModTags.Base.UI.Buttons;
using HamstarHelpers.Services.UI.Menus;
using System;
using System.Collections.Generic;
using System.Linq;


namespace HamstarHelpers.Internals.ModTags.Base.UI {
	abstract partial class UIModTagsInterface : UIThemedPanel {
		private void InitializeControls() {
			this.InitializeControlButtons();
			this.InitializeCategoryButtons();
			this.InitializeTagButtons();
		}

		////

		private void InitializeControlButtons() {
			this.ResetButton = new UIResetTagsMenuButton( UITheme.Vanilla, this.Manager );
		}

		private void InitializeCategoryButtons() {
			this.CategoryButtons = UICategoryMenuButton.CreateButtons( this.Theme, this.Manager );

			this.LayoutCategoryButtons();
		}

		private void InitializeTagButtons() {
			this.TagButtons = UITagMenuButton.CreateButtons( this.Theme, this.Manager );

			this.LayoutTagButtonsByCategory();
		}


		////////////////

		private void LayoutCategoryButtons() {
			float top = this.PositionY - 2;
			float x = this.PositionXCenterOffset;
			float y = top;

			foreach( UICategoryMenuButton catButton in this.CategoryButtons.Values ) {
				catButton.SetMenuSpacePosition( x, y );

				y += UICategoryMenuButton.ButtonHeight;
				if( y >= (UIModTagsInterface.CategoryPanelHeight + top - 2) ) {
					y = top;
					x += UICategoryMenuButton.ButtonWidth - 2;
				}
			}
		}

		private void LayoutTagButtonsByCategory() {
			float x, y, top = this.PositionY + UIModTagsInterface.CategoryPanelHeight;
			TagDefinition[] tags = this.Manager.MyTags;

			IEnumerable<IGrouping<string, TagDefinition>> groups = tags.GroupBy( tagDef => tagDef.Category );

			foreach( IGrouping<string, TagDefinition> group in groups ) {
				x = this.PositionXCenterOffset;
				y = top;

				foreach( TagDefinition tagDef in group ) {
					UITagMenuButton button = this.TagButtons[ tagDef.Tag ];

					if( group.Key == this.CurrentCategory ) {
						button.Show();
					} else {
						button.Hide();
					}

					button.SetMenuSpacePosition( x, y );

					y += UITagMenuButton.ButtonHeight;
					if( y >= (UIModTagsInterface.TagsPanelHeight + top) ) {
						y = this.PositionY + UIModTagsInterface.CategoryPanelHeight;
						x += UITagMenuButton.ButtonWidth;
					}
				}
			}
		}


		////////////////

		public virtual void ApplyMenuContext( MenuUIDefinition menuDef, string baseContextName ) {
			var thisWidgetCtx = new WidgetMenuContext( menuDef, baseContextName + " Tags Panel", this, false );
			MenuContextService.AddMenuContext( thisWidgetCtx );

			int i = 0;
			foreach( UIMenuButton categoryButton in this.CategoryButtons.Values ) {
				var buttonWidgetCtx = new WidgetMenuContext( menuDef,
					baseContextName + " Tag Category " + i,
					categoryButton,
					false );
				MenuContextService.AddMenuContext( buttonWidgetCtx );
				i++;
			}

			i = 0;
			foreach( UITagMenuButton tagButton in this.TagButtons.Values ) {
				var buttonWidgetCtx = new WidgetMenuContext( menuDef,
					baseContextName + " Tag " + i,
					tagButton,
					false );
				MenuContextService.AddMenuContext( buttonWidgetCtx );
				i++;
			}

			var resetButtonWidgetCtx = new WidgetMenuContext( menuDef,
				baseContextName + " Tag Reset Button",
				this.ResetButton,
				false );
			MenuContextService.AddMenuContext( resetButtonWidgetCtx );
		}
	}
}
