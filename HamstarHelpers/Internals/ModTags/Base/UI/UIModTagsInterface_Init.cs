using HamstarHelpers.Classes.UI.Elements;
using HamstarHelpers.Classes.UI.Elements.Menu;
using HamstarHelpers.Classes.UI.Menus;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Libraries.Debug;
using HamstarHelpers.Libraries.TModLoader.Menus;
using HamstarHelpers.Internals.ModTags.Base.UI.Buttons;
using HamstarHelpers.Services.UI.Menus;
using Microsoft.Xna.Framework;
using System;


namespace HamstarHelpers.Internals.ModTags.Base.UI {
	abstract partial class UIModTagsInterface : UIThemedPanel {
		private void InitializeControls() {
			this.InitializeControlButtons();
			this.InitializeCategoryButtons();
			this.InitializeTagButtons();
		}

		////

		private void InitializeControlButtons() {
			Vector2 pos = this.GetTagControlsTopLeftPositionOffset();

			this.ResetButton = new UIResetTagsMenuButton( UITheme.Vanilla,
				this.Manager,
				pos.X,
				pos.Y
			);
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
