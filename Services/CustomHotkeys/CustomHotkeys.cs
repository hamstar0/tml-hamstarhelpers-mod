using System;
using System.Collections.Generic;
using Terraria.GameInput;
using Terraria.ModLoader;


namespace HamstarHelpers.Services.CustomHotkeys {
	public partial class CustomHotkeys {
		public static void BindActionToKey1( string name, Action action ) {
			ModHelpersMod.Instance.CustomHotkeys.Key1Actions[ name ] = action;
		}

		public static void BindActionToKey2( string name, Action action ) {
			ModHelpersMod.Instance.CustomHotkeys.Key2Actions[ name ] = action;
		}
	}
}
