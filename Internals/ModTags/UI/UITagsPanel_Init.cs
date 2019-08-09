using HamstarHelpers.Classes.UI.Elements;
using HamstarHelpers.Classes.UI.Elements.Menu;
using HamstarHelpers.Classes.UI.Menus;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Reflection;
using HamstarHelpers.Helpers.TModLoader.Menus;
using HamstarHelpers.Services.UI.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.UI;
using Microsoft.Xna.Framework;


namespace HamstarHelpers.Internals.ModTags.UI {
	partial class UITagsPanel : UIThemedPanel {
		private void InitializeControls( UIState uiContext, TagDefinition[] tags, bool canExcludeTags ) {
			this.InitializeControlButtons( uiContext );

			float y = 0;

			foreach( string category in new HashSet<string>( tags.Select( t => t.Category ) ) ) {
				this.CategoryButtons[category] = new UIMenuButton( this.Theme, category, 160f, 32f, 0f, y );
				y += 32;
			}

			for( int i = 0; i < tags.Length; i++ ) {
				string tag = tags[i].Tag;

				this.TagButtons[tag] = new UITagButton( this.Theme, this.Manager, tag, tags[i].Description, canExcludeTags );
			}
		}


		private void InitializeControlButtons( UIState uiContext ) {
			if( uiContext.GetType().Name == "UIModInfo" ) {
				Func<Rectangle> getRect = () => {
					UIElement homepageButton;
					if( ReflectionHelpers.Get( uiContext, "_modHomepageButton", out homepageButton ) ) {
						return homepageButton?.GetOuterDimensions().ToRectangle() ?? new Rectangle( -1, -1, 0, 0 );
					} else {
						LogHelpers.Warn( "Could not get _modHomepageButton" );
						return default( Rectangle );
					}
				};

				Action onHover = () => {
					string url;
					if( ReflectionHelpers.Get( uiContext, "_url", out url ) ) {
						this.Manager.SetInfoText( "" + url );
					} else {
						LogHelpers.Warn( "Could not get url" );
					}
				};
				Action onExit = () => {
					this.Manager.SetInfoText( "" );
				};

				//this.BlankButton = new UIMenuButton( UITheme.Vanilla, "", 98f, 24f, -196f, 172f, 0.36f, true );
				this.HiddenPanel = new UIHiddenPanel( getRect, onHover, onExit );
			}

			this.FinishButton = new UITagFinishButton( UITheme.Vanilla, this.Manager );
			this.ResetButton = new UITagResetButton( UITheme.Vanilla, this.Manager );

			//this.BlankButton.Disable();
		}


		////

		public void ApplyMenuContext( MenuUIDefinition menuDef, string contextName ) {
			int i = 0;

			foreach( UITagButton button in this.TagButtons.Values ) {
				var buttonWidgetCtx = new WidgetMenuContext( button, false );

				MenuContextService.AddMenuContext( menuDef, contextName + " Tag " + i, buttonWidgetCtx );
				i++;
			}

			var hiddenWidgetCtx = new WidgetMenuContext( this.HiddenPanel, false );
			var finishButtonWidgetCtx = new WidgetMenuContext( this.FinishButton, false );
			var resetButtonWidgetCtx = new WidgetMenuContext( this.ResetButton, false );

			MenuContextService.AddMenuContext( menuDef, contextName + " Hidden", hiddenWidgetCtx );
			MenuContextService.AddMenuContext( menuDef, contextName + " Tag Finish Button", finishButtonWidgetCtx );
			MenuContextService.AddMenuContext( menuDef, contextName + " Tag Reset Button", resetButtonWidgetCtx );
		}
	}
}
