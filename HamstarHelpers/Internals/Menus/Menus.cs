using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Xna.Framework;
using ReLogic.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using HamstarHelpers.Classes.UI.Elements;
using HamstarHelpers.Classes.UI.Menus;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Libraries.Debug;
using HamstarHelpers.Libraries.DotNET.Reflection;
using HamstarHelpers.Libraries.TModLoader;
using HamstarHelpers.Libraries.TModLoader.Menus;
using HamstarHelpers.Libraries.XNA;
using HamstarHelpers.Internals.Menus.MenuTweaks;
using HamstarHelpers.Internals.Menus.ModUpdates;
using HamstarHelpers.Internals.ModTags.ModBrowser.MenuContext;
using HamstarHelpers.Internals.ModTags.ModInfo.MenuContext;
using HamstarHelpers.Services.Hooks.LoadHooks;
using HamstarHelpers.Services.UI.Menus;


namespace HamstarHelpers.Internals.Menus {
	/// @private
	class Menus {
		public static void OnPostModsLoad() {
			if( Main.dedServ ) { return; }

			LoadHooks.AddPostModLoadHook( () => {

				Menus.InitializeOpenConfigButton();
				Menus.InitializeDebugModeMenuInfo();
				ModTagsEditorMenuContext.Initialize();
				ModTagsModBrowserMenuContext.Initialize();
				ModUpdatesMenuContext.Initialize();
				MenuTweaksMenuContext.Initialize();
				//if( AprilFoolsMenuContext.IsAprilFools() ) {
				//	AprilFoolsMenuContext.Initialize();
				//}
			} );
		}


		////////////////

		private static void InitializeOpenConfigButton() {
			bool isShowingMem = false;
			ReflectionLibraries.Get( typeof(ModLoader), null, "showMemoryEstimates", out isShowingMem );

			var button = new UITextPanelButton( UITheme.Vanilla, "Open Mod Config Folder" );
			button.Top.Set( isShowingMem ? -2f : 11f, 0f );
			button.Left.Set( -104f, 0.5f );
			button.Width.Set( 208f, 0f );
			button.Height.Set( 20f, 0f );
			button.OnClick += ( UIMouseEvent evt, UIElement listeningElement ) => {
				string fullpath = Main.SavePath + Path.DirectorySeparatorChar + TmlLibraries.ConfigRelativeFolder;

				try {
					Process.Start( fullpath );
				} catch( Exception ) { }
			};

			var buttonWidgetCtx = new WidgetMenuContext(
				MenuUIDefinition.UIMods,
				"ModHelpers: Mod Menu Config Folder Button",
				button,
				true );

			MenuContextService.AddMenuContext( buttonWidgetCtx );
		}

		////

		private static bool DebugModeMenuInfoLoaded = false;

		private static void InitializeDebugModeMenuInfo() {
			if( !ModHelpersConfig.Instance.DebugModeMenuInfo ) { return; }

			Main.OnPostDraw += Menus.DebugModeMenuInfo;
			Menus.DebugModeMenuInfoLoaded = true;

			LoadHooks.AddModUnloadHook( () => {
				try {
					if( Menus.DebugModeMenuInfoLoaded ) {
						Main.OnPostDraw -= Menus.DebugModeMenuInfo;
					}
				} catch { }

				Menus.DebugModeMenuInfoLoaded = false;
			} );
		}


		private static void DebugModeMenuInfo( GameTime _ ) {
			bool __;
			XNALibraries.DrawBatch( ( sb ) => {
				sb.DrawString(
					Main.fontMouseText,
					Main.menuMode + "",
					new Vector2( Main.screenWidth - 32, Main.screenHeight - 32 ),
					Color.White
				);
			}, out __ );
		}
	}
}
