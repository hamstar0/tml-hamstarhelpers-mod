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

			this.Panel.ResetTagButtons( true );
		}


		////////////////

		private void SetCurrentMod( UIState ui, string modName ) {
			this.CurrentModName = modName;

			CustomLoadHooks.AddHook( GetModTags.TagsReceivedHookValidator, ( args ) => {
				if( !args.Found ) {
					LogHelpers.Warn();
					return false;
				}

				this.AllModTagsSnapshot = args.ModTags;

				ISet<string> netModTags = args.Found && args.ModTags.ContainsKey( modName ) ?
						args.ModTags[ modName ] :
						new HashSet<string>();
				bool hasNetTags = netModTags.Count > 0;

				//LogHelpers.Log( "SetCurrentMod modname: " + modName + ", modTags: " + string.Join(",", netModTags ) );
				if( hasNetTags ) {
					this.InfoDisplay.SetDefaultText( "Do these tags look incorrect? If so, modify them." );
					this.FinishButton.SetModeReadOnly();
					this.ResetButton.Disable();
				} else {
					this.InfoDisplay.SetDefaultText( "No tags set for this mod. Why not add some?" );
					this.FinishButton.SetModeSubmit();
				}

				this.Panel.SetCurrentMod( modName, netModTags );

				return false;
			} );
		}
	}
}
