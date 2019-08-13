using HamstarHelpers.Helpers.Debug;
using Microsoft.Xna.Framework;
using System;


namespace HamstarHelpers.Internals.ModTags.Base {
	abstract partial class ModTagsManager {
		public string GetInfoText() {
			return this.InfoDisplay?.GetText() ?? null;
		}

		public void SetInfoText( string text, Color? color = null ) {
			this.InfoDisplay?.SetText( text, color );
		}

		////

		public void SetInfoTextDefault( string text ) {
			this.InfoDisplay?.SetDefaultText( text );
		}


		////////////////

		public string GetSelectedCategory() {
			return this.TagsUI.CurrentCategory;
		}
	}
}
