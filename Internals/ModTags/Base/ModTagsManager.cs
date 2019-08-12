using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Internals.ModTags.Base.UI;
using System;
using System.Collections.Generic;


namespace HamstarHelpers.Internals.ModTags.Base {
	abstract partial class ModTagsManager {
		protected static ISet<string> RecentTaggedMods = new HashSet<string>();



		////////////////

		public virtual TagDefinition[] MyTags => ModTagsManager.Tags;
		public UIModTags<ModTagsManager> TagsUI { get; protected set; }
		public string CurrentModName { get; protected set; }
		public bool CanExcludeTags { get; private set; }
		public IDictionary<string, ISet<string>> AllModTagsSnapshot { get; protected set; }



		////////////////

		public ModTagsManager( bool canExcludeTags ) {
			this.CanExcludeTags = canExcludeTags;
			//this.TagsUI = new UIModTagsPanel( UITheme.Vanilla, this, uiContext, this.CanExcludeTags );
		}


		////////////////

		public bool IsCurrentModRecentlyTagged() {
			return ModTagsManager.RecentTaggedMods.Contains( this.CurrentModName );
		}

		////////////////

		public ISet<string> GetTagsWithGivenState( int state ) {
			return this.TagsUI.GetTagsWithGivenState( state );
		}


		////////////////

		public virtual bool CanEditTags() {
			return false;
		}
	}
}
