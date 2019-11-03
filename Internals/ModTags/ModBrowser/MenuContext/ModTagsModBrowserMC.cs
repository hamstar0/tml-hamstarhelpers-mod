using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Reflection;
using HamstarHelpers.Helpers.TModLoader.Menus;
using HamstarHelpers.Internals.ModTags.Base.MenuContext;
using HamstarHelpers.Internals.ModTags.Base.UI.Buttons;
using HamstarHelpers.Internals.ModTags.ModBrowser.Manager;
using HamstarHelpers.Services.UI.Menus;
using Terraria.UI;


namespace HamstarHelpers.Internals.ModTags.ModBrowser.MenuContext {
	/// @private
	partial class ModTagsModBrowserMenuContext : ModTagsMenuContextBase {
		public static void Initialize() {
			if( !ModHelpersMod.Instance.Data.ModTagsOpened ) { return; }
			if( ModHelpersMod.Config.DisableModTags ) { return; }

			if( MenuContextService.GetMenuContext( MenuUIDefinition.UIModBrowser, "ModHelpers: Mod Tags Browser" ) == null ) {
				var ctx = new ModTagsModBrowserMenuContext( MenuUIDefinition.UIModBrowser, "ModHelpers: Mod Tags Browser" );
				MenuContextService.AddMenuContext( ctx );
			}
		}



		////////////////

		protected ModTagsModBrowserMenuContext( MenuUIDefinition menuDef, string contextName )
				: base( menuDef, contextName ) {
			this.Manager = new ModTagsModBrowserManager( this.InfoDisplay, menuDef );
		}

		////

		public override void OnModsUnloading() { }


		////////////////

		public override void OnActivationForModTags( UIState ui ) {
			if( ui.GetType().Name != "UIModBrowser" ) {
				LogHelpers.Warn( "Invalid UI. Expected UIModBrowser, found "+ui.GetType().Name+"." );
				return;
			}

			UIElement elem;
			if( !ReflectionHelpers.Get( ui, "_rootElement", out elem ) || elem == null ) {
				LogHelpers.Alert( "_rootElement not found for " + ui.GetType().Name );
				return;
			}

			elem.Left.Pixels += UITagMenuButton.ButtonWidth;

			elem.Top.Set( 80f, 0f );
			elem.Height.Set( -88f, 1f );

			elem.Recalculate();
		}

		public override void OnDeactivation() {
			UIState modBrowserUi = MainMenuHelpers.GetMenuUI( this.MenuDefinitionOfContext );
			if( modBrowserUi?.GetType().Name != "UIModBrowser" ) {
				LogHelpers.Warn( "Invalid UI. Expected UIModBrowser, found " + modBrowserUi?.GetType().Name + "." );
				return;
			}

			UIElement elem;
			if( !ReflectionHelpers.Get( modBrowserUi, "_rootElement", out elem ) || elem == null ) {
				LogHelpers.Alert( "_rootElement not found for UIModBrowser." );
				return;
			}

			elem.Left.Pixels -= UITagMenuButton.ButtonWidth;

			elem.Recalculate();
		}
	}
}
