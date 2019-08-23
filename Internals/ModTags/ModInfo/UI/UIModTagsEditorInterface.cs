using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Classes.UI.Elements;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Extensions;
using HamstarHelpers.Internals.ModTags.Base.UI;
using HamstarHelpers.Internals.ModTags.Base.UI.Buttons;
using HamstarHelpers.Internals.ModTags.ModInfo.UI.Buttons;
using HamstarHelpers.Internals.ModTags.ModInfo.Manager;
using HamstarHelpers.Helpers.TModLoader.Menus;
using HamstarHelpers.Services.TML;
using System;
using Terraria.UI;
using System.Collections.Generic;
using HamstarHelpers.Internals.ModTags.Base.Manager;


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

			this.LayoutTagButtonsByOnState();

			this.CheckForAndApplyLowEffortTag( modName );
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


		public void ApplyTagConstraints( string tagName ) {
			this.ApplyTagConstraintsCyclic( tagName, new HashSet<string>(), new HashSet<string>() );
		}

		public void ApplyTagConstraintsCyclic( string tagName, ISet<string> addedTags, ISet<string> removedTags ) {
			TagDefinition tagDef = this.Manager.MyTagMap[tagName];

			foreach( string forcedTag in tagDef.ForcesTags ) {
				if( addedTags.Contains(forcedTag) ) { continue; }

				addedTags.Add( forcedTag );

				this.TagButtons[forcedTag].SetTagState( 1 );
				this.ApplyTagConstraintsCyclic( forcedTag, addedTags, removedTags );
			}

			foreach( string excludedTag in tagDef.ExcludesOnAdd ) {
				if( removedTags.Contains( excludedTag ) ) { continue; }

				removedTags.Add( excludedTag );

				this.TagButtons[excludedTag].SetTagState( 0 );
				this.ValidateConstraintsForRemovedTag( excludedTag, removedTags );
			}
		}

		////

		private void ValidateConstraintsForRemovedTag( string removedTag, ISet<string> removedTags ) {
			foreach( TagDefinition tagDef in this.Manager.MyTags ) {
				if( tagDef.Tag == removedTag ) { continue; }
				if( !this.TagButtons.ContainsKey(tagDef.Tag) ) { continue; }
				if( !tagDef.ForcesTags.Contains(removedTag) ) { continue; }

				UITagMenuButton tagButton = this.TagButtons[tagDef.Tag];
				if( tagButton.TagState != 1 ) { continue; }

				tagButton.SetTagState( 0 );
				removedTags.Add( tagDef.Tag );
				this.ValidateConstraintsForRemovedTag( tagDef.Tag, removedTags );
			}
		}
	}
}
