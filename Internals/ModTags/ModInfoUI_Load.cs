using HamstarHelpers.Components.UI.Menu;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Internals.WebRequests;
using HamstarHelpers.Services.Promises;
using System;
using System.Collections.Generic;
using Terraria.UI;


namespace HamstarHelpers.Internals.ModTags {
	partial class ModInfoUI : ModTagsUI {
		protected override void InitializeContext() {
			Action<UIState> ui_load = ui => {
				string modname = ModInfoUI.GetModNameFromUI( ui );
				if( modname == null ) { return; }

				this.SetCurrentMod( ui, modname );
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

		private void SetCurrentMod( UIState ui, string modname ) {
			if( !ModInfoUI.RecentTaggedMods.Contains(modname) ) {
				if( this.SubUpButton.IsLocked ) {
					this.SubUpButton.Unlock();
				}
			} else {
				if( !this.SubUpButton.IsLocked ) {
					this.SubUpButton.Lock();
				}
			}

			this.ModName = modname;

			foreach( var kv in this.TagButtons ) {
				kv.Value.Disable();
				kv.Value.SetTagState( 0 );
			}

			Promises.AddValidatedPromise<ModTagsPromiseArguments>( GetModTags.TagsReceivedPromiseValidator, ( args ) => {
				ISet<string> net_modtags = args.Found && args.ModTags.ContainsKey( modname ) ?
						args.ModTags[ modname ] :
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
