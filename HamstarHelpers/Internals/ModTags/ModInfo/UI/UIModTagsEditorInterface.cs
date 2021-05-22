using System;
using Terraria.UI;
using System.Collections.Generic;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Classes.UI.Elements;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Libraries.Debug;
using HamstarHelpers.Libraries.DotNET.Extensions;
using HamstarHelpers.Libraries.TModLoader.Menus;
using HamstarHelpers.Internals.ModTags.Base.UI;
using HamstarHelpers.Internals.ModTags.Base.UI.Buttons;
using HamstarHelpers.Internals.ModTags.ModInfo.UI.Buttons;
using HamstarHelpers.Internals.ModTags.ModInfo.Manager;
using HamstarHelpers.Services.TML;


namespace HamstarHelpers.Internals.ModTags.ModInfo.UI {
	partial class UIModTagsEditorInterface : UIModTagsInterface {
		protected UIHiddenPanel HiddenPanel;
		protected UIEditModeMenuButton EditButton;


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

		public void SetTagsForCurrentMod( string modName, ISet<string> tags ) {
			bool hasExistingTags = tags.Count > 0;

			foreach( (string tagName, UITagMenuButton tagButton) in this.TagButtons ) {
				bool hasTag = tags.Contains( tagName );

				if( !hasExistingTags ) {
					tagButton.Enable();
				}

				if( !hasTag ) {
					tagButton.SetTagState( 0 );
				} else {
					tagButton.SetTagState( 1 );
				}
			}

			this.CheckForAndApplyLowEffortTag( modName );

			this.LayoutTagButtonsByOnState();

			if( hasExistingTags ) {
				this.SetReadOnlyMode( true );
			}
		}


		private void CheckForAndApplyLowEffortTag( string modName ) {
			UITagMenuButton tagButton;
			if( !this.TagButtons.TryGetValue( "Low Effort", out tagButton ) ) {
				return;
			}
			if( tagButton.TagState == 1 ) {
				return;
			}

			BuildPropertiesViewer viewer = BuildPropertiesViewer.GetBuildPropertiesForActiveMod( modName );
			string desc = viewer?.Description;

			if( viewer == null || string.IsNullOrEmpty( desc ) ) {
				if( !ModMenuLibraries.GetModDescriptionFromCurrentMenuUI( out desc ) ) {
					LogLibraries.WarnOnce( "Failed for "+modName+": "+desc );
					return;
				}
			}

			if( desc == "" || desc.Contains( "Modify this file with a description of your mod." ) ) {
			//	tagButton.SetTagState( 1 );
				// TODO
			}
		}
	}
}
