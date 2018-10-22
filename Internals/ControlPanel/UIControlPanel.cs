using HamstarHelpers.Components.UI;
using HamstarHelpers.Components.UI.Elements;
using HamstarHelpers.Helpers.TmlHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;


namespace HamstarHelpers.Internals.ControlPanel {
	partial class UIControlPanel : UIState {
		private static object ModDataListLock = new object();



		////////////////

		public bool IsOpen { get; private set; }

		private UITheme Theme = new UITheme();
		private ControlPanelLogic Logic = new ControlPanelLogic();
		private UserInterface Backend = null;

		private IList<UIModData> ModDataList = new List<UIModData>();
		private UIModData CurrentModListItem = null;

		private UIElement OuterContainer = null;
		private UIPanel InnerContainer = null;
		private UIList ModListElem = null;
		private UITextPanelButton DialogClose = null;

		private UITextArea IssueTitleInput = null;
		private UITextArea IssueBodyInput = null;

		private UITextPanelButton IssueSubmitButton = null;
		private UITextPanelButton ApplyConfigButton = null;
		private UITextPanelButton ModLockButton = null;

		private UIWebUrl TipUrl = null;
		private UIWebUrl SupportUrl = null;

		private bool HasClicked = false;
		private bool ModListUpdateRequired = false;
		public bool AwaitingReport { get; private set; }

		private bool ResetIssueInput = false;
		private bool SetDialogToClose = false;
		private bool IsPopulatingList = false;



		////////////////

		public UIControlPanel() {
			this.IsOpen = false;
			this.AwaitingReport = false;
			this.InitializeToggler();
		}

		////////////////

		public override void OnInitialize() {
			this.InitializeComponents();
		}

		public override void OnActivate() {
			base.OnActivate();

			this.RefreshApplyConfigButton();

			int count;
			lock( UIControlPanel.ModDataListLock ) {
				count = this.ModDataList.Count;
			}

			if( count == 0 && !this.IsPopulatingList ) {
				this.LoadModListAsync();
			}
		}


		////////////////

		public override void Update( GameTime gameTime ) {
			base.Update( gameTime );

			if( !this.IsOpen ) { return; }

			if( Main.playerInventory || Main.npcChatText != "" ) {
				this.Close();
				return;
			}

			if( this.OuterContainer.IsMouseHovering ) {
				Main.LocalPlayer.mouseInterface = true;
			}

			if( this.AwaitingReport || this.CurrentModListItem == null || !ModMetaDataManager.HasGithub( this.CurrentModListItem.Mod ) ) {
				this.DisableIssueInput();
			} else {
				this.EnableIssueInput();
			}

			if( this.ResetIssueInput ) {
				this.ResetIssueInput = false;
				this.IssueTitleInput.SetText( "" );
				this.IssueBodyInput.SetText( "" );
			}

			if( this.SetDialogToClose ) {
				this.SetDialogToClose = false;
				this.Close();
				return;
			}

			this.UpdateElements( ModHelpersMod.Instance );
		}


		////////////////

		public void RecalculateMe() {
			if( this.Backend != null ) {
				this.Backend.Recalculate();
			} else {
				this.Recalculate();
			}
		}

		public override void Recalculate() {
			base.Recalculate();

			if( this.OuterContainer != null ) {
				this.RecalculateContainer();
			}
		}


		////////////////

		public override void Draw( SpriteBatch sb ) {
			if( !this.IsOpen ) { return; }

			base.Draw( sb );
			this.DrawHoverElements( sb );
		}

		public void DrawHoverElements( SpriteBatch sb ) {
			if( !this.ModListElem.IsMouseHovering ) { return; }
			
			foreach( UIElement elem in this.ModListElem._items ) {
				if( elem.IsMouseHovering ) {
					( (UIModData)elem ).DrawHoverEffects( sb );
					break;
				}
			}

			if( this.TipUrl.IsMouseHovering ) {
				if( !this.TipUrl.WillDrawOwnHoverUrl ) {
					this.TipUrl.DrawHoverEffects( sb );
				}
			}

			if( this.SupportUrl.IsMouseHovering ) {
				if( !this.SupportUrl.WillDrawOwnHoverUrl ) {
					this.SupportUrl.DrawHoverEffects( sb );
				}
			}
		}
	}
}
