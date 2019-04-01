using HamstarHelpers.Components.UI.Menu;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using HamstarHelpers.Services.Menus;
using HamstarHelpers.Services.Timers;
using System;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader.IO;
using Terraria.UI;


namespace HamstarHelpers.Internals.Menus.ModUpdates {
	partial class AprilFoolsMenuContext : SessionMenuContext {
		public static bool IsAprilFools() {
			return DateTime.Today.Month == 4 && DateTime.Today.Day == 1;
		}

		////

		public static void Initialize() {
			if( ModHelpersMod.Instance.Config.DisableModMenuUpdates ) { return; }
			
			var ctx = new AprilFoolsMenuContext();
			MenuContextService.AddMenuContext( "UIMods", "ModHelpers: April Fools", ctx );
		}



		////////////////

		private AprilFoolsMenuContext() : base( false, false ) {
		}
		
		public override void OnContexualize( string uiClassName, string contextName ) {
			base.OnContexualize( uiClassName, contextName );
		}


		public override void Show( UIState ui ) {
			base.Show( ui );
			
			Timers.SetTimer( "ModHelpersAprilFoolsLoaderPause", 5, () => {
				this.ApplyModRenames( ui );
				return false;
			} );
		}


		////////////////

		private void ApplyModRenames( UIState ui ) {
			object items;
			if( !ReflectionHelpers.Get( ui, "items", out items ) ) {
				LogHelpers.Warn( "No 'items' field in ui " + ui );
				return;
			}

			UIList list;
			if( !ReflectionHelpers.Get( ui, "modList", out list ) ) {
				LogHelpers.Warn( "No 'modList' field in ui " + ui );
				return;
			}

			var itemsArr = (Array)items.GetType()
				.GetMethod( "ToArray" )
				.Invoke( items, new object[] { } );

			for( int i = 0; i < itemsArr.Length; i++ ) {
				object item = itemsArr.GetValue( i );

				object mod;
				ReflectionHelpers.Get( item, "mod", out mod );

				TmodFile tmod;
				ReflectionHelpers.Get( mod, "modFile", out tmod );

				this.ApplyModRename( list, tmod.name, tmod.version );
			}
		}


		////////////////

		public void ApplyModRename( UIList modList, string modName, Version modVersion ) {
			UIPanel uiModItem = null;
			TmodFile tmod = null;

			foreach( UIElement modItem in modList._items ) {
				object mod;

				ReflectionHelpers.Get( modItem, "mod", out mod );
				ReflectionHelpers.Get( mod, "modFile", out tmod );

				if( tmod.name == modName ) {
					uiModItem = (UIPanel)modItem;
					break;
				}
			}

			if( uiModItem != null ) {
				UIText modNameUI;
				if( ReflectionHelpers.Get( uiModItem, "modName", out modNameUI ) ) {
					if( modNameUI.Text == "Mod Helpers" ) {
						modNameUI.SetText( "Hamburger Helpers" );
					} else {
						modNameUI.SetText( "Hamstar's " + modNameUI.Text );
					}
				}
				
				uiModItem.Recalculate();
				uiModItem.Parent?.Recalculate();
				uiModItem.Parent?.Parent?.Recalculate();
				uiModItem.Parent?.Parent?.Parent?.Recalculate();
			}
		}
	}
}
