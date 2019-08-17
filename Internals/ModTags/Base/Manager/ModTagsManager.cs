using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.UI.Menu.UI;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TModLoader.Menus;
using HamstarHelpers.Internals.ModTags.Base.UI;
using System;
using System.Collections.Generic;


namespace HamstarHelpers.Internals.ModTags.Base.Manager {
	abstract partial class ModTagsManager {
		protected static ISet<string> RecentTaggedMods = new HashSet<string>();

		private UIInfoDisplay InfoDisplay;



		////////////////

		public MenuUIDefinition MenuDefinition { get; private set; }
		public UIModTagsInterface TagsUI { get; protected set; }

		public virtual IDictionary<string, string> MyTagCategoryDescriptions => ModTagsManager.CategoryDescriptions;
		public virtual TagDefinition[] MyTags => ModTagsManager.Tags;
		public virtual IDictionary<string, TagDefinition> MyTagMap => ModTagsManager.TagMap;

		public string CurrentModName { get; protected set; }
		public bool CanExcludeTags { get; private set; }

		public IDictionary<string, ISet<string>> AllModTagsSnapshot { get; protected set; }



		////////////////

		protected ModTagsManager( UIInfoDisplay infoDisplay, MenuUIDefinition menuDef, bool canExcludeTags ) {
			this.MenuDefinition = menuDef;
			this.InfoDisplay = infoDisplay;
			this.CanExcludeTags = canExcludeTags;
		}


		////////////////

		public bool IsCurrentModRecentlyTagged() {
			return ModTagsManager.RecentTaggedMods.Contains( this.CurrentModName );
		}


		////////////////

		public virtual bool CanEditTags() {
			return false;
		}


		////////////////

		public ISet<string> GetTagsWithGivenState( int state, string category = null ) {
			return this.TagsUI.GetTagsWithGivenState( state, category );
		}

		////////////////

		public void SetTagState( string tag, int state ) {
			this.OnSetTagState( tag, state );
			this.TagsUI.OnTagStateChange( tag, state );
		}

		public abstract void OnSetTagState( string tag, int state );
	}
}
