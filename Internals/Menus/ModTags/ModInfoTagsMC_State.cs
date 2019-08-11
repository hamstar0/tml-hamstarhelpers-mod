using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Reflection;
using HamstarHelpers.Helpers.TModLoader.Menus;
using HamstarHelpers.Internals.ModTags.UI;
using HamstarHelpers.Services.UI.Menus;
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
				return;
			}

			this.ResetUIState( modName );
			this.SetCurrentMod( ui, modName );
			
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
				this.Manager.TagsUI.UnlockFinishButton();
			} else {
				this.Manager.TagsUI.LockFinishButton();
			}

			this.Manager.TagsUI.ResetTagButtons( true );
		}


		////////////////

		private void SetCurrentMod( UIState ui, string modName ) {
			this.Manager.SetCurrentMod( modName );
			//this.CurrentModName = modName;
		}

		public override void OnTagStateChange( UITagButton tagButton ) {
			this.Manager.TagsUI.RefreshButtonEnableStates();
		}
	}
}
