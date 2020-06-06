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
		/// Initializes containers and inner components.
		/// </summary>
		public override void OnInitialize() {
			this.InitializeContainer( this.InitialContainerWidth, this.InitialContainerHeight );
			this.InitializeComponents();

			this.RefreshTheme();
		}


		/// <summary>
		/// Initializes inner (content-bearing) and outer (screen-positioned) containers.
		/// </summary>
		/// <param name="width">Outer container width.</param>
		/// <param name="height">Outer container height.</param>
		public void InitializeContainer( int width, int height ) {
			this.OuterContainer = new UIElement();
			this.OuterContainer.Width.Set( width, 0f );
			this.OuterContainer.Height.Set( height, 0f );
			this.OuterContainer.MaxWidth.Set( width, 0f );
			this.OuterContainer.MaxHeight.Set( height, 0f );
			this.OuterContainer.HAlign = 0f;
			this.Append( this.OuterContainer );

			this.RefreshOuterContainerPosition();

			this.InnerContainer = new UIThemedPanel( this.Theme, false );
			this.InnerContainer.Width.Set( 0f, 1f );
			this.InnerContainer.Height.Set( 0f, 1f );
			this.OuterContainer.Append( (UIElement)this.InnerContainer );
		}


		/// <summary>
		/// Used to initialize dialog's contents.
		/// </summary>
		public abstract void InitializeComponents();
	}
}
