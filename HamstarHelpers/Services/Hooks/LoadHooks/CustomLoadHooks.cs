using HamstarHelpers.Libraries.Debug;
using System;
using Terraria;


namespace HamstarHelpers.Services.Hooks.LoadHooks {
	/// @private
	public interface ICustomLoadHookValidator { }




	/// <summary>
	/// Exists for use in validating custom load hooks.
	/// </summary>
	sealed public class CustomLoadHookValidator<T> : ICustomLoadHookValidator {
		internal object ValidatorKey;


		/// <summary></summary>
		/// <param name="validatorKey">Key to match for validation.</param>
		public CustomLoadHookValidator( object validatorKey ) {
			this.ValidatorKey = validatorKey;
		}
	}




	/// <summary>
	/// Allows defining custom load hooks. Like the preset load hooks, these will activate when conditions are triggered
	/// for hooks that are later added.
	/// </summary>
	public partial class CustomLoadHooks {
		//private static object HookLock = new object();
	}
}
