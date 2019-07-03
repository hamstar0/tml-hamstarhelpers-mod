using System;


namespace HamstarHelpers.Services.Debug.CustomHotkeys {
	/// <summary>
	/// Provides a pair of hotkeys that may be dynamically bound with custom functions (mostly for debug use).
	/// </summary>
	public partial class CustomHotkeys {
		public static void BindActionToKey1( string name, Action action ) {
			ModHelpersMod.Instance.CustomHotkeys.Key1Actions[ name ] = action;
		}

		public static void BindActionToKey2( string name, Action action ) {
			ModHelpersMod.Instance.CustomHotkeys.Key2Actions[ name ] = action;
		}
	}
}
