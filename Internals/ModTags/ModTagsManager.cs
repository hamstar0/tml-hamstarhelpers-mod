using HamstarHelpers.Classes.UI.Menu;
using HamstarHelpers.Helpers.Debug;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;


namespace HamstarHelpers.Internals.ModTags {
	partial class ModTagsManager {
		private SessionMenuContext Context;


		////////////////

		public string CurrentModName { get; private set; }
		public IDictionary<string, ISet<string>> AllModTagsSnapshot { get; private set; }



		////////////////

		public ModTagsManager( SessionMenuContext menuContext ) {
			this.Context = menuContext;
		}


		////////////////

		public string GetInfoText() {
			return this.Context.InfoDisplay?.GetText() ?? null;
		}

		public void SetInfoText( string text, Color? color = null ) {
			this.Context.InfoDisplay?.SetText( text, color );
		}


		////////////////

		public void UpdateMode( bool isSubmitMode ) {
		}


		////////////////

		public bool CanEditTags() {

		}

		public bool IsCurrentModRecentlyTagged() {

		}

		////////////////

		public ISet<string> GetTagsWithGivenState( int state ) {
		}


		////////////////

		public void SubmitTags() {
		}
	}
}
