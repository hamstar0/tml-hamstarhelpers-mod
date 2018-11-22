using HamstarHelpers.Components.UI.Menu;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using HamstarHelpers.Services.Tml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;


namespace HamstarHelpers.Internals.Menus.ModRecommendations {
	partial class ModRecommendsMenuContext : SessionMenuContext {
		private IList<Tuple<string, string>> GetRecommendsFromActiveMod( string mod_name ) {
			Mod mod = ModLoader.GetMod( mod_name );
			if( mod == null ) {
				return null;
			}

			//byte[] file_data = ModHelpers.UnsafeLoadFileFromMod( mod.File, "description.txt" );    //recommendations.txt
			//if( file_data == null ) {
			//	return new List<Tuple<string, string>>();
			//}

			var build_edit = BuildPropertiesEditor.GetBuildPropertiesForModFile( mod.File );
			string description = (string)build_edit.GetField( "description" );
			byte[] desc_data = Encoding.UTF8.GetBytes( string.IsNullOrEmpty( description ) ? "" : description );

			return this.ParseRecommendationsFromModDescription( desc_data );
		}

		public IList<Tuple<string, string>> GetRecommendsFromInactiveMod( string mod_name, ref string err ) {
			throw new NotImplementedException( "GetRecommendsFromInactiveMod not implemented." );
			/*TmodFile tmod = null;
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

			byte[] file_data = ModHelpers.UnsafeLoadFileFromMod( tmod, "recommendations.txt" );
			if( file_data == null ) {
				return new List<Tuple<string, string>>();
			}*/

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

		public IList<Tuple<string, string>> GetRecommendsFromUI( string mod_name, ref string err ) {
			UIPanel msg_box;
			if( this.MyUI == null || !ReflectionHelpers.GetField<UIPanel>( this.MyUI, "modInfo", out msg_box ) ) {
				err = "No modInfo field.";
				return new List<Tuple<string, string>>();
			}

			string mod_desc;
			if( !ReflectionHelpers.GetField<string>( msg_box, "text", BindingFlags.NonPublic | BindingFlags.Instance, out mod_desc ) ) {
				err = "No modInfo.text field.";
				return new List<Tuple<string, string>>();
			}

			return this.ParseRecommendationsFromModDescription( Encoding.UTF8.GetBytes(mod_desc) );
		}


		////////////////

		private IList<Tuple<string, string>> ParseRecommendationsFromModDescription( byte[] file_data ) {
			string data = Encoding.UTF8.GetString( file_data );
			//string[] lines = data.Substring(3).Trim().Split( '\n' ).ToArray();
			string[] lines = data.Trim().Split( '\n' ).ToArray();
			int line_pos = lines.Length;

			for( int i=0; i<lines.Length; i++ ) {
				if( lines[i].Trim() == "Mod recommendations:" ) {
					line_pos = i + 1;
					break;
				}
			}

			IEnumerable<string[]> recommendations = lines.Skip( line_pos ).Select(
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
