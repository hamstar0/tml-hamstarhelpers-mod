using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace HamstarHelpers.Services.Tml {
	public partial class BuildPropertiesEditor {
		private static Type GetBuildPropertiesClassType() {
			//IEnumerable<Type> bpClassTypes;

			try {
				Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
				Func<Assembly, IEnumerable<Type>> selectMany = delegate ( Assembly a ) {
					try {
						return a.GetTypes();
					} catch {
						return new List<Type>();
					}
				};

				foreach( var ass in assemblies ) {
					foreach( Type t in selectMany( ass ) ) {
						if( t.IsClass && t.Namespace == "Terraria.ModLoader" && t.Name == "BuildProperties" ) {
							return t;
						}
					}
				}
				//bpClassTypes = from t in assemblies.SelectMany( selectMany )
				//				 where t.IsClass && t.Namespace == "Terraria.ModLoader" && t.Name == "BuildProperties"
				//				 select t;
			} catch( Exception e ) {
				LogHelpers.Warn( e.ToString() );
				return (Type)null;
			}

			//if( bpClassTypes.Count() == 0 ) {
			//	return (Type)null;
			//}

			//return bpClassTypes.FirstOrDefault();
			return null;
		}


		public static BuildPropertiesEditor GetBuildPropertiesForModFile( TmodFile modfile ) {
			Type buildPropType = BuildPropertiesEditor.GetBuildPropertiesClassType();
			if( buildPropType == null ) { return (BuildPropertiesEditor)null; }

			MethodInfo method = buildPropType.GetMethod( "ReadModFile", BindingFlags.NonPublic | BindingFlags.Static );
			if( method == null ) { return (BuildPropertiesEditor)null; }

			object buildProps = method.Invoke( null, new object[] { modfile } );
			if( buildProps == null ) {
				LogHelpers.Log( "BuildProperties has changed." );
				return (BuildPropertiesEditor)null;
			}

			return new BuildPropertiesEditor( buildProps );
		}
	}
}
