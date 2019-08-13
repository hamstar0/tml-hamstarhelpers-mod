using HamstarHelpers.Classes.UI.Elements;
using HamstarHelpers.Classes.UI.Elements.Menu;
using HamstarHelpers.Classes.UI.Menus;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TModLoader.Menus;
using HamstarHelpers.Services.UI.Menus;
using System;
using System.Collections.Generic;
using System.Linq;


namespace HamstarHelpers.Internals.ModTags.Base.UI {
	abstract partial class UIModTagsInterface : UIThemedPanel {
		private void InitializeControls( bool canExcludeTags ) {
			this.InitializeControlButtons();

			TagDefinition[] tags = this.Manager.MyTags;
			float x = 0, y = 0;

			foreach( string category in new HashSet<string>( tags.Select(t => t.Category) ) ) {
				this.CategoryButtons[category] = new UIMenuButton( this.Theme,
					category,
					UIModTagsInterface.CategoryButtonWidth,
					UIModTagsInterface.CategoryButtonHeight,
					0f,
					y
				);

				y += 32f;
				if( y >= UIModTagsInterface.CategoryPanelHeight ) {
					y = 0;
					x = UIModTagsInterface.CategoryButtonWidth;
				}
			}

			for( int i = 0; i < tags.Length; i++ ) {
				string tag = tags[i].Tag;

				this.TagButtons[tag] = new UITagMenuButton( this.Theme,
					this.Manager,
					tag,
					tags[i].Description,
					canExcludeTags
				);
			}

			this.LayoutTagButtonsByCategory();
		}


		private void InitializeControlButtons() {
			this.ResetButton = new UIResetTagsMenuButton( UITheme.Vanilla, this.Manager );
		}


		////////////////

		public void LayoutTagButtonsByCategory() {
			float x, y;
			TagDefinition[] tags = this.Manager.MyTags;

			IEnumerable<IGrouping<string, TagDefinition>> groups = tags.GroupBy( tagDef => tagDef.Category );

			foreach( IGrouping<string, TagDefinition> group in groups ) {
				x = 0;
				y = 0;

				foreach( TagDefinition tagDef in group ) {
					UITagMenuButton button = this.TagButtons[tagDef.Tag];

					if( group.Key == this.SelectedCategory ) {
						button.Show();
					} else {
						button.Hide();
					}

					button.SetMenuSpacePosition( this.PositionXCenterOffset + x, y );

					y += UITagMenuButton.ButtonHeight;
					if( y >= UIModTagsInterface.TagsPanelHeight ) {
						y = 0;
						x += UITagMenuButton.ButtonWidth;
					}
				}
			}
		}


		////

		public virtual void ApplyMenuContext( MenuUIDefinition menuDef, string baseContextName ) {
			var thisWidgetCtx = new WidgetMenuContext( menuDef,
				baseContextName + " Tags Panel",
				this,
				false );

			MenuContextService.AddMenuContext( thisWidgetCtx );

			int i = 0;
			foreach( UIMenuButton button in this.CategoryButtons.Values ) {
				var buttonWidgetCtx = new WidgetMenuContext( menuDef,
					baseContextName + " Tag Category " + i,
					button,
					false );

				MenuContextService.AddMenuContext( buttonWidgetCtx );
				i++;
			}

			i = 0;
			foreach( UITagMenuButton button in this.TagButtons.Values ) {
				var buttonWidgetCtx = new WidgetMenuContext( menuDef,
					baseContextName + " Tag " + i,
					button,
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
