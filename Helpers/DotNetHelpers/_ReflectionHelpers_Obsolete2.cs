using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.DotNetHelpers {
	public partial class ReflectionHelpers {
		[Obsolete( "use `IEnumerable<Type> GetAllAvailableSubTypesFromMods( Type parentType )`", true )]
		public static IEnumerable<Type> GetAllAvailableSubTypes( Type parentType ) {
			return ReflectionHelpers.GetAllAvailableSubTypesFromMods( parentType );
		}


		[Obsolete( "use `mod.GetType().Assembly.GetType( typeName )`", true )]
		public static Type GetTypeFromMod( Mod mod, string typeName ) {
			return mod.GetType().Assembly.GetType( typeName );
		}

		[Obsolete( "use `Type GetClassTypeFromMod(...)`)", true )]
		public static Type GetClassTypeFrom( string assemblyName, string className ) {
			return ReflectionHelpers.GetTypesFromAssembly( assemblyName, className ).FirstOrDefault();
		}


		////////////////

		[Obsolete( "use `bool Get<T>(object, string, out T)`", true )]
		public static bool Get<T>( Object instance, string propOrFieldName, BindingFlags flags, out T val ) {
			if( !ReflectionHelpers.GetField<T>( instance, propOrFieldName, out val ) ) {
				return ReflectionHelpers.GetProperty<T>( instance, propOrFieldName, out val );
			}
			return true;
		}

		[Obsolete( "use `bool Get<T>(Type, object, string, out T)`", true )]
		public static bool Get<T>( Type objType, Object instance, string propOrFieldName, BindingFlags flags, out T val ) {
			if( !ReflectionHelpers.GetField<T>( objType, instance, propOrFieldName, out val ) ) {
				return ReflectionHelpers.GetProperty<T>( objType, instance, propOrFieldName, out val );
			}
			return true;
		}

		////

		[Obsolete( "use `bool Get<T>(object, string, out fieldVal)`", true )]
		public static bool GetField<T>( Object instance, string fieldName, BindingFlags flags, out T fieldVal ) {
			fieldVal = default( T );
			if( instance == null ) { return false; }

			Type objtype = instance.GetType();
			FieldInfo field = objtype.GetField( fieldName, flags );
			if( field == null ) { return false; }

			fieldVal = (T)field.GetValue( instance );
			return true;
		}
		
		[Obsolete( "use `bool Get<T>(Type, object, string, out T)`", true )]
		public static bool GetField<T>( Type objType, Object instance, string fieldName, BindingFlags flags, out T fieldVal ) {
			fieldVal = default( T );

			FieldInfo field = objType.GetField( fieldName, flags );
			if( field == null ) { return false; }

			fieldVal = (T)field.GetValue( instance );
			return true;
		}

		[Obsolete( "use `bool Get(..., out object fieldVal)`)", true)]
		public static object GetField( Object instance, string fieldName, out bool success ) {
			object obj;
			success = ReflectionHelpers.GetField( instance, fieldName, out obj );
			return success;
		}
		[Obsolete( "use `bool Get(..., out object fieldVal)`)", true )]
		public static object GetField( Object instance, string fieldName, BindingFlags flags, out bool success ) {
			object obj;
			success = ReflectionHelpers.GetField( instance, fieldName, flags, out obj );
			return success;
		}
		[Obsolete( "use `bool Get(..., out object fieldVal)`)", true )]
		public static object GetField( Type objType, Object instance, string fieldName, BindingFlags flags, out bool success ) {
			object obj;
			success = ReflectionHelpers.GetField( objType, instance, fieldName, flags, out obj );
			return success;
		}


		[Obsolete( "use `bool Set(object, string, value)`", true )]
		public static bool SetField( Object instance, string fieldName, BindingFlags flags, object value ) {
			if( instance == null ) { return false; }

			Type objType = instance.GetType();
			FieldInfo field = objType.GetField( fieldName, flags );
			if( field == null ) { return false; }

			field.SetValue( instance, value );
			return true;
		}

		[Obsolete( "use `bool Set(Type, object, string, object)`", true )]
		public static bool SetField( Type objType, Object instance, string fieldName, BindingFlags flags, object value ) {
			if( instance == null ) { return false; }

			FieldInfo field = objType.GetField( fieldName, flags );
			if( field == null ) { return false; }

			field.SetValue( instance, value );
			return true;
		}

		[Obsolete( "use `bool Set(...)`)", true )]
		public static void SetField( Object obj, string fieldName, object value, out bool success ) {
			success = ReflectionHelpers.SetField( obj, fieldName, value );
		}
		[Obsolete( "use `bool Set(...)`)", true )]
		public static void SetField( Object obj, string fieldName, object value, BindingFlags flags, out bool success ) {
			success = ReflectionHelpers.SetField( obj, fieldName, flags, value );
		}

		////

		[Obsolete( "use `bool Get<T>(object, string, out T)`", true )]
		public static bool GetProperty<T>( Object instance, string propName, BindingFlags flags, out T propVal ) {
			propVal = default( T );
			if( instance == null ) { return false; }

			Type objType = instance.GetType();
			PropertyInfo prop = objType.GetProperty( propName, flags );
			if( prop == null ) { return false; }

			propVal = (T)prop.GetValue( instance );
			return true;
		}

		[Obsolete( "use `bool Get<T>(Type, object, string, out T)`", true )]
		public static bool GetProperty<T>( Type objType, Object instance, string propName, BindingFlags flags, out T propVal ) {
			propVal = default( T );

			PropertyInfo prop = objType.GetProperty( propName, flags );
			if( prop == null ) { return false; }

			propVal = (T)prop.GetValue( instance );
			return true;
		}

		[Obsolete( "use `bool Get(..., out object propVal)`)", true )]
		public static object GetProperty( Object instance, string propName, out bool success ) {
			object obj;
			success = ReflectionHelpers.GetProperty( instance, propName, out obj );
			return obj;
		}
		[Obsolete( "use `bool Get(..., out object propVal)`)", true )]
		public static object GetProperty( Object instance, string propName, BindingFlags flags, out bool success ) {
			object obj;
			success = ReflectionHelpers.GetProperty( instance, propName, flags, out obj );
			return obj;
		}
		[Obsolete( "use `bool Get(..., out object propVal)`)", true )]
		public static object GetProperty( Type objType, Object instance, string propName, BindingFlags flags, out bool success ) {
			object obj;
			success = ReflectionHelpers.GetProperty( objType, instance, propName, flags, out obj );
			return obj;
		}


		[Obsolete( "use `bool Set(...)`)", true )]
		public static void SetProperty( Object obj, string propName, object value, out bool success ) {
			success = ReflectionHelpers.SetProperty( obj, propName, value );
		}

		////

		[Obsolete( "use `bool RunMethod(..., out object returnVal)`)", true )]
		public static object RunMethod( Object obj, string methodName, object[] args, out bool success ) {
			object returnVal;
			success = ReflectionHelpers.RunMethod( obj, methodName, args, out returnVal );
			return returnVal;
		}
		[Obsolete( "use `bool RunMethod(..., out object returnVal)`)", true )]
		public static object RunMethod( Object obj, string methodName, BindingFlags flags, object[] args, out bool success ) {
			object returnVal;
			success = ReflectionHelpers.RunMethod( obj, methodName, flags, args, out returnVal );
			return returnVal;
		}
	}
}
