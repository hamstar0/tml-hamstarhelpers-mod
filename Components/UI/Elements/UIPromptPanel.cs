using System;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;


namespace HamstarHelpers.Components.UI.Elements.Dialogs {
	public class UIPromptPanel : UIPanel {
		protected UITextPanelButton ConfirmButton;
		protected UITextPanelButton CancelButton;

		protected Action ConfirmAction;
		protected Action CancelAction;

		protected bool SetDialogToClose = false;

		protected float TopPixels = 32f;
		protected float TopPercent = 0.5f;
		protected float LeftPixels = 0f;
		protected float LeftPercent = 0.5f;
		protected bool TopCentered = true;
		protected bool LeftCentered = true;

		////////////////

		public int MyWidth { get; private set; }  //600, 112
		public int MyHeight { get; private set; }

		public UITheme Theme { get; protected set; }
		public string TitleText { get; private set; }
		
		public bool IsOpen { get; private set; }



		////////////////

		public UIPromptPanel( UITheme theme, int width, int height, string title, Action confirm, Action cancel=null ) {
			this.Theme = theme;
			this.MyWidth = width;
			this.MyHeight = height;
			this.TitleText = title;
			this.ConfirmAction = confirm;
			this.CancelAction = cancel;
		}

		////////////////

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
			this.ConfirmButton.OnClick += delegate ( UIMouseEvent evt, UIElement listeningElement ) {
				this.ConfirmAction();
				this.Remove();
			};
			this.Append( this.ConfirmButton );

			if( this.CancelAction != null ) {
				this.CancelButton = new UITextPanelButton( this.Theme, "Cancel" );
				this.CancelButton.Top.Set( -32f, 1f );
				this.CancelButton.Left.Set( 64f, 0.5f );
				this.CancelButton.Width.Set( 128f, 0f );
				this.CancelButton.OnClick += delegate ( UIMouseEvent evt, UIElement listeningElement ) {
					this.CancelAction.Invoke();
					this.Remove();
				};
				this.Append( this.CancelButton );
			}

			this.Theme.ApplyPanel( this );
		}


		////////////////

		public virtual void RefreshTheme() {
			this.CancelButton.RefreshTheme();
			this.ConfirmButton.RefreshTheme();
		}


		////////////////

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
