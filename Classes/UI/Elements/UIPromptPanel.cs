using HamstarHelpers.Classes.UI.Theme;
using System;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;


namespace HamstarHelpers.Classes.UI.Elements.Dialogs {
	/// <summary>
	/// Defines a simple, dialog-like 'Confirm'/'Cancel' prompt panel element.
	/// </summary>
	public class UIPromptPanel : UIThemedPanel {
		/// @private
		protected UITextPanelButton ConfirmButton;
		/// @private
		protected UITextPanelButton CancelButton;

		/// @private
		protected Action ConfirmAction;
		/// @private
		protected Action CancelAction;

		/// @private
		protected bool SetDialogToClose = false;

		/// @private
		protected float TopPixels = 32f;
		/// @private
		protected float TopPercent = 0.5f;
		/// @private
		protected float LeftPixels = 0f;
		/// @private
		protected float LeftPercent = 0.5f;
		/// @private
		protected bool TopCentered = true;
		/// @private
		protected bool LeftCentered = true;

		////////////////

		/// <summary>
		/// Recommended width.
		/// </summary>
		public int MyWidth { get; private set; }  //600, 112
		/// <summary>
		/// Recommended height.
		/// </summary>
		public int MyHeight { get; private set; }

		/// <summary>
		/// Mouse hover text.
		/// </summary>
		public string TitleText { get; private set; }
		
		/// <summary>
		/// Prompt is open.
		/// </summary>
		public bool IsOpen { get; private set; }



		////////////////

		/// <param name="theme">Appearance style.</param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="title"></param>
		/// <param name="confirm"></param>
		/// <param name="cancel"></param>
		public UIPromptPanel( UITheme theme, int width, int height, string title, Action confirm, Action cancel=null )
				: base( theme, true ) {
			this.MyWidth = width;
			this.MyHeight = height;
			this.TitleText = title;
			this.ConfirmAction = confirm;
			this.CancelAction = cancel;
		}

		////////////////

		/// <summary>
		/// Initializes elements of this prompt.
		/// </summary>
		public override void OnInitialize() {
			this.Width.Set( this.MyWidth, 0f );
			this.Height.Set( this.MyHeight, 0f );
			this.MaxWidth.Set( this.MyWidth, 0f );
			this.MaxHeight.Set( this.MyHeight, 0f );
			this.HAlign = 0f;

			this.Recalculate();

			var title = new UIText( this.TitleText );
			this.Append( (UIElement)title );
			
			this.ConfirmButton = new UITextPanelButton( this.Theme, "Ok" );
			this.ConfirmButton.Top.Set( -32f, 1f );
			this.ConfirmButton.Left.Set( (this.CancelAction != null ? -192f : -64f), 0.5f );
			this.ConfirmButton.Width.Set( 128f, 0f );
			this.ConfirmButton.OnClick += ( _, __ ) => {
				this.ConfirmAction();
				this.Remove();
			};
			this.Append( this.ConfirmButton );

			if( this.CancelAction != null ) {
				this.CancelButton = new UITextPanelButton( this.Theme, "Cancel" );
				this.CancelButton.Top.Set( -32f, 1f );
				this.CancelButton.Left.Set( 64f, 0.5f );
				this.CancelButton.Width.Set( 128f, 0f );
				this.CancelButton.OnClick += ( _, __ ) => {
					this.CancelAction.Invoke();
					this.Remove();
				};
				this.Append( this.CancelButton );
			}

			this.RefreshTheme();
		}


		////////////////

		/// <summary>
		/// Refreshes visual theming.
		/// </summary>
		public override void RefreshTheme() {
			this.Theme.ApplyPanel( this );
			this.CancelButton.RefreshTheme();
			this.ConfirmButton.RefreshTheme();
		}


		////////////////

		/// <summary>
		/// Recalculates positions of elements.
		/// </summary>
		public override void Recalculate() {
			base.Recalculate();

			CalculatedStyle dim = this.GetDimensions();
			float offsetX = this.LeftPixels;
			float offsetY = this.TopPixels;

			if( this.LeftCentered ) {
				offsetX -= dim.Width * 0.5f;
			}
			if( this.TopCentered ) {
				offsetY -= dim.Height * 0.5f;
			}

			this.Left.Set( offsetX, this.LeftPercent );
			this.Top.Set( offsetY, this.TopPercent );
		}
	}
}
