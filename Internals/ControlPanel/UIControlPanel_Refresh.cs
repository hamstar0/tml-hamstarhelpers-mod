using HamstarHelpers.Services.ModHelpers;
using Terraria;
using Terraria.UI;


namespace HamstarHelpers.Internals.ControlPanel {
	/// @private
	partial class UIControlPanel : UIState {
		public void RecalculateContainer() {
			CalculatedStyle dim = this.OuterContainer.GetDimensions();

			this.OuterContainer.Top.Set( ( dim.Height * -0.5f ) + 32, 0.5f );
			this.OuterContainer.Left.Set( ( dim.Width * -0.5f ), 0.5f );
		}
	}
}
