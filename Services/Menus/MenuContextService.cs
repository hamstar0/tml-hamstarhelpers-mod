using HamstarHelpers.Helpers.DebugHelpers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.UI;


namespace HamstarHelpers.Services.Menus {
	class MenuContextServiceManager {
		internal IDictionary<string, IDictionary<string, Action<UIState>>> Show = new Dictionary<string, IDictionary<string, Action<UIState>>>();
		internal IDictionary<string, IDictionary<string, Action<UIState>>> Hide = new Dictionary<string, IDictionary<string, Action<UIState>>>();

		internal Tuple<string, UIState> CurrentMenuUI = null;
		internal Tuple<string, UIState> PreviousMenuUI = null;
		


		////////////////
		
		public MenuContextServiceManager() {
			if( Main.dedServ ) { return; }

			Main.OnPostDraw += MenuContextServiceManager._Update;
		}

		~MenuContextServiceManager() {
			if( Main.dedServ ) { return; }

			try {
				Main.OnPostDraw -= MenuContextServiceManager._Update;
				this.HideAll();

				this.Show.Clear();
				this.Hide.Clear();
			} catch { }
		}


		////////////////

		private void HideAll() {
			if( this.CurrentMenuUI != null ) {
				foreach( Action<UIState> hide in this.Hide[ this.CurrentMenuUI.Item1 ].Values ) {
					hide( this.CurrentMenuUI.Item2 );
				}
			}
		}


		////////////////

		private static void _Update( GameTime gametime ) {   // <- Just in case references are doing something funky...
			ModHelpersMod mymod = ModHelpersMod.Instance;
			if( mymod == null ) { return; }

			if( mymod.MenuContextMngr == null ) { return; }
			mymod.MenuContextMngr.Update();
		}

		private void Update() {
			UIState ui = Main.MenuUI.CurrentState;

			string prev_ui_name = this.CurrentMenuUI?.Item1;
			string curr_ui_name = ui?.GetType().Name;

			if( prev_ui_name == curr_ui_name ) {
				return;
			}

			this.LoadUI( ui );
		}


		private void LoadUI( UIState ui ) {
			string prev_ui_name = this.CurrentMenuUI?.Item1;
			string curr_ui_name = ui?.GetType().Name;

			this.PreviousMenuUI = this.CurrentMenuUI;

			if( prev_ui_name != null && this.Hide.ContainsKey(prev_ui_name) ) {
				var hiders = this.Hide[ prev_ui_name ].Values;
				
				foreach( Action<UIState> hide in hiders ) {
					hide( this.CurrentMenuUI.Item2 );
				}
				//this.Unloaders.Remove( prev_ui_name );
			}
			
			if( ui == null ) {
				this.CurrentMenuUI = null;
				return;
			}

			if( this.Show.ContainsKey( curr_ui_name ) ) {
				KeyValuePair<string, Action<UIState>>[] loaders = this.Show[curr_ui_name].ToArray();

				foreach( var kv in loaders ) {
					kv.Value( ui );
				}
			}

			this.CurrentMenuUI = Tuple.Create( curr_ui_name, ui );
		}
	}
}
