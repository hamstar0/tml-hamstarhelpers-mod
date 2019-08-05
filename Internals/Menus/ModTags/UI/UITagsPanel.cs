using HamstarHelpers.Classes.UI.Elements;
using HamstarHelpers.Classes.UI.Elements.Menu;
using HamstarHelpers.Classes.UI.Menus;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Services.UI.Menus;
using System;
using System.Collections.Generic;
using System.Linq;


namespace HamstarHelpers.Internals.Menus.ModTags.UI {
	class UITagsPanel : UIThemedPanel {
		private readonly IDictionary<string, UIMenuButton> CategoryButtons = new Dictionary<string, UIMenuButton>();
		private readonly IDictionary<string, UITagButton> TagButtons = new Dictionary<string, UITagButton>();



		////////////////

		public UITagsPanel( UITheme theme, TagsMenuContextBase context, TagDefinition[] tags, bool canDisableTags ) : base( theme ) {
			float y = 0;

			foreach( string category in new HashSet<string>( tags.Select(t=>t.Category) ) ) {
				this.CategoryButtons[category] = new UIMenuButton( theme, category, 160f, 32f, 0f, y );
				y += 32;
			}

			for( int i = 0; i < tags.Length; i++ ) {
				string tag = tags[i].Tag;

				this.TagButtons[tag] = new UITagButton( this.Theme, context, i, tag, tags[i].Description, canDisableTags );
			}
		}

		////

		public void ApplyMenuContext( string uiClassName, string contextName ) {
			int i = 0;

			foreach( UITagButton button in this.TagButtons.Values ) {
				var buttonWidgetCtx = new WidgetMenuContext( button, false );

				MenuContextService.AddMenuContext( uiClassName, contextName + " Tag " + i, buttonWidgetCtx );
				i++;
			}
		}


		////////////////

		public ISet<string> GetTagsWithGivenState( int state ) {
			ISet<string> tags = new HashSet<string>();

			foreach( var kv in this.TagButtons ) {
				if( kv.Value.TagState == state ) {
					tags.Add( kv.Key );
				}
			}
			return tags;
		}


		////////////////

		public void EnableTagButtons() {
			foreach( var kv in this.TagButtons ) {
				kv.Value.Enable();
			}
		}

		public void DisableTagButtons() {
			foreach( var kv in this.TagButtons ) {
				kv.Value.Disable();
			}
		}

		////////////////

		public void ResetTagButtons() {
			foreach( var kv in this.TagButtons ) {
				kv.Value.SetTagState( 0 );
			}
		}
	}
}
