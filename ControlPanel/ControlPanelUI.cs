using HamstarHelpers.UIHelpers;
using HamstarHelpers.Utilities.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
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

		private bool HasClicked = false;
		private bool ModListUpdateRequired = false;



		////////////////

		public ControlPanelUI() {
			this.IsOpen = false;
			this.InitializeToggler();
		}


		public override void OnActivate() {
			base.OnActivate();

			if( this.ModDataList.Count == 0 ) {
				if( SynchronizationContext.Current == null ) {
					SynchronizationContext.SetSynchronizationContext( new SynchronizationContext() );
				}
				Task.Factory.StartNew( task => { }, TaskScheduler.FromCurrentSynchronizationContext() ).ContinueWith( task => {
					this.LoadModList();
				} );
			}
		}

		////////////////

		private void LoadModList() {
			foreach( var mod in this.Logic.GetMods() ) {
				this.ModDataList.Add( this.CreateModListItem( mod ) );
			}

			this.ModListUpdateRequired = true;
		}

		////////////////

		public void UpdateMe( GameTime game_time ) {
			if( this.Backend != null ) {
				this.Backend.Update( game_time );

				if( this.ModListUpdateRequired ) {
					this.ModListUpdateRequired = false;

					this.ModListElem.Clear();
					this.ModListElem.AddRange( this.ModDataList );
				}
			}

			if( this.IsOpen ) {
				if( Main.playerInventory || Main.npcChatText != "" ) {
					this.Close();
					return;
				}

				if( this.OuterContainer.IsMouseHovering ) {
					Main.LocalPlayer.mouseInterface = true;
				}
			}

			this.UpdateToggler();
		}

		public void RecalculateBackend() {
			if( this.Backend != null ) {
				this.Backend.Recalculate();
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
			
			this.OuterContainer.Top.Set( -(ControlPanelUI.ContainerHeight / 2f), 0.5f );
			this.Recalculate();

			this.Backend = Main.InGameUI;
		}


		public void Close() {
			this.IsOpen = false;

			//this.Container.Top.Set( -ControlPanelUI.PanelHeight * 2f, 0f );
			this.Deactivate();
			
			Main.inFancyUI = false;
			Main.InGameUI.SetState( (UIState)null );

			this.Backend = null;
		}


		////////////////

		public void SelectModFromList( UIModData list_item ) {
			if( this.CurrentModListItem != null ) {
				this.Theme.ApplyModListItem( this.CurrentModListItem );
			}
			this.Theme.ApplyModListItemSelected( list_item );

			this.CurrentModListItem = list_item;

			this.Logic.SetCurrentMod( list_item.Mod );
		}
	}
}
