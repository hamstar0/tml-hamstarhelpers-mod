using HamstarHelpers.Classes.UI.Elements;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.DotNET.Reflection;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Internals.ModTags.Base.UI;
using System;
using Terraria.UI;
using Microsoft.Xna.Framework;
using HamstarHelpers.Helpers.TModLoader.Menus;
using HamstarHelpers.Classes.UI.Menus;
using HamstarHelpers.Services.UI.Menus;


namespace HamstarHelpers.Internals.ModTags.ModInfo.UI {
	partial class UIModTagsEditor : UIModTags {
		private void InitializeEditorControls( UIState modInfoUi ) {
			Func<Rectangle> getRect = () => {
				UIElement homepageButton;
				if( !ReflectionHelpers.Get( modInfoUi, "_modHomepageButton", out homepageButton ) ) {
					LogHelpers.Warn( "Could not get _modHomepageButton" );
					return default( Rectangle );
				}

				return homepageButton?.GetOuterDimensions().ToRectangle() ?? new Rectangle( -1, -1, 0, 0 );
			};

			Action onHover = () => {
				string url;
				if( !ReflectionHelpers.Get( modInfoUi, "_url", out url ) ) {
					LogHelpers.Warn( "Could not get url" );
					return;
				}

				this.Manager.SetInfoText( "" + url );
			};

			Action onExit = () => {
				this.Manager.SetInfoText( "" );
			};

			//this.BlankButton = new UIMenuButton( UITheme.Vanilla, "", 98f, 24f, -196f, 172f, 0.36f, true );
			this.FinishButton = new UITagFinishButton( this.Theme, this.MyManager );
			this.HiddenPanel = new UIHiddenPanel( getRect, onHover, onExit );
		}


		////

		public override void ApplyMenuContext( MenuUIDefinition menuDef, string baseContextName ) {
			base.ApplyMenuContext( menuDef, baseContextName );

			var finishButtonWidgetCtx = new WidgetMenuContext( menuDef,
				baseContextName + " Tag Finish Button",
				this.FinishButton,
				false );
			var hiddenWidgetCtx = new WidgetMenuContext( menuDef,
				baseContextName+" Hidden",
				this.HiddenPanel,
				false );

			MenuContextService.AddMenuContext( hiddenWidgetCtx );
			MenuContextService.AddMenuContext( finishButtonWidgetCtx );
		}
	}
}
