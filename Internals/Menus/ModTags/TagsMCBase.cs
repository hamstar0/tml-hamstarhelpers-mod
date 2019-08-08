using HamstarHelpers.Classes.UI.Menu;
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



		////////////////

		protected TagsMenuContextBase( bool canDisableTags ) : base( true, true ) {
			this.Manager = new ModTagsManager( this, canDisableTags );
		}

		public override void OnContexualize( MenuUIDefinition menuDef, string contextName ) {
			base.OnContexualize( menuDef, contextName );

			this.Manager.TagsUI.ApplyMenuContext( menuDef, contextName );
		}


		////////////////

		public abstract void OnTagStateChange( UITagButton tagButton );


		public ISet<string> GetTagsWithGivenState( int state ) {
			return this.Manager.TagsUI.GetTagsWithGivenState( state );
		}
	}
}
