using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.DotNetHelpers {
	public partial class ReflectionHelpers {
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
		
		public static bool GetField( Object instance, string field_name, out object field_val ) {
			field_val = null;

			Type objtype = instance.GetType();
			FieldInfo field = objtype.GetField( field_name );
			if( field == null ) { return false; }
			
			field_val = field.GetValue( instance );
			return true;
		}

		public static bool GetField( Object instance, string field_name, BindingFlags flags, out object field_val ) {
			field_val = null;

			Type objtype = instance.GetType();
			FieldInfo field = objtype.GetField( field_name, flags );
			if( field == null ) { return false; }

			field_val = field.GetValue( instance );
			return true;
		}

		public static bool GetField( Type obj_type, Object instance, string field_name, BindingFlags flags, out object field_val ) {
			field_val = null;

			FieldInfo field = obj_type.GetField( field_name, flags );
			if( field == null ) { return false; }

			field_val = field.GetValue( instance );
			return true;
		}

		////

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
		
		public static bool GetProperty( Object instance, string prop_name, out object prop_val ) {
			prop_val = null;

			Type objtype = instance.GetType();
			PropertyInfo prop = objtype.GetProperty( prop_name );
			if( prop == null ) { return false; }

			prop_val = prop.GetValue( instance );
			return true;
		}

		public static bool GetProperty( Object instance, string prop_name, BindingFlags flags, out object prop_val ) {
			prop_val = null;

			Type objtype = instance.GetType();
			PropertyInfo prop = objtype.GetProperty( prop_name, flags );
			if( prop == null ) { return false; }

			prop_val = prop.GetValue( instance );
			return true;
		}

		public static bool GetProperty( Type obj_type, Object instance, string prop_name, BindingFlags flags, out object prop_val ) {
			prop_val = null;

			PropertyInfo prop = obj_type.GetProperty( prop_name, flags );
			if( prop == null ) { return false; }

			prop_val = prop.GetValue( instance );
			return true;
		}

		////

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
		
		public static bool RunMethod( Object obj, string method_name, object[] args, out object return_val ) {
			return_val = false;
			Type objtype = obj.GetType();
			MethodInfo method = objtype.GetMethod( method_name );

			if( method != null ) {
				return_val = method.Invoke( obj, args );
			}
			return method != null;
		}

		public static bool RunMethod( Object obj, string method_name, BindingFlags flags, object[] args, out object return_val ) {
			return_val = false;
			Type objtype = obj.GetType();
			MethodInfo method = objtype.GetMethod( method_name, flags );

			if( method != null ) {
				return_val = method.Invoke( obj, args );
			}
			return method != null;
		}
	}
}
