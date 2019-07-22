using HamstarHelpers.Helpers.Debug;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Services.Hooks.LoadHooks {
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
