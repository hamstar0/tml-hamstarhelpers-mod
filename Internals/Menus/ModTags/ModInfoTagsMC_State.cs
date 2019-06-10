using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Reflection;
using HamstarHelpers.Helpers.TModLoader.Menus;
using HamstarHelpers.Internals.Menus.ModTags.UI;
using HamstarHelpers.Internals.WebRequests;
using HamstarHelpers.Services.Menus;
using HamstarHelpers.Services.Promises;
using System.Collections.Generic;
using Terraria.UI;


namespace HamstarHelpers.Internals.Menus.ModTags {
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
			if( ReflectionHelpers.Get( ui, "uIElement", out elem ) ) {
				elem.Left.Pixels += UITagButton.ColumnWidth;
				elem.Recalculate();
			}
		}

		////////////////

		private void HideGeneral( UIState ui ) {
			this.InfoDisplay.SetDefaultText( "" );

			this.ResetMenuObjects();

			UIElement elem;
			if( ReflectionHelpers.Get( ui, "uIElement", out elem ) ) {
				elem.Left.Pixels -= UITagButton.ColumnWidth;
				elem.Recalculate();
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

			foreach( var kv in this.TagButtons ) {
				kv.Value.Disable();
				kv.Value.SetTagState( 0 );
			}
		}


		////////////////

		private void SetCurrentMod( UIState ui, string modName ) {
			this.CurrentModName = modName;

			Promises.AddValidatedPromise<ModTagsPromiseArguments>( GetModTags.TagsReceivedPromiseValidator, ( args ) => {
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
				
				foreach( var kv in this.TagButtons ) {
					string tagName = kv.Key;
					UITagButton button = kv.Value;
					bool hasTag = netModTags.Contains( tagName );

					if( !hasNetTags ) {
						button.Enable();
					}

					if( tagName == "Low Effort" ) {
						if( hasTag ) {
							button.SetTagState( 1 );
						} else {
							string desc = this.GetModDataFromActiveMod( modName, "description" );
							if( string.IsNullOrEmpty(desc) ) {
								string _ = "";
								desc = this.GetModDescriptionFromUI( modName, ref _ );
							}

							if( desc.Contains("Modify this file with a description of your mod.") ) {
								button.SetTagState( 1 );
							}
						}
					} else {
						button.SetTagState( hasTag ? 1 : 0 );
					}
				}

				return false;
			} );
		}
	}
}
