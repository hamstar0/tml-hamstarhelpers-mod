﻿using HamstarHelpers.Components.UI;
using HamstarHelpers.Components.UI.Elements;
using HamstarHelpers.Components.UI.Menus;
using HamstarHelpers.Internals.Menus.ModRecommendations;
using HamstarHelpers.Internals.Menus.ModTags;
using HamstarHelpers.Internals.Menus.ModUpdates;
using HamstarHelpers.Services.Menus;
using HamstarHelpers.Services.Promises;
using System;
using System.Diagnostics;
using System.IO;
using Terraria;
using Terraria.UI;


namespace HamstarHelpers.Internals.Menus {
	class Menus {
		public static void OnPostSetupContent() {
			if( Main.dedServ ) { return; }

			Promises.AddPostModLoadPromise( () => {
				Menus.InitializeOpenConfigButton();
				ModInfoTagsMenuContext.Initialize();
				ModBrowserTagsMenuContext.Initialize();
				ModRecommendsMenuContext.Initialize();
				ModUpdatesMenuContext.Initialize();
				if( AprilFoolsMenuContext.IsAprilFools() ) {
					AprilFoolsMenuContext.Initialize();
				}
			} );
		}


		private static void InitializeOpenConfigButton() {
			var button = new UITextPanelButton( UITheme.Vanilla, "Open Mod Config Folder" );
			button.Top.Set( 11f, 0f );
			button.Left.Set( -104f, 0.5f );
			button.Width.Set( 208f, 0f );
			button.Height.Set( 20f, 0f );
			button.OnClick += ( UIMouseEvent evt, UIElement listeningElement ) => {
				string fullpath = Main.SavePath + Path.DirectorySeparatorChar + HamstarHelpersConfigData.RelativePath;

				try {
					Process.Start( fullpath );
				} catch( Exception ) { }
			};

			var buttonWidgetCtx = new WidgetMenuContext( button, true );

			MenuContextService.AddMenuContext( "UIMods", "ModHelpers: Mod Menu Config Folder Button", buttonWidgetCtx );
		}
	}
}
