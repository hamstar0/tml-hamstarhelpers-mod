using HamstarHelpers.UIHelpers.Elements;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;


namespace HamstarHelpers.ControlPanel {
	partial class ControlPanelUI : UIState {
		public static float ContainerWidth = 600f;
		public static float ContainerHeight = 512f;
		public static float ModListHeight = 300f;
		
		public static Texture2D ControlPanelLabel { get; private set; }
		public static Texture2D ControlPanelLabelLit { get; private set; }


		////////////////

		public static void Load( HamstarHelpersMod mymod ) {
			if( !Main.dedServ ) {
				ControlPanelUI.ControlPanelLabel = mymod.GetTexture( "ControlPanel/ControlPanelLabel" );
				ControlPanelUI.ControlPanelLabelLit = mymod.GetTexture( "ControlPanel/ControlPanelLabelLit" );
			}
		}



		////////////////

		private void InitializeComponents() {
			ControlPanelUI self = this;
			ControlPanelLogic logic = this.Logic;
			var mymod = HamstarHelpersMod.Instance;
			float top = 0;
			
			this.OuterContainer = new UIElement();
			this.OuterContainer.Width.Set( ControlPanelUI.ContainerWidth, 0f );
			this.OuterContainer.Height.Set( ControlPanelUI.ContainerHeight, 0f );
			this.OuterContainer.MaxWidth.Set( ControlPanelUI.ContainerWidth, 0f );
			this.OuterContainer.MaxHeight.Set( ControlPanelUI.ContainerHeight, 0f );
			this.OuterContainer.HAlign = 0f;
			//this.MainElement.BackgroundColor = ControlPanelUI.MainBgColor;
			//this.MainElement.BorderColor = ControlPanelUI.MainEdgeColor;
			this.Append( this.OuterContainer );

			CalculatedStyle dim = this.OuterContainer.GetDimensions();
			this.OuterContainer.Left.Set( dim.Width * -0.5f, 0.5f );
			this.OuterContainer.Top.Set( dim.Height * -0.5f, 0.5f );

			this.InnerContainer = new UIPanel();
			this.InnerContainer.Width.Set( 0f, 1f );
			this.InnerContainer.Height.Set( 0f, 1f );
			this.OuterContainer.Append( (UIElement)this.InnerContainer );

			this.Theme.ApplyPanel( this.InnerContainer );

			var tip = new UIText( "To get your own mod issue reporting, " );
			this.InnerContainer.Append( (UIElement)tip );

			var tip_url = new UIWebUrl( "read this.",
				"https://forums.terraria.org/index.php?threads/hamstars-helpers-a-modders-mod-for-mods-and-modding.63670/#modders",
				true, 1f );
			tip_url.Left.Set( tip.GetInnerDimensions().Width, 0f );
			tip_url.Top.Set( -2f, 0f );
			this.InnerContainer.Append( (UIElement)tip_url );

			top += 24f;

			var mod_list_panel = new UIPanel();
			{
				mod_list_panel.Top.Set( top, 0f );
				mod_list_panel.Width.Set( 0f, 1f );
				mod_list_panel.Height.Set( ControlPanelUI.ModListHeight, 0f );
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

					top += ControlPanelUI.ModListHeight + this.InnerContainer.PaddingTop;

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

			this.IssueSubmitButton = new UITextPanelButton( this.Theme, "Submit Issue" );
			this.IssueSubmitButton.Top.Set( top, 0f );
			this.IssueSubmitButton.Left.Set( 0f, 0f );
			this.IssueSubmitButton.Width.Set( 128f, 0f );
			this.IssueSubmitButton.Disable();
			this.IssueSubmitButton.OnClick += delegate ( UIMouseEvent evt, UIElement listening_element ) {
				if( self.AwaitingReport || !self.IssueSubmitButton.IsEnabled ) { return; }
				self.SubmitIssue();
			};
			this.InnerContainer.Append( this.IssueSubmitButton );

			if( Main.netMode != 1 ) {
				var apply_config_button = new UITextPanelButton( this.Theme, "Apply Config Changes" );
				apply_config_button.Top.Set( top, 0f );
				apply_config_button.Left.Set( 0f, 0f );
				apply_config_button.Width.Set( 200f, 0f );
				apply_config_button.HAlign = 1f;
				apply_config_button.OnClick += delegate ( UIMouseEvent evt, UIElement listening_element ) {
					self.ApplyConfigChanges();
				};
				this.InnerContainer.Append( apply_config_button );
			}

			top += 56f;

			var modrec_url = new UIWebUrl( "Need mods?", "https://sites.google.com/site/terrariamodsuggestions/" );
			modrec_url.Top.Set( top, 0f );
			modrec_url.Left.Set( 0f, 0f );
			this.InnerContainer.Append( modrec_url );

			var support_url = new UIWebUrl( "Support my mods!", "https://www.patreon.com/hamstar0" );
			support_url.Top.Set( top, 0f );
			this.InnerContainer.Append( support_url );
			support_url.Left.Set( -support_url.GetDimensions().Width, 1f );
		}


		////////////////

		public UIModData CreateModListItem( Mod mod ) {
			ControlPanelUI self = this;
			var elem = new UIModData( this.Theme, mod, false );

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
	}
}
