using Terraria;
using Terraria.UI;
using HamstarHelpers.Helpers.Debug;


namespace HamstarHelpers.Internals.ControlPanel {
	/// @private
	partial class UIControlPanel : UIState {
		private bool _IsRecalculating = false;

		public override void Recalculate() {
			if( !this._IsRecalculating && this.Backend != null ) {
				this._IsRecalculating = true;

				this.Backend.Recalculate();
			} else {
				if( this.OuterContainer != null ) {
					this.RecalculateContainerDimensions();
				}
				base.Recalculate();
			}

			this._IsRecalculating = false;
		}


		////////////////

		public void RecalculateContainerDimensions() {
			CalculatedStyle dim = this.OuterContainer.GetDimensions();
			float offsetX = dim.Width * -0.5f;
			float offsetY = (dim.Height * -0.5f) + 32;

			this.OuterContainer.Left.Set( offsetX * Main.UIScale, 0.5f );
			this.OuterContainer.Top.Set( offsetY * Main.UIScale, 0.5f );
		}
	}
}
