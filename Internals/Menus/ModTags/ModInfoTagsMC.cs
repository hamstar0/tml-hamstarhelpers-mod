using HamstarHelpers.Classes.UI.Elements;
using HamstarHelpers.Classes.UI.Menus;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Reflection;
using HamstarHelpers.Helpers.TModLoader.Menus;
using HamstarHelpers.Helpers.TModLoader.Mods;
using HamstarHelpers.Internals.ModTags;
using HamstarHelpers.Internals.ModTags.UI;
using HamstarHelpers.Internals.WebRequests;
using HamstarHelpers.Services.Hooks.LoadHooks;
using HamstarHelpers.Services.UI.Menus;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria.UI;


namespace HamstarHelpers.Internals.Menus.ModTags {
	/// @private
	partial class ModInfoTagsMenuContext : TagsMenuContextBase {
		internal static ISet<string> RecentTaggedMods = new HashSet<string>();


		////////////////

		public static void Initialize( bool onModLoad ) {
			if( ModHelpersMod.Instance.Config.DisableModTags ) { return; }

			if( !onModLoad ) {
				var manager = new ModTagsManager();
				var ctx = new ModInfoTagsMenuContext( manager );
				MenuContextService.AddMenuContext( TModLoaderMenuDefinition.ModInfo, "ModHelpers: Mod Info", ctx );
			}
		}



		////////////////

		private UIHiddenPanel HiddenPanel;
		internal UITagFinishButton FinishButton;
		internal UITagResetButton ResetButton;

		public string CurrentModName = "";

		internal IDictionary<string, ISet<string>> AllModTagsSnapshot = null;



		////////////////

		private ModInfoTagsMenuContext( ModTagsManager manager ) : base( manager, false ) {
			Func<Rectangle> getRect = () => {
				UIElement homepageButton;
				if( ReflectionHelpers.Get( this.MyMenuUI, "_modHomepageButton", out homepageButton ) ) {
					return homepageButton?.GetOuterDimensions().ToRectangle() ?? new Rectangle( -1, -1, 0, 0 );
				} else {
					LogHelpers.Warn( "Could not get _modHomepageButton" );
					return default( Rectangle );
				}
			};

			Action onHover = () => {
				string url;
				if( ReflectionHelpers.Get( this.MyMenuUI, "_url", out url ) ) {
					this.InfoDisplay?.SetText( "" + url );
				} else {
					LogHelpers.Warn( "Could not get url" );
				}
			};
			Action onExit = () => {
				this.InfoDisplay?.SetText( "" );
			};

			this.HiddenPanel = new UIHiddenPanel( getRect, onHover, onExit );
			this.FinishButton = new UITagFinishButton( UITheme.Vanilla, manager );
			this.ResetButton = new UITagResetButton( UITheme.Vanilla, manager );
		}

		////

		public override void OnContexualize( TModLoaderMenuDefinition menuDef, string contextName ) {
			base.OnContexualize( menuDef, contextName );

			var hiddenWidgetCtx = new WidgetMenuContext( this.HiddenPanel, false );
			var finishButtonWidgetCtx = new WidgetMenuContext( this.FinishButton, false );
			var resetButtonWidgetCtx = new WidgetMenuContext( this.ResetButton, false );

			MenuContextService.AddMenuContext( menuDef, contextName + " Hidden", hiddenWidgetCtx );
			MenuContextService.AddMenuContext( menuDef, contextName + " Tag Finish Button", finishButtonWidgetCtx );
			MenuContextService.AddMenuContext( menuDef, contextName + " Tag Reset Button", resetButtonWidgetCtx );
		}


		////////////////

		public override void OnTagStateChange( UITagButton tagButton ) {
			this.FinishButton.UpdateEnableState();
			this.ResetButton.UpdateEnableState();
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
				this.Panel.SafelySetTagButton( "Misleading Info" );
			}
		}
	}
}
