using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using System;


namespace HamstarHelpers.Helpers.TmlHelpers.ModHelpers {
	public class ModBoilerplateHelpers {
		public static object HandleModCall( Type apiClassType, params object[] args ) {
			if( args == null || args.Length == 0 ) { throw new HamstarException( "Undefined call." ); }

			string callType = args[0] as string;
			if( callType == null ) {
				LogHelpers.Alert( "Invalid call binding: " + args[0] );
				return null;
			}

			var methodInfo = apiClassType.GetMethod( callType );
			if( methodInfo == null ) {
				LogHelpers.Alert( "Unrecognized call binding " + callType + " with args:\n"
					+ string.Join( ",\n  ", args.SafeSelect( a => a.GetType().Name + ": " + a == null ? "null" : a.ToString() ) ) );
				return null;
			}

			var newArgs = new object[args.Length - 1];
			Array.Copy( args, 1, newArgs, 0, args.Length - 1 );

			try {
				return ReflectionHelpers.SafeCall( methodInfo, null, newArgs );
			} catch( Exception e ) {
				throw new HamstarException( "Bad API call.", e );
			}
		}
	}
}
