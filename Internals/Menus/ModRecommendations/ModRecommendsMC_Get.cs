using HamstarHelpers.Components.UI.Menu;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET;
using HamstarHelpers.Helpers.DotNET.Reflection;
using HamstarHelpers.Services.Tml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;


namespace HamstarHelpers.Internals.Menus.ModRecommendations {
	/** @private */
	partial class ModRecommendsMenuContext : SessionMenuContext {
		private IList<Tuple<string, string>> GetRecommendsFromActiveMod( string modName ) {
			Mod mod = ModLoader.GetMod( modName );
			if( mod == null ) {
				return null;
			}

			//byte[] fileData = ModHelpers.UnsafeLoadFileFromMod( mod.File, "description.txt" );    //recommendations.txt
			//if( fileData == null ) {
			//	return new List<Tuple<string, string>>();
			//}

			var buildEdit = BuildPropertiesEditor.GetBuildPropertiesForModFile( mod.File );
			string description = (string)buildEdit.GetField( "description" );
			byte[] descData = Encoding.UTF8.GetBytes( string.IsNullOrEmpty( description ) ? "" : description );

			return this.ParseRecommendationsFromModDescription( descData );
		}

		public IList<Tuple<string, string>> GetRecommendsFromInactiveMod( string modName, ref string err ) {
			throw new NotImplementedException( "GetRecommendsFromInactiveMod not implemented." );
			/*TmodFile tmod = null;
			string[] fileNames = Directory.GetFiles( ModLoader.ModPath, "*.tmod", SearchOption.TopDirectoryOnly );
			Type type = typeof( TmodFile );

			foreach( string fileName in fileNames ) {
				if( Path.GetFileName( fileName ) != modName + ".tmod" ) { continue; }
					
				try {
					tmod = (TmodFile)type.Assembly.CreateInstance(
						type.FullName, false,
						BindingFlags.Instance | BindingFlags.NonPublic,
						null, new object[] { fileName }, null, null
					);

					object _;
					ReflectionHelpers.RunMethod( tmod, "Read", new object[] { TmodFile.LoadedState.Code }, out _ );
				} catch( Exception ) {
					err = "Could not read mod file data for "+modName;
					return null;
				}
			}

			if( tmod == null ) {
				err = "No " + modName + " mod found.";
				return null;
			}

			byte[] file_data = ModHelpers.UnsafeLoadFileFromMod( tmod, "recommendations.txt" );
			if( file_data == null ) {
				return new List<Tuple<string, string>>();
			}*/

			/*var asm = Assembly.Load( tmod.GetMainAssembly(), null );
			if( asm == null ) {
				err = "Could not load assembly for mod " + modName;
				return null;
			}

			Type mod_type = asm.GetTypes().SingleOrDefault( t => t.IsSubclassOf( typeof( Mod ) ) );
			if( mod_type == null ) {
				err = "Mod " +modName+" has no Mod subclass.";
				return null;
			}

			if( !ReflectionHelpers.GetProperty( mod_type, null, "RecommendedMods", out list ) ) {
				return new List<Tuple<string, string>>();
			}

			return (IList<Tuple<string, string>>)list;*/
		}

		public IList<Tuple<string, string>> GetRecommendsFromUI( string modName, ref string err ) {
			UIPanel msgBox;
			if( this.MyUI == null || !ReflectionHelpers.Get( this.MyUI, "modInfo", out msgBox ) ) {
				err = "No modInfo field.";
				return new List<Tuple<string, string>>();
			}

			string modDesc;
			if( !ReflectionHelpers.Get( msgBox, "text", out modDesc ) ) {
				err = "No modInfo.text field.";
				return new List<Tuple<string, string>>();
			}

			return this.ParseRecommendationsFromModDescription( Encoding.UTF8.GetBytes(modDesc) );
		}


		////////////////

		private IList<Tuple<string, string>> ParseRecommendationsFromModDescription( byte[] fileData ) {
			string data = Encoding.UTF8.GetString( fileData );
			//string[] lines = data.Substring(3).Trim().Split( '\n' ).ToArray();
			string[] lines = data.Trim().Split( '\n' ).ToArray();
			int linePos = lines.Length;

			for( int i=0; i<lines.Length; i++ ) {
				if( lines[i].Trim() == "Mod recommendations:" ) {
					linePos = i + 1;
					break;
				}
			}

			IEnumerable<string[]> recommendations = lines.Skip( linePos ).SafeSelect(
				line => line.Split( '=' ).SafeSelect( s => s.Trim() ).ToArray()
			);
			
			return recommendations.Where(
				entry => entry.Length == 2
			).SafeSelect(
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
