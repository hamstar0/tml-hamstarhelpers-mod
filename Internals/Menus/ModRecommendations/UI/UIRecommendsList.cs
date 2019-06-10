using HamstarHelpers.Components.UI;
using HamstarHelpers.Components.UI.Elements.Menu;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TModLoader.Menus;
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

		public UIRecommendsList( ModRecommendsMenuContext mc, float width, float height, float xCenterOffset, float y )
				: base( UITheme.Vanilla, width, height, xCenterOffset, y ) {
			this.MenuContext = mc;

			this.Label = new UIText( "Also recommended:" );
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
			bool isEmpty = this.ModNameList.Count == 0;

			this.ModNameList.Clear();
			this.Descriptions.Clear();
			this.List.RemoveAllChildren();
			this.Recalculate();

			if( !isEmpty ) {
				this.Append( this.EmptyText );
				this.Recalculate();
			}
		}


		////////////////

		public void AddModEntriesAsync( string forModName, IEnumerable<Tuple<string, string>> recomMods ) {
			foreach( var recomMod in recomMods ) {
				string recomModName = recomMod.Item1;
				string recommendedBecause = recomMod.Item2;

				Mod mod = ModLoader.GetMod( recomModName );

				this.AddRawModEntry( mod?.DisplayName, recomModName, recommendedBecause );
			}

			Promises.AddValidatedPromise<ModInfoListPromiseArguments>( GetModInfo.ModInfoListPromiseValidator, ( args ) => {
				string currModName = ModMenuHelpers.GetModName( MenuContextService.GetPreviousMenuUI(),
						this.MenuContext.MyUI ?? MenuContextService.GetCurrentMenuUI() );

				// Validate we're in the same UI
				if( this.MenuContext.MyUI.GetType().Name != "UIModInfo" ) { return false; }
				// Validate we're viewing the mod we started with
				if( forModName != currModName ) { return false; }
				
				this.List.RemoveAllChildren();
				this.ModNameList.Clear();

				foreach( var recomMod in recomMods ) {
					string recomModName = recomMod.Item1;
					string recommendedBecause = recomMod.Item2;

					if( args.ModInfo.ContainsKey( recomModName ) ) {
						this.AddModEntry( args.ModInfo[recomModName].DisplayName, recomModName, recommendedBecause );
					} else {
						this.AddRawModEntry( null, recomModName, recommendedBecause );
					}
				}

				return false;
			} );
			
			this.EmptyText.Remove();
			this.Recalculate();
		}


		////////////////

		private void AddRawModEntry( string modDisplayName, string modName, string recommendedBecause ) {
			string fmtDisplayName;

			if( modDisplayName != null ) {
				fmtDisplayName = modDisplayName.Length > 28 ? modDisplayName.Substring( 0, 26 ) : modDisplayName;

				if( fmtDisplayName.Length != modDisplayName.Length ) {
					fmtDisplayName += "...";
				}
			} else {
				fmtDisplayName = '"' + modName + '"';
			}

			Rectangle listRect = this.List.GetOuterDimensions().ToRectangle();
			var uiItem = new UIText( fmtDisplayName, 0.75f );

			this.List.Append( uiItem );

			Rectangle itemRect = uiItem.GetOuterDimensions().ToRectangle();
			uiItem.Top.Set( itemRect.Height * this.ModNameList.Count, 0f );

			this.List.Recalculate();

			Rectangle descRect = uiItem.GetOuterDimensions().ToRectangle();

			this.ModNameList.Add( modName );
			this.Descriptions[ recommendedBecause ] = descRect;
		}


		private void AddModEntry( string modDisplayName, string modName, string recommendedBecause ) {
			string fmtDisplayName = modDisplayName.Length > 28 ? modDisplayName.Substring( 0, 26 ) : modDisplayName;
			if( fmtDisplayName.Length != modDisplayName.Length ) {
				fmtDisplayName += "...";
			}

			Rectangle listRect = this.List.GetOuterDimensions().ToRectangle();
			var uiItem = new UIText( fmtDisplayName, 0.75f );

			this.List.Append( uiItem );

			Rectangle itemRect = uiItem.GetOuterDimensions().ToRectangle();
			uiItem.Top.Set( itemRect.Height * this.ModNameList.Count, 0f );

			this.List.Recalculate();

			Rectangle descRect = uiItem.GetOuterDimensions().ToRectangle();

			this.ModNameList.Add( modName );
			this.Descriptions[recommendedBecause] = descRect;
		}
	}
}
