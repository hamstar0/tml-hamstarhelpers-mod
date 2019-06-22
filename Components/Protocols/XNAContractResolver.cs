using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;


namespace HamstarHelpers.Components.Protocols {
	public class XnaContractResolver : DefaultContractResolver {
		public readonly static JsonSerializerSettings DefaultSettings = new JsonSerializerSettings() {
			ContractResolver = new XnaContractResolver()
		};


		////////////////

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
