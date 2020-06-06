using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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


		/// <summary>Horizontal position within the panel to align upon.</summary>
		protected float OriginPercentHorizontal = 0.5f;

		/// <summary>Vertical position within the panel to align upon.</summary>
		protected float OriginPercentVertical = 0.5f;

		private float TopPixels = 32f;
		private float TopPercent = 0.5f;
		private float LeftPixels = 0f;
		private float LeftPercent = 0.5f;



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

		/// @private
		public override void Update( GameTime gameTime ) {
			base.Update( gameTime );

			if( !this.IsOpen ) {
				return;
			}

			this.UpdateOpenState();

			if( this.OuterContainer.IsMouseHovering ) {
				Main.LocalPlayer.mouseInterface = true;
			}
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
