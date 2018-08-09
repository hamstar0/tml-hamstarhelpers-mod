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

		public static void OnPostSetupContent( HamstarHelpersMod mymod ) {
			if( !Main.dedServ ) {
				UIControlPanel.ControlPanelIcon = mymod.GetTexture( "Internals/ControlPanel/ControlPanelIcon" );
				UIControlPanel.ControlPanelIconLit = mymod.GetTexture( "Internals/ControlPanel/ControlPanelIconLit" );
			}
		}



		////////////////

		private void InitializeComponents() {
			UIControlPanel self = this;
			ControlPanelLogic logic = this.Logic;
			var mymod = HamstarHelpersMod.Instance;
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
			this.DialogClose.OnClick += delegate ( UIMouseEvent evt, UIElement listening_element ) {
				self.Close();
				Main.PlaySound( SoundID.MenuClose );
			};
			this.DialogClose.OnMouseOver += delegate ( UIMouseEvent evt, UIElement listening_element ) {
				self.Theme.ApplyButtonLit( self.DialogClose );
			};
			this.DialogClose.OnMouseOut += delegate ( UIMouseEvent evt, UIElement listening_element ) {
				self.Theme.ApplyButton( self.DialogClose );
			};
			this.InnerContainer.Append( this.DialogClose );

			////

			var tip = new UIText( "To enable issue reports for your mod, " );
			this.InnerContainer.Append( (UIElement)tip );

			var tip_url = new UIWebUrl( this.Theme, "read this.",
				"https://forums.terraria.org/index.php?threads/mod-helpers-a-modders-mod-for-mods-and-modding.63670/#modders",
				true, 1f );
			tip_url.Left.Set( tip.GetInnerDimensions().Width, 0f );
			tip_url.Top.Set( -2f, 0f );
			this.InnerContainer.Append( (UIElement)tip_url );

			top += 24f;

			////

			var mod_list_panel = new UIPanel();
			{
				mod_list_panel.Top.Set( top, 0f );
				mod_list_panel.Width.Set( 0f, 1f );
				mod_list_panel.Height.Set( UIControlPanel.ModListHeight, 0f );
				mod_list_panel.HAlign = 0f;
				mod_list_panel.SetPadding( 4f );
				mod_list_panel.PaddingTop = 0.0f;
				mod_list_panel.BackgroundColor = this.Theme.ListBgColor;
				mod_list_panel.BorderColor = this.Theme.ListEdgeColor;
				this.InnerContainer.Append( (UIElement)mod_list_panel );

				this.ModListElem = new UIList();
				{
					this.ModListElem.Width.Set( -25, 1f );
					this.ModListElem.Height.Set( 0f, 1f );
					this.ModListElem.HAlign = 0f;
					this.ModListElem.ListPadding = 4f;
					this.ModListElem.SetPadding( 0f );
					mod_list_panel.Append( (UIElement)this.ModListElem );

					top += UIControlPanel.ModListHeight + this.InnerContainer.PaddingTop;

					UIScrollbar scrollbar = new UIScrollbar();
					{
						scrollbar.Top.Set( 8f, 0f );
						scrollbar.Height.Set( -16f, 1f );
						scrollbar.SetView( 100f, 1000f );
						scrollbar.HAlign = 1f;
						mod_list_panel.Append( (UIElement)scrollbar ); 
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
			this.IssueTitleInput.OnPreChange += delegate ( StringBuilder new_text ) {
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
			this.IssueBodyInput.OnPreChange += delegate ( StringBuilder new_text ) {
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
			this.IssueSubmitButton.OnClick += delegate ( UIMouseEvent evt, UIElement listening_element ) {
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
			this.ApplyConfigButton.OnClick += delegate ( UIMouseEvent evt, UIElement listening_element ) {
				if( !self.ApplyConfigButton.IsEnabled ) { return; }
				self.ApplyConfigChanges( HamstarHelpersMod.Instance );
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
			this.ModLockButton.OnClick += delegate ( UIMouseEvent evt, UIElement listening_element ) {
				if( !self.ModLockButton.IsEnabled ) { return; }
				self.ToggleModLock( HamstarHelpersMod.Instance );
				Main.PlaySound( SoundID.Unlock );
			};
			this.InnerContainer.Append( this.ModLockButton );

			this.RefreshModLockButton( mymod );

			top += 36f;

			////

			var modrec_url = new UIWebUrl( this.Theme, "Need mods?", "https://sites.google.com/site/terrariamodsuggestions/" );
			modrec_url.Top.Set( top, 0f );
			modrec_url.Left.Set( 0f, 0f );
			this.InnerContainer.Append( modrec_url );

			var serverbrowser_url = new UIWebUrl( this.Theme, "Lonely?", "https://forums.terraria.org/index.php?threads/server-browser-early-beta.68346/" );
			serverbrowser_url.Top.Set( top, 0f );
			this.InnerContainer.Append( serverbrowser_url );
			serverbrowser_url.Left.Set( -serverbrowser_url.GetDimensions().Width * 0.5f, 0.5f );

			var support_url = new UIWebUrl( this.Theme, "Support my mods!", "https://www.patreon.com/hamstar0" );
			support_url.Top.Set( top, 0f );
			this.InnerContainer.Append( support_url );
			support_url.Left.Set( -support_url.GetDimensions().Width, 1f );
		}


		////////////////

		public UIModData CreateModListItem( int i, Mod mod ) {
			UIControlPanel self = this;
			var elem = new UIModData( this.Theme, i, mod, false );

			this.Theme.ApplyListItem( elem );

			elem.OnMouseOver += delegate ( UIMouseEvent evt, UIElement from_elem ) {
				if( !(from_elem is UIModData) ) { return; }

				if( self.Logic.CurrentMod != null && elem.Mod.Name == self.Logic.CurrentMod.Name ) { return; }

				self.Theme.ApplyListItemLit( elem );
			};
			elem.OnMouseOut += delegate ( UIMouseEvent evt, UIElement from_elem ) {
				if( !(from_elem is UIModData) ) { return; }
				if( self.Logic.CurrentMod != null && elem.Mod.Name == self.Logic.CurrentMod.Name ) { return; }

				self.Theme.ApplyListItem( elem );
			};

			elem.OnClick += delegate ( UIMouseEvent evt, UIElement from_elem ) {
				if( !(from_elem is UIModData) ) { return; }
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

		public void RefreshModLockButton( HamstarHelpersMod mymod ) {
			bool are_mods_locked = ModLockHelpers.IsWorldLocked();
			string status = are_mods_locked ? ": ON" : ": OFF";
			bool is_enabled = true;

			if( !mymod.Config.WorldModLockEnable ) {
				status += " (disabled)";
				is_enabled = false;
			} else if( Main.netMode != 0 ) {
				status += " (single-player only)";
				is_enabled = false;
			}

			if( !is_enabled ) {
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

		public void UpdateElements( HamstarHelpersMod mymod ) {
			if( !mymod.Config.WorldModLockEnable ) {
				if( this.ModLockButton.IsEnabled ) {
					this.RefreshModLockButton( mymod );
				}
			} else {
				if( !this.ModLockButton.IsEnabled ) {
					this.RefreshModLockButton( mymod );
				}
			}
		}
	}
}
