using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;


namespace HamstarHelpers.UIHelpers.Elements {
	class DialogManager {
		public static DialogManager Instance {
			get {
				var myplayer = Main.LocalPlayer.GetModPlayer<HamstarHelpersPlayer>();
				return myplayer.Logic.DialogManager;
			}
		}


		////////////////

		public bool ForcedModalDialog { get; private set; }
		public UIDialog CurrentDialog { get; private set; }


		////////////////

		public DialogManager() {
			this.ForcedModalDialog = false;
			this.CurrentDialog = null;
		}

		////////////////

		internal void SetForcedModality() {
			if( this.CurrentDialog != null ) {
				this.ForcedModalDialog = true;
			}
		}

		internal void UnsetForcedModality() {
			this.ForcedModalDialog = false;
		}


		internal void SetCurrentDialog( UIDialog dlg ) {
			if( this.CurrentDialog != null && this.CurrentDialog != dlg ) {
				this.CurrentDialog.Close();
			}
			this.CurrentDialog = dlg;
		}

		////////////////

		internal void Update( HamstarHelpersMod mymod ) {
			if( this.CurrentDialog == null ) {
				return;
			}

			if( Main.InGameUI.CurrentState != this.CurrentDialog ) {
				this.CurrentDialog.Close();
			}
			
			if( !this.CurrentDialog.IsOpen ) {
				if( this.ForcedModalDialog ) {
					this.CurrentDialog.Open();
				} else {
					this.CurrentDialog = null;
				}
			}
		}
	}




	public class UIDialog : UIState {
		public virtual int ContainerWidth { get; protected set; }
		public virtual int ContainerHeight { get; protected set; }

		public bool IsOpen { get; private set; }

		protected UserInterface Backend = null;
		protected UITheme Theme;

		protected UIElement OuterContainer = null;
		protected UIPanel InnerContainer = null;

		protected bool SetDialogToClose = false;


		////////////////

		public UIDialog( UITheme theme, int width, int height ) {
			this.IsOpen = false;
			this.Theme = theme;
			this.ContainerWidth = width;
			this.ContainerHeight = height;
		}

		////////////////

		public override void OnActivate() {
			base.OnActivate();
		}

		public override void OnInitialize() {
			this.InitializeContainer( this.ContainerWidth, this.ContainerHeight );
			this.InitializeComponents();
		}


		public void InitializeContainer( int width, int height ) {
			this.OuterContainer = new UIElement();
			this.OuterContainer.Width.Set( width, 0f );
			this.OuterContainer.Height.Set( height, 0f );
			this.OuterContainer.MaxWidth.Set( width, 0f );
			this.OuterContainer.MaxHeight.Set( height, 0f );
			this.OuterContainer.HAlign = 0f;
			this.Append( this.OuterContainer );

			this.RecalculateContainer();

			this.InnerContainer = new UIPanel();
			this.InnerContainer.Width.Set( 0f, 1f );
			this.InnerContainer.Height.Set( 0f, 1f );
			this.OuterContainer.Append( (UIElement)this.InnerContainer );

			this.Theme.ApplyPanel( this.InnerContainer );
		}


		public virtual void InitializeComponents() { }


		////////////////

		public override void Update( GameTime game_time ) {
			base.Update( game_time );

			if( !this.IsOpen ) {
				return;
			}

			if( Main.playerInventory || Main.npcChatText != "" || this.Backend == null || this.Backend.CurrentState != this ) {
				this.Close();
				return;
			}
			
			if( this.SetDialogToClose ) {
				this.SetDialogToClose = false;
				this.Close();
				return;
			}

			if( this.OuterContainer.IsMouseHovering ) {
				Main.LocalPlayer.mouseInterface = true;
			}
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

		public void RecalculateContainer() {
			CalculatedStyle dim = this.OuterContainer.GetDimensions();

			this.OuterContainer.Top.Set( ( dim.Height * -0.5f ) + 32, 0.5f );
			this.OuterContainer.Left.Set( ( dim.Width * -0.5f ), 0.5f );
		}


		////////////////

		public override void Draw( SpriteBatch sb ) {
			if( !this.IsOpen ) { return; }

			base.Draw( sb );
		}


		////////////////

		public virtual bool CanOpen() {
			return !this.IsOpen && !Main.inFancyUI && DialogManager.Instance.CurrentDialog == null;
		}


		public virtual void Open() {
			this.IsOpen = true;

			Main.playerInventory = false;
			Main.editChest = false;
			Main.npcChatText = "";

			Main.inFancyUI = true;
			Main.InGameUI.SetState( (UIState)this );

			this.Backend = Main.InGameUI;

			this.RecalculateMe();

			DialogManager.Instance.SetCurrentDialog( this );
		}


		public virtual void Close() {
			this.IsOpen = false;

			if( Main.InGameUI.CurrentState == this ) {
				Main.inFancyUI = false;
				Main.InGameUI.SetState( (UIState)null );
			}

			this.Backend = null;
		}
	}
}
