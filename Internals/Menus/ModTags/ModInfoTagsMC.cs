using HamstarHelpers.Classes.UI.Menus;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TModLoader.Menus;
using HamstarHelpers.Helpers.TModLoader.Mods;
using HamstarHelpers.Internals.ModTags.UI;
using HamstarHelpers.Internals.WebRequests;
using HamstarHelpers.Services.Hooks.LoadHooks;
using HamstarHelpers.Services.UI.Menus;
using System;
using System.Collections.Generic;


namespace HamstarHelpers.Internals.Menus.ModTags {
	/// @private
	partial class ModInfoTagsMenuContext : TagsMenuContextBase {
		internal static ISet<string> RecentTaggedMods = new HashSet<string>();


		////////////////

		public static void Initialize( bool onModLoad ) {
			if( ModHelpersMod.Instance.Config.DisableModTags ) { return; }

			if( !onModLoad ) {
				var ctx = new ModInfoTagsMenuContext();
				MenuContextService.AddMenuContext( MenuUIDefinition.UIModInfo, "ModHelpers: Mod Info", ctx );
			}
		}



		////////////////

		private ModInfoTagsMenuContext() : base( false ) {
		}

		////

		public override void OnContexualize( MenuUIDefinition menuDef, string contextName ) {
			base.OnContexualize( menuDef, contextName );

			MenuContextService.AddMenuContext( menuDef, contextName + " Hidden", hiddenWidgetCtx );
			MenuContextService.AddMenuContext( menuDef, contextName + " Tag Finish Button", finishButtonWidgetCtx );
			MenuContextService.AddMenuContext( menuDef, contextName + " Tag Reset Button", resetButtonWidgetCtx );
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
			if( !modInfos.ContainsKey( this.CurrentModName ) ) {
				return;
			}

			var modInfo = modInfos[this.CurrentModName];
			if( modInfo.IsBadMod ) {
				this.UI.SafelySetTagButton( "Misleading Info" );
			}
		}
	}
}
