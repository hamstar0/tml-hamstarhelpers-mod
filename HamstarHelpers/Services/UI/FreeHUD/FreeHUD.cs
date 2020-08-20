using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Terraria;
using Terraria.UI;
using Terraria.ModLoader;
using HamstarHelpers.Classes.Loadable;
using HamstarHelpers.Helpers.DotNET.Extensions;


namespace HamstarHelpers.Services.UI.FreeHUD {
	/// <summary>
	/// Allows adding arbitrary UI elements to the HUD. Elements are interactive and non-modal.
	/// </summary>
	public class FreeHUD : ILoadable {
		/// <summary></summary>
		public static FreeHUD Instance => ModContent.GetInstance<FreeHUD>();



		////////////////

		/// <summary></summary>
		/// <param name="id"></param>
		/// <param name="elem"></param>
		public static void AddElement( string id, UIElement elem ) {
			var hud = FreeHUD.Instance;
			
			if( hud.ElemMap.ContainsKey(id) ) {
				hud.HUDComponents.RemoveChild( elem );
			}
			hud.HUDComponents.Append( elem );

			hud.ElemMap[ id ] = elem;
		}

		/// <summary></summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static bool RemoveElement( string id ) {
			var hud = FreeHUD.Instance;

			if( hud.ElemMap.ContainsKey( id ) ) {
				hud.HUDComponents.RemoveChild( hud.ElemMap[id] );
				hud.ElemMap[id].Remove();
			}

			return hud.ElemMap.Remove( id );
		}

		/////

		/// <summary></summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static UIElement GetElement( string id ) {
			var hud = FreeHUD.Instance;
			return hud.ElemMap.GetOrDefault( id );
		}



		////////////////

		internal UserInterface UIContext;
		private UIState HUDComponents;

		private IDictionary<string, UIElement> ElemMap = new ConcurrentDictionary<string, UIElement>();



		////////////////

		void ILoadable.OnModsLoad() {
			this.UIContext = new UserInterface();
			this.HUDComponents = new UIState();

			this.HUDComponents.Activate();
			this.UIContext.SetState( this.HUDComponents );
		}

		void ILoadable.OnModsUnload() {
			this.HUDComponents.Deactivate();
			this.UIContext.SetState( null );
		}

		void ILoadable.OnPostModsLoad() { }
	}
}
