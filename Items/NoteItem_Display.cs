using Terraria;
using Terraria.UI;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Elements;
using HamstarHelpers.Services.UI.FreeHUD;
using HamstarHelpers.Helpers.UI;


namespace HamstarHelpers.Items {
	/// <summary>
	/// A readable note.
	/// </summary>
	public partial class NoteItem : ModItem {
		private static bool IsDisplayingNote = false;



		////////////////

		private static void SetDisplay( string titleText, string bodyText ) {
			NoteItem.SetDisplayElem( "ModHelpersNoteTitle", titleText, 1f, true, 128 );
			NoteItem.SetDisplayElem( "ModHelpersNoteBody", bodyText, 1f, false, 176 );
		}

		private static void SetDisplayElem( string elemName, string text, float scale, bool isLarge, float yPos ) {
			UIText elem = FreeHUD.GetElement( elemName ) as UIText;

			if( elem == null ) {
				elem = new UIText( text );
				FreeHUD.AddElement( elemName, elem );
			}

			elem.SetText( text, scale, isLarge );

			CalculatedStyle dim = elem.GetDimensions();

			elem.Left.Set( (scale * dim.Width) / -2f, 0.5f );
			elem.Top.Set( (scale * dim.Height) + yPos, 0f );

			elem.Recalculate();
		}


		////

		private static void ClearDisplay() {
			FreeHUD.RemoveElement( "ModHelpersNoteTitle" );
			//FreeHUD.RemoveElement( "ModHelpersNoteBody" );
		}


		////////////////

		internal static void UpdateDisplay() {
			if( !NoteItem.IsDisplayingNote ) {
				return;
			}

			bool uiAvailable = UIHelpers.IsUIAvailable(
				//mouseNotInUse: true,
				playerAvailable: true,
				playerNotTalkingToNPC: true,
				noFullscreenMap: true
			);

			if( Main.gameMenu || Main.playerInventory || !uiAvailable ) {
				NoteItem.IsDisplayingNote = false;
				NoteItem.ClearDisplay();
			}
		}



		////////////////

		private void DisplayCurrentNote() {
			Main.playerInventory = false;

			NoteItem.IsDisplayingNote = true;
			NoteItem.SetDisplay( this.TitleText, this.BodyTexts[0] );
		}
	}
}
