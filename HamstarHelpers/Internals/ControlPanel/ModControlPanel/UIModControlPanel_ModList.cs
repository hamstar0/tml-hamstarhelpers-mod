using System;
using System.Linq;
using Terraria.ModLoader;
using Terraria.UI;
using HamstarHelpers.Classes.UI.Elements;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Threading;
using HamstarHelpers.Helpers.ModHelpers;
using HamstarHelpers.Helpers.TModLoader.Mods;


namespace HamstarHelpers.Internals.ControlPanel.ModControlPanel {
	/// @private
	partial class UIModControlPanelTab : UIControlPanelTab {
		public static void UpdateModList() {
			var mymod = ModHelpersMod.Instance;
			var uiModCtrlPanel = (UIModControlPanelTab)mymod.ControlPanelUI.DefaultTab;

			if( uiModCtrlPanel == null || !uiModCtrlPanel.ModListUpdateRequired || !mymod.ControlPanelUI.IsOpen ) {
				return;
			}

			uiModCtrlPanel.ModListUpdateRequired = false;

			UIModData[] modDataList = null;

			lock( UIModControlPanelTab.ModDataListLock ) {
				try {
					modDataList = uiModCtrlPanel.ModDataList.ToArray();
				} catch( Exception ) { }
			}

			try {
				uiModCtrlPanel.SetModList( modDataList );
			} catch( Exception ) { }
		}



		////////////////

		private void SetModList( UIModData[] modDataList ) {
			this.ModListElem.Clear();
			if( modDataList.Length == 0 ) {
				return;
			}

			this.ModListElem.AddRange( modDataList );

			int i = 1;

			foreach( UIElement elem in this.ModListElem._items ) {
				var modDataElem = elem as UIModData;
				if( modDataElem == null ) { continue; }

				modDataElem.DisplayIndex?.SetText( "" + i );
				i++;
			}
		}

		////

		public void LoadModListAsync() {
			TaskLauncher.Run( (_) => {
				this.IsPopulatingList = true;

				lock( UIModControlPanelTab.ModDataListLock ) {
					this.ModDataList.Clear();
				}

				var mymod = ModHelpersMod.Instance;
				int i = 1;

				foreach( var mod in ModListHelpers.GetAllLoadedModsPreferredOrder() ) {
					UIModData moditem = this.CreateModListItem( i, mod );
					if( moditem == null ) {
						continue;
					}

					i++;

					lock( UIModControlPanelTab.ModDataListLock ) {
						this.ModDataList.Add( moditem );
					}

					if( !ModHelpersConfig.Instance.DisableModMenuUpdates ) {
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
