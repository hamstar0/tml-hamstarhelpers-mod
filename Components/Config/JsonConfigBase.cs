using Newtonsoft.Json;
using System;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Components.Errors;


namespace HamstarHelpers.Components.Config {
	/**
	 * <summary>Implements handling of json file loading and saving.</summary>
	 */
	[Obsolete( "use ModConfig", false )]
	public class JsonConfigBase {
		protected static readonly object MyLock = new object();
		protected static readonly object MyFileLock = new object();

		

		/**
		 * <summary>Serializes string data to a given object of the specified type.</summary>
		 * <param name="data">String data to deserialize into an object. Must be JSON formatted.</param>
		 * <param name="dataType">`Type` of object to deserialize to. Existing fields should match with JSON fields.</param>
		 * <param name="jsonSettings">Allows for applying custom settings. Typically uses `XnaContractResolver.DefaultSettings`.</param>
		 * <returns>Deserialized object of the given type.</returns>
		 */
		public static object Deserialize( string data, Type dataType, JsonSerializerSettings jsonSettings ) {
			lock( JsonConfigBase.MyLock ) {
				return JsonConvert.DeserializeObject( data, dataType, jsonSettings );
			}
		}

		

		/**
		 * <summary>Specifies the folder (under %DOCUMENTS%/My Games/Terraria/ModLoader) where all configs are saved.</summary>
		 */
		public static string ConfigSubfolder => "Mod Configs";
	}
}
