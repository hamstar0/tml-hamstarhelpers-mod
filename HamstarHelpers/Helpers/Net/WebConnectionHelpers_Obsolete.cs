using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Debug;
using System;


namespace HamstarHelpers.Helpers.Net {
	/// <summary>
	/// Assorted static "helper" functions pertaining to connecting to the web.
	/// </summary>
	public partial class WebConnectionHelpers {
		/// @private
		[Obsolete( "use MakePostRequestAsync(string, string, Action<Exception> onError, Action<bool, string>)", true )]
		public static void MakePostRequestAsync(
					string url,
					string jsonData,
					Action<Exception, string> onError,
					Action<bool, string> onCompletion=null ) {
			WebConnectionHelpers.MakePostRequestAsync( url, jsonData, ( e ) => onError( e, "" ), onCompletion );
		}


		/// @private
		[Obsolete( "use MakeGetRequestAsync(string, Action<Exception> onError, Action<bool, string>)", true )]
		public static void MakeGetRequestAsync(
					string url,
					Action<Exception, string> onError,
					Action<bool, string> onCompletion = null ) {
			WebConnectionHelpers.MakeGetRequestAsync( url, ( e ) => onError( e, "" ), onCompletion );
		}
	}
}
