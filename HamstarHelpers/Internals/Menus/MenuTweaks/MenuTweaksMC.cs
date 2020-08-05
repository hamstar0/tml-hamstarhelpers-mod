using HamstarHelpers.Classes.UI.Menu;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Reflection;
using HamstarHelpers.Helpers.TModLoader.Menus;
using HamstarHelpers.Services.UI.Menus;
using System;
using System.Collections.Generic;
using Terraria.UI;


namespace HamstarHelpers.Internals.Menus.MenuTweaks {
	partial class MenuTweaksMenuContext : SessionMenuContext {
		public static void Initialize() {
			if( ModHelpersConfig.Instance.DisableModMenuTweaks ) { return; }

			MenuTweaksMenuContext ctx;

			ctx = new MenuTweaksMenuContext( MenuUIDefinition.UIMods, "ModHelpers: Mods Menu Tweaks" );
			MenuContextService.AddMenuContext( ctx );

			ctx = new MenuTweaksMenuContext( MenuUIDefinition.UIModPacks, "ModHelpers: Mod Packs Menu Tweaks" );
			MenuContextService.AddMenuContext( ctx );

			ctx = new MenuTweaksMenuContext( MenuUIDefinition.UIModSources, "ModHelpers: Mod Sources Menu Tweaks" );
			MenuContextService.AddMenuContext( ctx );

			ctx = new MenuTweaksMenuContext( MenuUIDefinition.UIModConfig, "ModHelpers: Mod Config Menu Tweaks" );
			MenuContextService.AddMenuContext( ctx );
		}



		////////////////

		private MenuTweaksMenuContext( MenuUIDefinition menuDef, string contextName )
				: base( menuDef, contextName, false, false ) { }

		public override void OnModsUnloading() { }

		public override void OnActivationForSession( UIState ui ) {
			UIElement elem = this.GetContainer( ui );
			if( elem == null ) {
				LogHelpers.Alert( "Container element not found for " + ui.GetType().Name );
				return;
			}
			
			elem.Top.Set( 80f, 0f );
			elem.Height.Set( -88f, 1f );

			elem.Recalculate();
		}

		public override void OnDeactivation() {
			UIState ui = MainMenuHelpers.GetMenuUI( this.MenuDefinitionOfContext );
			if( ui == null ) {
				LogHelpers.Warn( "Invalid UI." );
				return;
			}

			/*UIElement elem = this.GetContainer( ui );
			if( ui == null ) {
				LogHelpers.Alert( "Container element not found for " + ui.GetType().Name );
				return;
			}

			elem.Recalculate();*/
		}


		////////////////

		public UIElement GetContainer( UIState ui ) {
			UIElement elem;

			if( !ReflectionHelpers.Get( ui, "_rootElement", out elem ) || elem == null ) {
				if( !ReflectionHelpers.Get( ui, "uIElement", out elem ) || elem == null ) {
					List<UIElement> elems;

					if( !ReflectionHelpers.Get( ui, "Elements", out elems ) || elems == null || elems.Count == 0 ) {
						return null;
					}

					elem = elems[0];
				}
			}

			return elem;
		}
	}
}
