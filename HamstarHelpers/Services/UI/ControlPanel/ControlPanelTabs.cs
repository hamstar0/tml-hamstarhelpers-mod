﻿using HamstarHelpers.Internals.ControlPanel;
using System;


namespace HamstarHelpers.Services.UI.ControlPanel {
	/// <summary>
	/// Supplies an interface to add to and manage control panel tabs. The control panel is accessible in the top left
	/// corner (by default) when the player's inventory is displayed.
	/// </summary>
	public class ControlPanelTabs {
		/// <summary>
		/// Adds a tab to the control panel UI.
		/// </summary>
		/// <param name="title"></param>
		/// <param name="tab"></param>
		public static void AddTab( string title, UIControlPanelTab tab ) {
			var mymod = ModHelpersMod.Instance;

			mymod.ControlPanelUI.AddTab( title, tab );
		}

		////////////////
		
		/// <summary>
		/// Gets the currently active tab (by name).
		/// </summary>
		/// <returns></returns>
		public static string GetCurrentTab() {
			var mymod = ModHelpersMod.Instance;

			return mymod.ControlPanelUI?.CurrentTabName;
		}

		/// <summary>
		/// Opens a given tab (by name).
		/// </summary>
		/// <param name="tabName"></param>
		public static void OpenTab( string tabName ) {
			var mymod = ModHelpersMod.Instance;
			if( mymod.ControlPanelUI == null ) {
				return;
			}

			if( !mymod.ControlPanelUI.IsOpen ) {
				mymod.ControlPanelUI.Open();
			}

			mymod.ControlPanelUI.ChangeToTab( tabName );
		}


		////////////////

		/// <summary>
		/// Indicates if the control panel dialog is open.
		/// </summary>
		/// <returns></returns>
		public static bool IsDialogOpen() {
			return ModHelpersMod.Instance.ControlPanelUI?.IsOpen ?? false;
		}

		/// <summary>
		/// Closes the control panel dialog.
		/// </summary>
		public static void CloseDialog() {
			var mymod = ModHelpersMod.Instance;

			mymod.ControlPanelUI?.Close();
			//this.SetDialogToClose = false;
			//this.Close();
		}


		////////////////

		/// <summary>
		/// Indicates that a given tab has important new information to be seen immediate.y
		/// </summary>
		/// <param name="tabName"></param>
		public static void AddTabAlert( string tabName ) {
			ModHelpersMod.Instance.ControlPanelUI?.AddTabAlert( tabName );
		}
	}
}
