using System.IO;
using Terraria;
using Newtonsoft.Json;
using System;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Protocols;


namespace HamstarHelpers.Components.Config {
	[Obsolete( "use ModConfig", false )]
	public partial class JsonConfig<T> : JsonConfigBase where T : class {
		/** <summary>Serializes data from the given class to a JSON string.</summary>
		 * <param name="data">Data object to serialize.</param>
		 * <param name="jsonSettings">Allows for applying custom settings. Typically uses `XnaContractResolver.DefaultSettings`.</param>
		 * <returns>Serialized JSON string of the given object.</returns>
		 */
		public static string Serialize( T data, JsonSerializerSettings jsonSettings ) {
			lock( JsonConfigBase.MyLock ) {
				return JsonConvert.SerializeObject( data, Formatting.Indented, jsonSettings );
			}
		}
		/**
		 * <summary>Deserializes string data into a given object of the specified type.</summary>
		 * <param name="data">String data to deserialize into an object. Must be JSON formatted.</param>
		 * <param name="jsonSettings">Allows for applying custom settings. Typically uses `XnaContractResolver.DefaultSettings`.</param>
		 * <returns>Deserialized object of the given type.</returns>
		 */
		public static T Deserialize( string data, JsonSerializerSettings jsonSettings ) {
			lock( JsonConfigBase.MyLock ) {
				return JsonConvert.DeserializeObject<T>( data, jsonSettings );
			}
		}

		/**
		 * <summary>Serializes data from the given class to a JSON string.</summary>
		 * <param name="data">Data object to serialize.</param>
		 * <returns>Serialized JSON string of the given object.</returns>
		 */
		public static string Serialize( T data ) {
			return JsonConfig<T>.Serialize( data, XnaContractResolver.DefaultSettings );
		}
		/**
		 * <summary>Deserializes string data into a given object of the specified type.</summary>
		 * <param name="data">String data to deserialize into an object. Must be JSON formatted.</param>
		 * <returns>Deserialized object of the given type.</returns>
		 */
		public static T Deserialize( string data ) {
			return JsonConfig<T>.Deserialize( data, XnaContractResolver.DefaultSettings );
		}


		
		/** <summary>Name of the JSON file.</summary> */
		public string FileName { get; private set; }
		/** <summary>JSON file's (relative) path. Relative to ModLoader folder.</summary> */
		public string PathName { get; private set; }
		/** <summary>Config data object.</summary> */
		public T Data { get; private set; }

		private JsonSerializerSettings JsonSettings = null;


		
		/**
		 * <param name="fileName">Name of JSON file (minus extension).</param>
		 * <param name="relativePath">Path to JSON file (relative to ModLoader folder).</param>
		 * <param name="defaultsCopyOnly">Snapshot of object data.</param>
		 * <param name="jsonSettings">Allows for applying custom settings. Typically uses `XnaContractResolver.DefaultSettings`.</param>
		 */
		public JsonConfig( string fileName, string relativePath, T defaultsCopyOnly, JsonSerializerSettings jsonSettings ) {
			this.FileName = fileName;
			this.PathName = relativePath;
			this.Data = defaultsCopyOnly;
			this.JsonSettings = jsonSettings;

			lock( JsonConfigBase.MyFileLock ) {
				Directory.CreateDirectory( Main.SavePath );
				Directory.CreateDirectory( this.GetPathOnly() );
			}
		}

		/**
		 * <param name="fileName">Name of JSON file (minus extension).</param>
		 * <param name="relativePath">Path to JSON file (relative to ModLoader folder).</param>
		 * <param name="jsonSettings">Allows for applying custom settings. Typically uses `XnaContractResolver.DefaultSettings`.</param>
		 */
		public JsonConfig( string fileName, string relativePath, JsonSerializerSettings jsonSettings ) :
			this( fileName, relativePath, (T)Activator.CreateInstance( typeof(T) ), jsonSettings ) { }

		/**
		 * <param name="fileName">Name of JSON file (minus extension).</param>
		 * <param name="relativePath">Path to JSON file (relative to ModLoader folder).</param>
		 */
		public JsonConfig( string fileName, string relativePath ) :
			this( fileName, relativePath, XnaContractResolver.DefaultSettings ) { }
		
		/**
		 * <param name="fileName">Name of JSON file (minus extension).</param>
		 * <param name="relativePath">Path to JSON file (relative to ModLoader folder).</param>
		 * <param name="defaultsCopyOnly">Snapshot of object data.</param>
		 */
		public JsonConfig( string fileName, string relativePath, T defaultsCopyOnly ) :
			this( fileName, relativePath, defaultsCopyOnly, XnaContractResolver.DefaultSettings ) { }


		/**
		 * <summary>Serializes our current object data into a JSON string.</summary>
		 * <returns>Serialized JSON string of the current object.</returns>
		 */
		public string SerializeMe() {
			return JsonConfig<T>.Serialize( this.Data, this.JsonSettings );
		}

		/**
		 * <summary>Deserializes string data into the current object.</summary>
		 * <param name="strData">String data to deserialize into our object. Must be JSON formatted.</param>
		 * <param name="success">Returns `true` if data successfully deserialized.</param>
		 */
		public void DeserializeMe( string strData, out bool success ) {
			T data = null;
			success = false;

			try {
				data = JsonConfig<T>.Deserialize( strData, this.JsonSettings );

				this.Data = data;
				success = true;
			} catch( Exception e ) {
				LogHelpers.Warn( "Error for "+this.FileName+" (no input? "+(strData==null)+", no output? "+(data==null)+"): " + e.ToString() );
			}
		}

		/**
		 * <summary>Sets the current data object with the given object.</summary>
		 * <param name="data">Data object of our given type.</param>
		 */
		public void SetData( T data ) {
			if( data == null ) {
				throw new HamstarException( "Data must not be null." );
			}
			this.Data = data;
		}
	}
}
