using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;


namespace HamstarHelpers.ControlPanel {
	class ControlPanelUI : UIState {
		public static Vector2 TogglerPosition = new Vector2( 128, 0 );
		public static float ContainerWidth = 600f;
		public static float ContainerHeight = 400f;
		public static float ModListHeight = 300f;

		public static Color MainBgColor = new Color( 160, 0, 32, 192 );
		public static Color MainEdgeColor = new Color( 224, 224, 224, 192 );
		public static Color ModListBgColor = new Color( 128, 0, 16, 128 );
		public static Color ModListEdgeColor = new Color( 224, 224, 224, 128 );
		public static Color ModListItemBgColor = new Color( 64, 0, 16, 128 );
		public static Color ModListItemEdgeColor = new Color( 224, 224, 224, 128 );

		public static Texture2D ControlPanelLabel;
		public static Texture2D ControlPanelLabelLit;



		////////////////

		private UserInterface Backend;
		public UIElement Container;
		public UIList ModList;

		public bool IsOpen { get; private set; }
		public bool IsTogglerLit { get; private set; }

		private bool HasBeenSetup = false;
		private bool HasClicked = false;



		public ControlPanelUI() {
			this.Backend = new UserInterface();
			this.IsOpen = false;
			this.IsTogglerLit = false;
		}

		public void PostSetupContent( HamstarHelpersMod mymod ) {
			ControlPanelUI.ControlPanelLabel = mymod.GetTexture( "ControlPanel/ControlPanelLabel" );
			ControlPanelUI.ControlPanelLabelLit = mymod.GetTexture( "ControlPanel/ControlPanelLabelLit" );

			this.Activate();
			this.Backend.SetState( this );
			this.Container.Deactivate();

			this.HasBeenSetup = true;
		}


		////////////////

		public void RecalculateBackend() {
			if( !this.HasBeenSetup ) { return; }
			this.Backend.Recalculate();
		}

		public void UpdateBackend( GameTime game_time ) {
			if( !this.HasBeenSetup ) { return; }
			this.Backend.Update( game_time );
		}

		protected override void DrawSelf( SpriteBatch sb ) {
			if( this.IsOpen && this.Container.ContainsPoint( Main.MouseScreen ) ) {
				Main.LocalPlayer.mouseInterface = true;
			}

			if( this.IsTogglerLit ) {
				Main.LocalPlayer.mouseInterface = true;
			}
		}

		public override void Update( GameTime gameTime ) {
			if( this.IsOpen ) {
				if( Main.playerInventory ) {
					this.Close();
				}
			}
		}


		////////////////

		public override void OnInitialize() {
			var mymod = HamstarHelpersMod.Instance;
			
			this.Container = new UIElement();
			this.Container.Left.Set( -(ControlPanelUI.ContainerWidth / 2f), 0.5f );
			this.Container.Top.Set( -ControlPanelUI.ContainerHeight * 2f, 0f );
			this.Container.Width.Set( ControlPanelUI.ContainerWidth, 0f );
			this.Container.Height.Set( ControlPanelUI.ContainerHeight, 0f );
			this.Container.MaxWidth.Set( ControlPanelUI.ContainerWidth, 0f );
			this.Container.MaxHeight.Set( ControlPanelUI.ContainerHeight, 0f );
			this.Container.HAlign = 0.5f;
			//this.MainElement.BackgroundColor = ControlPanelUI.MainBgColor;
			//this.MainElement.BorderColor = ControlPanelUI.MainEdgeColor;
			this.Append( this.Container );

			var mod_list_panel = new UIPanel();
			mod_list_panel.Width.Set( 0f, 1f );
			mod_list_panel.Height.Set( ControlPanelUI.ModListHeight, 0f );
			//mod_list_panel.BackgroundColor = ControlPanelUI.ModListBgColor;
			//mod_list_panel.BorderColor = ControlPanelUI.ModListEdgeColor;
			mod_list_panel.BackgroundColor = ControlPanelUI.MainBgColor;
			mod_list_panel.BorderColor = ControlPanelUI.MainEdgeColor;
			mod_list_panel.SetPadding( 8f );
			//mod_list_panel.PaddingTop = 0.0f;
			mod_list_panel.HAlign = 0f;
			this.Container.Append( (UIElement)mod_list_panel );

			this.ModList = new UIList();
			this.ModList.Width.Set( -25, 1f );
			this.ModList.Height.Set( 0f, 1f );
			this.ModList.HAlign = 0f;
			this.ModList.ListPadding = 5f;
			this.ModList.SetPadding( 0f );
			mod_list_panel.Append( (UIElement)this.ModList );

			this.InitializeModList();

			UIScrollbar scrollbar = new UIScrollbar();
			scrollbar.SetView( 100f, 1000f );
			scrollbar.Top.Set( 0f, 0f );
			scrollbar.Height.Set( 0f, 1f );
			scrollbar.HAlign = 1f;
			mod_list_panel.Append( (UIElement)scrollbar );
			this.ModList.SetScrollbar( scrollbar );

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

			this.Container.Deactivate();
		}


		////////////////

		public void Open() {
			this.IsOpen = true;
			this.Container.Top.Set( -(ControlPanelUI.ContainerHeight / 2f), 0.5f );
			this.Container.Activate();
			this.Recalculate();

			Main.playerInventory = false;
			Main.editChest = false;
			Main.npcChatText = "";

			Main.inFancyUI = true;
			Main.InGameUI.SetState( this );
		}

		public void Close() {
			this.IsOpen = false;
			//this.Container.Top.Set( -ControlPanelUI.PanelHeight * 2f, 0f );
			this.Container.Deactivate();
			this.Recalculate();

			Main.inFancyUI = false;
			Main.InGameUI.SetState( (UIState)null );
		}

		////////////////

		public bool IsTogglerShown() {
			return Main.playerInventory;
		}

		public void DrawToggler( SpriteBatch sb ) {
			if( !this.IsTogglerShown() ) { return; }

			Texture2D tex;
			Color color;

			if( this.IsTogglerLit ) {
				tex = ControlPanelUI.ControlPanelLabelLit;
				color = new Color( 192, 192, 192, 192 );
			} else {
				tex = ControlPanelUI.ControlPanelLabel;
				color = new Color( 160, 160, 160, 160 );
			}
			
			sb.Draw( tex, ControlPanelUI.TogglerPosition, null, color );
		}

		public void CheckTogglerMouseInteraction() {
			bool is_click = Main.mouseLeft && Main.mouseLeftRelease;
			Vector2 pos = ControlPanelUI.TogglerPosition;
			Vector2 size = ControlPanelUI.ControlPanelLabel.Size();

			this.IsTogglerLit = false;

			if( this.IsTogglerShown() ) {
				if( Main.mouseX >= pos.X && Main.mouseX < (pos.X + size.X) ) {
					if( Main.mouseY >= pos.Y && Main.mouseY < (pos.Y + size.Y) ) {
						if( is_click && !this.HasClicked ) {
							if( this.IsOpen ) {
								this.Close();
							} else {
								this.Open();
							}
						}

						this.IsTogglerLit = true;
					}
				}
			}
			
			this.HasClicked = is_click;
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
				if( mods.Contains(mod) ) { continue; }
				this.ModList.Add( this.CreateModListItem( mod ) );
			}
		}


		public UIPanel CreateModListItem( Mod mod ) {
			UIPanel elem = new UIPanel();
			
			elem.Width.Set( 0f, 1f );
			elem.Height.Set( 64, 0f );
			elem.BackgroundColor = ControlPanelUI.ModListItemBgColor;
			elem.BorderColor = ControlPanelUI.ModListItemEdgeColor;

			elem.Append( new UIText(mod.DisplayName) );

			return elem;
		}


		////////////////

		public override void Draw( SpriteBatch sb ) {
			if( this.IsOpen ) {
				base.Draw( sb );
			}
		}
	}
}
