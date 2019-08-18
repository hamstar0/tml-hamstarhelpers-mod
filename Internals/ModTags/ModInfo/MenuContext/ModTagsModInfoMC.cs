using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Reflection;
using HamstarHelpers.Helpers.TModLoader.Menus;
using HamstarHelpers.Helpers.TModLoader.Mods;
using HamstarHelpers.Internals.ModTags.Base.MenuContext;
using HamstarHelpers.Internals.ModTags.Base.UI.Buttons;
using HamstarHelpers.Internals.ModTags.ModInfo.Manager;
using HamstarHelpers.Internals.WebRequests;
using HamstarHelpers.Services.Hooks.LoadHooks;
using HamstarHelpers.Services.UI.Menus;
using System;
using System.Collections.Generic;
using Terraria.UI;


namespace HamstarHelpers.Internals.ModTags.ModInfo.MenuContext {
	/// @private
	partial class ModTagsEditorMenuContext : ModTagsMenuContextBase {
		protected ModTagsEditorManager MyManager => (ModTagsEditorManager)this.Manager;


		////////////////

		internal static ISet<string> RecentTaggedMods = new HashSet<string>();



		////////////////

		public static void Initialize( bool onModLoad ) {
			if( ModHelpersMod.Instance.Config.DisableModTags ) { return; }

			if( !onModLoad ) {
				var ctx = new ModTagsEditorMenuContext( MenuUIDefinition.UIModInfo, "ModHelpers: Mod Info" );
				MenuContextService.AddMenuContext( ctx );
			}
		}



		////////////////

		protected ModTagsEditorMenuContext( MenuUIDefinition menuDef, string contextName )
				: base( menuDef, contextName ) {
			UIState uiModInfo = MainMenuHelpers.GetMenuUI( menuDef );

			if( uiModInfo == null || uiModInfo.GetType().Name != "UIModInfo" ) {
				throw new ModHelpersException( "UI context not UIModInfo, found "
						+ ( uiModInfo?.GetType().Name ?? "null" ) + " (" + menuDef + ")" );
			}

			this.Manager = new ModTagsEditorManager( this.InfoDisplay, menuDef, uiModInfo );
		}

		////

		public override void OnModsUnloading() { }


		////////////////

		public override void OnActivationForModTags( UIState ui ) {
			if( ui.GetType().Name != "UIModInfo" ) {
				LogHelpers.Warn( "Invalid UI. Expected UIModInfo, found " + ui.GetType().Name + "." );
				return;
			}

			UIElement elem;
			if( ReflectionHelpers.Get( ui, "_uIElement", out elem ) ) {
				elem.Left.Pixels += UITagMenuButton.ButtonWidth;
				elem.Recalculate();
			} else {
				LogHelpers.Warn( "Could not get uiElement for mod info tags context " + ui.GetType().Name );
			}
		}

		public override void OnDeactivation() {
			UIState modInfoUi = MainMenuHelpers.GetMenuUI( this.MenuDefinitionOfContext );
			if( modInfoUi.GetType().Name != "UIModInfo" ) {
				LogHelpers.Warn( "Invalid UI. Expected UIModInfo, found " + modInfoUi.GetType().Name + "." );
				return;
			}

			UIElement elem;
			if( ReflectionHelpers.Get( modInfoUi, "_uIElement", out elem ) ) {
				elem.Left.Pixels -= UITagMenuButton.ButtonWidth;
				elem.Recalculate();
			} else {
				LogHelpers.Warn( "Could not get uiElement for mod info tags context " + modInfoUi.GetType().Name );
			}
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
