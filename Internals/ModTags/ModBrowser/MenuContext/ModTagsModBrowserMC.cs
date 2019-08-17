using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Reflection;
using HamstarHelpers.Helpers.TModLoader.Menus;
using HamstarHelpers.Internals.ModTags.Base.MenuContext;
using HamstarHelpers.Internals.ModTags.Base.UI.Buttons;
using HamstarHelpers.Services.UI.Menus;
using Terraria.UI;


namespace HamstarHelpers.Internals.ModTags.ModBrowser.MenuContext {
	/// @private
	partial class ModTagsModBrowserMenuContext : ModTagsMenuContextBase {
		public static void Initialize( bool onModLoad ) {
			if( ModHelpersMod.Instance.Config.DisableModTags ) { return; }

			if( !onModLoad ) {
				var ctx = new ModTagsModBrowserMenuContext( MenuUIDefinition.UIModBrowser, "ModHelpers: Mod Browser" );
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
