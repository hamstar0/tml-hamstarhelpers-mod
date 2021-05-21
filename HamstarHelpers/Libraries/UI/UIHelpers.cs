using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.GameInput;

namespace HamstarHelpers.Helpers.UI {
	/// <summary>
	/// Assorted static "helper" functions pertaining to the in-game UI (positions, interactions, etc.).
	/// </summary>
	public partial class UIHelpers {
		/// <summary></summary>
		public static readonly int MinScreenWidth = 800;
		/// <summary></summary>
		public static readonly int MinScreenHeight = 640;
		/// <summary></summary>
		public static readonly int MaxScreenWidth = 1920;
		/// <summary></summary>
		public static readonly int MaxScreenHeight = 1080;
		/// <summary></summary>
		public static readonly int MinScreenWidthTiles = 80;
		/// <summary></summary>
		public static readonly int MinScreenHeightTiles = 40;
		/// <summary></summary>
		public static readonly int MaxScreenWidthTiles = 124;   //1920 + 64
		/// <summary></summary>
		public static readonly int MaxScreenHeightTiles = 70;   //1080 + 40



		////////////////

		/// <summary>
		/// Reports whether a given keyboard key was just pressed. Used primarily for text field inputs.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static bool JustPressedKey( Keys key ) {
			return Main.inputText.IsKeyDown( key ) && !Main.oldInputText.IsKeyDown( key );
		}


		////

		/// <summary>
		/// Determines if game/player conditions are suitable for a UI to be available.
		/// </summary>
		/// <param name="notTabbedAway"></param>
		/// <param name="gameNotPaused"></param>
		/// <param name="mouseNotInUse">`!Player.mouseInterface`</param>
		/// <param name="keyboardNotInVanillaUI">No chat input, sign editing, or chest label editing.</param>
		/// <param name="keyboardNotInCustomUI"></param>
		/// <param name="playerAvailable">Not dead or stunned.</param>
		/// <param name="playerNotWieldingItem"></param>
		/// <param name="playerNotTalkingToNPC"></param>
		/// <param name="noFullscreenMap"></param>
		/// <param name="notShowingMouseIcon"></param>
		/// <returns></returns>
		public static bool IsUIAvailable(
					bool notTabbedAway = true,
					bool gameNotPaused = false,
					bool mouseNotInUse = false,
					bool keyboardNotInVanillaUI = false,
					bool keyboardNotInCustomUI = false,
					bool playerAvailable = false,
					bool playerNotWieldingItem = false,
					bool playerNotTalkingToNPC = false,
					bool noFullscreenMap = false,
					bool notShowingMouseIcon = false ) {
			var plr = Main.LocalPlayer;
/*if( !(!notTabbedAway || Main.hasFocus) ) { Main.NewText( "hasFocus" ); }
if( !(!gameNotPaused || !Main.gamePaused) ) { Main.NewText( "gamePaused" ); }
if( !(!mouseNotInUse || !plr.mouseInterface) ) { Main.NewText( "mouseInterface" ); }
if( !(!keyboardNotInVanillaUI || !Main.drawingPlayerChat) ) { Main.NewText( "drawingPlayerChat" ); }
if( !(!keyboardNotInVanillaUI || !Main.editSign) ) { Main.NewText( "editSign" ); }
if( !(!keyboardNotInVanillaUI || !Main.editChest) ) { Main.NewText( "editChest" ); }
if( !(!keyboardNotInCustomUI || !PlayerInput.WritingText) ) { Main.NewText( "WritingText" ); }
if( !(!keyboardNotInCustomUI || !Main.blockInput) ) { Main.NewText( "blockInput" ); }
if( !(!playerAvailable || !plr.dead) ) { Main.NewText( "dead" ); }
if( !(!playerAvailable || !plr.CCed) ) { Main.NewText( "CCed" ); }
if( !(!playerNotTalkingToNPC || plr.talkNPC == -1) ) { Main.NewText( "talkNPC" ); }
if( !(!playerNotWieldingItem || (plr.itemTime == 0 && plr.itemAnimation == 0)) ) { Main.NewText( "itemTime" ); }
if( !(!noFullscreenMap || !Main.mapFullscreen) ) { Main.NewText( "mapFullscreen" ); }
if( !(!notShowingMouseIcon || !Main.HoveringOverAnNPC) ) { Main.NewText( "HoveringOverAnNPC" ); }
if( !(!notShowingMouseIcon || !plr.showItemIcon) ) { Main.NewText( "showItemIcon" ); }*/
			return (!notTabbedAway || Main.hasFocus) &&
				(!gameNotPaused || !Main.gamePaused) &&
				(!mouseNotInUse || !plr.mouseInterface) &&
				(!keyboardNotInVanillaUI || !Main.drawingPlayerChat) &&
				(!keyboardNotInVanillaUI || !Main.editSign) &&
				(!keyboardNotInVanillaUI || !Main.editChest) &&
				(!keyboardNotInCustomUI || !PlayerInput.WritingText) &&
				(!keyboardNotInCustomUI || !Main.blockInput) &&
				(!playerAvailable || !plr.dead) &&
				(!playerAvailable || !plr.CCed) &&
				(!playerNotTalkingToNPC || plr.talkNPC == -1) &&
				(!playerNotWieldingItem || (plr.itemTime == 0 && plr.itemAnimation == 0)) &&
				(!noFullscreenMap || !Main.mapFullscreen) &&
				(!notShowingMouseIcon || !Main.HoveringOverAnNPC) &&
				(!notShowingMouseIcon || !plr.showItemIcon);
		}


		////////////////

		/// <summary>
		/// Gets the position to use to display a given string of text near the mouse position so as to fit on screen.
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static Vector2 GetHoverTipPosition( string str ) {
			Vector2 dim = Main.fontMouseText.MeasureString( str );
			Vector2 pos = new Vector2( Main.mouseX + 10f, Main.mouseY + 10f );

			if( ( pos.X + dim.X ) > Main.screenWidth ) {
				pos.X = Main.screenWidth - dim.X;
			}

			return pos;
		}


		////////////////

		/// <summary>
		/// Indicates if the mouse is within a given screen rectangle.
		/// </summary>
		/// <param name="rect"></param>
		/// <returns></returns>
		public static bool MouseInRectangle( Rectangle rect ) {
			if( Main.mouseX >= rect.X && Main.mouseX < ( rect.X + rect.Width ) ) {
				if( Main.mouseY >= rect.Y && Main.mouseY < ( rect.Y + rect.Height ) ) {
					return true;
				}
			}
			return false;
		}
	}
}
