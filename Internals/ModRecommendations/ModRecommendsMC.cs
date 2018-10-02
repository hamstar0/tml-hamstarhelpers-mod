using HamstarHelpers.Components.UI.Menu;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using HamstarHelpers.Internals.ModRecommendations.UI;
using HamstarHelpers.Internals.ModTags;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria.UI;


namespace HamstarHelpers.Internals.ModRecommendations {
	partial class ModRecommendsMenuContext : MenuContextBase {
		public override string UIName => "UIModInfo";
		public override string ContextName => "Mod Recommendations";

		////////////////
		
		internal UIRecommendsList RecommendsList;



		////////////////

		protected ModRecommendsMenuContext() : base( false ) {
			this.RecommendsList = new UIRecommendsList( this );

			MenuUI.AddMenuLoader( this.UIName, "ModHelpers: " + this.ContextName + " Info Display", this.RecommendsList, false );
			MenuUI.AddMenuLoader( this.UIName, "ModHelpers: " + this.ContextName + " Load Mods",
				ui => {
					string mod_name = MenuModGet.GetModName( MenuUI.GetCurrentMenu(), ui );
					if( mod_name == null ) {
						LogHelpers.Log( "Could not load mod recommendations." );
						return;
					}

					this.LoadLocalMod( ui, mod_name );
				},
				ui => { }
			);
		}


		////////////////

		private void LoadLocalMod( UIState ui, string mod_name ) {
			this.RecommendsList.Clear();

			Mod mod = ModLoader.GetMod( mod_name );
			if( mod == null ) {
				return;
			}

			object _data;

			if( !ReflectionHelpers.GetField( mod, "Recommendations", out _data ) || _data == null ) {
				if( !ReflectionHelpers.GetProperty( mod, "Recommendations", out _data ) || _data == null ) {
					return;
				}
			}
			IDictionary<string, string> recommends = (IDictionary<string, string>)_data;

			foreach( var kv in recommends ) {
				string other_mod_name = kv.Key;
				string why = kv.Value;

				this.RecommendsList.AddModEntry( other_mod_name, why );
			}
		}
	}
}
