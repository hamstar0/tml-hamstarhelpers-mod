using System;
using Terraria;
using HamstarHelpers.Classes.UI.Theme;


namespace HamstarHelpers.Classes.UI.Elements {
	/// <summary>
	/// Defines a UI dialog (stand-alone, centered panel) element. All dialogs are modal, and exclusively capture all interactions until closed.
	/// </summary>
	public abstract partial class UIDialog : UIThemedState {
		/// @private
		[Obsolete( "use RecalculateOuterContainerPosition", true )]
		public void RecalculateOuterContainer() {
			this.RefreshOuterContainerPosition();
		}
	}
}
