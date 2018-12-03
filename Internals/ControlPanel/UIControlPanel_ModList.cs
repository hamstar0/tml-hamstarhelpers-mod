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
			var ctrlPanel = mymod.ControlPanel;

			if( ctrlPanel == null || !ctrlPanel.ModListUpdateRequired || !ctrlPanel.IsOpen ) {
				return;
			}

			ctrlPanel.ModListUpdateRequired = false;

			lock( UIControlPanel.ModDataListLock ) {
				try {
					ctrlPanel.ModListElem.Clear();
					ctrlPanel.ModListElem.AddRange( ctrlPanel.ModDataList.ToArray() );
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

		private void SelectModFromList( UIModData listItem ) {
			Mod mod = listItem.Mod;

			if( this.CurrentModListItem != null ) {
				this.Theme.ApplyListItem( this.CurrentModListItem );
			}
			this.Theme.ApplyListItemSelected( listItem );
			this.CurrentModListItem = listItem;

			this.Logic.SetCurrentMod( mod );

			if( !ModMetaDataManager.HasGithub( mod ) ) {
				this.DisableIssueInput();
			} else {
				this.EnableIssueInput();
			}
		}
	}
}
