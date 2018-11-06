using HamstarHelpers.Components.UI;
using HamstarHelpers.Components.UI.Elements.Menu;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.TmlHelpers.Menus;
using HamstarHelpers.Internals.WebRequests;
using HamstarHelpers.Services.Menus;
using HamstarHelpers.Services.Promises;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;


namespace HamstarHelpers.Internals.Menus.ModRecommendations.UI {
	internal class UIRecommendsList : UIMenuPanel {
		private readonly ModRecommendsMenuContext MenuContext;

		private readonly UIText Label;
		private readonly UIElement List;
		private readonly UIText EmptyText;
		private IDictionary<string, Rectangle> Descriptions = new Dictionary<string, Rectangle>();
		private ISet<string> ModNameList = new HashSet<string>();



		////////////////

		public UIRecommendsList( ModRecommendsMenuContext mc, float width, float height, float x_center_offset, float y )
				: base( UITheme.Vanilla, width, height, x_center_offset, y ) {
			this.MenuContext = mc;

			this.Label = new UIText( "Recommendations:" );
			this.Label.Left.Set( -6f, 0f );
			this.Label.Top.Set( -6f, 0f );
			this.Append( this.Label );

			this.List = new UIElement();
			this.List.Left.Set( 0f, 0f );
			this.List.Top.Set( 16f, 0f );
			this.List.Width.Set( 0f, 1f );
			this.List.Height.Set( this.Height.Pixels - 16f, 0f );
			this.List.OnMouseOver += ( evt, elem ) => {
				foreach( var kv in this.Descriptions ) {
					string desc = kv.Key;
					Rectangle rect = kv.Value;

					if( rect.Contains( Main.mouseX, Main.mouseY ) ) { //evt.MousePosition
						MenuContext.InfoDisplay?.SetText( desc );
						break;
					}
				}
			};
			this.List.OnMouseOut += ( evt, elem ) => {
				MenuContext.InfoDisplay?.SetText( "" );
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
			bool is_empty = this.ModNameList.Count == 0;

			this.ModNameList.Clear();
			this.Descriptions.Clear();
			this.List.RemoveAllChildren();
			this.Recalculate();

			if( !is_empty ) {
				this.Append( this.EmptyText );
				this.Recalculate();
			}
		}


		////////////////

		public void AddModEntriesAsync( string for_mod_name, IEnumerable<Tuple<string, string>> recom_mods ) {
			foreach( var recom_mod in recom_mods ) {
				string recom_mod_name = recom_mod.Item1;
				string recommended_because = recom_mod.Item2;

				Mod mod = ModLoader.GetMod( recom_mod_name );

				this.AddRawModEntry( mod?.DisplayName, recom_mod_name, recommended_because );
			}

			Promises.AddValidatedPromise<ModVersionPromiseArguments>( GetModVersion.ModVersionPromiseValidator, ( args ) => {
				string curr_mod_name = MenuModHelper.GetModName( MenuContextService.GetPreviousMenuUI(),
						this.MenuContext.MyUI ?? MenuContextService.GetCurrentMenuUI() );

				// Validate we're in the same UI
				if( this.MenuContext.MyUI.GetType().Name != "UIModInfo" ) { return false; }
				// Validate we're viewing the mod we started with
				if( for_mod_name != curr_mod_name ) { return false; }
				
				this.List.RemoveAllChildren();
				this.ModNameList.Clear();

				foreach( var recom_mod in recom_mods ) {
					string recom_mod_name = recom_mod.Item1;
					string recommended_because = recom_mod.Item2;

					if( args.Info.ContainsKey( recom_mod_name ) ) {
						this.AddModEntry( args.Info[recom_mod_name].Item1, recom_mod_name, recommended_because );
					} else {
						this.AddRawModEntry( null, recom_mod_name, recommended_because );
					}
				}

				return false;
			} );
			
			this.EmptyText.Remove();
			this.Recalculate();
		}


		////////////////

		private void AddRawModEntry( string mod_display_name, string mod_name, string recommended_because ) {
			string fmt_display_name = mod_display_name?.Substring( 0, 18 ) ?? mod_name;
			if( mod_display_name != null ) {
				if( fmt_display_name.Length != mod_display_name.Length ) {
					fmt_display_name += "...";
				}
			} else {
				fmt_display_name = '"' + mod_name + '"';
			}

			Rectangle list_rect = this.List.GetOuterDimensions().ToRectangle();
			var ui_item = new UIText( fmt_display_name, 0.75f );

			this.List.Append( ui_item );

			Rectangle item_rect = ui_item.GetOuterDimensions().ToRectangle();
			ui_item.Top.Set( item_rect.Height * this.ModNameList.Count, 0f );

			this.List.Recalculate();

			Rectangle desc_rect = ui_item.GetOuterDimensions().ToRectangle();

			this.ModNameList.Add( mod_name );
			this.Descriptions[ recommended_because ] = desc_rect;
		}


		private void AddModEntry( string mod_display_name, string mod_name, string recommended_because ) {
			string fmt_display_name = mod_display_name.Substring( 18 );
			if( fmt_display_name.Length != mod_display_name.Length ) {
				fmt_display_name += "...";
			}

			Rectangle list_rect = this.List.GetOuterDimensions().ToRectangle();
			var ui_item = new UIText( fmt_display_name, 0.75f );

			this.List.Append( ui_item );

			Rectangle item_rect = ui_item.GetOuterDimensions().ToRectangle();
			ui_item.Top.Set( item_rect.Height * this.ModNameList.Count, 0f );

			this.List.Recalculate();

			Rectangle desc_rect = ui_item.GetOuterDimensions().ToRectangle();

			this.ModNameList.Add( mod_name );
			this.Descriptions[recommended_because] = desc_rect;
		}
	}
}
