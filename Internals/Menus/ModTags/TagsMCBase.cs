using HamstarHelpers.Classes.UI.Menu;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Internals.Menus.ModTags.UI;
using System;
using System.Collections.Generic;


namespace HamstarHelpers.Internals.Menus.ModTags {
	/// @private
	abstract partial class TagsMenuContextBase : SessionMenuContext {
		public readonly UITagsPanel Panel;



		////////////////

		protected TagsMenuContextBase( bool canDisableTags ) : base( true, true ) {
			this.Panel = new UITagsPanel( UITheme.Vanilla, this, ModTagsManager.Tags, canDisableTags );
		}

		public override void OnContexualize( string uiClassName, string contextName ) {
			base.OnContexualize( uiClassName, contextName );

			this.Panel.ApplyMenuContext( uiClassName, contextName );
		}


		////////////////

		public abstract void OnTagStateChange( UITagButton tagButton );


		public ISet<string> GetTagsWithGivenState( int state ) {
			return this.Panel.GetTagsWithGivenState( state );
		}
	}
}
