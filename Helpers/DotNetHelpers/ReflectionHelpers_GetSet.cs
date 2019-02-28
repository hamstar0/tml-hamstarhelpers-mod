using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Reflection;


namespace HamstarHelpers.Helpers.DotNetHelpers {
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
		
		public static bool Get<T>( Object instance, string fieldOrPropName, out T result ) {
			return ReflectionHelpers.Get<T>( instance.GetType(), instance, fieldOrPropName, out result );
		}

		public static bool Get<T>( Type objType, Object instance, string fieldOrPropName, out T result ) {
			MemberInfo rawMember = ReflectionHelpers.Instance.GetCachedInfoMember( objType, fieldOrPropName );
			if( rawMember == null ) {
				result = default( T );
				return false;
			}

			return ReflectionHelpers.GetMemberValue( rawMember, instance, out result );
		}

		////////////////

		public static bool Set( Object instance, string fieldOrPropName, object value ) {
			return ReflectionHelpers.Set( instance.GetType(), instance, fieldOrPropName, value );
		}

		public static bool Set( Type objType, Object instance, string fieldOrPropName, object newValue ) {
			MemberInfo rawMember = ReflectionHelpers.Instance.GetCachedInfoMember( objType, fieldOrPropName );
			if( rawMember == null ) {
				return false;
			}

			return ReflectionHelpers.SetMemberValue( rawMember, instance, newValue );
		}
	}
}
