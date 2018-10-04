using HamstarHelpers.Components.UI;
using HamstarHelpers.Components.UI.Elements.Menu;
using HamstarHelpers.Components.UI.Menu;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Internals.WebRequests;
using HamstarHelpers.Services.Promises;
using HamstarHelpers.Services.Timers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;


namespace HamstarHelpers.Internals.ModRecommendations.UI {
	internal class UIRecommendsList : UIMenuPanel {
		private readonly ModRecommendsMenuContext MenuContext;

		private readonly UIText Label;
		private readonly UIList List;
		private IDictionary<Rectangle, string> Descriptions = new Dictionary<Rectangle, string>();
		private ISet<string> ModNameList = new HashSet<string>();



		////////////////

		public UIRecommendsList( ModRecommendsMenuContext mc )
				: base( UITheme.Vanilla, 160f, 132f, 240f, 40f ) {
			this.MenuContext = mc;

			this.Label = new UIText( "Recommendations:" );
			this.Label.Left.Set( -6f, 0f );
			this.Label.Top.Set( -6f, 0f );
			this.Append( this.Label );

			this.List = new UIList();
			this.List.Left.Set( 0f, 0f );
			this.List.Top.Set( 16f, 0f );
			this.List.Width.Set( 0f, 1f );
			this.List.Height.Set( this.Height.Pixels - 16f, 0f );
			this.List.OnMouseOver += ( evt, elem ) => {
				foreach( var kv in this.Descriptions ) {
					Rectangle rect = kv.Key;
					string desc = kv.Value;
					
					if( rect.Contains( (int)evt.MousePosition.X, (int)evt.MousePosition.Y ) ) {
						MenuContextBase.InfoDisplay?.SetText( desc );
						this.Recalculate();
						break;
					}
				}
			};
			this.List.OnMouseOut += ( evt, elem ) => {
				MenuContextBase.InfoDisplay?.SetText( "" );
				this.Recalculate();
			};
			this.Append( this.List );

			this.Recalculate();
		}

		~UIRecommendsList() {
			this.Clear();
		}


		////////////////

		public IList<string> GetModNames() {
			return this.ModNameList.ToList();
		}


		////////////////

		public void Clear() {
			foreach( UIText elem in this.List._items ) {
				string timer_name = "ModHelpersUIRecommendsList_" + elem.Text;

				if( Timers.GetTimerTickDuration( timer_name ) > 0 ) {
					Timers.UnsetTimer( timer_name );
				}
			}

			this.ModNameList.Clear();
			this.List.Clear();
			this.Recalculate();
		}

		////////////////

		public void AddModEntry( string mod_name, string why ) {
			string timer_name = "ModHelpersUIRecommendsList_" + mod_name;
			UIText mod_entry = null;
			bool is_mod_loaded = false;

			Action<string> add_mod_entry = ( name ) => {
				if( mod_entry != null ) {
					this.List.RemoveChild( mod_entry );
					mod_entry.Remove();
				}

				mod_entry = new UIText( name, 0.75f );

				this.List.Add( mod_entry );
				this.Recalculate();

				this.ModNameList.Add( mod_name );
				this.Descriptions[ mod_entry.GetOuterDimensions().ToRectangle() ] = why;
			};

			string display_name = mod_name;
			Mod mod = ModLoader.GetMod( mod_name );

			if( mod != null ) {
				is_mod_loaded = true;
				display_name = mod.DisplayName;
			}

			add_mod_entry( display_name );

			Promises.AddValidatedPromise<ModVersionPromiseArguments>( GetModVersion.ModVersionPromiseValidator, ( args ) => {
				if( is_mod_loaded ) { return false; }

				if( Timers.GetTimerTickDuration( timer_name ) > 0 ) {
					Timers.UnsetTimer( timer_name );
				}

				if( args.Info.ContainsKey( mod_name ) ) {
					add_mod_entry( args.Info[mod_name].Item1 );
				}
				return false;
			} );
		}
	}
}
