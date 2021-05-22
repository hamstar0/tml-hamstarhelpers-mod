using HamstarHelpers.Classes.UI.Menu;
using HamstarHelpers.Libraries.Debug;
using HamstarHelpers.Libraries.TModLoader.Menus;
using HamstarHelpers.Internals.ModTags.Base.Manager;
using System;
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

		public override void Show( UIState ui ) {
			base.Show( ui );

			this.Manager.TagsUI.EnableCatTagInterface();
		}

		public override void Hide( UIState ui ) {
			base.Hide( ui );

			this.Manager.SetInfoTextDefault( "" );
		}
	}
}
