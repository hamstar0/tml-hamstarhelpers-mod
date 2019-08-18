using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Classes.UI.Elements;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Extensions;
using HamstarHelpers.Internals.ModTags.Base.UI;
using HamstarHelpers.Internals.ModTags.ModInfo.UI.Buttons;
using System;
using Terraria.UI;
using System.Collections.Generic;
using HamstarHelpers.Internals.ModTags.Base.UI.Buttons;
using HamstarHelpers.Services.TML;
using HamstarHelpers.Helpers.TModLoader.Menus;
using HamstarHelpers.Internals.ModTags.ModInfo.Manager;


namespace HamstarHelpers.Internals.ModTags.ModInfo.UI {
	partial class UIModTagsEditorInterface : UIModTagsInterface {
		protected UIHiddenPanel HiddenPanel;
		protected UIEditModeMenuButton FinishButton;


		////////////////

		protected ModTagsEditorManager MyManager => (ModTagsEditorManager)this.Manager;



		////////////////

		public UIModTagsEditorInterface( UITheme theme,
				ModTagsEditorManager manager,
				UIState uiModInfo )
				: base( theme, manager ) {
			this.InitializeEditorControls( uiModInfo );
		}


		////////////////

		public bool CanEditTags() {
			return this.FinishButton.Text == UIEditModeMenuButton.ModifyModeText;
		}


		////////////////

		public void SetCurrentMod( string modName, ISet<string> tags ) {
			bool hasExistingTags = tags.Count > 0;

			foreach( (string tagName, UITagMenuButton tagButton) in this.TagButtons ) {
				bool hasTag = tags.Contains( tagName );

				if( !hasExistingTags ) {
					tagButton.Enable();
				}

				if( tagName == "Low Effort" ) {
					if( hasTag ) {
						tagButton.SetTagState( 1 );
					} else {
						BuildPropertiesViewer viewer = BuildPropertiesViewer.GetBuildPropertiesForActiveMod( modName );
						string desc = viewer?.Description ?? "";

						if( viewer == null || string.IsNullOrEmpty( desc ) ) {
							if( !ModMenuHelpers.GetModDescriptionFromCurrentMenuUI( out desc ) ) {
								desc = "";
							}
						}

						if( desc == "" || desc.Contains( "Modify this file with a description of your mod." ) ) {
							tagButton.SetTagState( 1 );
						}
					}
				} else {
					tagButton.SetTagState( hasTag ? 1 : 0 );
				}
			}
		}
	}
}
