using HamstarHelpers.Classes.UI.Elements;
using HamstarHelpers.Helpers.TModLoader.Menus;
using HamstarHelpers.Internals.ModTags.Base.UI.Buttons;
using HamstarHelpers.Services.TML;
using System.Collections.Generic;
using Terraria;
using System;


namespace HamstarHelpers.Internals.ModTags.Base.UI {
	abstract partial class UIModTagsInterface : UIThemedPanel {
		public void SetCurrentMod( string modName, ISet<string> tags ) {
			bool hasNetTags = tags.Count > 0;

			foreach( var kv in this.TagButtons ) {
				string tagName = kv.Key;
				UITagMenuButton button = kv.Value;
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
