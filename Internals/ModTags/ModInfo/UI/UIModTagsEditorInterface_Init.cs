using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.UI.Elements;
using HamstarHelpers.Classes.UI.Menus;
using HamstarHelpers.Helpers.DotNET.Extensions;
using HamstarHelpers.Helpers.DotNET.Reflection;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TModLoader.Menus;
using HamstarHelpers.Internals.ModTags.Base.UI;
using HamstarHelpers.Internals.ModTags.Base.UI.Buttons;
using HamstarHelpers.Internals.ModTags.ModInfo.UI.Buttons;
using HamstarHelpers.Services.UI.Menus;
using System;
using Terraria.UI;
using Microsoft.Xna.Framework;
using HamstarHelpers.Internals.ModTags.Base.Manager;


namespace HamstarHelpers.Internals.ModTags.ModInfo.UI {
	partial class UIModTagsEditorInterface : UIModTagsInterface {
		private void InitializeEditorControls( UIState modInfoUi ) {
			this.InitializeCategoryButtons();
			this.InitializeTagButtons();
			this.InitializeEditButton();
			this.InitializeHiddenControl( modInfoUi );
		}

		////

		private void InitializeCategoryButtons() {
			foreach( UICategoryMenuButton catButton in this.CategoryButtons.Values ) {
				catButton.OnSelect += ( _, __ ) => {
					this.LayoutTagButtonsByCategory();	//<- Necessary because of initial tag button layout per mod
				};
			}
		}

		private void InitializeTagButtons() {
			foreach( (string tag, UITagMenuButton tagButton) in this.TagButtons ) {
				tagButton.OnClick += ( _, __ ) => {
					this.ApplyTagConstraints( tag );
				};
			}
		}

		////

		private void InitializeEditButton() {
			Vector2 pos = this.GetTagControlsTopLeftPositionOffset();

			//this.BlankButton = new UIMenuButton( UITheme.Vanilla, "", 98f, 24f, -196f, 172f, 0.36f, true );
			this.EditButton = new UIEditModeMenuButton( this.Theme,
				this.MyManager,
				pos.X + UIResetTagsMenuButton.ButtonWidth + 4,
				pos.Y
			);
		}

		private void InitializeHiddenControl( UIState modInfoUi ) {
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

			Vector2 pos = this.GetTagControlsTopLeftPositionOffset();

			//this.BlankButton = new UIMenuButton( UITheme.Vanilla, "", 98f, 24f, -196f, 172f, 0.36f, true );
			this.HiddenPanel = new UIHiddenPanel( getRect, onHover, onExit );
		}


		////////////////

		public override void ApplyMenuContext( MenuUIDefinition menuDef, string baseContextName ) {
			base.ApplyMenuContext( menuDef, baseContextName );

			var finishButtonWidgetCtx = new WidgetMenuContext( menuDef,
				baseContextName + " Tag Finish Button",
				this.EditButton,
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
