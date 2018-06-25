using HamstarHelpers.Components.UI.Elements;
using HamstarHelpers.DebugHelpers;
using HamstarHelpers.TmlHelpers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;


namespace HamstarHelpers.Components.UI.Menu {
	public class MenuUI {
		public static void AddMenuLoader( string ui_class_name, string elem_name, Action<UIState> on_load, Action<UIState> on_unload ) {
			var mymod = HamstarHelpersMod.Instance;

			if( !mymod.MenuUIMngr.Loaders.ContainsKey( ui_class_name ) ) {
				mymod.MenuUIMngr.Loaders[ ui_class_name ] = new Dictionary<string, Action<UIState>>();
				mymod.MenuUIMngr.Unloaders[ ui_class_name ] = new Dictionary<string, Action<UIState>>();
			}
			mymod.MenuUIMngr.Loaders[ ui_class_name ][ elem_name ] = on_load;
			mymod.MenuUIMngr.Unloaders[ ui_class_name ][ elem_name ] = on_unload;
		}
	}




	class MenuUIManager {
		internal IDictionary<string, IDictionary<string, Action<UIState>>> Loaders = new Dictionary<string, IDictionary<string, Action<UIState>>>();
		internal IDictionary<string, IDictionary<string, Action<UIState>>> Unloaders = new Dictionary<string, IDictionary<string, Action<UIState>>>();

		private IDictionary<string, UIState> CachedMenus = new Dictionary<string, UIState>();


		////////////////

		public MenuUIManager() {
			if( Main.dedServ ) { return; }

			Main.OnPostDraw += MenuUIManager._Update;
		}

		~MenuUIManager() {
			if( Main.dedServ ) { return; }

			try {
				this.Unload();
				Main.OnPostDraw -= MenuUIManager._Update;
			} catch { }
		}


		private void Unload() {
			foreach( var kv in this.CachedMenus ) {
				string ui_name = kv.Key;
				UIState ui = kv.Value;

				foreach( Action<UIState> unloader in this.Unloaders[ ui_name ].Values ) {
					unloader( ui );
				}
			}
		}
		

		public void OnPostSetupContent() {
			if( Main.dedServ ) { return; }

			var button = new UITextPanelButton( UITheme.Vanilla, "Open Mod Config Folder" );
			button.Top.Set( 11f, 0f );
			button.Left.Set( -104f, 0.5f );
			button.Width.Set( 208f, 0f );
			button.Height.Set( 20f, 0f );
			button.OnClick += ( UIMouseEvent evt, UIElement listeningElement ) => {
				string fullpath = Main.SavePath + Path.DirectorySeparatorChar + HamstarHelpersConfigData.RelativePath;

				try {
					Process.Start( fullpath );
				} catch( Exception ) { }
			};

			Func<UIState, UIElement> get_top_row = delegate ( UIState ui ) {
				Type ui_type = ui.GetType();
				FieldInfo ui_container_field = ui_type.GetField( "uIPanel", BindingFlags.Instance | BindingFlags.NonPublic );
				UIPanel ui_container = (UIPanel)ui_container_field.GetValue( ui );
				
				Type ui_container_type = ui_container.GetType();
				FieldInfo ui_container_elems_field = ui_container_type.GetField( "Elements", BindingFlags.Instance | BindingFlags.NonPublic );
				List<UIElement> ui_container_elems = (List<UIElement>)ui_container_elems_field.GetValue( ui_container );

				return (UIElement)ui_container_elems[2];
			};

			Action<UIState> on_load = delegate ( UIState ui ) {
				UIElement elem = get_top_row( ui );
				elem.Append( button );
			};

			Action<UIState> on_unload = delegate ( UIState ui ) {
				UIElement elem = get_top_row( ui );
				elem.RemoveChild( button );
			};

			MenuUI.AddMenuLoader( "UIMods", "ModHelpers: Mod Menu Config Folder Button", on_load, on_unload );
		}


		////////////////

		private static void _Update( GameTime gametime ) {   // <- Just in case references are doing something funky...
			HamstarHelpersMod mymod = HamstarHelpersMod.Instance;
			if( mymod == null ) { return; }

			if( mymod.MenuUIMngr == null ) { return; }
			mymod.MenuUIMngr.Update();
		}

		private void Update() {
			UIState ui = Main.MenuUI.CurrentState;
			if( ui == null ) { return; }

			string ui_name = ui.GetType().Name;
			if( !this.Loaders.ContainsKey( ui_name ) ) { return; }

			if( this.Loaders.ContainsKey( ui_name ) ) {
				foreach( Action<UIState> loader in this.Loaders[ui_name].Values ) {
					loader( ui );
				}

				this.CachedMenus[ ui_name ] = ui;
				this.Loaders.Remove( ui_name );
			}
		}
	}
}
