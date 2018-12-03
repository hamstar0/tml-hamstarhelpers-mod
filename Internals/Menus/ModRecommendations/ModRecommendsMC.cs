using HamstarHelpers.Components.UI;
using HamstarHelpers.Components.UI.Elements.Menu;
using HamstarHelpers.Components.UI.Menu;
using HamstarHelpers.Components.UI.Menus;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.TmlHelpers.Menus;
using HamstarHelpers.Helpers.TmlHelpers.ModHelpers;
using HamstarHelpers.Internals.Menus.ModRecommendations.UI;
using HamstarHelpers.Services.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.UI;


namespace HamstarHelpers.Internals.Menus.ModRecommendations {
	partial class ModRecommendsMenuContext : SessionMenuContext {
		public static int Limit => 6;


		////////////////

		public static void Initialize() {
			if( ModHelpersMod.Instance.Config.DisableModRecommendations ) { return; }
			
			var ctx = new ModRecommendsMenuContext();
			MenuContextService.AddMenuContext( "UIModInfo", "ModHelpers: Mod Recommendations", ctx );
		}



		////////////////
		
		internal UIRecommendsList RecommendsList;
		private UIMenuButton DownloadButton;



		////////////////

		private ModRecommendsMenuContext() : base( true, false ) {
			this.RecommendsList = new UIRecommendsList( this, 198f, 132f, 202f, 40f );

			this.DownloadButton = new UIMenuButton( UITheme.Vanilla, "Download All", 198f, 26f, 202f, 172f );
			this.DownloadButton.OnClick += ( evt, elem ) => {
				ModHelpers.PromptModDownloads( "Recommended", (List<string>)this.RecommendsList.GetModNames() );
			};
		}

		public override void OnContexualize( string uiClassName, string contextName ) {
			base.OnContexualize( uiClassName, contextName );

			var recomWidgetCtx = new WidgetMenuContext( this.RecommendsList, false );
			var dlWidgetCtx = new WidgetMenuContext( this.DownloadButton, false );

			MenuContextService.AddMenuContext( uiClassName, contextName + " Recommendations List", recomWidgetCtx );
			MenuContextService.AddMenuContext( uiClassName, contextName + " Download Button", dlWidgetCtx );
		}


		////////////////

		public override void Show( UIState ui ) {
			base.Show( ui );

			string modName = MenuModHelper.GetModName( MenuContextService.GetCurrentMenuUI(), ui );
			if( modName == null ) {
				LogHelpers.Log( "Could not load mod recommendations; no mod found." );
				return;
			}

			this.PopulateList( modName );
		}


		////////////////

		private void PopulateList( string modName ) {
			string currModName = MenuModHelper.GetModName( MenuContextService.GetPreviousMenuUI(),
					this.MyUI ?? MenuContextService.GetCurrentMenuUI() );
			if( modName != currModName ) {
				return;
			}

			this.RecommendsList.Clear();

			string err = "";
			IList<Tuple<string, string>> recommends = this.GetRecommendsFromActiveMod( modName ) ??
													  this.GetRecommendsFromUI( modName, ref err );

			if( string.IsNullOrEmpty(err) ) {
				this.RecommendsList.AddModEntriesAsync( modName, recommends.Take( ModRecommendsMenuContext.Limit ) );
			} else {
				LogHelpers.Log( "!ModHelpers.ModRecommendsMenuContext.PopulateList - " + err );
			}
		}
	}
}
