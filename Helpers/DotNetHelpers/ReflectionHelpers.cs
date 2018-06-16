using System;
using System.Reflection;


namespace HamstarHelpers.Helpers {
	public class ModReferenceHelpers {
		public static object GetVariable( Object obj, string field_or_prop_name, out bool success ) {
			success = false;
			Type objtype = obj.GetType();
			FieldInfo field = objtype.GetField( field_or_prop_name );

			if( field != null ) {
				success = true;
				return field.GetValue( obj );
			} else {
				PropertyInfo prop = objtype.GetProperty( field_or_prop_name );
				if( prop == null ) {
					return null;
				}

				success = true;
				return prop.GetValue( obj );
			}
		}
		
		public static void SetVariable( Object obj, string field_or_prop_name, object value, out bool success ) {
			success = false;
			Type objtype = obj.GetType();
			FieldInfo field = objtype.GetField( field_or_prop_name );

			if( field != null ) {
				success = true;
				field.SetValue( obj, value );
			} else {
				PropertyInfo prop = objtype.GetProperty( field_or_prop_name );
				if( prop == null ) {
					return;
				}

				success = true;
				prop.SetValue( obj, value );
			}
		}

		public static object RunMethod( Object obj, string method_name, object[] args, out bool success ) {
			success = false;
			Type objtype = obj.GetType();
			MethodInfo method = objtype.GetMethod( method_name );

			if( method != null ) {
				success = true;
				return method.Invoke( obj, args );
			}
			return null;
		}
	}
}
