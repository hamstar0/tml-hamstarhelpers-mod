using HamstarHelpers.Components.UI.Elements;
using HamstarHelpers.Components.UI.Menus;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers.Reflection;
using HamstarHelpers.Internals.Menus.ModTags.UI;
using HamstarHelpers.Internals.WebRequests;
using HamstarHelpers.Services.Menus;
using HamstarHelpers.Services.Promises;
using HamstarHelpers.Services.Timers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria.UI;


namespace HamstarHelpers.Internals.Menus.ModTags {
	partial class ModInfoTagsMenuContext : TagsMenuContextBase {
		internal static ISet<string> RecentTaggedMods = new HashSet<string>();


		////////////////

		public static void Initialize() {
			if( ModHelpersMod.Instance.Config.DisableModTags ) { return; }

			var ctx = new ModInfoTagsMenuContext();
			MenuContextService.AddMenuContext( "UIModInfo", "ModHelpers: Mod Info", ctx );
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
				ReflectionHelpers.Get( this.MyUI, "modHomepageButton", out homepageButton );
				return homepageButton?.GetOuterDimensions().ToRectangle() ?? new Rectangle(-1,-1,0,0);
			};
			Action onHover = () => {
				string url;
				ReflectionHelpers.Get( this.MyUI, "url", out url );
				this.InfoDisplay?.SetText( ""+url );
			};
			Action onExit = () => {
				this.InfoDisplay?.SetText( "" );
			};

			this.HiddenPanel = new UIHiddenPanel( getRect, onHover, onExit );
			this.FinishButton = new UITagFinishButton( this );
			this.ResetButton = new UITagResetButton( this );
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

			Func<ModInfoListPromiseArguments, bool> callback = ( modInfoArgs ) => {
				this.ApplyDefaultEditModeTags( modInfoArgs.ModInfo );
				return false;
			};

			Promises.AddValidatedPromise<ModInfoListPromiseArguments>( GetModInfo.BadModsListPromiseValidator, callback );
		}


		////////////////

		private void ApplyDefaultEditModeTags( IDictionary<string, BasicModInfoEntry> modInfos ) {
LogHelpers.LogOnce( "mod: "+this.CurrentModName+", is found? "+modInfos.ContainsKey( this.CurrentModName ));
			if( !modInfos.ContainsKey( this.CurrentModName ) ) {
				return;
			}

			var modInfo = modInfos[this.CurrentModName];
LogHelpers.LogOnce( "is bad? "+modInfos[this.CurrentModName].IsBadMod);
			if( modInfo.IsBadMod ) {
				var button = this.TagButtons["Misleading Info"];
				
LogHelpers.LogOnce( "0 ModHelpersTagsEditDefaults "+(button?.TagState ?? -2) );
				if( button.TagState != 1 ) {
LogHelpers.LogOnce( "1 ModHelpersTagsEditDefaults" );
					if( Timers.GetTimerTickDuration("ModHelpersTagsEditDefaults") <= 0 ) {
LogHelpers.LogOnce( "2 ModHelpersTagsEditDefaults" );
						Timers.SetTimer( "ModHelpersTagsEditDefaults", 60, () => {
LogHelpers.LogOnce( "3 ModHelpersTagsEditDefaults" );
							button.SetTagState( 1 );
							return false;
						} );
					}
				}
			}
		}
	}
}
