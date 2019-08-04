using HamstarHelpers.Components.UI.Theme;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;


namespace HamstarHelpers.Components.UI.Elements {
	/// <summary>
	/// Defines a UI dialog (stand-alone, centered panel) element. All dialogs are modal, and exclusively capture all interactions until closed.
	/// </summary>
	public abstract class UIDialog : UIThemedState {
		/// <summary>
		/// Recommended dialog width.
		/// </summary>
		public virtual int InitialContainerWidth { get; protected set; }
		/// <summary>
		/// Recommended dialog height.
		/// </summary>
		public virtual int InitialContainerHeight { get; protected set; }

		/// <summary>
		/// Indicates if dialog is open.
		/// </summary>
		public bool IsOpen { get; private set; }

		/// @private
		protected UserInterface Backend = null;

		/// @private
		protected UIElement OuterContainer = null;
		/// @private
		protected UIPanel InnerContainer = null;

		/// @private
		protected bool SetDialogToClose = false;

		private float TopPixels = 32f;
		private float TopPercent = 0.5f;
		private float LeftPixels = 0f;
		private float LeftPercent = 0.5f;
		private bool TopCentered = true;
		private bool LeftCentered = true;
		


		////////////////

		/// <param name="theme">Appearance style.</param>
		/// <param name="initialWidth">Recommended width.</param>
		/// <param name="initialHeight">Recommended height.</param>
		public UIDialog( UITheme theme, int initialWidth, int initialHeight ) : base( theme ) {
			this.IsOpen = false;
			this.InitialContainerWidth = initialWidth;
			this.InitialContainerHeight = initialHeight;
		}

		////////////////
		
		/// <summary>
		/// Initializes containers and inner components.
		/// </summary>
		public override void OnInitialize() {
			this.InitializeContainer( this.InitialContainerWidth, this.InitialContainerHeight );
			this.InitializeComponents();
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

			this.RecalculateOuterContainer();

			this.InnerContainer = new UIPanel();
			this.InnerContainer.Width.Set( 0f, 1f );
			this.InnerContainer.Height.Set( 0f, 1f );
			this.OuterContainer.Append( (UIElement)this.InnerContainer );

			this.Theme.ApplyPanel( this.InnerContainer );
		}


		/// <summary>
		/// Used to initialize dialog's contents.
		/// </summary>
		public abstract void InitializeComponents();


		////////////////

		/// <summary>
		/// Updates state.
		/// </summary>
		/// <param name="gameTime">Unused.</param>
		public override void Update( GameTime gameTime ) {
			base.Update( gameTime );

			if( !this.IsOpen ) {
				return;
			}

			if( Main.playerInventory || Main.npcChatText != "" || this.Backend == null || this.Backend.CurrentState != this ) {
				this.Close();
				return;
			}
			
			if( this.SetDialogToClose ) {
				this.SetDialogToClose = false;
				this.Close();
				return;
			}

			if( this.OuterContainer.IsMouseHovering ) {
				Main.LocalPlayer.mouseInterface = true;
			}
		}

		////////////////

		/// <summary>
		/// Intended to replace `Recalculate()` for technical reasons. Recalculates positions of dialog elements.
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
		public sealed override void Recalculate() {
			base.Recalculate();

			if( this.OuterContainer != null ) {
				this.RecalculateOuterContainer();
			}
		}

		/// <summary>
		/// Recalculates position of outer container
		/// </summary>
		public void RecalculateOuterContainer() {
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

		/// <returns>`true` if dialog can be opened (UI not otherwise captured, no other dialogs, etc.).</returns>
		public virtual bool CanOpen() {
			return !this.IsOpen && !Main.inFancyUI &&
				(DialogManager.Instance != null && DialogManager.Instance.CurrentDialog == null);
		}


		/// <summary>
		/// Opens the dialog. All input and UI context is captured.
		/// </summary>
		public virtual void Open() {
			this.IsOpen = true;

			Main.playerInventory = false;
			Main.editChest = false;
			Main.npcChatText = "";

			Main.inFancyUI = true;
			Main.InGameUI.SetState( (UIState)this );

			this.Backend = Main.InGameUI;

			this.RecalculateMe();

			if( DialogManager.Instance != null ) {
				DialogManager.Instance.SetCurrentDialog( this );
			}
		}


		/// <summary>
		/// Closes the current dialog. All UI context is reverted to the game's normal state.
		/// </summary>
		public virtual void Close() {
			this.IsOpen = false;

			if( Main.InGameUI.CurrentState == this ) {
				Main.inFancyUI = false;
				Main.InGameUI.SetState( (UIState)null );
			}

			this.Backend = null;
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
			this.RecalculateOuterContainer();
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
			this.RecalculateOuterContainer();
		}


		////////////////

		/// <summary>
		/// Refreshes visual theming.
		/// </summary>
		public override void RefreshTheme() {
			base.RefreshTheme();
			this.Theme.ApplyPanel( this.InnerContainer );
		}


		////////////////

		/// <summary>
		/// Draws the dialog if it's open.
		/// </summary>
		/// <param name="sb">SpriteBatch to draw to. Typically given `Main.spriteBatch`.</param>
		public override void Draw( SpriteBatch sb ) {
			if( !this.IsOpen ) {
				return;
			}

			base.Draw( sb );
		}
	}
}
