using HamstarHelpers.Components.UI.Elements;
using HamstarHelpers.Helpers.TmlHelpers;
using HamstarHelpers.Helpers.TmlHelpers.ModHelpers;
using System;
using System.Linq;
using System.Threading;
using Terraria.ModLoader;
using Terraria.UI;


namespace HamstarHelpers.Internals.ControlPanel {
	partial class UIControlPanel : UIState {
		public static void UpdateModList( ModHelpersMod mymod ) {
			var ctrl_panel = mymod.ControlPanel;

			if( ctrl_panel == null || !ctrl_panel.ModListUpdateRequired || !ctrl_panel.IsOpen ) {
				return;
			}

			ctrl_panel.ModListUpdateRequired = false;

			lock( UIControlPanel.ModDataListLock ) {
				try {
					ctrl_panel.ModListElem.Clear();
					ctrl_panel.ModListElem.AddRange( ctrl_panel.ModDataList.ToArray() );
				} catch( Exception ) { }
			}
		}



		////////////////

		public void LoadModListAsync() {
			ThreadPool.QueueUserWorkItem( _ => {
				this.IsPopulatingList = true;

				lock( UIControlPanel.ModDataListLock ) {
					this.ModDataList.Clear();
				}

				var mymod = ModHelpersMod.Instance;
				int i = 1;

				foreach( var mod in ModHelpers.GetAllPlayableModsPreferredOrder() ) {
					UIModData moditem = this.CreateModListItem( i++, mod );

					lock( UIControlPanel.ModDataListLock ) {
						this.ModDataList.Add( moditem );
					}

					if( mymod.Config.IsCheckingModVersions ) {
						moditem.CheckForNewVersionAsync();
					}
				}

				this.ModListUpdateRequired = true;
				this.IsPopulatingList = false;
			} );
		}
		

		////////////////

		private void SelectModFromList( UIModData list_item ) {
			Mod mod = list_item.Mod;

			if( this.CurrentModListItem != null ) {
				this.Theme.ApplyListItem( this.CurrentModListItem );
			}
			this.Theme.ApplyListItemSelected( list_item );
			this.CurrentModListItem = list_item;

			this.Logic.SetCurrentMod( mod );

			if( !ModMetaDataManager.HasGithub( mod ) ) {
				this.DisableIssueInput();
			} else {
				this.EnableIssueInput();
			}
		}
	}
}
