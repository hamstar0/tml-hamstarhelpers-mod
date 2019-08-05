using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;


namespace HamstarHelpers.Classes.Protocols {
	/// <summary>
	/// Handy Newtonsoft serialization "contract resolver" for handling XNA fringe behavior.
	/// </summary>
	public class XNAContractResolver : DefaultContractResolver {
		/// <summary>
		/// Default instance of XNAContractResolver.
		/// </summary>
		public readonly static JsonSerializerSettings DefaultSettings = new JsonSerializerSettings() {
			ContractResolver = new XNAContractResolver()
		};


		////////////////

		/// @private
		protected override JsonContract CreateContract( Type objectType ) {
			switch( objectType.Name ) {
			case "Rectangle":
				//case "Vector2":
				//case "Vector3":
				//case "Vector4":
				return this.CreateObjectContract( objectType );
			}

			return base.CreateContract( objectType );
		}
	}
}
