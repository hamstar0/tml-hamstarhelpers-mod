using System;
using Terraria;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Helpers.Debug;


namespace HamstarHelpers.Classes.UI.Elements {
	/// <summary>
	/// Defines a text area UI panel element with crop-to-fit text input. Captures focus while in use. Does not currently implement
	/// multi-line support (yet).
	/// </summary>
	public partial class UITextInputAreaPanel : UIThemedPanel, IToggleable {
		/// @private
		[Obsolete( "use SetTextWithValidation", true )]
		public void SetText( string text ) {
			this.SetTextWithValidation( text );
		}
	}
}
