using HamstarHelpers.Components.UI;
using HamstarHelpers.Components.UI.Elements;
using HamstarHelpers.Components.UI.Menu;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.UI;


namespace HamstarHelpers.Internals.ModTags {
	class ModBrowserExtension {
		private IDictionary<string, UITextPanelButton> TagButtons = new Dictionary<string, UITextPanelButton>();


		////////////////

		public ModBrowserExtension() {
			if( Main.dedServ ) { return; }

			Main.OnPostDraw += ModBrowserExtension._Update;
		}

		~ModBrowserExtension() {
			if( Main.dedServ ) { return; }

			try {
				Main.OnPostDraw -= ModBrowserExtension._Update;
			} catch { }
		}


		////////////////

		public void OnPostSetupContent() {
			if( Main.dedServ ) { return; }

			var button = new UITextPanelButton( UITheme.Vanilla, "Open Mod Config Folder" );
			button.Top.Set( 11f, 0f );
			button.Left.Set( -104f, 0.5f );
			button.Width.Set( 208f, 0f );
			button.Height.Set( 20f, 0f );
			button.OnClick += ( UIMouseEvent evt, UIElement listeningElement ) => {
			};

			Action<UIState> on_load = delegate ( UIState ui ) {
			};

			Action<UIState> on_unload = delegate ( UIState ui ) {
			};

			MenuUI.AddMenuLoader( "UIModBrowser", "ModHelpers: Mob Browser Tag Controls", on_load, on_unload );
		}


		////////////////

		private static void _Update( GameTime gametime ) {   // <- Just in case references are doing something funky...
			HamstarHelpersMod mymod = HamstarHelpersMod.Instance;
			if( mymod == null ) { return; }

			if( mymod.MenuUIMngr == null ) { return; }
			//mymod.ModBrowserExt.Update();
		}

		//private void Update() {
		//}
	}
}
