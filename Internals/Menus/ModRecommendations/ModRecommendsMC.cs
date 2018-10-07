using HamstarHelpers.Components.UI;
using HamstarHelpers.Components.UI.Elements.Menu;
using HamstarHelpers.Components.UI.Menu;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.TmlHelpers.Menus;
using HamstarHelpers.Helpers.TmlHelpers.ModHelpers;
using HamstarHelpers.Internals.Menus.ModRecommendations.UI;
using HamstarHelpers.Services.Menus;
using System;
using System.Collections.Generic;
using System.Linq;


namespace HamstarHelpers.Internals.Menus.ModRecommendations {
	partial class ModRecommendsMenuContext : MenuContextBase {
		public static int Limit => 6;


		////////////////

		public static void Initialize() {
			new ModRecommendsMenuContext();
		}



		////////////////
		
		public override string UIName => "UIModInfo";
		public override string ContextName => "Mod Recommendations";

		////////////////
		
		internal UIRecommendsList RecommendsList;
		private UIMenuButton DownloadButton;



		////////////////

		private ModRecommendsMenuContext() : base( true, false ) {
			this.RecommendsList = new UIRecommendsList( this );
			
			this.DownloadButton = new UIMenuButton( UITheme.Vanilla, "Download All", 206f, 26f, 194f, 172f );
			this.DownloadButton.OnClick += ( evt, elem ) => {
				ModHelpers.PromptModDownloads( "Recommended", (List<string>)this.RecommendsList.GetModNames() );
			};

			MenuContextService.AddMenuLoader( this.UIName, "ModHelpers: " + this.ContextName + " Info Display", this.RecommendsList, false );
			MenuContextService.AddMenuLoader( this.UIName, "ModHelpers: " + this.ContextName + " Download Button", this.DownloadButton, false );
			MenuContextService.AddMenuLoader( this.UIName, "ModHelpers: " + this.ContextName + " Load Mods",
				ui => {
					string mod_name = MenuModHelper.GetModName( MenuContextService.GetCurrentMenu(), ui );
					if( mod_name == null ) {
						LogHelpers.Log( "Could not load mod recommendations." );
						return;
					}

					this.PopulateList( mod_name );
				},
				ui => { }
			);
		}


		////////////////

		private void PopulateList( string mod_name ) {
			string curr_mod_name = MenuModHelper.GetModName( MenuContextService.GetPreviousMenu(), this.MyUI ?? MenuContextService.GetCurrentMenu() );
			if( mod_name != curr_mod_name ) {
				return;
			}

			this.RecommendsList.Clear();

			string err = "";
			IList<Tuple<string, string>> recommends = this.GetRecommendsFromActiveMod( mod_name, ref err ) ??
													  this.GetRecommendsFromInactiveMod( mod_name, ref err );

			if( string.IsNullOrEmpty(err) ) {
				foreach( Tuple<string, string> rec in recommends.Take( ModRecommendsMenuContext.Limit ) ) {
					this.RecommendsList.AddModEntry( rec.Item1, rec.Item2 );
				}
			} else {
				LogHelpers.Log( "!ModHelpers.ModRecommendsMenuContext.PopulateList - " + err );
			}
		}
	}
}
