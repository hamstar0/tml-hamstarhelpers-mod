using HamstarHelpers.TmlHelpers;
using HamstarHelpers.TmlHelpers.ModHelpers;
using HamstarHelpers.UIHelpers;
using HamstarHelpers.UIHelpers.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;


namespace HamstarHelpers.ControlPanel {
	partial class ControlPanelUI : UIState {
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

		private bool HasClicked = false;
		private bool ModListUpdateRequired = false;
		public bool AwaitingReport { get; private set; }

		private bool ResetIssueInput = false;
		private bool SetDialogToClose = false;
		private bool IsPopulatingList = false;


		////////////////

		public ControlPanelUI() {
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
			
			if( this.ModDataList.Count == 0 && !this.IsPopulatingList ) {
				this.LoadModList();
			}
		}


		////////////////
		
		public void LoadModList() {
			Task.Run( () => {
				this.IsPopulatingList = true;

				this.ModDataList.Clear();

				int i = 1;

				foreach( var mod in ModHelpers.GetAllMods() ) {
					UIModData moditem = this.CreateModListItem( i++, mod );

					this.ModDataList.Add( moditem );

					//if( ModMetaDataManager.HasGithub( moditem.Mod ) ) {
					moditem.CheckForNewVersion();
				}

				this.ModListUpdateRequired = true;
				this.IsPopulatingList = false;
			} );
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

			this.UpdateElements( HamstarHelpersMod.Instance );
		}


		public void UpdateModList() {
			if( !this.ModListUpdateRequired || !this.IsOpen ) { return; }
			this.ModListUpdateRequired = false;

			try {
				this.ModListElem.Clear();
				this.ModListElem.AddRange( this.ModDataList );
			} catch( Exception _ ) { }
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
					((UIModData)elem).DrawHoverEffects( sb );
				}
			}
		}


		////////////////

		public bool CanOpen() {
			return !this.IsOpen && !Main.inFancyUI;
		}


		public void Open() {
			this.IsOpen = true;

			Main.playerInventory = false;
			Main.editChest = false;
			Main.npcChatText = "";
			
			Main.inFancyUI = true;
			Main.InGameUI.SetState( (UIState)this );
			
			this.Backend = Main.InGameUI;

			this.RecalculateMe();
		}


		public void Close() {
			this.IsOpen = false;
			
			Main.inFancyUI = false;
			Main.InGameUI.SetState( (UIState)null );

			this.Backend = null;
		}

		////////////////

		private void SelectModFromList( UIModData list_item ) {
			Mod mod = list_item.Mod;

			if( this.CurrentModListItem != null ) {
				this.Theme.ApplyListItem( this.CurrentModListItem );
			}
			this.Theme.ApplyListItemSelected( list_item );
			this.CurrentModListItem = list_item;

			this.Logic.SetCurrentMod( mod );

			if( !ModMetaDataManager.HasGithub( mod ) ) {
				this.DisableIssueInput();
			} else {
				this.EnableIssueInput();
			}
		}

		////////////////

		private void ApplyConfigChanges( HamstarHelpersMod mymod ) {
			this.Logic.ApplyConfigChanges( mymod );
			
			this.SetDialogToClose = true;
		}

		private void ToggleModLock( HamstarHelpersMod mymod ) {
			if( !ModLockHelpers.IsWorldLocked() ) {
				ModLockHelpers.LockWorld();
			} else {
				ModLockHelpers.UnlockWorld();
			}

			this.RefreshModLockButton( mymod );
		}
	}
}
