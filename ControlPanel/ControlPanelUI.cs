using HamstarHelpers.Utilities.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;


namespace HamstarHelpers.ControlPanel {
	partial class ControlPanelUI : UIState {
		public static Vector2 TogglerPosition = new Vector2( 128, 0 );



		////////////////

		public bool IsOpen { get; private set; }
		public UserInterface Backend { get; private set; }
		
		public UIElement OuterContainer;
		public UIPanel InnerContainer;
		public UIList ModListElem;

		private IList<UIModData> ModDataList = new List<UIModData>();
		private bool HasClicked = false;
		private bool ModListUpdateRequired = false;



		////////////////

		public ControlPanelUI() {
			this.IsOpen = false;
			this.Backend = null;
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
			ISet<Mod> mods = ControlPanelLogic.GetTopMods();

			this.ModDataList.Add( this.CreateModListItem( HamstarHelpersMod.Instance ) );

			foreach( var mod in mods ) {
				if( mod == HamstarHelpersMod.Instance || mod.File == null ) { continue; }
				this.ModDataList.Add( this.CreateModListItem( mod ) );
			}

			foreach( var mod in ModLoader.LoadedMods ) {
				if( mods.Contains( mod ) || mod.File == null ) { continue; }
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

				if( this.OuterContainer.ContainsPoint( new Vector2(Main.mouseX, Main.mouseY) ) ) {
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
	}
}
