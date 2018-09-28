using HamstarHelpers.Components.UI.Menu;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Internals.WebRequests;
using HamstarHelpers.Services.Promises;
using System;
using System.Collections.Generic;
using Terraria.UI;


namespace HamstarHelpers.Internals.ModTags {
	partial class ModInfoTagsMenuContext : TagsMenuContextBase {
		protected override void InitializeContext() {
			Action<UIState> ui_load = ui => {
				string mod_name = ModInfoTagsMenuContext.GetModNameFromUI( ui );
				if( mod_name == null ) { return; }

				this.ResetUIState( mod_name );
				this.SetCurrentMod( ui, mod_name );
				this.RecalculateMenuObjects();
			};

			Action<UIState> ui_unload = ui => {
				this.ResetMenuObjects();
			};

			MenuUI.AddMenuLoader( this.UIName, "ModHelpers: "+this.ContextName+" Load", ui_load, ui_unload );
		}

		protected void InitializeSubUpButton() {
			this.SubUpButton = new UISubmitUpdateButton( this );

			MenuUI.AddMenuLoader( this.UIName, this.ContextName + " Tag Submit Or Update Button", this.SubUpButton, false );
		}


		////////////////

		private void ResetUIState( string mod_name ) {
			if( !ModInfoTagsMenuContext.RecentTaggedMods.Contains( mod_name ) ) {
				if( this.SubUpButton.IsLocked ) {
					this.SubUpButton.Unlock();
				}
			} else {
				if( !this.SubUpButton.IsLocked ) {
					this.SubUpButton.Lock();
				}
			}

			foreach( var kv in this.TagButtons ) {
				kv.Value.Disable();
				kv.Value.SetTagState( 0 );
			}
		}

		private void SetCurrentMod( UIState ui, string mod_name ) {
			this.ModName = mod_name;

			Promises.AddValidatedPromise<ModTagsPromiseArguments>( GetModTags.TagsReceivedPromiseValidator, ( args ) => {
				ISet<string> net_modtags = args.Found && args.ModTags.ContainsKey( mod_name ) ?
						args.ModTags[ mod_name ] :
						new HashSet<string>();
				bool has_net_tags = net_modtags.Count > 0;

//LogHelpers.Log( "SetCurrentMod modname: " + modname+", modtags: " + string.Join(",", modtags) );
				if( has_net_tags ) {
					this.SubUpButton.SetTagUpdateMode();
				} else {
					this.SubUpButton.SetTagSubmitMode();
				}
				
				foreach( var kv in this.TagButtons ) {
					if( !has_net_tags ) {
						kv.Value.Enable();
					}

					if( net_modtags.Contains( kv.Key ) ) {
						kv.Value.SetTagState( 1 );
					}
				}

				return false;
			} );
		}
	}
}
