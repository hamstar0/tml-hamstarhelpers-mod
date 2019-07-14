using HamstarHelpers.Helpers.Debug;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Services.LoadHooks {
	/// <summary>
	/// Allows defining custom load hooks. Like the preset load hooks, these will activate when conditions are triggered
	/// for hooks that are later added.
	/// </summary>
	public partial class CustomLoadHooks {
		private IDictionary<CustomLoadHookValidator, List<Func<object, bool>>> Hooks
				= new Dictionary<CustomLoadHookValidator, List<Func<object, bool>>>();
		private ISet<CustomLoadHookValidator> HookConditionsMet
				= new HashSet<CustomLoadHookValidator>();
		private IDictionary<CustomLoadHookValidator, object> HookArgs
				= new Dictionary<CustomLoadHookValidator, object>();



		////////////////

		internal CustomLoadHooks() { }
	}
}
