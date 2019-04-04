using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers.Reflection;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria.ModLoader.IO;


namespace HamstarHelpers.Services.Tml {
	public partial class BuildPropertiesEditor {
		private static Type GetBuildPropertiesClassType() {
			Type myClass;
			if( DataStore.DataStore.Get( "BuildPropertiesClass", out myClass ) ) {
				return myClass;
			}

			try {
				Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

				foreach( var ass in assemblies ) {
					IEnumerable<Type> types = ReflectionHelpers.GetTypesFromAssembly( ass, "BuildProperties" );

					foreach( Type t in types ) {
						if( t.IsClass && t.Namespace == "Terraria.ModLoader" && t.Name == "BuildProperties" ) {
							DataStore.DataStore.Set( "BuildPropertiesClass", t );
							return t;
						}
					}
				}
			} catch( Exception e ) {
				LogHelpers.Warn( e.ToString() );
				return (Type)null;
			}
			
			DataStore.DataStore.Set( "BuildPropertiesClass", null );
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
