using HamstarHelpers.Classes.UI.Elements;
using HamstarHelpers.Classes.UI.Elements.Menu;
using HamstarHelpers.Classes.UI.Menus;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Helpers.TModLoader.Menus;
using HamstarHelpers.Services.Timers;
using HamstarHelpers.Services.TML;
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

		public void SetCurrentMod( string modName, ISet<string> tags ) {
			bool hasNetTags = tags.Count > 0;

			foreach( var kv in this.TagButtons ) {
				string tagName = kv.Key;
				UITagButton button = kv.Value;
				bool hasTag = tags.Contains( tagName );

				if( !hasNetTags ) {
					button.Enable();
				}

				if( tagName == "Low Effort" ) {
					if( hasTag ) {
						button.SetTagState( 1 );
					} else {
						BuildPropertiesViewer viewer = BuildPropertiesViewer.GetBuildPropertiesForActiveMod( modName );
						string desc = viewer.Description ?? "";

						if( viewer == null || string.IsNullOrEmpty( desc ) ) {
							if( !ModMenuHelpers.GetModDescriptionFromCurrentMenuUI( out desc ) ) {
								desc = "";
							}
						}

						if( desc.Contains( "Modify this file with a description of your mod." ) ) {
							button.SetTagState( 1 );
						}
					}
				} else {
					button.SetTagState( hasTag ? 1 : 0 );
				}
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

		////

		public void ResetTagButtons( bool alsoDisable ) {
			foreach( var kv in this.TagButtons ) {
				if( alsoDisable ) {
					kv.Value.Disable();
				}
				kv.Value.SetTagState( 0 );
			}
		}


		////

		public void SafelySetTagButton( string tag ) {
			var button = this.TagButtons[tag];

			if( button.TagState != 1 ) {
				if( Timers.GetTimerTickDuration( "ModHelpersTagsEditDefaults" ) <= 0 ) {
					Timers.SetTimer( "ModHelpersTagsEditDefaults", 60, () => {
						button.SetTagState( 1 );
						return false;
					} );
				}
			}
		}
	}
}
