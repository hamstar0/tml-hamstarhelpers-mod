using HamstarHelpers.Components.UI.Menu;
using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Collections.Generic;
using Terraria.UI;


namespace HamstarHelpers.Internals.ModPackBrowser {
	partial class ModBrowserUI : ModTagsUI {
		public static void Initialize() {
			new ModBrowserUI();
		}



		////////////////
		
		private ModBrowserUI() : base() {
			string base_context_name = "Mod Browser";
			string ui_name = "UIModBrowser";

			this.InitializeTagButtons( ui_name, base_context_name );
			this.InitializeUI( ui_name, base_context_name );
			this.InitializeHoverText( ui_name, base_context_name );
		}

		////////////////

		private void InitializeUI( string ui_name, string base_context_name ) {
			Action<UIState> ui_load = ui => {
				string modname = ModInfoUI.GetModNameFromUI( ui );
				if( modname == null ) { return; }
				
				//TODO
				this.RecalculateMenuObjects();
			};
			Action<UIState> ui_unload = ui => {
				this.ResetMenuObjects();
			};

			MenuUI.AddMenuLoader(ui_name, "ModHelpers: "+base_context_name+" Load", ui_load, ui_unload );
		}


		////////////////

		public ISet<string> GetTags() {
			ISet<string> tags = new HashSet<string>();

			foreach( var kv in this.TagButtons ) {
				if( !kv.Value.IsTagEnabled ) { continue; }
				tags.Add( kv.Key );
			}

			return tags;
		}


		////////////////

		public override void OnTagChange() {
			//TODO
		}
	}
}
