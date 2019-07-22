using HamstarHelpers.Helpers.Debug;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Services.Hooks.LoadHooks {
	/// <summary>
	/// Allows defining custom load hooks. Like the preset load hooks, these will activate when conditions are triggered
	/// for hooks that are later added.
	/// </summary>
	public partial class CustomLoadHooks {
		private IDictionary<ICustomLoadHookValidator, List<Func<object, bool>>> Hooks
				= new Dictionary<ICustomLoadHookValidator, List<Func<object, bool>>>();

		private ISet<ICustomLoadHookValidator> HookConditionsMet
				= new HashSet<ICustomLoadHookValidator>();

		private IDictionary<ICustomLoadHookValidator, object> HookArgs
				= new Dictionary<ICustomLoadHookValidator, object>();



		////////////////

		internal CustomLoadHooks() { }
	}
}
