using HamstarHelpers.Components.UI.Menu;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using HamstarHelpers.Internals.ModRecommendations.UI;
using HamstarHelpers.Internals.ModTags;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
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

					this.PopulateList( mod_name );
				},
				ui => { }
			);
		}


		////////////////

		private void PopulateList( string mod_name ) {
			this.RecommendsList.Clear();

			string err = null;
			IList<Tuple<string, string>> recommends = this.GetRecommendsFromMod( mod_name )
				?? this.GetRecommendsFromModData( mod_name, out err );

			if( !string.IsNullOrEmpty(err) ) {
				foreach( Tuple<string, string> rec in recommends ) {
					this.RecommendsList.AddModEntry( rec.Item1, rec.Item2 );
				}
			}
		}


		////////////////
		
		private IList<Tuple<string, string>> GetRecommendsFromMod( string mod_name ) {
			Mod mod = ModLoader.GetMod( mod_name );
			object _data;

			if( !ReflectionHelpers.GetField( mod, "Recommendations", out _data ) || _data == null ) {
				if( !ReflectionHelpers.GetProperty( mod, "Recommendations", out _data ) || _data == null ) {
					return new List<Tuple<string, string>>();
				}
			}

			return (IList<Tuple<string, string>>)_data;
		}


		public IList<Tuple<string, string>> GetRecommendsFromModData( string mod_name, out string err ) {
			err = "";
			object list;
			TmodFile tmod = null;
			string[] file_names = Directory.GetFiles( ModLoader.ModPath, "*.tmod", SearchOption.TopDirectoryOnly );
			Type type = typeof( TmodFile );

			foreach( string file_name in file_names ) {
				if( Path.GetFileName( file_name ) != mod_name + ".tmod" ) { continue; }
					
				try {
					tmod = (TmodFile)type.Assembly.CreateInstance(
						type.FullName, false,
						BindingFlags.Instance | BindingFlags.NonPublic,
						null, new object[] { file_name }, null, null
					);

					object _;
					ReflectionHelpers.RunMethod( tmod, "Read", new object[] { TmodFile.LoadedState.Code }, out _ );
				} catch( Exception ) {
					err = "Could not read mod file data for "+mod_name;
					return null;
				}
			}

			if( tmod == null ) {
				err = "No " + mod_name + " mod found.";
				return null;
			}

			var asm = Assembly.Load( tmod.GetMainAssembly(), null );
			if( asm == null ) {
				err = "Could not load assembly for mod " + mod_name;
				return null;
			}

			Type mod_type = asm.GetTypes().SingleOrDefault( t => t.IsSubclassOf( typeof( Mod ) ) );
			if( mod_type == null ) {
				err = "Mod " +mod_name+" has no Mod subclass.";
				return null;
			}

			if( !ReflectionHelpers.GetProperty( mod_type, null, "", out list ) ) {
				return new List<Tuple<string, string>>();
			}

			return (IList<Tuple<string, string>>)list;
		}
	}
}
