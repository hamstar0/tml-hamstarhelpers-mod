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
	abstract partial class UIModTags : UIThemedPanel {
		private void InitializeControls( TagDefinition[] tags, bool canExcludeTags ) {
			this.InitializeControlButtons();

			float y = 0;

			foreach( string category in new HashSet<string>( tags.Select( t => t.Category ) ) ) {
				this.CategoryButtons[category] = new UIMenuButton( this.Theme, category, 160f, 32f, 0f, y );
				y += 32;
			}

			for( int i = 0; i < tags.Length; i++ ) {
				string tag = tags[i].Tag;

				this.TagButtons[tag] = new UITagButton( this.Theme, this.Manager, tag, tags[i].Description, canExcludeTags );
			}
		}


		private void InitializeControlButtons() {
			this.ResetButton = new UITagResetButton( UITheme.Vanilla, this.Manager );
		}


		////

		public virtual void ApplyMenuContext( MenuUIDefinition menuDef, string baseContextName ) {
			int i = 0;

			foreach( UITagButton button in this.TagButtons.Values ) {
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
