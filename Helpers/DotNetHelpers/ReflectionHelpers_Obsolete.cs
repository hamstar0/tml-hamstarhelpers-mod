using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Reflection;


namespace HamstarHelpers.Helpers.DotNetHelpers {
	public partial class ReflectionHelpers {
		[Obsolete( "use `bool GetField(..., out object field_val)`)", true)]
		public static object GetField( Object instance, string field_name, out bool success ) {
			object obj;
			success = ReflectionHelpers.GetField( instance, field_name, out obj );
			return success;
		}
		[Obsolete( "use `bool GetField(..., out object field_val)`)", true )]
		public static object GetField( Object instance, string field_name, BindingFlags flags, out bool success ) {
			object obj;
			success = ReflectionHelpers.GetField( instance, field_name, flags, out obj );
			return success;
		}
		[Obsolete( "use `bool GetField(..., out object field_val)`)", true )]
		public static object GetField( Type obj_type, Object instance, string field_name, BindingFlags flags, out bool success ) {
			object obj;
			success = ReflectionHelpers.GetField( obj_type, instance, field_name, flags, out obj );
			return success;
		}


		[Obsolete( "use `bool SetField(...)`)", true )]
		public static void SetField( Object obj, string field_name, object value, out bool success ) {
			success = ReflectionHelpers.SetField( obj, field_name, value );
		}
		[Obsolete( "use `bool SetField(...)`)", true )]
		public static void SetField( Object obj, string field_name, object value, BindingFlags flags, out bool success ) {
			success = ReflectionHelpers.SetField( obj, field_name, value, flags );
		}


		[Obsolete( "use `bool GetProperty(..., out object prop_val)`)", true )]
		public static object GetProperty( Object instance, string prop_name, out bool success ) {
			object obj;
			success = ReflectionHelpers.GetProperty( instance, prop_name, out obj );
			return obj;
		}
		[Obsolete( "use `bool GetProperty(..., out object prop_val)`)", true )]
		public static object GetProperty( Object instance, string prop_name, BindingFlags flags, out bool success ) {
			object obj;
			success = ReflectionHelpers.GetProperty( instance, prop_name, flags, out obj );
			return obj;
		}
		[Obsolete( "use `bool GetProperty(..., out object prop_val)`)", true )]
		public static object GetProperty( Type obj_type, Object instance, string prop_name, BindingFlags flags, out bool success ) {
			object obj;
			success = ReflectionHelpers.GetProperty( obj_type, instance, prop_name, flags, out obj );
			return obj;
		}


		[Obsolete( "use `bool SetProperty(...)`)", true )]
		public static void SetProperty( Object obj, string prop_name, object value, out bool success ) {
			success = ReflectionHelpers.SetProperty( obj, prop_name, value );
		}


		[Obsolete( "use `bool RunMethod(..., out object return_val)`)", true )]
		public static object RunMethod( Object obj, string method_name, object[] args, out bool success ) {
			object return_val;
			success = ReflectionHelpers.RunMethod( obj, method_name, args, out return_val );
			return return_val;
		}
		[Obsolete( "use `bool RunMethod(..., out object return_val)`)", true )]
		public static object RunMethod( Object obj, string method_name, BindingFlags flags, object[] args, out bool success ) {
			object return_val;
			success = ReflectionHelpers.RunMethod( obj, method_name, flags, args, out return_val );
			return return_val;
		}
	}
}
