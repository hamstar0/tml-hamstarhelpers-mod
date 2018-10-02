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
