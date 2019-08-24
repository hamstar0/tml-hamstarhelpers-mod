using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Internals.ModTags.Base.UI;
using System;
using HamstarHelpers.Internals.ModTags.Base.Manager;
using System.Collections.Generic;


namespace HamstarHelpers.Internals.ModTags.ModInfo.UI {
	partial class UIModTagsEditorInterface : UIModTagsInterface {
		public void SetTag( string tagName ) {
			if( this.TagButtons.ContainsKey( tagName ) ) {
				if( this.TagButtons[tagName].TagState == 0 ) {
					this.TagButtons[tagName].SetTagState( 1 );
					this.ApplyTagsByConstraintsOfTag( tagName );
				}
			}
		}


		public void RemoveTag( string tagName ) {
			if( this.TagButtons.ContainsKey( tagName ) ) {
				if( this.TagButtons[tagName].TagState == 1 ) {
					this.TagButtons[tagName].SetTagState( 0 );
					this.UnapplyTagsByConstraintsOfTag( tagName );
				}
			}
		}


		////////////////

		public void ApplyTagsByConstraintsOfTag( string tagName ) {
			TagDefinition tagDef = this.Manager.MyTagMap[tagName];

			foreach( string forcedTag in tagDef.ForcesTags ) {
				if( forcedTag == tagName ) { continue; }
				this.SetTag( forcedTag );
			}

			foreach( string excludedTag in tagDef.ExcludesOnAdd ) {
				if( excludedTag == tagName ) { continue; }
				this.RemoveTag( excludedTag );
			}
		}


		public void UnapplyTagsByConstraintsOfTag( string tagName ) {
			ISet<string> forcingTags = this.Manager.GetTagsForcingGivenTag( tagName );

			foreach( string forcingTag in forcingTags ) {
				if( forcingTag == tagName ) {
					continue;
				}

				this.RemoveTag( forcingTag );
			}
		}
	}
}
