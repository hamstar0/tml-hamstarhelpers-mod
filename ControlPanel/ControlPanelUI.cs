using HamstarHelpers.Utilities.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;


namespace HamstarHelpers.ControlPanel {
	partial class ControlPanelUI : UIState {
		public static Vector2 TogglerPosition = new Vector2( 128, 0 );
		public static float ContainerWidth = 600f;
		public static float ContainerHeight = 400f;
		public static float ModListHeight = 300f;

		public static Color MainBgColor = new Color( 160, 0, 32, 192 );
		public static Color MainEdgeColor = new Color( 224, 224, 224, 192 );
		public static Color ModListBgColor = new Color( 0, 0, 0, 128 );
		public static Color ModListEdgeColor = new Color( 32, 32, 32, 32 );
		public static Color ModListItemBgColor = new Color( 64, 0, 16, 128 );
		public static Color ModListItemEdgeColor = new Color( 224, 224, 224, 128 );
		public static Color IssueInputBgColor = new Color( 128, 0, 16, 128 );
		public static Color IssueInputEdgeColor = new Color( 224, 224, 224, 128 );

		public static Texture2D ControlPanelLabel;
		public static Texture2D ControlPanelLabelLit;


		public static void PostSetupContent( HamstarHelpersMod mymod ) {
			ControlPanelUI.ControlPanelLabel = mymod.GetTexture( "ControlPanel/ControlPanelLabel" );
			ControlPanelUI.ControlPanelLabelLit = mymod.GetTexture( "ControlPanel/ControlPanelLabelLit" );
		}



		////////////////

		private UserInterface Backend;
		public UIElement OuterContainer;
		public UIPanel InnerContainer;
		public UIList ModList;

		public bool IsOpen { get; private set; }

		private bool HasBeenSetup = false;
		private bool HasClicked = false;



		////////////////

		public ControlPanelUI() {
			this.Backend = new UserInterface();
			this.IsOpen = false;
			this.InitializeToggler();
		}

		public void PostSetupComponents() {
			this.Activate();
			this.Backend.SetState( this );
			this.OuterContainer.Deactivate();

			this.HasBeenSetup = true;
		}


		////////////////

		public override void OnInitialize() {
			var mymod = HamstarHelpersMod.Instance;
			float top = 0;

			this.OuterContainer = new UIElement();
			this.OuterContainer.Left.Set( -(ControlPanelUI.ContainerWidth / 2f), 0.5f );
			this.OuterContainer.Top.Set( -ControlPanelUI.ContainerHeight * 2f, 0f );
			this.OuterContainer.Width.Set( ControlPanelUI.ContainerWidth, 0f );
			this.OuterContainer.Height.Set( ControlPanelUI.ContainerHeight, 0f );
			this.OuterContainer.MaxWidth.Set( ControlPanelUI.ContainerWidth, 0f );
			this.OuterContainer.MaxHeight.Set( ControlPanelUI.ContainerHeight, 0f );
			this.OuterContainer.HAlign = 0.5f;
			//this.MainElement.BackgroundColor = ControlPanelUI.MainBgColor;
			//this.MainElement.BorderColor = ControlPanelUI.MainEdgeColor;
			this.Append( this.OuterContainer );

			this.InnerContainer = new UIPanel();
			this.InnerContainer.Width.Set( 0f, 1f );
			this.InnerContainer.Height.Set( 0f, 1f );
			this.InnerContainer.BackgroundColor = ControlPanelUI.MainBgColor;
			this.InnerContainer.BorderColor = ControlPanelUI.MainEdgeColor;
			this.OuterContainer.Append( (UIElement)this.InnerContainer );

			var mod_list_panel = new UIPanel(); {
				mod_list_panel.Width.Set( 0f, 1f );
				mod_list_panel.Height.Set( ControlPanelUI.ModListHeight, 0f );
				mod_list_panel.HAlign = 0f;
				mod_list_panel.SetPadding( 0f );
				//mod_list_panel.PaddingTop = 0.0f;
				mod_list_panel.BackgroundColor = ControlPanelUI.ModListBgColor;
				mod_list_panel.BorderColor = ControlPanelUI.ModListEdgeColor;
				this.InnerContainer.Append( (UIElement)mod_list_panel );

				this.ModList = new UIList(); {
					this.ModList.Width.Set( -25, 1f );
					this.ModList.Height.Set( 0f, 1f );
					this.ModList.HAlign = 0f;
					this.ModList.ListPadding = 5f;
					this.ModList.SetPadding( 0f );
					mod_list_panel.Append( (UIElement)this.ModList );

					this.InitializeModList();

					top += ControlPanelUI.ModListHeight + this.InnerContainer.PaddingTop;

					UIScrollbar scrollbar = new UIScrollbar(); {
						scrollbar.Top.Set( 0f, 0f );
						scrollbar.Height.Set( 0f, 1f );
						scrollbar.SetView( 100f, 1000f );
						scrollbar.HAlign = 1f;
						mod_list_panel.Append( (UIElement)scrollbar );
						this.ModList.SetScrollbar( scrollbar );
					}
				}
			}

			UIText issue_label = new UIText( "Report issue for selected mod:" );
			issue_label.Top.Set( top, 0f );
			issue_label.Height.Set( 16f, 0f );
			issue_label.SetPadding( 4f );
			this.InnerContainer.Append( (UIElement)issue_label );

			top += 24f;

			UITextArea issue_text_box = new UITextArea();
			issue_text_box.Top.Set( top, 0f );
			issue_text_box.Width.Set( 0f, 1f );
			issue_text_box.Height.Pixels = 16f;
			issue_text_box.HAlign = 0f;
			issue_text_box.SetPadding( 2f );
			issue_text_box.BackgroundColor = ControlPanelUI.IssueInputBgColor;
			issue_text_box.BorderColor = ControlPanelUI.IssueInputEdgeColor;
			this.InnerContainer.Append( (UIElement)issue_text_box );

			/*top += 2f;
			var enable_button = new UITextPanel<string>( "Enable Nihilism for this world" );
			enable_button.SetPadding( 4f );
			enable_button.Width.Set( ControlPanelUI.PanelWidth - 24f, 0f );
			enable_button.Left.Set( 0f, 0 );
			enable_button.Top.Set( top, 0f );
			enable_button.BackgroundColor = ControlPanelUI.ButtonBodyColor;
			enable_button.BorderColor = ControlPanelUI.ButtonEdgeColor;
			enable_button.OnClick += delegate ( UIMouseEvent evt, UIElement listeningElement ) {
				this.ActivateNihilism();
			};
			enable_button.OnMouseOver += delegate ( UIMouseEvent evt, UIElement listeningElement ) {
				enable_button.BackgroundColor = ControlPanelUI.ButtonBodyLitColor;
			};
			enable_button.OnMouseOut += delegate ( UIMouseEvent evt, UIElement listeningElement ) {
				enable_button.BackgroundColor = ControlPanelUI.ButtonBodyColor;
			};
			this.MainPanel.Append( enable_button );*/

			this.OuterContainer.Deactivate();
		}

		////////////////

		public void InitializeModList() {
			ISet<Mod> mods = ControlPanelLogic.GetTopMods();

			this.ModList.Add( this.CreateModListItem( HamstarHelpersMod.Instance ) );

			foreach( var mod in mods ) {
				if( mod == HamstarHelpersMod.Instance ) { continue; }
				this.ModList.Add( this.CreateModListItem( mod ) );
			}

			foreach( var mod in ModLoader.LoadedMods ) {
				if( mods.Contains( mod ) ) { continue; }
				this.ModList.Add( this.CreateModListItem( mod ) );
			}
		}


		public UIPanel CreateModListItem( Mod mod ) {
			UIPanel elem = new UIPanel();

			elem.Width.Set( 0f, 1f );
			elem.Height.Set( 64, 0f );
			elem.BackgroundColor = ControlPanelUI.ModListItemBgColor;
			elem.BorderColor = ControlPanelUI.ModListItemEdgeColor;

			elem.Append( new UIText( mod.DisplayName ) );

			return elem;
		}


		////////////////

		public override void Draw( SpriteBatch sb ) {
			if( !this.IsOpen ) { return; }

			base.Draw( sb );
		}


		////////////////

		public void UpdateMe( GameTime game_time ) {
			this.Backend.Update( game_time );

			if( !this.HasBeenSetup ) { return; }

			if( this.IsOpen ) {
				if( Main.playerInventory || Main.npcChatText != "" ) {
					this.Close();
					return;
				}

				if( this.OuterContainer.ContainsPoint( new Vector2(Main.mouseX, Main.mouseY) ) ) {
					Main.LocalPlayer.mouseInterface = true;
				}
			}

			this.UpdateToggler();
		}

		////////////////

		public void RecalculateBackend() {
			if( !this.HasBeenSetup ) { return; }
			this.Backend.Recalculate();
		}
		

		////////////////

		public bool CanOpen() {
			return !this.IsOpen && !Main.blockInput && !Main.inFancyUI;
		}

		public void Open() {
			this.IsOpen = true;
			this.OuterContainer.Top.Set( -(ControlPanelUI.ContainerHeight / 2f), 0.5f );
			this.OuterContainer.Activate();
			this.Recalculate();

			Main.playerInventory = false;
			Main.editChest = false;
			Main.npcChatText = "";
			
			Main.inFancyUI = true;
			Main.InGameUI.SetState( (UIState)this );
		}

		public void Close() {
			this.IsOpen = false;
			//this.Container.Top.Set( -ControlPanelUI.PanelHeight * 2f, 0f );
			this.OuterContainer.Deactivate();
			this.Recalculate();
			
			Main.inFancyUI = false;
			Main.InGameUI.SetState( (UIState)null );
		}
	}
}
