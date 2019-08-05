using HamstarHelpers.Classes.UI.Elements;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Helpers.ModHelpers;
using HamstarHelpers.Services.Hooks.LoadHooks;
using HamstarHelpers.Services.UI.ControlPanel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;


namespace HamstarHelpers.Internals.ControlPanel.ModControlPanel {
	/// @private
	partial class UIModControlPanelTab : UIControlPanelTab {
		private static object ModDataListLock = new object();

		private static IList<string> SupportMessages = new List<string> {
			"Buy me coffee for coding! :)",
			"Did you know I make other mods?",
			"Want more?",
			"Please support Mod Helpers!"
		};



		////////////////

		private ModControlPanelLogic Logic = new ModControlPanelLogic();

		////

		private UIList ModListElem = null;

		private UITextArea IssueTitleInput = null;
		private UITextArea IssueBodyInput = null;

		private UITextPanelButton IssueSubmitButton = null;
		private UITextPanelButton OpenConfigList = null;
		private UITextPanelButton ModLockButton = null;
		private UITextPanelButton CleanupModTiles = null;

		private UIWebUrl TipUrl = null;
		private UIWebUrl SupportUrl = null;

		////

		private IList<UIModData> ModDataList = new List<UIModData>();
		private UIModData CurrentModListItem = null;

		////

		private bool ModListUpdateRequired = false;
		public bool AwaitingReport { get; private set; }

		private bool ResetIssueInput = false;
		private bool IsPopulatingList = false;
		private bool RequestClose = false;
		
		private int RandomSupportTextIdx = -1;



		////////////////

		public UIModControlPanelTab( UITheme theme ) {
			this.Theme = theme;
			this.AwaitingReport = false;
		}

		public void AddCloseButton( UITextPanelButton button ) {
			this.Append( button );
		}

		////////////////

		public override void OnInitializeMe() {
			this.RandomSupportTextIdx = Main.rand.Next( UIModControlPanelTab.SupportMessages.Count );

			LoadHooks.AddWorldUnloadEachHook( () => {
				this.RandomSupportTextIdx = Main.rand.Next( UIModControlPanelTab.SupportMessages.Count );
				this.SupportUrl.SetText( UIModControlPanelTab.SupportMessages[this.RandomSupportTextIdx] );
			} );
			
			this.InitializeComponents();
		}

		////

		public override void OnActivate() {
			base.OnActivate();

			int count;
			lock( UIModControlPanelTab.ModDataListLock ) {
				count = this.ModDataList.Count;
			}

			if( count == 0 && !this.IsPopulatingList ) {
				this.LoadModListAsync();
			}
		}


		////////////////

		public override void Update( GameTime gameTime ) {
			base.Update( gameTime );

			if( this.AwaitingReport || this.CurrentModListItem == null || !ModFeaturesHelpers.HasGithub( this.CurrentModListItem.Mod ) ) {
				this.DisableIssueInput();
			} else {
				this.EnableIssueInput();
			}

			if( this.ResetIssueInput ) {
				this.ResetIssueInput = false;
				this.IssueTitleInput.SetText( "" );
				this.IssueBodyInput.SetText( "" );
			}

			if( this.RequestClose ) {
				this.RequestClose = false;
				ControlPanelTabs.CloseDialog();
				return;
			}

			this.UpdateElements();
		}


		////////////////

		public override void Draw( SpriteBatch sb ) {
			base.Draw( sb );
			
			if( this.ModListElem.IsMouseHovering ) {
				foreach( UIElement elem in this.ModListElem._items ) {
					if( elem.IsMouseHovering ) {
						( (UIModData)elem ).DrawHoverEffects( sb );
						break;
					}
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
