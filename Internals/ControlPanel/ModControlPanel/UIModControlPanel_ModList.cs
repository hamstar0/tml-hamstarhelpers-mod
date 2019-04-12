using HamstarHelpers.Components.UI.Elements;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.TmlHelpers;
using HamstarHelpers.Helpers.TmlHelpers.ModHelpers;
using System;
using System.Linq;
using System.Threading;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;


namespace HamstarHelpers.Internals.ControlPanel.ModControlPanel {
	partial class UIModControlPanel : UIPanel {
		public static void UpdateModList() {
			var mymod = ModHelpersMod.Instance;
			var uiModCtrlPanel = (UIModControlPanel)mymod.ControlPanel.Tabs[ "Mod Control Panel" ];

			if( uiModCtrlPanel == null || !uiModCtrlPanel.ModListUpdateRequired || !uiModCtrlPanel.IsOpen ) {
				return;
			}

			uiModCtrlPanel.ModListUpdateRequired = false;

			lock( UIControlPanel.ModDataListLock ) {
				try {
					UIModData[] modDataList = uiModCtrlPanel.ModDataList.ToArray();

					uiModCtrlPanel.ModListElem.Clear();

					if( modDataList.Length > 0 ) {
						uiModCtrlPanel.ModListElem.AddRange( modDataList );
					}
				} catch( Exception ) { }
			}
		}



		////////////////

		public void LoadModListAsync() {
			ThreadPool.QueueUserWorkItem( _ => {
				this.IsPopulatingList = true;

				lock( UIModControlPanel.ModDataListLock ) {
					this.ModDataList.Clear();
				}

				var mymod = ModHelpersMod.Instance;
				int i = 1;

				foreach( var mod in ModListHelpers.GetAllLoadedModsPreferredOrder() ) {
					UIModData moditem = this.CreateModListItem( i++, mod );

					lock( UIModControlPanel.ModDataListLock ) {
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
