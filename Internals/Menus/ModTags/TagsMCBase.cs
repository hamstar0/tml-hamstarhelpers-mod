﻿using HamstarHelpers.Classes.UI.Menu;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TModLoader.Menus;
using HamstarHelpers.Internals.ModTags;
using HamstarHelpers.Internals.ModTags.UI;
using System;
using System.Collections.Generic;
using Terraria.UI;


namespace HamstarHelpers.Internals.Menus.ModTags {
	/// @private
	abstract partial class TagsMenuContextBase : SessionMenuContext {
		public readonly ModTagsManager Manager;



		////////////////

		protected TagsMenuContextBase( MenuUIDefinition menuDef,
				string contextName,
				bool canExcludeTags )
				: base( menuDef, contextName, true, true ) {
			UIState uiContext = MainMenuHelpers.GetMenuUI( menuDef );

			this.Manager = new ModTagsManager( uiContext, canExcludeTags );
		}


		public sealed override void OnSessionContextualize() {
			this.Manager.TagsUI.ApplyMenuContext( this.MenuDefinitionOfContext, this.ContextName );
		}


		////////////////

		public abstract void OnTagStateChange( UITagButton tagButton );


		public ISet<string> GetTagsWithGivenState( int state ) {
			return this.Manager.TagsUI.GetTagsWithGivenState( state );
		}
	}
}
