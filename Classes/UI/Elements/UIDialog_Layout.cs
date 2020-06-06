using System;
using Terraria;
using Terraria.UI;
using HamstarHelpers.Classes.UI.Theme;


namespace HamstarHelpers.Classes.UI.Elements {
	/// <summary>
	/// Defines a UI dialog (stand-alone, centered panel) element. All dialogs are modal, and exclusively capture all interactions until closed.
	/// </summary>
	public abstract partial class UIDialog : UIThemedState {
		/// <summary>
		/// Intended to replace `Recalculate()` for technical reasons (accomodates 'backend' `UserInterface`). Recalculates
		/// positions of dialog elements.
		/// </summary>
		public virtual void RecalculateMe() {	// Call this instead of Recalculate
			if( this.Backend != null ) {
				this.Backend.Recalculate();
			} else {
				this.Recalculate();
			}
		}

		/// @private
		[Obsolete("use RecalculateMe()")]
#pragma warning disable CS0809 // Obsolete member overrides non-obsolete member
		public sealed override void Recalculate() {
			if( this.OuterContainer != null ) {
				this.RefreshOuterContainerPosition();
			}

			base.Recalculate();
		}
#pragma warning restore CS0809 // Obsolete member overrides non-obsolete member


		////////////////

		/// <summary>
		/// Recalculates position of outer container
		/// </summary>
		public void RefreshOuterContainerPosition() {
			CalculatedStyle dim = this.OuterContainer.GetDimensions();
			float offsetX = this.LeftPixels;
			float offsetY = this.TopPixels;

			if( this.LeftCentered ) {
				offsetX -= dim.Width * 0.5f;
			}
			if( this.TopCentered ) {
				offsetY -= dim.Height * 0.5f;
			}
			
			this.OuterContainer.Left.Set( offsetX, this.LeftPercent );
			this.OuterContainer.Top.Set( offsetY, this.TopPercent );
		}


		////////////////

		/// <summary>
		/// Repositions the dialog horizontally (via standard `StyleDimension.Set(...)`).
		/// </summary>
		/// <param name="pixels">Pixel amount from the left.</param>
		/// <param name="percent">Percent amount from the left.</param>
		/// <param name="centered">Subtracts half the screen width from the pixel amount.</param>
		public void SetLeftPosition( float pixels, float percent, bool centered ) {
			this.LeftPixels = pixels;
			this.LeftPercent = percent;
			this.LeftCentered = centered;

			this.RefreshOuterContainerPosition();
			this.RecalculateMe();
		}

		/// <summary>
		/// Repositions the dialog vertically (via standard `StyleDimension.Set(...)`).
		/// </summary>
		/// <param name="pixels">Pixel amount from the top.</param>
		/// <param name="percent">Percent amount from the top.</param>
		/// <param name="centered">Subtracts half the screen height from the pixel amount.</param>
		public void SetTopPosition( float pixels, float percent, bool centered ) {
			this.TopPixels = pixels;
			this.TopPercent = percent;
			this.TopCentered = centered;

			this.RefreshOuterContainerPosition();
			this.RecalculateMe();
		}
	}
}
