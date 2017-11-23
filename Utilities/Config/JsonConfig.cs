using System.IO;
using Terraria;
using Newtonsoft.Json;
using System;

namespace HamstarHelpers.Utilities.Config {
	public class JsonConfig<T> {
		public string FileName { get; private set; }
		public string PathName { get; private set; }
		public T Data { get; private set; }


		////////////////

		public static string Serialize( T data ) {
			return JsonConvert.SerializeObject( data, Formatting.Indented );
		}
		public static T Deserialize( string data ) {
			return JsonConvert.DeserializeObject<T>( data );
		}


		////////////////

		public JsonConfig( string filename, string pathname, T data ) {
			this.FileName = filename;
			this.PathName = pathname;
			this.Data = data;

			Directory.CreateDirectory( Main.SavePath );
			Directory.CreateDirectory( this.GetPathOnly() );
		}

		////////////////

		public string SerializeMe() {
			return JsonConfig<T>.Serialize( this.Data );
		}

		public void DeserializeMe( string data ) {
			this.Data = JsonConfig<T>.Deserialize( data );
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


		public bool LoadFile() {
			string path = this.GetFullPath();
			bool success = true;
			if( !File.Exists( path ) ) {
				success = false;
			}

			if( success ) {
				try {
					using( StreamReader r = new StreamReader( path ) ) {
						string json = r.ReadToEnd();
						this.DeserializeMe( json );
					}
				} catch( Exception e ) {
					DebugHelpers.DebugHelpers.Log( "JsonConfig.LoadFile() failed - " + e.ToString() );
					success = false;
				}
			}

			if( this.Data is ConfigurationDataBase ) {
				( (ConfigurationDataBase)(object)this.Data).OnLoad( success );
			}

			return success;
		}

		public void SaveFile() {
			string path = this.GetFullPath();
			string json = this.SerializeMe();
			File.WriteAllText( path, json );

			if( this.Data is ConfigurationDataBase ) {
				((ConfigurationDataBase)(object)this.Data).OnSave();
			}
		}

		public bool DestroyFile() {
			string path = this.GetFullPath();
			if( !File.Exists( path ) ) { return false; }

			File.Delete( path );
			return true;
		}
	}
}
