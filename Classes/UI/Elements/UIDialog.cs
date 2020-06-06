using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Internals.UI;


namespace HamstarHelpers.Classes.UI.Elements {
	/// <summary>
	/// Defines a UI dialog (stand-alone, centered panel) element. All dialogs are modal, and exclusively capture all interactions until closed.
	/// </summary>
	public abstract partial class UIDialog : UIThemedState {
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
		protected UIThemedPanel InnerContainer = null;

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
		public UIDialog( UITheme theme, int initialWidth, int initialHeight ) : base( theme, true ) {
			this.IsOpen = false;
			this.InitialContainerWidth = initialWidth;
			this.InitialContainerHeight = initialHeight;
		}


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
