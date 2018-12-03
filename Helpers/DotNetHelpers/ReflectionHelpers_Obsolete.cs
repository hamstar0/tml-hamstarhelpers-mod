using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Reflection;


namespace HamstarHelpers.Helpers.DotNetHelpers {
	public partial class ReflectionHelpers {
		[Obsolete( "use `bool GetField(..., out object fieldVal)`)", true)]
		public static object GetField( Object instance, string fieldName, out bool success ) {
			object obj;
			success = ReflectionHelpers.GetField( instance, fieldName, out obj );
			return success;
		}
		[Obsolete( "use `bool GetField(..., out object fieldVal)`)", true )]
		public static object GetField( Object instance, string fieldName, BindingFlags flags, out bool success ) {
			object obj;
			success = ReflectionHelpers.GetField( instance, fieldName, flags, out obj );
			return success;
		}
		[Obsolete( "use `bool GetField(..., out object fieldVal)`)", true )]
		public static object GetField( Type objType, Object instance, string fieldName, BindingFlags flags, out bool success ) {
			object obj;
			success = ReflectionHelpers.GetField( objType, instance, fieldName, flags, out obj );
			return success;
		}


		[Obsolete( "use `bool SetField(...)`)", true )]
		public static void SetField( Object obj, string fieldName, object value, out bool success ) {
			success = ReflectionHelpers.SetField( obj, fieldName, value );
		}
		[Obsolete( "use `bool SetField(...)`)", true )]
		public static void SetField( Object obj, string fieldName, object value, BindingFlags flags, out bool success ) {
			success = ReflectionHelpers.SetField( obj, fieldName, flags, value );
		}


		[Obsolete( "use `bool GetProperty(..., out object propVal)`)", true )]
		public static object GetProperty( Object instance, string propName, out bool success ) {
			object obj;
			success = ReflectionHelpers.GetProperty( instance, propName, out obj );
			return obj;
		}
		[Obsolete( "use `bool GetProperty(..., out object propVal)`)", true )]
		public static object GetProperty( Object instance, string propName, BindingFlags flags, out bool success ) {
			object obj;
			success = ReflectionHelpers.GetProperty( instance, propName, flags, out obj );
			return obj;
		}
		[Obsolete( "use `bool GetProperty(..., out object propVal)`)", true )]
		public static object GetProperty( Type objType, Object instance, string propName, BindingFlags flags, out bool success ) {
			object obj;
			success = ReflectionHelpers.GetProperty( objType, instance, propName, flags, out obj );
			return obj;
		}


		[Obsolete( "use `bool SetProperty(...)`)", true )]
		public static void SetProperty( Object obj, string propName, object value, out bool success ) {
			success = ReflectionHelpers.SetProperty( obj, propName, value );
		}


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
