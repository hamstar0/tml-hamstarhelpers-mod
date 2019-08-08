using HamstarHelpers.Classes.UI.Elements;
using HamstarHelpers.Classes.UI.Elements.Menu;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Helpers.TModLoader.Menus;
using HamstarHelpers.Services.TML;
using System;
using System.Collections.Generic;
using Terraria.UI;


namespace HamstarHelpers.Internals.ModTags.UI {
	partial class UITagsPanel : UIThemedPanel {
		private readonly IDictionary<string, UIMenuButton> CategoryButtons = new Dictionary<string, UIMenuButton>();
		private readonly IDictionary<string, UITagButton> TagButtons = new Dictionary<string, UITagButton>();

		////////////////

		private ModTagsManager Manager;

		private UIHiddenPanel HiddenPanel;
		private UITagFinishButton FinishButton;
		private UITagResetButton ResetButton;



		////////////////

		public UITagsPanel( UIState uiContext, UITheme theme, ModTagsManager manager, TagDefinition[] tags, bool canDisableTags ) : base( theme ) {
			this.Manager = manager;

			this.InitializeControls( uiContext, tags, canDisableTags );
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
	}
}
