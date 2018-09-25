using HamstarHelpers.Components.UI.Menu;
using HamstarHelpers.Helpers.DebugHelpers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;


namespace HamstarHelpers.Internals.ModPackBrowser {
	abstract partial class ModTagsUI {
		public IDictionary<string, UIModTagButton> TagButtons = new Dictionary<string, UIModTagButton>();

		public UIText HoverElement;

		protected Vector2 OldOverhaulLogoPos = default( Vector2 );



		////////////////

		protected void InitializeHoverText( string ui_name, string base_context_name ) {
			this.HoverElement = new UIText( "" );
			this.HoverElement.Width.Set( 0, 0 );
			this.HoverElement.Height.Set( 0, 0 );
			this.HoverElement.TextColor = Color.Aquamarine;

			MenuUI.AddMenuLoader( ui_name, "ModHelpers: " + base_context_name + " Tag Hover", this.HoverElement, false );
		}

		protected void InitializeTagButtons( string ui_name, string base_context_name ) {
			int i = 0;

			foreach( var kv in ModTagsUI.Tags ) {
				string tag_text = kv.Key;
				string tag_desc = kv.Value;

				var button = new UIModTagButton( this, false, false, i, tag_text, tag_desc, 0.6f );

				MenuUI.AddMenuLoader( ui_name, "ModHelpers: " + base_context_name + " Tag " + i, button, false );
				this.TagButtons[tag_text] = button;

				i++;
			}
		}


		////////////////

		protected void RecalculateMenuObjects() {
			if( Main.screenWidth < ( 800 + 128 ) || Main.screenHeight < ( 640 + 128 ) ) {
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
		}

		protected void ResetMenuObjects() {
			if( this.OldOverhaulLogoPos != default( Vector2 ) ) {
				Mod oh_mod = ModLoader.GetMod( "OverhaulMod" );

				if( oh_mod != null ) {
					Type overhaul_mod_type = oh_mod.GetType();
					var menu_data_pos_field = overhaul_mod_type.GetField( "mainMenuDataOffset", BindingFlags.Public | BindingFlags.Static );

					if( menu_data_pos_field != null ) {
						menu_data_pos_field.SetValue( oh_mod, this.OldOverhaulLogoPos );
					}
				}
			}
		}


		////////////////

		public abstract void OnTagChange();
	}
}
