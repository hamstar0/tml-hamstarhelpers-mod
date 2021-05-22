using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Libraries.Debug;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;


namespace HamstarHelpers.Services.DataStore {
	/// @private
	public partial class DataStore {
		private IDictionary<object, object> Data = new Dictionary<object, object>();



		////////////////

		internal DataStore() { }


		////////////////

		/// <summary></summary>
		public string Serialize() {
			return JsonConvert.SerializeObject( DataStore.GetAll(), Formatting.Indented );
		}
	}
}
