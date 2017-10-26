using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;


namespace HamstarHelpers {
	class ControlPanelUI : UIState {
		public static float PanelWidth = 288f;
		public static float PanelHeight = 84f;
		public static Color ButtonEdgeColor = new Color( 224, 224, 224 );
		public static Color ButtonBodyColor = new Color( 160, 0, 32 );
		public static Color ButtonBodyLitColor = new Color( 192, 32, 64 );
		public static Texture2D ControlPanelLabel;
		public static Texture2D ControlPanelLabelLit;
		public static Vector2 TogglerPosition = new Vector2( 128, 0 );



		////////////////

		public UIPanel MainPanel;
		private UserInterface Backend;

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
			ControlPanelUI.ControlPanelLabel = mymod.GetTexture( "ControlPanelLabel" );
			ControlPanelUI.ControlPanelLabelLit = mymod.GetTexture( "ControlPanelLabelLit" );

			this.Activate();
			this.Backend.SetState( this );
			this.MainPanel.Deactivate();

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
			if( this.IsOpen && this.MainPanel.ContainsPoint( Main.MouseScreen ) ) {
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

			float top = 0;

			this.MainPanel = new UIPanel();
			this.MainPanel.Left.Set( -(ControlPanelUI.PanelWidth / 2f), 0.5f );
			this.MainPanel.Top.Set( -8f - ControlPanelUI.PanelHeight, 0f );
			this.MainPanel.Width.Set( ControlPanelUI.PanelWidth, 0f );
			this.MainPanel.Height.Set( ControlPanelUI.PanelHeight, 0f );
			this.MainPanel.SetPadding( 12f );
			this.MainPanel.BackgroundColor = ControlPanelUI.ButtonBodyColor;
			this.MainPanel.BorderColor = ControlPanelUI.ButtonEdgeColor;

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
			this.MainPanel.Append( enable_button );

			top += 32f;
			var disable_button = new UITextPanel<string>( "Disable Nihilism for this world" );
			disable_button.SetPadding( 4f );
			disable_button.Width.Set( ControlPanelUI.PanelWidth - 24f, 0f );
			disable_button.Left.Set( 0f, 0 );
			disable_button.Top.Set( top, 0f );
			disable_button.BackgroundColor = ControlPanelUI.ButtonBodyColor;
			disable_button.BorderColor = ControlPanelUI.ButtonEdgeColor;
			disable_button.OnClick += delegate ( UIMouseEvent evt, UIElement listeningElement ) {
				this.DeactivateNihilism();
			};
			disable_button.OnMouseOver += delegate ( UIMouseEvent evt, UIElement listeningElement ) {
				disable_button.BackgroundColor = ControlPanelUI.ButtonBodyLitColor;
			};
			disable_button.OnMouseOut += delegate ( UIMouseEvent evt, UIElement listeningElement ) {
				disable_button.BackgroundColor = ControlPanelUI.ButtonBodyColor;
			};
			this.MainPanel.Append( disable_button );*/

			this.Append( this.MainPanel );
		}


		////////////////

		public void Open() {
			this.IsOpen = true;
			this.MainPanel.Top.Set( -(ControlPanelUI.PanelHeight / 2f), 0.5f );
			this.MainPanel.Activate();
			this.Recalculate();

			Main.playerInventory = false;
		}

		public void Close() {
			this.IsOpen = false;
			this.MainPanel.Top.Set( -8f - ControlPanelUI.PanelHeight, 0f );
			this.MainPanel.Deactivate();
			this.Recalculate();
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
	}
}
