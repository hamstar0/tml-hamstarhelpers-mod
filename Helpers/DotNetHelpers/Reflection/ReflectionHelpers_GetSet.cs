using HamstarHelpers.Components.DataStructures;
using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.Debug;
using System;
using System.Reflection;


namespace HamstarHelpers.Helpers.DotNET.Reflection {
	public partial class ReflectionHelpers {
		private static bool GetMemberValue<T>( MemberInfo member, object instance, out T result ) {
			var field = member as FieldInfo;
			if( field != null ) {
				result = (T)field.GetValue( instance );
				return true;
			}

			var prop = member as PropertyInfo;
			if( prop != null ) {
				result = (T)prop.GetValue( instance );
				return true;
			}

			result = default( T );
			return false;
		}

		private static bool SetMemberValue<T>( MemberInfo member, object instance, T newValue ) {
			var field = member as FieldInfo;
			if( field != null ) {
				field.SetValue( instance, newValue );
				return true;
			}

			var prop = member as PropertyInfo;
			if( prop != null ) {
				prop.SetValue( instance, newValue );
				return true;
			}

			return false;
		}


		////////////////
		
		public static bool Get<T>( object instance, string fieldOrPropName, out T result ) {
			return ReflectionHelpers.Get<T>( instance.GetType(), instance, fieldOrPropName, out result );
		}

		public static bool Get<T>( Type objType, object instance, string fieldOrPropName, out T result ) {
			MemberInfo rawMember = ReflectionHelpers.Instance.GetCachedInfoMember( objType, fieldOrPropName );
			if( rawMember == null ) {
				result = default( T );
				return false;
			}

			return ReflectionHelpers.GetMemberValue( rawMember, instance, out result );
		}

		////////////////

		public static bool Set( object instance, string fieldOrPropName, object value ) {
			return ReflectionHelpers.Set( instance.GetType(), instance, fieldOrPropName, value );
		}

		public static bool Set( Type instanceType, object instance, string fieldOrPropName, object newValue ) {
			MemberInfo rawMember = ReflectionHelpers.Instance.GetCachedInfoMember( instanceType, fieldOrPropName );
			if( rawMember == null ) {
				return false;
			}

			return ReflectionHelpers.SetMemberValue( rawMember, instance, newValue );
		}


		////////////////

		public static bool GetDeep<T>( object instance, string concatenatedNames, out T result ) {
			return ReflectionHelpers.GetDeep<T>( instance.GetType(), instance, concatenatedNames.Split('.'), out result );
		}

		public static bool GetDeep<T>( object instance, string[] nestedNames, out T result ) {
			return ReflectionHelpers.GetDeep<T>( instance.GetType(), instance, nestedNames, out result );
		}

		public static bool GetDeep<T>( Type objType, object instance, string[] nestedNames, out T result ) {
			object prevObj = instance;

			int len = nestedNames.Length;
			for( int i=0; i<len; i++ ) {
				string name = nestedNames[i];

				if( !ReflectionHelpers.Get( prevObj, name, out prevObj ) ) {
					result = default( T );
					return false;
				}
			}

			result = (T)prevObj;
			return true;
		}

		////////////////

		public static bool SetDeep( object instance, string concatenatedNames, object value ) {
			return ReflectionHelpers.SetDeep( instance.GetType(), instance, concatenatedNames.Split('.'), value );
		}

		public static bool SetDeep( object instance, string[] nestedNames, object value ) {
			return ReflectionHelpers.SetDeep( instance.GetType(), instance, nestedNames, value );
		}

		public static bool SetDeep( Type instanceType, object instance, string[] nestedNames, object newValue ) {
			int namesLenCropped = nestedNames.Length - 1;
			object finalObj = instance;

			if( namesLenCropped > 0 ) {
				string[] nestedNamesCropped = nestedNames.Copy( namesLenCropped );

				if( !ReflectionHelpers.GetDeep( instanceType, instance, nestedNamesCropped, out finalObj ) ) {
					return false;
				}
			}

			return ReflectionHelpers.Set( instanceType, finalObj, nestedNames[namesLenCropped], newValue );
		}
	}
}
