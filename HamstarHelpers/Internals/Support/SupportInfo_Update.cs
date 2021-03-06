﻿using System;
using Terraria;
using Terraria.GameContent.UI.Elements;
using HamstarHelpers.Classes.UI.Elements;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TModLoader.Menus;
using HamstarHelpers.Internals.ModTags.ModBrowser.MenuContext;
using HamstarHelpers.Internals.ModTags.ModInfo.MenuContext;
using HamstarHelpers.Services.Timers;


namespace HamstarHelpers.Internals.Menus.Support {
	/// @private
	internal partial class SupportInfoDisplay {
		public void Update() {
			if( !Main.hasFocus ) {
				return;
			}

			bool isClicking = Main.mouseLeft && !this.IsClicking;

			this.IsClicking = Main.mouseLeft;
			this.IsHoveringBox = this.GetInnerBox()
				.Contains( Main.mouseX, Main.mouseY );

			this.UpdateElementMouseInteractions( isClicking );

			if( isClicking && this.IsHoveringBox ) {
				if( !this.IsExtended ) {
					this.IsExtended = true;

					this.ExtendLabel.Remove();
					this.Elements.Remove( this.ExtendLabel );
					this.ExpandUI();
				}
			}

			if( this.EnableModTagsLabel != null ) {
				this.UpdateModTagsLabel( isClicking );
			}
		}


		private void UpdateElementMouseInteractions( bool isClicking ) {
			for( int i = 0; i < this.Elements.Count; i++ ) {
				var elem = this.Elements[i];
				if( !( elem is UIWebUrl ) ) {
					if( elem is UIText && ( (UIText)elem ).Text != "..." ) {
						continue;
					}
					if( !( elem is UIImageUrl ) ) {
						continue;
					}
				}

				bool isElementHover = elem.GetOuterDimensions()
					.ToRectangle()
					.Contains( Main.mouseX, Main.mouseY );

				if( isElementHover ) {
					if( isClicking ) { elem.Click( null ); }
					elem.MouseOver( null );
				} else {
					if( elem.IsMouseHovering ) {
						elem.MouseOut( null );
					}
				}
			}
		}

		private void UpdateModTagsLabel( bool isClicking ) {
			var mymod = ModHelpersMod.Instance;
			
			if( mymod.Data.ModTagsOpened ) {    // This exists because ModHelpersData loads late
				this.RemoveModTagsMessage();	// This Update no longer gets called

				ModTagsEditorMenuContext.Initialize();
				ModTagsModBrowserMenuContext.Initialize();
			}

			bool isHoveringModTagsLabel = this.EnableModTagsLabel?.GetOuterDimensions()
				.ToRectangle()
				.Contains( Main.mouseX, Main.mouseY )
				?? false;
			
			if( isClicking && isHoveringModTagsLabel ) {
				mymod.Data.ModTagsOpened = true;

				mymod.SaveModData();

				this.RemoveModTagsMessage();    // This Update no longer gets called

				ModTagsEditorMenuContext.Initialize();
				ModTagsModBrowserMenuContext.Initialize();

				Timers.SetTimer( "ModHelpersModBrowserActivate", 5, true, () => {
					MainMenuHelpers.OpenModBrowserMenu();
					return false;
				} );
			}
		}
	}
}
