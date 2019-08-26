using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.ModTagDefinitions;
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

		public virtual IReadOnlyDictionary<string, string> MyTagCategoryDescriptions => ModTagDefinition.CategoryDescriptions;
		public virtual IReadOnlyList<ModTagDefinition> MyTags => ModTagDefinition.Tags;
		public virtual IReadOnlyDictionary<string, ModTagDefinition> MyTagMap => ModTagDefinition.TagMap;

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
			return !string.IsNullOrEmpty(this.CurrentModName)
				&& ModTagsManager.RecentTaggedMods.Contains( this.CurrentModName );
		}


		////////////////

		public ISet<string> GetTagsWithGivenState( int state, string category = null ) {
			return this.TagsUI.GetTagsWithGivenState( state, category );
		}

		////////////////

		public void OnTagButtonStateChange( string tag, int state ) {
			this.OnTagStateChange( tag, state );
			this.TagsUI.OnTagStateChangeForManager( tag, state );
		}

		public abstract void OnTagStateChange( string tag, int state );
	}
}
