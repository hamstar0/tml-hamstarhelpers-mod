using HamstarHelpers.Utilities.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;


namespace HamstarHelpers.ControlPanel {
	partial class ControlPanelUI : UIState {
		public static float ContainerWidth = 600f;
		public static float ContainerHeight = 480f;
		public static float ModListHeight = 300f;

		public static Color MainBgColor = new Color( 160, 0, 32, 192 );
		public static Color MainEdgeColor = new Color( 224, 224, 224, 192 );
		public static Color ModListBgColor = new Color( 0, 0, 0, 128 );
		public static Color ModListEdgeColor = new Color( 32, 32, 32, 32 );
		public static Color ModListItemBgColor = new Color( 64, 0, 16, 128 );
		public static Color ModListItemEdgeColor = new Color( 224, 224, 224, 128 );
		public static Color IssueInputBgColor = new Color( 128, 0, 16, 128 );
		public static Color IssueInputEdgeColor = new Color( 224, 224, 224, 128 );
		public static Color ButtonBgColor = new Color( 160, 32, 48, 128 );
		public static Color ButtonEdgeColor = new Color( 224, 224, 224, 128 );
		public static Color ButtonBgLitColor = new Color( 128, 0, 16, 128 );


		public static Texture2D ControlPanelLabel { get; private set; }
		public static Texture2D ControlPanelLabelLit { get; private set; }


		////////////////

		public static void PostSetupContent( HamstarHelpersMod mymod ) {
			ControlPanelUI.ControlPanelLabel = mymod.GetTexture( "ControlPanel/ControlPanelLabel" );
			ControlPanelUI.ControlPanelLabelLit = mymod.GetTexture( "ControlPanel/ControlPanelLabelLit" );
		}



		////////////////

		public override void OnInitialize() {
			var mymod = HamstarHelpersMod.Instance;
			float top = 0;

			this.OuterContainer = new UIElement();
			this.OuterContainer.Width.Set( ControlPanelUI.ContainerWidth, 0f );
			this.OuterContainer.Height.Set( ControlPanelUI.ContainerHeight, 0f );
			this.OuterContainer.MaxWidth.Set( ControlPanelUI.ContainerWidth, 0f );
			this.OuterContainer.MaxHeight.Set( ControlPanelUI.ContainerHeight, 0f );
			this.OuterContainer.HAlign = 0.5f;
			//this.MainElement.BackgroundColor = ControlPanelUI.MainBgColor;
			//this.MainElement.BorderColor = ControlPanelUI.MainEdgeColor;
			this.Append( this.OuterContainer );

			CalculatedStyle dim = this.OuterContainer.GetDimensions();
			this.OuterContainer.Left.Set( dim.Width * -0.5f, 0.5f );
			this.OuterContainer.Top.Set( dim.Height * -0.5f, 0.5f );

			this.InnerContainer = new UIPanel();
			this.InnerContainer.Width.Set( 0f, 1f );
			this.InnerContainer.Height.Set( 0f, 1f );
			this.InnerContainer.BackgroundColor = ControlPanelUI.MainBgColor;
			this.InnerContainer.BorderColor = ControlPanelUI.MainEdgeColor;
			this.OuterContainer.Append( (UIElement)this.InnerContainer );

			var mod_list_panel = new UIPanel();
			{
				mod_list_panel.Width.Set( 0f, 1f );
				mod_list_panel.Height.Set( ControlPanelUI.ModListHeight, 0f );
				mod_list_panel.HAlign = 0f;
				mod_list_panel.SetPadding( 4f );
				mod_list_panel.PaddingTop = 0.0f;
				mod_list_panel.BackgroundColor = ControlPanelUI.ModListBgColor;
				mod_list_panel.BorderColor = ControlPanelUI.ModListEdgeColor;
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
						scrollbar.Height.Set( -24f, 1f );
						scrollbar.SetView( 100f, 1000f );
						scrollbar.HAlign = 1f;
						mod_list_panel.Append( (UIElement)scrollbar );
						this.ModListElem.SetScrollbar( scrollbar );
					}
				}
			}

			UITextArea issue_text_box = new UITextArea( "Enter issue to report for mod" );
			issue_text_box.Top.Set( top, 0f );
			issue_text_box.Width.Set( 0f, 1f );
			issue_text_box.Height.Pixels = 56f;
			issue_text_box.HAlign = 0f;
			issue_text_box.SetPadding( 8f );
			issue_text_box.BackgroundColor = ControlPanelUI.IssueInputBgColor;
			issue_text_box.BorderColor = ControlPanelUI.IssueInputEdgeColor;
			this.InnerContainer.Append( (UIElement)issue_text_box );

			top += 64f;

			var submit_button = this.CreateButton( "Submit", 0f, top, 128f, delegate ( UIMouseEvent evt, UIElement listening_element ) {
Main.NewText( "Submit" );
			} );
			this.InnerContainer.Append( submit_button );

			var cancel_button = this.CreateButton( "Cancel", 136f, top, 128f, delegate ( UIMouseEvent evt, UIElement listening_element ) {
Main.NewText( "Cancel" );
			} );
			this.InnerContainer.Append( cancel_button );

			top += 32f;

			var apply_config_button = this.CreateButton( "Apply Config Changes", 0f, top, 264f, delegate ( UIMouseEvent evt, UIElement listening_element ) {
Main.NewText( "Apply" );
			} );
			this.InnerContainer.Append( apply_config_button );

			top += 32f;

			var support_url = new UIWebUrl( "Support my mods!", "" );
			support_url.Top.Set( top, 0f );
			this.InnerContainer.Append( support_url );
			support_url.Left.Set( -support_url.GetDimensions().Width * 0.5f, 0.5f );
		}


		////////////////

		public UIModData CreateModListItem( Mod mod ) {
			UIModData elem = new UIModData( mod );
			elem.BackgroundColor = ControlPanelUI.ModListItemBgColor;
			elem.BorderColor = ControlPanelUI.ModListItemEdgeColor;
			return elem;
		}

		public UITextPanel<string> CreateButton( string label, float left, float top, float width, MouseEvent click ) {
			var button = new UITextPanel<string>( label );

			button.Width.Set( width, 0f );
			button.Left.Set( left, 0f );
			button.Top.Set( top, 0f );

			button.SetPadding( 5f );

			button.BackgroundColor = ControlPanelUI.ButtonBgColor;
			button.BorderColor = ControlPanelUI.ButtonEdgeColor;

			button.OnClick += click;
			button.OnMouseOver += delegate ( UIMouseEvent evt, UIElement listeningElement ) {
				button.BackgroundColor = ControlPanelUI.ButtonBgLitColor;
			};
			button.OnMouseOut += delegate ( UIMouseEvent evt, UIElement listeningElement ) {
				button.BackgroundColor = ControlPanelUI.ButtonBgColor;
			};

			return button;
		}
	}
}
