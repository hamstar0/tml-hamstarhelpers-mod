using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Internals.UI;
using System;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;


namespace HamstarHelpers.Classes.UI.Elements.Dialogs {
	/// <summary>
	/// Defines a 'Confirm'/'Cancel' prompting UI as a full dialog element. Forces modality.
	/// </summary>
	public class UIPromptDialog : UIDialog {
		/// @private
		protected UITextPanelButton ConfirmButton;
		/// @private
		protected UITextPanelButton CancelButton;

		/// @private
		protected Action<bool> OnReply;

		/// <summary>
		/// Prompt message (title) to display.
		/// </summary>
		public string TitleText { get; private set; }


		////////////////

		/// <param name="theme">Visual appearance.</param>
		/// <param name="width">Recommended width.</param>
		/// <param name="height">Recommended height.</param>
		/// <param name="title">Prompt message (title) to display.</param>
		/// <param name="onReply">Action to perform on pressing a button.</param>
		public UIPromptDialog( UITheme theme, int width, int height, string title, Action<bool> onReply )
				: base( theme, width, height ) {
			this.TitleText = title;
			this.OnReply = onReply;
		}

		////////////////
		
		/// <summary>
		/// Initializes components.
		/// </summary>
		public override void InitializeComponents() {
			var self = this;

			var title = new UIText( this.TitleText );
			this.InnerContainer.Append( (UIElement)title );
			
			this.ConfirmButton = new UITextPanelButton( this.Theme, "Ok" );
			this.ConfirmButton.Top.Set( -32f, 1f );
			this.ConfirmButton.Left.Set( -192f, 0.5f );
			this.ConfirmButton.Width.Set( 128f, 0f );
			this.ConfirmButton.OnClick += ( _, __ ) => {
				self.OnReply( true );
				self.SetDialogToClose = true;
				DialogManager.Instance.UnsetForcedModality();
			};
			this.InnerContainer.Append( this.ConfirmButton );

			this.CancelButton = new UITextPanelButton( this.Theme, "Cancel" );
			this.CancelButton.Top.Set( -32f, 1f );
			this.CancelButton.Left.Set( 64f, 0.5f );
			this.CancelButton.Width.Set( 128f, 0f );
			this.CancelButton.OnClick += ( _, __ ) => {
				self.OnReply( false );
				self.SetDialogToClose = true;
				DialogManager.Instance.UnsetForcedModality();
			};
			this.InnerContainer.Append( this.CancelButton );
		}


		////////////////

		/// <summary>
		/// Opens the dialog, and keeps it open.
		/// </summary>
		public override void Open() {
			base.Open();

			DialogManager.Instance.SetForcedPersistence();
		}


		////////////////

		/// <summary>
		/// Refreshes visual theming.
		/// </summary>
		public override void RefreshTheme() {
			base.RefreshTheme();

			this.CancelButton.RefreshTheme();
			this.ConfirmButton.RefreshTheme();
		}
	}
}
