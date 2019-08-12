using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TModLoader.Menus;
using HamstarHelpers.Helpers.TModLoader.Mods;
using HamstarHelpers.Internals.ModTags.Base.MenuContext;
using HamstarHelpers.Internals.WebRequests;
using HamstarHelpers.Services.Hooks.LoadHooks;
using HamstarHelpers.Services.UI.Menus;
using System;
using System.Collections.Generic;
using Terraria.UI;


namespace HamstarHelpers.Internals.ModTags.ModInfo.MenuContext {
	/// @private
	partial class ModInfoTagsMenuContext : ModTagsMenuContextBase<ModTagsEditorManager> {
		internal static ISet<string> RecentTaggedMods = new HashSet<string>();


		////////////////

		public static void Initialize( bool onModLoad ) {
			if( ModHelpersMod.Instance.Config.DisableModTags ) { return; }

			if( !onModLoad ) {
				var ctx = new ModInfoTagsMenuContext( MenuUIDefinition.UIModInfo, "ModHelpers: Mod Info" );
				MenuContextService.AddMenuContext( ctx );
			}
		}



		////////////////

		protected ModInfoTagsMenuContext( MenuUIDefinition menuDef, string contextName )
				: base( menuDef, contextName ) {
			UIState uiModInfo = MainMenuHelpers.GetMenuUI( menuDef );

			if( uiModInfo.GetType().Name != "UIModInfo" ) {
				throw new ModHelpersException( "UI context not UIModInfo, found " + uiModInfo.GetType().Name );
			}

			this.Manager = new ModTagsEditorManager( uiModInfo );
		}


		////////////////

		internal void UpdateMode( bool isEditing ) {
			if( !isEditing ) { return; }

			CustomLoadHooks.AddHook( GetModInfo.BadModsListLoadHookValidator, (modInfoArgs) => {
				this.ApplyDefaultEditModeTags( modInfoArgs.ModInfo );
				return false;
			} );
		}


		////////////////

		private void ApplyDefaultEditModeTags( IDictionary<string, BasicModInfo> modInfos ) {
			if( !modInfos.ContainsKey( this.Manager.CurrentModName ) ) {
				return;
			}

			var modInfo = modInfos[this.Manager.CurrentModName];
			if( modInfo.IsBadMod ) {
				this.Manager.TagsUI.SafelySetTagButton( "Misleading Info" );
			}
		}
	}
}
