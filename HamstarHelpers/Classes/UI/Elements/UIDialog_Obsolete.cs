using System;
using Terraria;
using HamstarHelpers.Classes.UI.Theme;


namespace HamstarHelpers.Classes.UI.Elements {
	/// <summary>
	/// Defines a UI dialog (stand-alone, centered panel) element. All dialogs are modal, and exclusively capture all
	/// interactions until closed.
	/// </summary>
	public abstract partial class UIDialog : UIThemedState {
		/// @private
		[Obsolete( "use InitializeContainers", true )]
		public void InitializeContainer( int width, int height ) {
			this.InitializeContainers( width, height );
		}

		/// @private
		[Obsolete( "use Recalculate", true )]
		public virtual void RecalculateMe() {
			if( this.Backend != null ) {
				this.Backend.Recalculate();
			} else {
				this.Recalculate();
			}
		}

		/// @private
		[Obsolete( "use RecalculateOuterContainerPosition", true )]
		public void RecalculateOuterContainer() {
			this.RefreshOuterContainerPosition();
		}

		/// @private
		[Obsolete( "use alternative", true )]
		public void SetLeftPosition( float pixels, float percent, bool centered ) {
			this.SetLeftPosition( pixels, percent, centered ? 0.5f : 0f );
		}

		/// @private
		[Obsolete( "use alternative", true )]
		public void SetTopPosition( float pixels, float percent, bool centered ) {
			this.SetTopPosition( pixels, percent, centered ? 0.5f : 0f );
		}
	}
}
