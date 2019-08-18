using HamstarHelpers.Classes.UI.Elements;
using HamstarHelpers.Classes.UI.Menus;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Helpers.TModLoader;
using HamstarHelpers.Helpers.TModLoader.Menus;
using HamstarHelpers.Internals.Menus.ModUpdates;
using HamstarHelpers.Internals.ModTags.ModBrowser.MenuContext;
using HamstarHelpers.Internals.ModTags.ModInfo.MenuContext;
using HamstarHelpers.Services.Hooks.LoadHooks;
using HamstarHelpers.Services.UI.Menus;
using Microsoft.Xna.Framework;
using ReLogic.Graphics;
using System;
using System.Diagnostics;
using System.IO;
using Terraria;
using Terraria.UI;


namespace HamstarHelpers.Internals.Menus {
	/// @private
	class Menus {
		public static void OnPostSetupContent() {
			if( Main.dedServ ) { return; }

			LoadHooks.AddPostModLoadHook( () => {
				Menus.InitializeOpenConfigButton();
				Menus.InitializeDebugModeMenuInfo();
				ModTagsEditorMenuContext.Initialize( true );
				ModTagsModBrowserMenuContext.Initialize( true );
				ModUpdatesMenuContext.Initialize();
				//if( AprilFoolsMenuContext.IsAprilFools() ) {
				//	AprilFoolsMenuContext.Initialize();
				//}
			} );
		}


		private static void InitializeOpenConfigButton() {
			var button = new UITextPanelButton( UITheme.Vanilla, "Open Mod Config Folder" );
			button.Top.Set( -2f, 0f );
			button.Left.Set( -104f, 0.5f );
			button.Width.Set( 208f, 0f );
			button.Height.Set( 20f, 0f );
			button.OnClick += ( UIMouseEvent evt, UIElement listeningElement ) => {
				string fullpath = Main.SavePath + Path.DirectorySeparatorChar + TmlHelpers.ConfigRelativeFolder;

				try {
					Process.Start( fullpath );
				} catch( Exception ) { }
			};

			var buttonWidgetCtx = new WidgetMenuContext( MenuUIDefinition.UIMods,
				"ModHelpers: Mod Menu Config Folder Button",
				button,
				true );

			MenuContextService.AddMenuContext( buttonWidgetCtx );
		}


		private static void InitializeDebugModeMenuInfo() {
			var mymod = ModHelpersMod.Instance;
			if( !mymod.Config.DebugModeMenuInfo ) { return; }

			Main.OnPostDraw += ( _ ) => {
				Main.spriteBatch.DrawString(
					Main.fontMouseText,
					Main.menuMode + "",
					new Vector2( Main.screenWidth - 32, Main.screenHeight - 32 ),
					Color.White
				);
			};
		}
	}
}
