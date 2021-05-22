using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Terraria;
using HamstarHelpers.Libraries.Debug;


namespace HamstarHelpers.Services.Hooks.LoadHooks {
	public partial class CustomLoadHooks {
		private IDictionary<ICustomLoadHookValidator, List<Func<object, bool>>> Hooks
				= new ConcurrentDictionary<ICustomLoadHookValidator, List<Func<object, bool>>>();

		private ISet<ICustomLoadHookValidator> HookConditionsMet
				= new HashSet<ICustomLoadHookValidator>();

		private IDictionary<ICustomLoadHookValidator, object> HookArgs
				= new ConcurrentDictionary<ICustomLoadHookValidator, object>();



		////////////////

		internal CustomLoadHooks() { }
	}
}
