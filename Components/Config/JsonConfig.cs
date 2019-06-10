using System.IO;
using Terraria;
using Newtonsoft.Json;
using System;
using HamstarHelpers.Helpers.Debug;
using Newtonsoft.Json.Serialization;
using HamstarHelpers.Components.Errors;

namespace HamstarHelpers.Components.Config {
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




	public class JsonConfig {
		protected static readonly object MyLock = new object();
		protected static readonly object MyFileLock = new object();


		////////////////
		
		public static object Deserialize( string data, Type dataType, JsonSerializerSettings jsonSettings ) {
			lock( JsonConfig.MyLock ) {
				return JsonConvert.DeserializeObject( data, dataType, jsonSettings );
			}
		}


		////////////////

		public static string ConfigSubfolder => "Mod Configs";
	}




	public partial class JsonConfig<T> : JsonConfig where T : class {
		public static string Serialize( T data, JsonSerializerSettings jsonSettings ) {
			lock( JsonConfig.MyLock ) {
				return JsonConvert.SerializeObject( data, Formatting.Indented, jsonSettings );
			}
		}
		public static T Deserialize( string data, JsonSerializerSettings jsonSettings ) {
			lock( JsonConfig.MyLock ) {
				return JsonConvert.DeserializeObject<T>( data, jsonSettings );
			}
		}

		public static string Serialize( T data ) {
			return JsonConfig<T>.Serialize( data, XnaContractResolver.DefaultSettings );
		}
		public static T Deserialize( string data ) {
			return JsonConfig<T>.Deserialize( data, XnaContractResolver.DefaultSettings );
		}



		////////////////

		public string FileName { get; private set; }
		public string PathName { get; private set; }
		public T Data { get; private set; }

		private JsonSerializerSettings JsonSettings = null;



		////////////////

		public JsonConfig( string fileName, string relativePath, T defaultsCopyOnly, JsonSerializerSettings jsonSettings ) {
			this.FileName = fileName;
			this.PathName = relativePath;
			this.Data = defaultsCopyOnly;
			this.JsonSettings = jsonSettings;

			lock( JsonConfig.MyFileLock ) {
				Directory.CreateDirectory( Main.SavePath );
				Directory.CreateDirectory( this.GetPathOnly() );
			}
		}

		public JsonConfig( string fileName, string relativePath, JsonSerializerSettings jsonSettings ) :
			this( fileName, relativePath, (T)Activator.CreateInstance( typeof(T) ), jsonSettings ) { }

		public JsonConfig( string fileName, string relativePath ) :
			this( fileName, relativePath, XnaContractResolver.DefaultSettings ) { }

		public JsonConfig( string fileName, string relativePath, T defaultsCopyOnly ) :
			this( fileName, relativePath, defaultsCopyOnly, XnaContractResolver.DefaultSettings ) { }


		////////////////

		public string SerializeMe() {
			return JsonConfig<T>.Serialize( this.Data, this.JsonSettings );
		}

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

		public void SetData( T data ) {
			if( data == null ) {
				throw new HamstarException( "Data must not be null." );
			}
			this.Data = data;
		}
	}
}
