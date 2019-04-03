using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Reflection;


namespace HamstarHelpers.Helpers.DotNetHelpers {
	public partial class ReflectionHelpers {
		[Obsolete( "use `bool Get<T>(object, string, out T)`", true )]
		public static bool GetField<T>( Object instance, string fieldName, out T fieldVal ) {
			return ReflectionHelpers.GetField<T>( instance.GetType(), instance, fieldName, out fieldVal );
		}

		[Obsolete( "use `bool Get<T>(Type, object, string, out T)`", true )]
		public static bool GetField<T>( Type objType, Object instance, string fieldName, out T fieldVal ) {
			bool success;
			fieldVal = (T)ReflectionHelpers.GetField( objType, instance, fieldName, ReflectionHelpers.MostAccess, out success );
			return success;
		}

		////

		[Obsolete( "use `bool Set<T>(object, string, object)`", true )]
		public static bool SetField( Object instance, string fieldName, object value ) {
			return ReflectionHelpers.SetField( instance.GetType(), instance, fieldName, value );
		}

		[Obsolete( "use `bool Set<T>(Type, object, string, object)`", true )]
		public static bool SetField( Type objType, Object instance, string fieldName, object value ) {
			MemberInfo rawMember = ReflectionHelpers.Instance.GetCachedInfoMember( objType, fieldName );
			if( rawMember == null ) {
				return false;
			}
			
			FieldInfo field = objType.GetField( fieldName, ReflectionHelpers.MostAccess );
			if( field == null ) { return false; }

			field.SetValue( instance, value );
			return true;
		}


		////////////////

		[Obsolete( "use `bool Get<T>(object, string, out T)`", true )]
		public static bool GetProperty<T>( Object instance, string propName, out T propVal ) {
			propVal = default( T );
			if( instance == null ) { return false; }

			Type objType = instance.GetType();
			PropertyInfo prop = objType.GetProperty( propName, ReflectionHelpers.MostAccess );
			if( prop == null ) { return false; }

			propVal = (T)prop.GetValue( instance );
			return true;
		}

		[Obsolete( "use `bool Get<T>(Type, object, string, out T)`", true )]
		public static bool GetProperty<T>( Type objType, Object instance, string propName, out T propVal ) {
			propVal = default( T );

			PropertyInfo prop = objType.GetProperty( propName, ReflectionHelpers.MostAccess );
			if( prop == null ) { return false; }

			propVal = (T)prop.GetValue( instance );
			return true;
		}

		////

		[Obsolete( "use `bool Set<T>(object, string, object)`", true )]
		public static bool SetProperty( Object instance, string propName, object value ) {
			if( instance == null ) { return false; }

			Type objtype = instance.GetType();
			PropertyInfo prop = objtype.GetProperty( propName, ReflectionHelpers.MostAccess );
			if( prop == null ) { return false; }
			
			prop.SetValue( instance, value );
			return true;
		}
	}
}
