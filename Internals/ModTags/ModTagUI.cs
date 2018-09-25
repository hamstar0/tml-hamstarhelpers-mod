using HamstarHelpers.Components.UI.Menu;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Internals.WebRequests;
using HamstarHelpers.Services.Promises;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
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

		public string ModName = "";

		private Vector2 OldOverhaulLogoPos;



		////////////////

		private ModTagUI() {
			this.HoverElement = new UIText( "" );
			this.HoverElement.Width.Set( 0, 0 );
			this.HoverElement.Height.Set( 0, 0 );
			this.HoverElement.TextColor = Color.Aquamarine;

			this.SubUpButton = new UISubmitUpdateButton( this );

			this.InitializeTagButtons();
			this.InitializeUI();
			MenuUI.AddMenuLoader( "UIModInfo", "ModHelpers: Mod Info Tags Submit+Update", this.SubUpButton, false );
			MenuUI.AddMenuLoader( "UIModInfo", "ModHelpers: Mod Info Tags Hover", this.HoverElement, false );
		}

		////////////////

		private void InitializeUI() {
			Action<UIState> ui_load = ui => {
				string modname = ModTagUI.GetModNameFromUI( ui );
				if( modname == null ) { return; }

				this.SetCurrentMod( modname );
				this.RecalculateMenuObjects();
			};
			Action<UIState> ui_unload = ui => {
				this.ResetMenuObjects();
			};

			MenuUI.AddMenuLoader( "UIModInfo", "ModHelpers: Mod Info Load", ui_load, ui_unload );
		}

		private void InitializeTagButtons() {
			int i = 0;

			foreach( var kv in ModTagUI.Tags ) {
				string tag_text = kv.Key;
				string tag_desc = kv.Value;

				var button = new UIModTagButton( this, false, false, i, tag_text, tag_desc, 0.6f );

				MenuUI.AddMenuLoader( "UIModInfo", "ModHelpers: Mod Info Tags " + i, button, false );
				this.TagButtons[ tag_text ] = button;

				i++;
			}
		}


		////////////////

		public void EnableButtons() {
			foreach( var kv in this.TagButtons ) {
				kv.Value.Enable();
			}
		}


		////////////////

		public void RecalculateMenuObjects() {
			Mod oh_mod = ModLoader.GetMod( "OverhaulMod" );

			if( oh_mod != null ) {
				Type oh_mod_type = oh_mod.GetType();
				var oh_logo_pos_field = oh_mod_type.GetField( "mainMenuDataOffset", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static );

				if( oh_logo_pos_field != null ) {
					if( this.OldOverhaulLogoPos != default( Vector2 ) ) {
						this.OldOverhaulLogoPos = (Vector2)oh_logo_pos_field.GetValue( oh_mod );
					}

					oh_logo_pos_field.SetValue( oh_mod, new Vector2( -256, -256 ) );
				}
			}
		}

		public void ResetMenuObjects() {
			Mod oh_mod = ModLoader.GetMod( "OverhaulMod" );

			if( oh_mod != null ) {
				Type overhaul_mod_type = oh_mod.GetType();
				var menu_data_pos_field = overhaul_mod_type.GetField( "mainMenuDataOffset", BindingFlags.Public | BindingFlags.Static );

				if( menu_data_pos_field != null ) {
					menu_data_pos_field.SetValue( oh_mod, this.OldOverhaulLogoPos );
				}
			}
		}


		////////////////

		public ISet<string> GetTags() {
			ISet<string> tags = new HashSet<string>();

			foreach( var kv in this.TagButtons ) {
				if( !kv.Value.IsTagEnabled ) { continue; }
				tags.Add( kv.Key );
			}

			return tags;
		}


		////////////////

		private void SetCurrentMod( string modname ) {
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
						kv.Value.EnableTag();
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

			PostModInfo.SubmitModInfo( this.ModName, this.GetTags(), on_success, on_fail );

			this.SubUpButton.IsLocked = true;
			this.SubUpButton.Disable();
		}
	}
}
