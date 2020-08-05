using System;
using Terraria;
using Terraria.UI;
using HamstarHelpers.Classes.UI.Theme;


namespace HamstarHelpers.Classes.UI.Elements {
	/// <summary>
	/// Defines a simpler append-only text field input element (no panel). Suited for main menu use.
	/// </summary>
	public partial class UITextInputElement : UIElement, IToggleable {
		/// @private
		[Obsolete( "use HasFocus", true )]
		public bool IsSelected {
			get => this.HasFocus;
			private set => this.HasFocus = value;
		}
	}
}
