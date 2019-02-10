using HamstarHelpers.Components.UI.Elements;
using HamstarHelpers.Helpers.TmlHelpers.ModHelpers;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;


namespace HamstarHelpers.Internals.ControlPanel {
	partial class UIControlPanel : UIState {
		public static float ContainerWidth = 600f;
		public static float ContainerHeight = 520f;
		public static float ModListHeight = 300f;
		
		public static Texture2D ControlPanelIcon { get; private set; }
		public static Texture2D ControlPanelIconLit { get; private set; }

		public readonly static string ModLockTitle = "Mods locked for current world";


		////////////////

		public static void OnPostSetupContent() {
			if( Main.dedServ ) { return; }

			var mymod = ModHelpersMod.Instance;

			UIControlPanel.ControlPanelIcon = mymod.GetTexture( "Internals/ControlPanel/ControlPanelIcon" );
			UIControlPanel.ControlPanelIconLit = mymod.GetTexture( "Internals/ControlPanel/ControlPanelIconLit" );
		}



		////////////////

		private void InitializeComponents() {
			var mymod = ModHelpersMod.Instance;
			UIControlPanel self = this;
			ControlPanelLogic logic = this.Logic;
			float top = 0;
			
			this.OuterContainer = new UIElement();
			this.OuterContainer.Width.Set( UIControlPanel.ContainerWidth, 0f );
			this.OuterContainer.Height.Set( UIControlPanel.ContainerHeight, 0f );
			this.OuterContainer.MaxWidth.Set( UIControlPanel.ContainerWidth, 0f );
			this.OuterContainer.MaxHeight.Set( UIControlPanel.ContainerHeight, 0f );
			this.OuterContainer.HAlign = 0f;
			//this.MainElement.BackgroundColor = ControlPanelUI.MainBgColor;
			//this.MainElement.BorderColor = ControlPanelUI.MainEdgeColor;
			this.Append( this.OuterContainer );

			this.RecalculateContainer();

			this.InnerContainer = new UIPanel();
			this.InnerContainer.Width.Set( 0f, 1f );
			this.InnerContainer.Height.Set( 0f, 1f );
			this.OuterContainer.Append( (UIElement)this.InnerContainer );

			this.Theme.ApplyPanel( this.InnerContainer );


			////////

			this.DialogClose = new UITextPanelButton( this.Theme, "X" );
			this.DialogClose.Top.Set( -8f, 0f );
			this.DialogClose.Left.Set( -16f, 1f );
			this.DialogClose.Width.Set( 24f, 0f );
			this.DialogClose.Height.Set( 24f, 0f );
			this.DialogClose.OnClick += delegate ( UIMouseEvent evt, UIElement listeningElement ) {
				self.Close();
				Main.PlaySound( SoundID.MenuClose );
			};
			this.DialogClose.OnMouseOver += delegate ( UIMouseEvent evt, UIElement listeningElement ) {
				self.Theme.ApplyButtonLit( self.DialogClose );
			};
			this.DialogClose.OnMouseOut += delegate ( UIMouseEvent evt, UIElement listeningElement ) {
				self.Theme.ApplyButton( self.DialogClose );
			};
			this.InnerContainer.Append( this.DialogClose );

			////

			var tip = new UIText( "To enable issue reports for your mod, " );
			this.InnerContainer.Append( (UIElement)tip );

			this.TipUrl = new UIWebUrl( this.Theme, "read this.",
				"https://forums.terraria.org/index.php?threads/mod-helpers.63670/#modders",
				false, 1f );
			this.TipUrl.Left.Set( tip.GetInnerDimensions().Width, 0f );
			this.TipUrl.Top.Set( -2f, 0f );
			this.InnerContainer.Append( (UIElement)this.TipUrl );

			top += 24f;

			////

			var modListPanel = new UIPanel();
			{
				modListPanel.Top.Set( top, 0f );
				modListPanel.Width.Set( 0f, 1f );
				modListPanel.Height.Set( UIControlPanel.ModListHeight, 0f );
				modListPanel.HAlign = 0f;
				modListPanel.SetPadding( 4f );
				modListPanel.PaddingTop = 0.0f;
				modListPanel.BackgroundColor = this.Theme.ListBgColor;
				modListPanel.BorderColor = this.Theme.ListEdgeColor;
				this.InnerContainer.Append( (UIElement)modListPanel );

				this.ModListElem = new UIList();
				{
					this.ModListElem.Width.Set( -25, 1f );
					this.ModListElem.Height.Set( 0f, 1f );
					this.ModListElem.HAlign = 0f;
					this.ModListElem.ListPadding = 4f;
					this.ModListElem.SetPadding( 0f );
					modListPanel.Append( (UIElement)this.ModListElem );

					top += UIControlPanel.ModListHeight + this.InnerContainer.PaddingTop;

					UIScrollbar scrollbar = new UIScrollbar();
					{
						scrollbar.Top.Set( 8f, 0f );
						scrollbar.Height.Set( -16f, 1f );
						scrollbar.SetView( 100f, 1000f );
						scrollbar.HAlign = 1f;
						modListPanel.Append( (UIElement)scrollbar ); 
						this.ModListElem.SetScrollbar( scrollbar );
					}
				}
			}

			////

			this.IssueTitleInput = new UITextArea( this.Theme, "Enter title of mod issue", 128 );
			this.IssueTitleInput.Top.Set( top, 0f );
			this.IssueTitleInput.Width.Set( 0f, 1f );
			this.IssueTitleInput.Height.Pixels = 36f;
			this.IssueTitleInput.HAlign = 0f;
			this.IssueTitleInput.SetPadding( 8f );
			this.IssueTitleInput.Disable();
			this.IssueTitleInput.OnPreChange += delegate ( StringBuilder newText ) {
				self.RefreshIssueSubmitButton();
			};
			this.InnerContainer.Append( (UIElement)this.IssueTitleInput );

			top += 40f;
			
			this.IssueBodyInput = new UITextArea( this.Theme, "Describe mod issue" );
			this.IssueBodyInput.Top.Set( top, 0f );
			this.IssueBodyInput.Width.Set( 0f, 1f );
			this.IssueBodyInput.Height.Pixels = 36f;
			this.IssueBodyInput.HAlign = 0f;
			this.IssueBodyInput.SetPadding( 8f );
			this.IssueBodyInput.Disable();
			this.IssueBodyInput.OnPreChange += delegate ( StringBuilder newText ) {
				self.RefreshIssueSubmitButton();
			};
			this.InnerContainer.Append( (UIElement)this.IssueBodyInput );
			
			top += 40f;

			////

			this.IssueSubmitButton = new UITextPanelButton( this.Theme, "Submit Issue" );
			this.IssueSubmitButton.Top.Set( top, 0f );
			this.IssueSubmitButton.Left.Set( 0f, 0f );
			this.IssueSubmitButton.Width.Set( 200f, 0f );
			this.IssueSubmitButton.Disable();
			this.IssueSubmitButton.OnClick += delegate ( UIMouseEvent evt, UIElement listeningElement ) {
				if( self.AwaitingReport || !self.IssueSubmitButton.IsEnabled ) { return; }
				self.SubmitIssue();
			};
			this.InnerContainer.Append( this.IssueSubmitButton );
			
			this.ApplyConfigButton = new UITextPanelButton( this.Theme, "Apply Config Changes" );
			this.ApplyConfigButton.Top.Set( top, 0f );
			this.ApplyConfigButton.Left.Set( 0f, 0f );
			this.ApplyConfigButton.Width.Set( 200f, 0f );
			this.ApplyConfigButton.HAlign = 1f;
			if( Main.netMode != 0  ) {
				this.ApplyConfigButton.Disable();
			}
			this.ApplyConfigButton.OnClick += delegate ( UIMouseEvent evt, UIElement listeningElement ) {
				if( !self.ApplyConfigButton.IsEnabled ) { return; }
				self.ApplyConfigChanges();
			};
			this.InnerContainer.Append( this.ApplyConfigButton );

			top += 30f;

			this.ModLockButton = new UITextPanelButton( this.Theme, UIControlPanel.ModLockTitle );
			this.ModLockButton.Top.Set( top, 0f );
			this.ModLockButton.Left.Set( 0f, 0f );
			this.ModLockButton.Width.Set( 0f, 1f );
			if( Main.netMode != 0 || !mymod.Config.WorldModLockEnable ) {
				this.ModLockButton.Disable();
			}
			this.ModLockButton.OnClick += delegate ( UIMouseEvent evt, UIElement listeningElement ) {
				if( !self.ModLockButton.IsEnabled ) { return; }
				self.ToggleModLock();
				Main.PlaySound( SoundID.Unlock );
			};
			this.InnerContainer.Append( this.ModLockButton );

			this.RefreshModLockButton();

			top += 36f;

			////

			/*var modrec_url = new UIWebUrl( this.Theme, "Need mods?", "https://sites.google.com/site/terrariamodsuggestions/" );
			modrecUrl.Top.Set( top, 0f );
			modrecUrl.Left.Set( 0f, 0f );
			this.InnerContainer.Append( modrecUrl );

			var serverbrowser_url = new UIWebUrl( this.Theme, "Lonely?", "https://forums.terraria.org/index.php?threads/server-browser-early-beta.68346/" );
			serverbrowser_url.Top.Set( top, 0f );
			this.InnerContainer.Append( serverbrowser_url );
			serverbrowser_url.Left.Set( -serverbrowser_url.GetDimensions().Width * 0.5f, 0.5f );*/

			this.SupportUrl = new UIWebUrl( this.Theme, "Support my mods!", "https://www.patreon.com/hamstar0", false );
			this.SupportUrl.Top.Set( top, 0f );
			this.InnerContainer.Append( this.SupportUrl );
			//this.SupportUrl.Left.Set( -this.SupportUrl.GetDimensions().Width, 1f );
			this.SupportUrl.Left.Set( -this.SupportUrl.GetDimensions().Width * 0.5f, 0.5f );
		}


		////////////////

		public UIModData CreateModListItem( int i, Mod mod ) {
			UIControlPanel self = this;
			var elem = new UIModData( this.Theme, i, mod, false );

			this.Theme.ApplyListItem( elem );

			elem.OnMouseOver += delegate ( UIMouseEvent evt, UIElement fromElem ) {
				if( !(fromElem is UIModData) ) { return; }

				if( self.Logic.CurrentMod != null && elem.Mod.Name == self.Logic.CurrentMod.Name ) { return; }

				self.Theme.ApplyListItemLit( elem );
			};
			elem.OnMouseOut += delegate ( UIMouseEvent evt, UIElement fromElem ) {
				if( !(fromElem is UIModData) ) { return; }
				if( self.Logic.CurrentMod != null && elem.Mod.Name == self.Logic.CurrentMod.Name ) { return; }

				self.Theme.ApplyListItem( elem );
			};

			elem.OnClick += delegate ( UIMouseEvent evt, UIElement fromElem ) {
				if( !(fromElem is UIModData) ) { return; }
				if( self.Logic.CurrentMod != null && elem.Mod.Name == self.Logic.CurrentMod.Name ) { return; }
				if( self.AwaitingReport ) { return; }

				self.SelectModFromList( elem );
			};

			return elem;
		}


		////////////////

		public void RecalculateContainer() {
			CalculatedStyle dim = this.OuterContainer.GetDimensions();

			this.OuterContainer.Top.Set( ( dim.Height * -0.5f ) + 32, 0.5f );
			this.OuterContainer.Left.Set( ( dim.Width * -0.5f ), 0.5f );
		}


		////////////////

		public void RefreshModLockButton() {
			var mymod = ModHelpersMod.Instance;
			bool areModsLocked = ModLockHelpers.IsWorldLocked();
			string status = areModsLocked ? ": ON" : ": OFF";
			bool isEnabled = true;

			if( !mymod.Config.WorldModLockEnable ) {
				status += " (disabled)";
				isEnabled = false;
			} else if( Main.netMode != 0 ) {
				status += " (single-player only)";
				isEnabled = false;
			}

			if( !isEnabled ) {
				if( this.ModLockButton.IsEnabled ) {
					this.ModLockButton.Disable();
				}
			} else {
				if( !this.ModLockButton.IsEnabled ) {
					this.ModLockButton.Enable();
				}
			}

			this.ModLockButton.SetText( UIControlPanel.ModLockTitle + status );
		}

		public void RefreshApplyConfigButton() {
			if( Main.netMode == 0 ) {
				if( !this.ApplyConfigButton.IsEnabled ) {
					this.ApplyConfigButton.Enable();
				}
			} else {
				if( this.ApplyConfigButton.IsEnabled ) {
					this.ApplyConfigButton.Disable();
				}
			}
		}


		////////////////

		public void UpdateElements() {
			var mymod = ModHelpersMod.Instance;

			if( !mymod.Config.WorldModLockEnable ) {
				if( this.ModLockButton.IsEnabled ) {
					this.RefreshModLockButton();
				}
			} else {
				if( !this.ModLockButton.IsEnabled ) {
					this.RefreshModLockButton();
				}
			}
		}
	}
}
