using HamstarHelpers.Components.UI;
using HamstarHelpers.Components.UI.Elements.Menu;
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


namespace HamstarHelpers.Internals.Menus.ModRecommendations.UI {
	internal class UIRecommendsList : UIMenuPanel {
		private readonly ModRecommendsMenuContext MenuContext;

		private readonly UIText Label;
		private readonly UIList List;
		private readonly UIText EmptyText;
		private IDictionary<Rectangle, string> Descriptions = new Dictionary<Rectangle, string>();
		private ISet<string> ModNameList = new HashSet<string>();



		////////////////
		
		public UIRecommendsList( ModRecommendsMenuContext mc, float width, float height, float x_center_offset, float y )
				: base( UITheme.Vanilla, width, height, x_center_offset, y ) {
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
						MenuContext.InfoDisplay?.SetText( desc );
						this.Recalculate();
						break;
					}
				}
			};
			this.List.OnMouseOut += ( evt, elem ) => {
				MenuContext.InfoDisplay?.SetText( "" );
				this.Recalculate();
			};
			this.Append( this.List );

			this.EmptyText = new UIText( "See the Mod Helpers\nhomepage for listing\nother mods here." );
			this.EmptyText.TextColor = new Color( 128, 128, 128 );
			this.EmptyText.Top.Set( 16f, 0f );
			this.Append( this.EmptyText );

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
			bool is_empty = this.List._items.Count == 0;

			foreach( UIText elem in this.List._items ) {
				string timer_name = "ModHelpersUIRecommendsList_" + elem.Text;

				if( Timers.GetTimerTickDuration( timer_name ) > 0 ) {
					Timers.UnsetTimer( timer_name );
				}
			}

			this.ModNameList.Clear();
			this.Descriptions.Clear();
			this.List.Clear();
			this.Recalculate();

			if( !is_empty ) {
				this.Append( this.EmptyText );
				this.Recalculate();
			}
		}

		////////////////

		public void AddModEntry( string mod_name, string why ) {
			string timer_name = "ModHelpersUIRecommendsList_" + mod_name;
			UIText mod_entry = null;
			bool is_mod_loaded = false;

			Action<string, string> add_mod_entry = ( my_display_name, my_mod_name ) => {
				if( mod_entry != null ) {
					this.List.RemoveChild( mod_entry );
					mod_entry.Remove();
					this.ModNameList.Remove( my_mod_name );
				}

				mod_entry = new UIText( my_display_name, 0.75f );

				this.List.Add( mod_entry );
				this.Recalculate();

				this.ModNameList.Add( my_mod_name );
				this.Descriptions[ mod_entry.GetOuterDimensions().ToRectangle() ] = why;
			};

			string display_name = mod_name;
			Mod mod = ModLoader.GetMod( mod_name );
			string new_mod_name = mod_name;

			while( mod == null && new_mod_name.Length > 0 ) {
				new_mod_name = new_mod_name.Substring( 1 );
				mod = ModLoader.GetMod( new_mod_name );
			}

			if( mod != null ) {
				mod_name = new_mod_name;
				is_mod_loaded = true;
				display_name = mod.DisplayName;
			}

			add_mod_entry( display_name, mod_name );

			Promises.AddValidatedPromise<ModVersionPromiseArguments>( GetModVersion.ModVersionPromiseValidator, ( args ) => {
				if( is_mod_loaded ) { return false; }

				if( Timers.GetTimerTickDuration( timer_name ) > 0 ) {
					Timers.UnsetTimer( timer_name );
				}
				
				for( new_mod_name = mod_name;
					new_mod_name.Length > 0;
					new_mod_name = new_mod_name.Substring( 1 )
				) {
					if( args.Info.ContainsKey( new_mod_name ) ) {
						add_mod_entry( args.Info[ new_mod_name ].Item1, new_mod_name );
						break;
					}
				}

				return false;
			} );

			this.RemoveChild( this.EmptyText );
			this.EmptyText.Remove();
			this.Recalculate();
		}
	}
}
