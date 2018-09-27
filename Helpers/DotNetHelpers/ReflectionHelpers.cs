using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.DotNetHelpers {
	public class ReflectionHelpers {
		public static Type GetClassTypeFrom( string assemble_name, string class_name ) {
			return typeof( ModLoader ).Assembly.GetType( class_name );
		}


		public static IEnumerable<Type> GetAllAvailableSubTypes( Type parent_type ) {
			var mod_types = ModLoader.LoadedMods.Select( mod => mod.GetType() );
			var assemblies = mod_types.Select( mod_type => mod_type.Assembly );
			var subclasses = from assembly in assemblies
							 from type in assembly.GetTypes()
							 where type.IsSubclassOf( parent_type ) && !type.IsAbstract
							 select type;

			return subclasses;
		}


		////////////////

		public static object GetField( Object instance, string field_name, out bool success ) {
			success = false;

			Type objtype = instance.GetType();
			FieldInfo field = objtype.GetField( field_name );
			if( field == null ) { return null; }

			success = true;
			return field.GetValue( instance );
		}

		public static object GetField( Object instance, string field_name, BindingFlags flags, out bool success ) {
			success = false;

			Type objtype = instance.GetType();
			FieldInfo field = objtype.GetField( field_name, flags );
			if( field == null ) { return null; }

			success = true;
			return field.GetValue( instance );
		}

		public static object GetField( Type obj_type, Object instance, string field_name, BindingFlags flags, out bool success ) {
			success = false;

			FieldInfo field = obj_type.GetField( field_name, flags );
			if( field == null ) { return null; }

			success = true;
			return field.GetValue( instance );
		}


		public static void SetField( Object obj, string field_name, object value, out bool success ) {
			success = false;
			Type objtype = obj.GetType();
			FieldInfo field = objtype.GetField( field_name );

			if( field == null ) { return; }

			success = true;
			field.SetValue( obj, value );
		}

		public static void SetField( Object obj, string field_name, object value, BindingFlags flags, out bool success ) {
			success = false;
			Type objtype = obj.GetType();
			FieldInfo field = objtype.GetField( field_name, flags );

			if( field == null ) { return; }

			success = true;
			field.SetValue( obj, value );
		}


		////////////////

		public static object GetProperty( Object instance, string prop_name, out bool success ) {
			success = false;

			Type objtype = instance.GetType();
			PropertyInfo prop = objtype.GetProperty( prop_name );
			if( prop == null ) { return null; }

			success = true;
			return prop.GetValue( instance );
		}

		public static object GetProperty( Object instance, string prop_name, BindingFlags flags, out bool success ) {
			success = false;

			Type objtype = instance.GetType();
			PropertyInfo prop = objtype.GetProperty( prop_name, flags );
			if( prop == null ) { return null; }

			success = true;
			return prop.GetValue( instance );
		}

		public static object GetProperty( Type obj_type, Object instance, string prop_name, BindingFlags flags, out bool success ) {
			success = false;
			
			PropertyInfo prop = obj_type.GetProperty( prop_name, flags );
			if( prop == null ) { return null; }

			success = true;
			return prop.GetValue( instance );
		}


		public static void SetProperty( Object obj, string prop_name, object value, out bool success ) {
			success = false;
			Type objtype = obj.GetType();
			FieldInfo field = objtype.GetField( prop_name );
			PropertyInfo prop = objtype.GetProperty( prop_name );

			if( prop == null ) { return; }

			success = true;
			prop.SetValue( obj, value );
		}


		////////////////

		public static object RunMethod( Object obj, string method_name, object[] args, out bool success ) {
			success = false;
			Type objtype = obj.GetType();
			MethodInfo method = objtype.GetMethod( method_name );

			if( method != null ) {
				success = true;
				return method.Invoke( obj, args );
			}
			return null;
		}

		public static object RunMethod( Object obj, string method_name, BindingFlags flags, object[] args, out bool success ) {
			success = false;
			Type objtype = obj.GetType();
			MethodInfo method = objtype.GetMethod( method_name, flags );

			if( method != null ) {
				success = true;
				return method.Invoke( obj, args );
			}
			return null;
		}
	}
}
