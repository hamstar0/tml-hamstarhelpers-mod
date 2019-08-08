using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Reflection;
using HamstarHelpers.Helpers.TModLoader.Menus;
using HamstarHelpers.Internals.ModTags.UI;
using HamstarHelpers.Internals.WebRequests;
using HamstarHelpers.Services.Hooks.LoadHooks;
using HamstarHelpers.Services.UI.Menus;
using System.Collections.Generic;
using Terraria.UI;


namespace HamstarHelpers.Internals.Menus.ModTags {
	/// @private
	partial class ModInfoTagsMenuContext : TagsMenuContextBase {
		public override void Show( UIState ui ) {
			base.Show( ui );
			this.ShowGeneral( ui );
		}

		public override void Hide( UIState ui ) {
			base.Hide( ui );
			this.HideGeneral( ui );
		}


		////////////////

		private void ShowGeneral( UIState ui ) {
			string modName = ModMenuHelpers.GetModName( MenuContextService.GetCurrentMenuUI(), ui );

			this.InfoDisplay.SetDefaultText( "" );

			if( modName == null ) {
				LogHelpers.Warn( "Could not load mod tags; no mod found" );
			} else {
				this.ResetUIState( modName );
				this.SetCurrentMod( ui, modName );
				this.RecalculateMenuObjects();
			}
			
			
			UIElement elem;
			if( ReflectionHelpers.Get( ui, "_uIElement", out elem ) ) {
				elem.Left.Pixels += UITagButton.ButtonWidth;
				elem.Recalculate();
			} else {
				LogHelpers.Warn( "Could not get uiElement for mod info tags context "+ui.GetType().Name );
			}
		}

		////////////////

		private void HideGeneral( UIState ui ) {
			this.InfoDisplay.SetDefaultText( "" );

			this.ResetMenuObjects();

			UIElement elem;
			if( ReflectionHelpers.Get( ui, "_uIElement", out elem ) ) {
				elem.Left.Pixels -= UITagButton.ButtonWidth;
				elem.Recalculate();
			} else {
				LogHelpers.Warn( "Could not get uiElement for mod info tags context " + ui.GetType().Name );
			}
		}


		////////////////

		private void ResetUIState( string modName ) {
			if( !ModInfoTagsMenuContext.RecentTaggedMods.Contains( modName ) ) {
				if( this.FinishButton.IsLocked ) {
					this.FinishButton.Unlock();
				}
			} else {
				if( !this.FinishButton.IsLocked ) {
					this.FinishButton.Lock();
				}
			}

			this.UI.ResetTagButtons( true );
		}


		////////////////

		private void SetCurrentMod( UIState ui, string modName ) {
			this.Manager.SetCurrentMod( modName );
			//this.CurrentModName = modName;
		}
	}
}
