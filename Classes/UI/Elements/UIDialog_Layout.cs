using System;
using Terraria;
using Terraria.UI;
using HamstarHelpers.Classes.UI.Theme;


namespace HamstarHelpers.Classes.UI.Elements {
	/// <summary>
	/// Defines a UI dialog (stand-alone, centered panel) element. All dialogs are modal, and exclusively capture all
	/// interactions until closed.
	/// </summary>
	public abstract partial class UIDialog : UIThemedState {
		/// <summary>
		/// Recalculates position and size of dialog and all its contents.
		/// </summary>
		public override void Recalculate() {
			if( this.OuterContainer != null ) {
				this.RefreshOuterContainerPosition();
			}

			base.Recalculate();
		}


		////////////////

		/// <summary>
		/// Recalculates position of outer container
		/// </summary>
		public void RefreshOuterContainerPosition() {
			CalculatedStyle dim = this.OuterContainer.GetOuterDimensions();
			float offsetX = this.LeftPixels;
			float offsetY = this.TopPixels;

			offsetX -= dim.Width * this.OriginPercentHorizontal;
			offsetY -= dim.Height * this.OriginPercentVertical;
			
			this.OuterContainer.Left.Set( offsetX, this.LeftPercent );
			this.OuterContainer.Top.Set( offsetY, this.TopPercent );
		}


		////////////////

		/// <summary>
		/// Repositions the dialog horizontally (via standard `StyleDimension.Set(...)`). Dialog layout is not recalculated.
		/// </summary>
		/// <param name="pixels">Pixel amount from the left.</param>
		/// <param name="percent">Percent amount from the left.</param>
		/// <param name="originPercent">Adjusts the position within the panel to align upon.</param>
		public void SetLeftPosition( float pixels, float percent, float originPercent = 0.5f ) {
			this.LeftPixels = pixels;
			this.LeftPercent = percent;
			this.OriginPercentHorizontal = originPercent;
		}

		/// <summary>
		/// Repositions the dialog vertically (via standard `StyleDimension.Set(...)`). Dialog layout is not recalculated.
		/// </summary>
		/// <param name="pixels">Pixel amount from the top.</param>
		/// <param name="percent">Percent amount from the top.</param>
		/// <param name="originPercent">Adjusts the position within the panel to align upon.</param>
		public void SetTopPosition( float pixels, float percent, float originPercent = 0.5f ) {
			this.TopPixels = pixels;
			this.TopPercent = percent;
			this.OriginPercentVertical = originPercent;
		}
	}
}
