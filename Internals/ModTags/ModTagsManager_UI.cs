using HamstarHelpers.Helpers.Debug;
using Microsoft.Xna.Framework;
using System;


namespace HamstarHelpers.Internals.ModTags {
	partial class ModTagsManager {
		public string GetInfoText() {
			return this.Context.InfoDisplay?.GetText() ?? null;
		}

		public void SetInfoText( string text, Color? color = null ) {
			this.Context.InfoDisplay?.SetText( text, color );
		}

		////

		public void SetInfoTextDefault( string text ) {
			this.Context.InfoDisplay?.SetDefaultText( text );
		}
	}
}
