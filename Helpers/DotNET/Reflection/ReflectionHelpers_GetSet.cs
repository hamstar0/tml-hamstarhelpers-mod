using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Extensions;
using System;
using System.Reflection;


namespace HamstarHelpers.Helpers.DotNET.Reflection {
	/// <summary>
	/// Assorted static "helper" functions pertaining to reflection
	/// </summary>
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

		/// <summary>
		/// General use getter for fields or properties.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="instance">Instance of the object to get the member value of.</param>
		/// <param name="fieldOrPropName"></param>
		/// <param name="result"></param>
		/// <returns>`true` if field or property exist.</returns>
		public static bool Get<T>( object instance, string fieldOrPropName, out T result ) {
			if( instance == null ) {
				throw new ModHelpersException( "Null instances not allowed. Use the other Get function for static classes." );
			}
			if( instance is Type ) {
				throw new ModHelpersException( "Cannot get fields or properties from Type. Use the other Get<T>(...)." );
			}

			return ReflectionHelpers.Get<T>( instance.GetType(), instance, fieldOrPropName, out result );
		}

		/// <summary>
		/// General use getter for fields or properties. Accepts a `Type` parameter for static members.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="classType">Type of the object bearing the member to be set.</param>
		/// <param name="instance">Instance of the object to set the member value of. Use `null` for static.</param>
		/// <param name="fieldOrPropName"></param>
		/// <param name="result"></param>
		/// <returns>`true` if field or property exist.</returns>
		public static bool Get<T>( Type classType, object instance, string fieldOrPropName, out T result ) {
			ReflectionHelpers rh = ReflectionHelpers.Instance;
			MemberInfo rawMember = rh.GetCachedInfoMember( classType, fieldOrPropName );
			if( rawMember == null ) {
				result = default( T );
				return false;
			}

			return ReflectionHelpers.GetMemberValue( rawMember, instance, out result );
		}

		////////////////

		/// <summary>
		/// Sets field or property.
		/// </summary>
		/// <param name="instance">Instance of the object to set the member value of.</param>
		/// <param name="fieldOrPropName"></param>
		/// <param name="value"></param>
		/// <returns>`true` if member exists.</returns>
		public static bool Set( object instance, string fieldOrPropName, object value ) {
			return ReflectionHelpers.Set( instance.GetType(), instance, fieldOrPropName, value );
		}

		/// <summary>
		/// Sets field or property. Accepts a `Type` parameter for static members.
		/// </summary>
		/// <param name="classType">Type of the object bearing the member to be set.</param>
		/// <param name="instance">Instance of the object to set the member value of. Use `null` for static.</param>
		/// <param name="fieldOrPropName"></param>
		/// <param name="newValue"></param>
		/// <returns>`true` if member exists.</returns>
		public static bool Set( Type classType, object instance, string fieldOrPropName, object newValue ) {
			ReflectionHelpers rh = ReflectionHelpers.Instance;
			MemberInfo rawMember = rh.GetCachedInfoMember( classType, fieldOrPropName );
			if( rawMember == null ) { 
				return false;
			}

			return ReflectionHelpers.SetMemberValue( rawMember, instance, newValue );
		}


		////////////////

		/// <summary>
		/// Digs through a nested succession of (non-static) fields or properties of a given class to get a member's value.
		/// </summary>
		/// <typeparam name="T">Type of the value to obtain.</typeparam>
		/// <param name="instance">Top level class to start digging through.</param>
		/// <param name="concatenatedNames">Succession of member names at each level of the dig.</param>
		/// <param name="result">Deepest member's value.</param>
		/// <returns>`true` if member is found.</returns>
		public static bool GetDeep<T>( object instance, string concatenatedNames, out T result ) {
			return ReflectionHelpers.GetDeep<T>( instance, concatenatedNames.Split('.'), out result );
		}

		/// <summary>
		/// Digs through a nested succession of (non-static) fields or properties of a given class to get a member's value.
		/// </summary>
		/// <typeparam name="T">Type of the value to obtain.</typeparam>
		/// <param name="instance">Top level class to start digging through.</param>
		/// <param name="nestedNames">Succession of member names at each level of the dig.</param>
		/// <param name="result">Deepest member's value.</param>
		/// <returns>`true` if member is found.</returns>
		public static bool GetDeep<T>( object instance, string[] nestedNames, out T result ) {
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

		/// <summary>
		/// Digs through a nested succession of (non-static) fields or properties of a given class to set a member's value.
		/// </summary>
		/// <param name="instance">Top level class to start digging through.</param>
		/// <param name="concatenatedNames">Succession of member names at each level of the dig.</param>
		/// <param name="value"></param>
		/// <returns>`true` if member is found.</returns>
		public static bool SetDeep( object instance, string concatenatedNames, object value ) {
			return ReflectionHelpers.SetDeep( instance, concatenatedNames.Split('.'), value );
		}

		/// <summary>
		/// Digs through a nested succession of (non-static) fields or properties of a given class to set a member's value.
		/// </summary>
		/// <param name="instance">Top level class to start digging through.</param>
		/// <param name="nestedNames">Succession of member names at each level of the dig.</param>
		/// <param name="newValue"></param>
		/// <returns>`true` if member is found.</returns>
		public static bool SetDeep( object instance, string[] nestedNames, object newValue ) {
			int namesLenCropped = nestedNames.Length - 1;
			object finalObj = instance;

			if( namesLenCropped > 0 ) {
				string[] nestedNamesCropped = nestedNames.Copy( namesLenCropped );

				if( !ReflectionHelpers.GetDeep( instance, nestedNamesCropped, out finalObj ) ) {
					return false;
				}
			}

			return ReflectionHelpers.Set( finalObj, nestedNames[namesLenCropped], newValue );
		}
	}
}
