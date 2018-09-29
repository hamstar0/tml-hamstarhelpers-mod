using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Internals.ModTags.UI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;


namespace HamstarHelpers.Internals.ModTags {
	abstract partial class TagsMenuContextBase {
		public abstract string UIName { get; }
		public abstract string ContextName { get; }

		////////////////

		internal IDictionary<string, UITagButton> TagButtons = new Dictionary<string, UITagButton>();
		internal UIText HoverElement = null;
		protected UIState MyUI = null;

		protected Vector2 OldOverhaulLogoPos = default( Vector2 );



		////////////////

		protected void RecalculateMenuObjects() {
			if( Main.screenWidth < (800 + 128) || Main.screenHeight < (640 + 128) ) {
				Mod oh_mod = ModLoader.GetMod( "OverhaulMod" );

				if( oh_mod != null ) {
					Type oh_mod_type = oh_mod.GetType();
					var oh_logo_pos_field = oh_mod_type.GetField( "mainMenuDataOffset", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static );

					if( oh_logo_pos_field != null ) {
						if( this.OldOverhaulLogoPos != default( Vector2 ) ) {
							this.OldOverhaulLogoPos = (Vector2)oh_logo_pos_field.GetValue( oh_mod );
						}

						oh_logo_pos_field.SetValue( oh_mod, new Vector2( -384, -384 ) );
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

		public abstract void OnTagStateChange( UITagButton tag_button );

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

		public void DisableTagButtons() {
			foreach( var kv in this.TagButtons ) {
				kv.Value.Disable();
			}
		}

		////////////////

		public void ResetTagButtons() {
			foreach( var kv in this.TagButtons ) {
				kv.Value.SetTagState( 0 );
			}
		}
	}
}
