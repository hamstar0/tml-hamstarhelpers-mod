using HamstarHelpers.Components.UI.Menu;
using HamstarHelpers.Helpers.DebugHelpers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;


namespace HamstarHelpers.Internals.ModTags {
	abstract partial class ModTagsUI {
		protected abstract string UIName { get; }
		protected abstract string BaseContextName { get; }

		////////////////

		internal IDictionary<string, UIModTagButton> TagButtons = new Dictionary<string, UIModTagButton>();
		internal UIText HoverElement;

		protected UIState MyUI = null;

		protected Vector2 OldOverhaulLogoPos = default( Vector2 );



		////////////////

		public ModTagsUI( bool can_disable_tags ) {
			MenuUI.AddMenuLoader( this.UIName, "ModHelpers: " + this.BaseContextName + " Set UI",
				ui => { this.MyUI = ui; },
				ui => { this.MyUI = null; }
			);

			this.InitializeTagButtons( can_disable_tags );
			this.InitializeUI();
		}

		////////////////

		protected abstract void InitializeUI();

		protected void InitializeHoverText() {
			this.HoverElement = new UIText( "" );
			this.HoverElement.Width.Set( 0, 0 );
			this.HoverElement.Height.Set( 0, 0 );
			this.HoverElement.TextColor = Color.Aquamarine;

			MenuUI.AddMenuLoader( this.UIName, "ModHelpers: " + this.BaseContextName + " Tag Hover", this.HoverElement, false );
		}

		protected void InitializeTagButtons( bool can_disable_tags ) {
			int i = 0;

			foreach( var kv in ModTagsUI.Tags ) {
				string tag_text = kv.Key;
				string tag_desc = kv.Value;

				var button = new UIModTagButton( this, i, tag_text, tag_desc, can_disable_tags );

				MenuUI.AddMenuLoader( this.UIName, "ModHelpers: " + this.BaseContextName + " Tag " + i, button, false );
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

		public abstract void OnTagStateChange( UIModTagButton tag_button );

		public ISet<string> GetTagsOfState( int state ) {
			ISet<string> tags = new HashSet<string>();

			foreach( var kv in this.TagButtons ) {
				if( kv.Value.TagState == state ) {
					tags.Add( kv.Key );
				}
			}
			return tags;
		}


		////////////////

		public void EnableTagButtons() {
			foreach( var kv in this.TagButtons ) {
				kv.Value.Enable();
			}
		}
	}
}
