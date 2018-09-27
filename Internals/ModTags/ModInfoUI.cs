using HamstarHelpers.Components.UI.Menu;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Internals.WebRequests;
using HamstarHelpers.Services.Promises;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;


namespace HamstarHelpers.Internals.ModTags {
	partial class ModInfoUI : ModTagsUI {
		public static string GetModNameFromUI( UIState ui ) {
			Type ui_type = ui.GetType();
			FieldInfo ui_localmod_field = ui_type.GetField( "localMod", BindingFlags.NonPublic | BindingFlags.Instance );
			if( ui_localmod_field == null ) {
				LogHelpers.Log( "No 'localMod' field in " + ui_type );
				return null;
			}
			
			object localmod = ui_localmod_field.GetValue( ui );
			Type localmod_type = localmod.GetType();
			FieldInfo localmod_modfile_field = localmod_type.GetField( "modFile", BindingFlags.Public | BindingFlags.Instance );
			if( localmod_modfile_field == null ) {
				LogHelpers.Log( "No 'modFile' field in " + localmod_type );
				return null;
			}

			var modfile = (TmodFile)localmod_modfile_field.GetValue( localmod );
			if( modfile == null ) {
				LogHelpers.Log( "Empty 'mod' field" );
				return null;
			}

			return modfile.name;
		}

		////////////////

		public static void Initialize() {
			new ModInfoUI();
		}



		////////////////
		
		public UISubmitUpdateButton SubUpButton;

		public string ModName = "";

		////////////////

		protected override string UIName => "UIModInfo";
		protected override string BaseContextName => "Mod Info";



		////////////////

		private ModInfoUI() : base(false) {
			this.SubUpButton = new UISubmitUpdateButton( this );
			
			MenuUI.AddMenuLoader( this.UIName, this.BaseContextName+" Tag Submit + Update", this.SubUpButton, false );
			this.InitializeHoverText();
		}

		////////////////

		protected override void InitializeUI() {
			Action<UIState> ui_load = ui => {
				string modname = ModInfoUI.GetModNameFromUI( ui );
				if( modname == null ) { return; }

				this.SetCurrentMod( ui, modname );
				this.RecalculateMenuObjects();
			};
			Action<UIState> ui_unload = ui => {
				this.ResetMenuObjects();
			};

			MenuUI.AddMenuLoader( this.UIName, "ModHelpers: "+this.BaseContextName+" Load", ui_load, ui_unload );
		}


		////////////////

		public override void OnTagStateChange( UIModTagButton tag_button ) {
			this.SubUpButton.UpdateEnableState();
		}


		////////////////

		private void SetCurrentMod( UIState ui, string modname ) {
			this.ModName = modname;

			Promises.AddValidatedPromise<ModTagsPromiseArguments>( GetModTags.TagsReceivedPromiseValidator, ( args ) => {
				ISet<string> modtags = args.Found && args.ModTags.ContainsKey( modname ) ?
						args.ModTags[ modname ] :
						new HashSet<string>();
				bool has_tags = modtags.Count > 0;

//LogHelpers.Log( "SetCurrentMod modname: " + modname+", modtags: " + string.Join(",", modtags) );
				if( has_tags ) {
					this.SubUpButton.SetTagUpdateMode();
				} else {
					this.SubUpButton.SetTagSubmitMode();
				}
				
				foreach( var kv in this.TagButtons ) {
					if( has_tags ) {
						kv.Value.Disable();
					}

					if( modtags.Contains( kv.Key ) ) {
						kv.Value.SetTagState( 1 );
					}
				}

				return false;
			} );
		}


		////////////////

		internal void SubmitTags() {
			if( this.ModName == "" ) {
				throw new Exception( "Invalid mod name." );
			}

			Action<string> on_success = delegate ( string output ) {
				ErrorLogger.Log( "Mod info submit result: " + output );
			};
			Action<Exception, string> on_fail = ( e, output ) => {
				Main.NewText( "Mod info submit error: " + e.Message, Color.Red );
				LogHelpers.Log( e.ToString() );
			};

			PostModInfo.SubmitModInfo( this.ModName, this.GetTagsOfState(1), on_success, on_fail );

			this.SubUpButton.IsLocked = true;
			this.SubUpButton.Disable();
		}
	}
}
