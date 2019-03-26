using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;


namespace HamstarHelpers.Services.DataStore {
	public partial class DataStore {
		private IDictionary<object, object> Data = new Dictionary<object, object>();



		////////////////

		internal DataStore() { }


		////////////////

		public string Serialize() {
			return JsonConvert.SerializeObject( DataStore.GetAll(), Formatting.Indented );
		}
	}
}
