using HamstarHelpers.Components.UI.Menu;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Internals.WebRequests;
using HamstarHelpers.Services.Promises;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader.IO;
using Terraria.UI;


namespace HamstarHelpers.Internals.ModPackBrowser {
	partial class ModTagUI {
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
			new ModTagUI();
		}



		////////////////

		public UIText HoverElement;
		public UISubmitUpdateButton SubUpButton;
		public IDictionary<string, UIModTagButton> TagButtons = new Dictionary<string, UIModTagButton>();


		////////////////

		private ModTagUI() {
			this.HoverElement = new UIText( "" );
			this.HoverElement.Width.Set( 0, 0 );
			this.HoverElement.Height.Set( 0, 0 );

			this.SubUpButton = new UISubmitUpdateButton( this );

			Action<UIState> ui_load = ( ui ) => {
				string modname = ModTagUI.GetModNameFromUI( ui );
				if( modname == null ) { return; }

				this.InitializeUI( modname );
			};

			MenuUI.AddMenuLoader( "UIModInfo", "ModHelpers: Mod Info Tags Submit+Update", this.SubUpButton, false );
			MenuUI.AddMenuLoader( "UIModInfo", "ModHelpers: Mod Info Tags Hover", this.HoverElement, false );
			MenuUI.AddMenuLoader( "UIModInfo", "ModHelpers: Mod Info Load", ui_load, _ => { } );
		}

		////////////////

		private void InitializeUI( string modname ) {
			Promises.AddValidatedPromise<ModTagsPromiseArguments>( GetModTags.TagsReceivedPromiseValidator, ( args ) => {
				ISet<string> modtags = args.Found && args.ModTags.ContainsKey( modname ) ?
					args.ModTags[ modname ] :
					new HashSet<string>();
				bool mod_has_tags = modtags.Count > 0;

				this.InitializeButtons( mod_has_tags, modtags );

				if( mod_has_tags ) {
					this.SubUpButton.SetTagUpdateMode();
				} else {
					this.SubUpButton.SetTagSubmitMode();
				}

				return false;
			} );
		}


		private void InitializeButtons( bool mod_has_tags, ISet<string> modtags ) {
			var buttons = new Dictionary<string, UIModTagButton>();

			int i = 0;
			foreach( var kv in ModTagUI.Tags ) {
				string tag_text = kv.Key;
				string tag_desc = kv.Value;
				bool mod_has_curr_tag = modtags.Contains( tag_text );

				var button = new UIModTagButton( this, mod_has_curr_tag, i, tag_text, tag_desc, 0.6f );

				if( mod_has_tags ) {
					button.Disable();
				}

				button.OnClick += ( UIMouseEvent evt, UIElement listeningElement ) => {
					if( !button.IsEnabled ) { return; }

					button.ToggleTag();
					this.SubUpButton.UpdateEnableState( buttons );
				};

				MenuUI.AddMenuLoader( "UIModInfo", "ModHelpers: Mod Info Tags " + i, button, false );
				buttons[tag_text] = button;

				i++;
			}
		}


		////////////////

		public void EnableButtons() {
			foreach( var kv in this.TagButtons ) {
				kv.Value.Enable();
			}
		}
	}
}
