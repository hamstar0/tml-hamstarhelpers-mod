using System;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;


namespace HamstarHelpers.Components.UI.Elements.Dialogs {
	public class UIPromptDialog : UIDialog {
		protected UITextPanelButton ConfirmButton;
		protected UITextPanelButton CancelButton;

		protected Action ConfirmAction;
		protected Action CancelAction;

		public string TitleText { get; private set; }


		////////////////

		public UIPromptDialog( UITheme theme, int width, int height, string title, Action confirm, Action cancel=null )
				: base( theme, width, height ) {
			this.TitleText = title;
			this.ConfirmAction = confirm;
			this.CancelAction = cancel;
		}

		////////////////

		public override void InitializeComponents() {
			var self = this;

			var title = new UIText( this.TitleText );
			this.InnerContainer.Append( (UIElement)title );
			
			this.ConfirmButton = new UITextPanelButton( this.Theme, "Ok" );
			this.ConfirmButton.Top.Set( -32f, 1f );
			this.ConfirmButton.Left.Set( -192f, 0.5f );
			this.ConfirmButton.Width.Set( 128f, 0f );
			this.ConfirmButton.OnClick += delegate ( UIMouseEvent evt, UIElement listening_element ) {
				self.ConfirmAction();
				self.SetDialogToClose = true;
				DialogManager.Instance.UnsetForcedModality();
			};
			this.InnerContainer.Append( this.ConfirmButton );

			this.CancelButton = new UITextPanelButton( this.Theme, "Cancel" );
			this.CancelButton.Top.Set( -32f, 1f );
			this.CancelButton.Left.Set( 64f, 0.5f );
			this.CancelButton.Width.Set( 128f, 0f );
			this.CancelButton.OnClick += delegate ( UIMouseEvent evt, UIElement listening_element ) {
				if( self.CancelAction != null ) {
					self.CancelAction.Invoke();
				}
				self.SetDialogToClose = true;
				DialogManager.Instance.UnsetForcedModality();
			};
			this.InnerContainer.Append( this.CancelButton );
		}


		////////////////

		public override void Open() {
			base.Open();

			DialogManager.Instance.SetForcedModality();
		}


		////////////////

		public override void RefreshTheme() {
			base.RefreshTheme();

			this.CancelButton.RefreshTheme();
			this.ConfirmButton.RefreshTheme();
		}
	}
}
