using HamstarHelpers.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria.ModLoader.IO;


namespace HamstarHelpers.TmlHelpers {
	public class BuildPropertiesEditor {
			public static BuildPropertiesEditor GetBuildPropertiesForModFile( TmodFile modfile ) {
			IEnumerable<Type> class_types;

			try {
				Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
				class_types = from t in assemblies.SelectMany( a => a.GetTypes() )
							  where t.IsClass && t.Namespace == "Terraria.ModLoader" && t.Name == "BuildProperties"
							  select t;
			} catch( Exception e ) {
				LogHelpers.Log( "BuildPropertiesEditor.GetBuildPropertiesForModFile - " + e.ToString() );
				return (BuildPropertiesEditor)null;
			}

			if( class_types.Count() == 0 ) {
				return (BuildPropertiesEditor)null;
			}

			Type build_prop_type = class_types.First();
			MethodInfo method = build_prop_type.GetMethod( "ReadModFile", BindingFlags.NonPublic | BindingFlags.Static );
			if( method == null ) { return (BuildPropertiesEditor)null; }

			object build_props = method.Invoke( null, new object[] { modfile } );
			if( build_props == null ) {
				LogHelpers.Log( "BuildProperties has changed." );
				return (BuildPropertiesEditor)null;
			}

			return new BuildPropertiesEditor( build_props );
		}



		////////////////

		internal object BuildProps;


		////////////////

		internal BuildPropertiesEditor( object build_props ) {
			this.BuildProps = build_props;
		}

		////////////////

		public object GetField( string prop_name ) {
			Type mod_props_type = this.BuildProps.GetType();
			FieldInfo field_info = mod_props_type.GetField( prop_name, BindingFlags.NonPublic | BindingFlags.Instance );
			if( field_info == null ) { return null; }

			return field_info.GetValue( this.BuildProps );
		}
	}
}
