using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Reflection;


namespace HamstarHelpers.Helpers.DotNetHelpers {
	public partial class ReflectionHelpers {
		public static bool GetStatic<T>( Type classType, string fieldOrPropName, out T result ) {
			FieldInfo field = classType.GetField( fieldOrPropName, ReflectionHelpers.MostAccess );
			if( field != null ) {
				result = (T)field.GetValue( null );
				return true;
			}

			PropertyInfo prop = classType.GetProperty( fieldOrPropName, ReflectionHelpers.MostAccess );
			if( prop != null ) {
				result = (T)prop.GetValue( null );
				return true;
			}

			result = default( T );
			return false;
		}
		
		////////////////

		public static bool SetStatic<T>( Type classType, string fieldOrPropName, T newValue ) {
			FieldInfo field = classType.GetField( fieldOrPropName, ReflectionHelpers.MostAccess );
			if( field != null ) {
				field.SetValue( null, newValue );
				return true;
			}

			PropertyInfo prop = classType.GetProperty( fieldOrPropName, ReflectionHelpers.MostAccess );
			if( prop != null ) {
				prop.SetValue( null, newValue );
				return true;
			}
			
			return false;
		}
		
		
		////////////////

		public static bool Get<T>( Object instance, string propOrFieldName, out T val ) {
			if( !ReflectionHelpers.GetField<T>( instance, propOrFieldName, out val ) ) {
				return ReflectionHelpers.GetProperty<T>( instance, propOrFieldName, ReflectionHelpers.MostAccess, out val );
			}
			return true;
		}

		public static bool Get<T>( Object instance, string propOrFieldName, BindingFlags flags, out T val ) {
			if( !ReflectionHelpers.GetField<T>( instance, propOrFieldName, flags, out val ) ) {
				return ReflectionHelpers.GetProperty<T>( instance, propOrFieldName, flags, out val );
			}
			return true;
		}

		public static bool Get<T>( Type objType, Object instance, string propOrFieldName, out T val ) {
			if( !ReflectionHelpers.GetField<T>( objType, instance, propOrFieldName, out val ) ) {
				return ReflectionHelpers.GetProperty<T>( objType, instance, propOrFieldName, ReflectionHelpers.MostAccess, out val );
			}
			return true;
		}

		public static bool Get<T>( Type objType, Object instance, string propOrFieldName, BindingFlags flags, out T val ) {
			if( !ReflectionHelpers.GetField<T>( objType, instance, propOrFieldName, flags, out val ) ) {
				return ReflectionHelpers.GetProperty<T>( objType, instance, propOrFieldName, flags, out val );
			}
			return true;
		}
		
		////////////////
		
		public static bool Set( Object instance, string propOrFieldName, object value ) {
			if( !ReflectionHelpers.SetField(instance, propOrFieldName, value) ) {
				return ReflectionHelpers.SetProperty( instance, propOrFieldName, value );
			}
			return true;
		}
	}
}
