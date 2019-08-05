using HamstarHelpers.Classes.UI.Elements;
using HamstarHelpers.Classes.UI.Menus;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Reflection;
using HamstarHelpers.Helpers.TModLoader.Mods;
using HamstarHelpers.Internals.Menus.ModTags.UI;
using HamstarHelpers.Internals.WebRequests;
using HamstarHelpers.Services.Hooks.LoadHooks;
using HamstarHelpers.Services.Timers;
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
				var ctx = new ModInfoTagsMenuContext();
				MenuContextService.AddMenuContext( "UIModInfo", "ModHelpers: Mod Info", ctx );
			}
		}



		////////////////

		private UIHiddenPanel HiddenPanel;
		internal UITagFinishButton FinishButton;
		internal UITagResetButton ResetButton;

		public string CurrentModName = "";

		internal IDictionary<string, ISet<string>> AllModTagsSnapshot = null;



		////////////////

		private ModInfoTagsMenuContext() : base( false ) {
			Func<Rectangle> getRect = () => {
				UIElement homepageButton;
				if( ReflectionHelpers.Get( this.MyUI, "_modHomepageButton", out homepageButton ) ) {
					return homepageButton?.GetOuterDimensions().ToRectangle() ?? new Rectangle( -1, -1, 0, 0 );
				} else {
					LogHelpers.Warn( "Could not get _modHomepageButton" );
					return default( Rectangle );
				}
			};

			Action onHover = () => {
				string url;
				if( ReflectionHelpers.Get( this.MyUI, "_url", out url ) ) {
					this.InfoDisplay?.SetText( "" + url );
				} else {
					LogHelpers.Warn( "Could not get url" );
				}
			};
			Action onExit = () => {
				this.InfoDisplay?.SetText( "" );
			};

			this.HiddenPanel = new UIHiddenPanel( getRect, onHover, onExit );
			this.FinishButton = new UITagFinishButton( UITheme.Vanilla, this );
			this.ResetButton = new UITagResetButton( UITheme.Vanilla, this );
		}

		////

		public override void OnContexualize( string uiClassName, string contextName ) {
			base.OnContexualize( uiClassName, contextName );

			var hiddenWidgetCtx = new WidgetMenuContext( this.HiddenPanel, false );
			var finishButtonWidgetCtx = new WidgetMenuContext( this.FinishButton, false );
			var resetButtonWidgetCtx = new WidgetMenuContext( this.ResetButton, false );

			MenuContextService.AddMenuContext( uiClassName, contextName + " Hidden", hiddenWidgetCtx );
			MenuContextService.AddMenuContext( uiClassName, contextName + " Tag Finish Button", finishButtonWidgetCtx );
			MenuContextService.AddMenuContext( uiClassName, contextName + " Tag Reset Button", resetButtonWidgetCtx );
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
				var button = this.Panel.TagButtons["Misleading Info"];
				
				if( button.TagState != 1 ) {
					if( Timers.GetTimerTickDuration("ModHelpersTagsEditDefaults") <= 0 ) {
						Timers.SetTimer( "ModHelpersTagsEditDefaults", 60, () => {
							button.SetTagState( 1 );
							return false;
						} );
					}
				}
			}
		}
	}
}
