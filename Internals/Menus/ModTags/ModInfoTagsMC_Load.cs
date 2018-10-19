using HamstarHelpers.Components.UI.Menu;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.TmlHelpers.Menus;
using HamstarHelpers.Internals.Menus.ModTags.UI;
using HamstarHelpers.Internals.WebRequests;
using HamstarHelpers.Services.Menus;
using HamstarHelpers.Services.Promises;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria.UI;


namespace HamstarHelpers.Internals.Menus.ModTags {
	partial class ModInfoTagsMenuContext : TagsMenuContextBase {
		private void InitializeContext() {
			Action<UIState> ui_load = ui => {
				string mod_name = MenuModHelper.GetModName( MenuContextService.GetCurrentMenu(), ui );
				if( mod_name == null ) {
					LogHelpers.Log( "Could not load mod tags." );
					return;
				}

				this.ResetUIState( mod_name );
				this.SetCurrentMod( ui, mod_name );
				this.RecalculateMenuObjects();
			};

			Action<UIState> ui_unload = ui => {
				this.ResetMenuObjects();
			};

			MenuContextService.AddMenuLoader( this.UIName, "ModHelpers: " + this.ContextName + " Load", ui_load, ui_unload );
		}


		private void InitializeControls() {
			this.FinishButton = new UITagFinishButton( this );
			this.ResetButton = new UITagResetButton( this );

			MenuContextService.AddMenuLoader( this.UIName, "ModHelpers: " + this.ContextName + " Tag Finish Button", this.FinishButton, false );
			MenuContextService.AddMenuLoader( this.UIName, "ModHelpers: " + this.ContextName + " Tag Reset Button", this.ResetButton, false );
		}


		////////////////

		private void ResetUIState( string mod_name ) {
			if( !ModInfoTagsMenuContext.RecentTaggedMods.Contains( mod_name ) ) {
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

		private void SetCurrentMod( UIState ui, string mod_name ) {
			this.CurrentModName = mod_name;

			Promises.AddValidatedPromise<ModTagsPromiseArguments>( GetModTags.TagsReceivedPromiseValidator, ( args ) => {
				this.AllModTagsSnapshot = args.ModTags;

				ISet<string> net_modtags = args.Found && args.ModTags.ContainsKey( mod_name ) ?
						args.ModTags[ mod_name ] :
						new HashSet<string>();
				bool has_net_tags = net_modtags.Count > 0;

//LogHelpers.Log( "SetCurrentMod modname: " + mod_name + ", modtags: " + string.Join(",", net_modtags ) );
				if( has_net_tags ) {
					MenuContextBase.InfoDisplay.SetText( "", Color.Gray );
					this.FinishButton.SetModeReadOnly();
					this.ResetButton.Disable();
				} else {
					MenuContextBase.InfoDisplay.SetText( "No tags set for this mod. Why not add some?", Color.Gray );
					this.FinishButton.SetModeSubmit();
				}
				
				foreach( var kv in this.TagButtons ) {
					if( !has_net_tags ) {
						kv.Value.Enable();
					}

					kv.Value.SetTagState( net_modtags.Contains(kv.Key) ? 1 : 0 );
				}

				return false;
			} );
		}
	}
}
