using HamstarHelpers.Components.UI.Menu;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using HamstarHelpers.Helpers.TmlHelpers.Menus;
using HamstarHelpers.Helpers.TmlHelpers.ModHelpers;
using HamstarHelpers.Internals.ModRecommendations.UI;
using HamstarHelpers.Services.Menus;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace HamstarHelpers.Internals.ModRecommendations {
	partial class ModRecommendsMenuContext : MenuContextBase {
		public static void Initialize() {
			new ModRecommendsMenuContext();
		}



		////////////////

		public override string UIName => "UIModInfo";
		public override string ContextName => "Mod Recommendations";

		////////////////
		
		internal UIRecommendsList RecommendsList;



		////////////////

		private ModRecommendsMenuContext() : base( false ) {
			this.RecommendsList = new UIRecommendsList( this );

			MenuContextService.AddMenuLoader( this.UIName, "ModHelpers: " + this.ContextName + " Info Display", this.RecommendsList, false );
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
			this.RecommendsList.Clear();

			string err = "";
			IList<Tuple<string, string>> recommends =   this.GetRecommendsFromActiveMod( mod_name, ref err ) ??
														this.GetRecommendsFromInactiveMod( mod_name, ref err );

			if( string.IsNullOrEmpty(err) ) {
				foreach( Tuple<string, string> rec in recommends ) {
					this.RecommendsList.AddModEntry( rec.Item1, rec.Item2 );
				}
			} else {
				LogHelpers.Log( "!ModHelpers.ModRecommendsMC.PopulateList - " + err );
			}
		}


		////////////////

		private IList<Tuple<string, string>> GetRecommendsFromActiveMod( string mod_name, ref string err ) {
			Mod mod = ModLoader.GetMod( mod_name );
			if( mod == null ) {
				err = "No mod found by name " + mod_name;
				return null;
			}
			
			byte[] file_data = ModHelpers.UnsafeLoadFileFromMod( mod.File, "recommendations.txt" );
			if( file_data == null ) {
				return new List<Tuple<string, string>>();
			}

			return this.ParseRecommendations( file_data );
		}
		
		public IList<Tuple<string, string>> GetRecommendsFromInactiveMod( string mod_name, ref string err ) {
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

			byte[] file_data = ModHelpers.UnsafeLoadFileFromMod( tmod, "recommendations.txt" );	// tmod.GetFile( "recommendations.txt" );
			if( file_data == null ) {
				return new List<Tuple<string, string>>();
			}

			return this.ParseRecommendations( file_data );

			/*var asm = Assembly.Load( tmod.GetMainAssembly(), null );
			if( asm == null ) {
				err = "Could not load assembly for mod " + mod_name;
				return null;
			}

			Type mod_type = asm.GetTypes().SingleOrDefault( t => t.IsSubclassOf( typeof( Mod ) ) );
			if( mod_type == null ) {
				err = "Mod " +mod_name+" has no Mod subclass.";
				return null;
			}

			if( !ReflectionHelpers.GetProperty( mod_type, null, "RecommendedMods", out list ) ) {
				return new List<Tuple<string, string>>();
			}

			return (IList<Tuple<string, string>>)list;*/
		}


		private IList<Tuple<string, string>> ParseRecommendations( byte[] file_data ) {
			string data = Encoding.Default.GetString( file_data );
			string[] lines = data.Substring(3).Trim().Split( '\n' ).ToArray();
			IEnumerable<string[]> recommendations = lines.Select(
				line => line.Split( '=' ).Select( s => s.Trim() ).ToArray()
			);
			
			return recommendations.Where(
				entry => entry.Length == 2
			).Select(
				entry => Tuple.Create(entry[0], entry[1])
			).ToList();

			/*object _data;

			if( !ReflectionHelpers.GetField( mod, "RecommendedMods", out _data ) || _data == null ) {
				if( !ReflectionHelpers.GetProperty( mod, "RecommendedMods", out _data ) || _data == null ) {
					return new List<Tuple<string, string>>();
				}
			}

			return (IList<Tuple<string, string>>)_data;*/
		}
	}
}
