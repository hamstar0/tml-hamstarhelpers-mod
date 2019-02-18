using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Linq;
using System.Reflection;


namespace HamstarHelpers.Helpers.DotNetHelpers {
	public partial class ReflectionHelpers {
		public static object SafeCall( MethodInfo method, object methodContext, object[] args ) {
			var paramInfos = method.GetParameters();

			if( args.Length != paramInfos.Length ) {
				throw new HamstarException( "Mismatched input argument quantity. (for call " + method.Name + ")" );
			}
			
			for( int i = 0; i < paramInfos.Length; i++ ) {
				Type paramType = paramInfos[i].ParameterType;

				if( args[i] == null ) {
					if( !paramType.IsClass || paramInfos[i].GetCustomAttribute<NullableAttribute>() == null ) {
						throw new HamstarException( "Invalid param "+paramInfos[i].Name+" (#"+i+"): Expected "+paramType.Name+", found null" );
					}
				} else if( args[i].GetType() != paramType ) {
					throw new HamstarException( "Invalid param " + paramInfos[i].Name+" (#"+i+"): Expected "+paramType.Name+", found "+args[i].GetType() );
				}
			}

			return method.Invoke( methodContext, args );
		}
		
		////////////////
		
		public static bool RunMethod<T>( Object instance, string methodName, object[] args, out T returnVal ) {
			returnVal = default( T );
			if( instance == null ) { return false; }

			Type[] paramTypes = args?.Select( o => o.GetType() ).ToArray()
				?? new Type[] { };

			Type objtype = instance.GetType();
			//MethodInfo method = objtype.GetMethod( methodName, ReflectionHelpers.MostAccess );
			MethodInfo method = objtype.GetMethod( methodName, ReflectionHelpers.MostAccess, null, paramTypes, null );
			if( method == null ) { return false; }

			method.Invoke( instance, args );
			return true;
		}

		public static bool RunMethod<T>( Object instance, string methodName, BindingFlags flags, object[] args, out T returnVal ) {
			returnVal = default( T );
			if( instance == null ) { return false; }

			Type[] paramTypes = args?.Select( o => o.GetType() ).ToArray()
				?? new Type[] { };

			Type objtype = instance.GetType();
			//MethodInfo method = objtype.GetMethod( methodName, flags );
			MethodInfo method = objtype.GetMethod( methodName, ReflectionHelpers.MostAccess, null, new Type[] { typeof( int ) }, null );
			if( method == null ) { return false; }

			method.Invoke( instance, args );
			return true;
		}
	}
}
