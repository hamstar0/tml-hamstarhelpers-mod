using HamstarHelpers.Classes.UI.Menu;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TModLoader.Menus;
using HamstarHelpers.Internals.ModTags;
using HamstarHelpers.Internals.ModTags.UI;
using System;
using System.Collections.Generic;


namespace HamstarHelpers.Internals.Menus.ModTags {
	/// @private
	abstract partial class TagsMenuContextBase : SessionMenuContext {
		public readonly UITagsPanel Panel;



		////////////////

		protected TagsMenuContextBase( ModTagsManager manager, bool canDisableTags ) : base( true, true ) {
			this.Panel = new UITagsPanel( UITheme.Vanilla, manager, ModTagsManager.Tags, canDisableTags );
		}

		public override void OnContexualize( TModLoaderMenuDefinition menuDef, string contextName ) {
			base.OnContexualize( menuDef, contextName );

			this.Panel.ApplyMenuContext( menuDef, contextName );
		}


		////////////////

		public abstract void OnTagStateChange( UITagButton tagButton );


		public ISet<string> GetTagsWithGivenState( int state ) {
			return this.Panel.GetTagsWithGivenState( state );
		}
	}
}
