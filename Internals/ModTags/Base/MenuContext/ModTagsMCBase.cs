using HamstarHelpers.Classes.UI.Menu;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TModLoader.Menus;
using HamstarHelpers.Internals.ModTags.Base.UI.Buttons;
using System;
using System.Collections.Generic;
using Terraria.UI;


namespace HamstarHelpers.Internals.ModTags.Base.MenuContext {
	/// @private
	abstract partial class ModTagsMenuContextBase : SessionMenuContext {
		public ModTagsManager Manager { get; protected set; }



		////////////////

		protected ModTagsMenuContextBase( MenuUIDefinition menuDef, string contextName )
				: base( menuDef, contextName, true, true ) {
		}

		////

		public sealed override void OnActivationForSession( UIState ui ) {
			this.Manager.TagsUI.ApplyMenuContext( this.MenuDefinitionOfContext, this.ContextName );
			this.OnActivationForModTags( ui );
		}

		public abstract void OnActivationForModTags( UIState ui );


		////////////////

		public abstract void OnTagStateChange( UITagMenuButton tagButton );


		public ISet<string> GetTagsWithGivenState( int state ) {
			return this.Manager.TagsUI.GetTagsWithGivenState( state );
		}
	}
}
