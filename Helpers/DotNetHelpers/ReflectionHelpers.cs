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
		
		public static bool GetField<T>( Object instance, string field_name, out T field_val ) {
			field_val = default( T );

			Type objtype = instance.GetType();
			FieldInfo field = objtype.GetField( field_name );
			if( field == null ) { return false; }
			
			field_val = (T)field.GetValue( instance );
			return true;
		}

		public static bool GetField<T>( Object instance, string field_name, BindingFlags flags, out T field_val ) {
			field_val = default( T );

			Type objtype = instance.GetType();
			FieldInfo field = objtype.GetField( field_name, flags );
			if( field == null ) { return false; }

			field_val = (T)field.GetValue( instance );
			return true;
		}

		public static bool GetField<T>( Type obj_type, Object instance, string field_name, out T field_val ) {
			field_val = default( T );

			FieldInfo field = obj_type.GetField( field_name );
			if( field == null ) { return false; }

			field_val = (T)field.GetValue( instance );
			return true;
		}

		public static bool GetField<T>( Type obj_type, Object instance, string field_name, BindingFlags flags, out T field_val ) {
			field_val = default( T );

			FieldInfo field = obj_type.GetField( field_name, flags );
			if( field == null ) { return false; }

			field_val = (T)field.GetValue( instance );
			return true;
		}

		////

		public static bool SetField( Object obj, string field_name, object value ) {
			Type objtype = obj.GetType();
			FieldInfo field = objtype.GetField( field_name );
			if( field == null ) { return false; }
			
			field.SetValue( obj, value );
			return true;
		}

		public static bool SetField( Object obj, string field_name, object value, BindingFlags flags ) {
			Type objtype = obj.GetType();
			FieldInfo field = objtype.GetField( field_name, flags );
			if( field == null ) { return false; }
			
			field.SetValue( obj, value );
			return true;
		}


		////////////////

		public static bool GetProperty<T>( Object instance, string prop_name, out T prop_val ) {
			prop_val = default( T );

			Type objtype = instance.GetType();
			PropertyInfo prop = objtype.GetProperty( prop_name );
			if( prop == null ) { return false; }

			prop_val = (T)prop.GetValue( instance );
			return true;
		}

		public static bool GetProperty<T>( Object instance, string prop_name, BindingFlags flags, out T prop_val ) {
			prop_val = default( T );

			Type objtype = instance.GetType();
			PropertyInfo prop = objtype.GetProperty( prop_name, flags );
			if( prop == null ) { return false; }

			prop_val = (T)prop.GetValue( instance );
			return true;
		}

		public static bool GetProperty<T>( Type obj_type, Object instance, string prop_name, out T prop_val ) {
			prop_val = default( T );

			PropertyInfo prop = obj_type.GetProperty( prop_name );
			if( prop == null ) { return false; }

			prop_val = (T)prop.GetValue( instance );
			return true;
		}

		public static bool GetProperty<T>( Type obj_type, Object instance, string prop_name, BindingFlags flags, out T prop_val ) {
			prop_val = default( T );

			PropertyInfo prop = obj_type.GetProperty( prop_name, flags );
			if( prop == null ) { return false; }

			prop_val = (T)prop.GetValue( instance );
			return true;
		}

		////

		public static bool SetProperty( Object obj, string prop_name, object value ) {
			Type objtype = obj.GetType();
			FieldInfo field = objtype.GetField( prop_name );
			PropertyInfo prop = objtype.GetProperty( prop_name );
			if( prop == null ) { return false; }
			
			prop.SetValue( obj, value );
			return true;
		}


		////////////////
		
		public static bool RunMethod<T>( Object obj, string method_name, object[] args, out T return_val ) {
			return_val = default( T );

			Type objtype = obj.GetType();
			MethodInfo method = objtype.GetMethod( method_name );
			if( method == null ) { return false; }

			method.Invoke( obj, args );
			return true;
		}

		public static bool RunMethod<T>( Object obj, string method_name, BindingFlags flags, object[] args, out T return_val ) {
			return_val = default( T );

			Type objtype = obj.GetType();
			MethodInfo method = objtype.GetMethod( method_name, flags );
			if( method == null ) { return false; }

			method.Invoke( obj, args );
			return true;
		}
	}
}
