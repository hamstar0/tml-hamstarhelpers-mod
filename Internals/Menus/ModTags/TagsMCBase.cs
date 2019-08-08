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
		public readonly ModTagsManager Manager;
		public readonly UITagsPanel Panel;



		////////////////

		protected TagsMenuContextBase( bool canDisableTags ) : base( true, true ) {
			this.Manager = new ModTagsManager( this );
			this.Panel = new UITagsPanel( UITheme.Vanilla, this.Manager, ModTagsManager.Tags, canDisableTags );
		}

		public override void OnContexualize( MenuUIDefinition menuDef, string contextName ) {
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
