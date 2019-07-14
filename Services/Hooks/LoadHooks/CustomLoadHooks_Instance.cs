using HamstarHelpers.Helpers.Debug;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Services.LoadHooks {
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
