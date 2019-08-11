using HamstarHelpers.Classes.UI.Menu;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TModLoader.Menus;
using HamstarHelpers.Internals.ModTags.Base.UI;
using System;
using System.Collections.Generic;
using Terraria.UI;


namespace HamstarHelpers.Internals.ModTags.Base.MenuContext {
	/// @private
	abstract partial class ModTagsMenuContextBase : SessionMenuContext {
		public ModTagsManager Manager { get; protected set; }



		////////////////

		protected ModTagsMenuContextBase( MenuUIDefinition menuDef,
				string contextName,
				bool canExcludeTags )
				: base( menuDef, contextName, true, true ) {
			UIState uiContext = MainMenuHelpers.GetMenuUI( menuDef );
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
