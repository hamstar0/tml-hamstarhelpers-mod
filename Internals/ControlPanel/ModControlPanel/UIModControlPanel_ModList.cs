using HamstarHelpers.Classes.UI.Elements;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.ModHelpers;
using HamstarHelpers.Helpers.TModLoader.Mods;
using System;
using System.Linq;
using System.Threading;
using Terraria.ModLoader;


namespace HamstarHelpers.Internals.ControlPanel.ModControlPanel {
	/// @private
	partial class UIModControlPanelTab : UIControlPanelTab {
		public static void UpdateModList() {
			var mymod = ModHelpersMod.Instance;
			var uiModCtrlPanel = (UIModControlPanelTab)mymod.ControlPanel.DefaultTab;

			if( uiModCtrlPanel == null || !uiModCtrlPanel.ModListUpdateRequired || !mymod.ControlPanel.IsOpen ) {
				return;
			}

			uiModCtrlPanel.ModListUpdateRequired = false;

			lock( UIModControlPanelTab.ModDataListLock ) {
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

				lock( UIModControlPanelTab.ModDataListLock ) {
					this.ModDataList.Clear();
				}

				var mymod = ModHelpersMod.Instance;
				int i = 1;

				foreach( var mod in ModListHelpers.GetAllLoadedModsPreferredOrder() ) {
					UIModData moditem = this.CreateModListItem( i++, mod );
					if( moditem == null ) {
						continue;
					}

					lock( UIModControlPanelTab.ModDataListLock ) {
						this.ModDataList.Add( moditem );
					}

					if( !ModHelpersMod.Config.DisableModMenuUpdates ) {
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

			if( !ModFeaturesHelpers.HasGithub( mod ) ) {
				this.DisableIssueInput();
			} else {
				this.EnableIssueInput();
			}
		}

		
		////////////////

		public int GetModUpdatesAvailable() {
			int updates = 0;

			lock( UIModControlPanelTab.ModDataListLock ) {
				foreach( var moditem in this.ModDataList ) {
					if( moditem.LatestAvailableVersion > moditem.Mod.Version ) {
						updates++;
					}
				}
			}

			return updates;
		}
	}
}
