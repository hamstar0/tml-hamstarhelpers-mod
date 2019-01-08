using System.IO;
using Terraria;
using Newtonsoft.Json;
using System;
using HamstarHelpers.Helpers.DebugHelpers;


namespace HamstarHelpers.Components.Config {
	public class JsonConfig {
		protected static readonly object MyLock = new object();
		protected static readonly object MyFileLock = new object();


		////////////////

		public static string ConfigSubfolder { get { return "Mod Configs"; } }
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
			return JsonConfig<T>.Serialize( data, new JsonSerializerSettings() );
		}
		public static T Deserialize( string data ) {
			return JsonConfig<T>.Deserialize( data, new JsonSerializerSettings() );
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
			this( fileName, relativePath, new JsonSerializerSettings() ) { }

		public JsonConfig( string fileName, string relativePath, T defaultsCopyOnly ) :
			this( fileName, relativePath, defaultsCopyOnly, new JsonSerializerSettings() ) { }


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
			this.Data = data;
		}


		////////////////

		public string GetPathOnly() {
			if( this.PathName != "" ) {
				return Main.SavePath + Path.DirectorySeparatorChar + this.PathName;
			}
			return Main.SavePath;
		}
		public string GetFullPath() {
			return this.GetPathOnly() + Path.DirectorySeparatorChar + this.FileName;
		}

		public void SetFilePath( string filename, string pathname ) {
			this.FileName = filename;
			this.PathName = pathname;
		}


		public bool FileExists() {
			lock( JsonConfig.MyFileLock ) {
				return File.Exists( this.GetFullPath() );
			}
		}

		////////////////

		public bool LoadFile() {
			string path = this.GetFullPath();
			string json;
			bool success = true;

			lock( JsonConfig.MyFileLock ) {
				if( !File.Exists( path ) ) {
					success = false;
				}
			}

			if( success ) {
				using( StreamReader r = new StreamReader( path ) ) {
					lock( JsonConfig.MyFileLock ) {
						json = r.ReadToEnd();
					}
					this.DeserializeMe( json, out success );
				}
			}

			if( this.Data is ConfigurationDataBase ) {
				var data = (object)this.Data;
				var configData = (ConfigurationDataBase)data;
				configData.OnLoad( success );
			}

			return success;
		}

		public void SaveFile() {
			string path = this.GetFullPath();
			string json = this.SerializeMe();

			lock( JsonConfig.MyFileLock ) {
				File.WriteAllText( path, json );
			}

			if( this.Data is ConfigurationDataBase ) {
				var data = (object)this.Data;
				var configData = (ConfigurationDataBase)data;
				configData.OnSave();
			}
		}

		public bool DestroyFile() {
			string path = this.GetFullPath();

			lock( JsonConfig.MyFileLock ) {
				if( !File.Exists( path ) ) { return false; }

				File.Delete( path );
			}

			return true;
		}
	}
}
