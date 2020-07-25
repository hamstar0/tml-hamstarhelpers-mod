using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Services.UI.FreeHUD;
using HamstarHelpers.Helpers.UI;
using HamstarHelpers.Items.NoteItem.UI;
using HamstarHelpers.Services.UI.LayerDisable;

namespace HamstarHelpers.Items.NoteItem {
	/// <summary>
	/// A readable note.
	/// </summary>
	public partial class NoteItem : ModItem {
		private static bool IsDisplayingNote = false;



		////////////////

		private static void DisplayNote( string titleText, string[] pages ) {
			UINote elem = FreeHUD.GetElement( "ModHelpersNote" ) as UINote;

			if( elem == null ) {
				elem = new UINote( titleText, pages );
				elem.Initialize();
				FreeHUD.AddElement( "ModHelpersNote", elem );
			} else {
				elem.SetTitle( titleText );
				elem.SetPages( pages );
			}

			LayerDisable.Instance.DisabledLayers.Add( LayerDisable.InfoAccessoriesBar );
		}


		////

		private static void ClearDisplay() {
			FreeHUD.RemoveElement( "ModHelpersNote" );

			LayerDisable.Instance.DisabledLayers.Remove( LayerDisable.InfoAccessoriesBar );
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
			NoteItem.DisplayNote( this.TitleText, this.Pages );
		}
	}
}
