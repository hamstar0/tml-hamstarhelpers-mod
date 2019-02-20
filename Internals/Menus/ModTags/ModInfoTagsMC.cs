﻿using HamstarHelpers.Components.UI.Elements;
using HamstarHelpers.Components.UI.Menus;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using HamstarHelpers.Internals.Menus.ModTags.UI;
using HamstarHelpers.Services.Menus;
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
	}
}
