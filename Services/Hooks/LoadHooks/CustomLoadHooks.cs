using HamstarHelpers.Helpers.Debug;
using System;
using Terraria;


namespace HamstarHelpers.Services.LoadHooks {
	/// <summary>
	/// Exists for use in validating custom load hooks.
	/// </summary>
	sealed public class CustomLoadHookValidator {
		internal object MyLock = new object();
		internal object ValidatorKey;
		internal Type ArgType;


		/// <summary></summary>
		/// <param name="validatorKey">Key to match for validation.</param>
		/// <param name="argType">Validates the object type used for passing arguments.</param>
		public CustomLoadHookValidator( object validatorKey, Type argType ) {
			this.ValidatorKey = validatorKey;
			this.ArgType = argType;
		}
	}




	public partial class CustomLoadHooks {
		private static object CustomHookLock = new object();
	}
}
