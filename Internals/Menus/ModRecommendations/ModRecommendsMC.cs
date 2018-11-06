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

		public override void OnContexualize( string ui_class_name, string context_name ) {
			base.OnContexualize( ui_class_name, context_name );

			var recom_widget_ctx = new WidgetMenuContext( this.RecommendsList, false );
			var dl_widget_ctx = new WidgetMenuContext( this.DownloadButton, false );

			MenuContextService.AddMenuContext( ui_class_name, context_name + " Recommendations List", recom_widget_ctx );
			MenuContextService.AddMenuContext( ui_class_name, context_name + " Download Button", dl_widget_ctx );
		}


		////////////////

		public override void Show( UIState ui ) {
			base.Show( ui );

			string mod_name = MenuModHelper.GetModName( MenuContextService.GetCurrentMenuUI(), ui );
			if( mod_name == null ) {
				LogHelpers.Log( "Could not load mod recommendations; no mod found." );
				return;
			}

			this.PopulateList( mod_name );
		}


		////////////////

		private void PopulateList( string mod_name ) {
			string curr_mod_name = MenuModHelper.GetModName( MenuContextService.GetPreviousMenuUI(),
					this.MyUI ?? MenuContextService.GetCurrentMenuUI() );
			if( mod_name != curr_mod_name ) {
				return;
			}

			this.RecommendsList.Clear();

			string err = "";
			IList<Tuple<string, string>> recommends = this.GetRecommendsFromActiveMod( mod_name ) ??
													  this.GetRecommendsFromUI( mod_name, ref err );

			if( string.IsNullOrEmpty(err) ) {
				this.RecommendsList.AddModEntriesAsync( mod_name, recommends.Take( ModRecommendsMenuContext.Limit ) );
			} else {
				LogHelpers.Log( "!ModHelpers.ModRecommendsMenuContext.PopulateList - " + err );
			}
		}
	}
}
