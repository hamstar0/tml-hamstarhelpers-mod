using System.IO;
using Terraria;
using Newtonsoft.Json;
using System;
using HamstarHelpers.DebugHelpers;

namespace HamstarHelpers.Components.Config {
	public class JsonConfig {
		public static string ConfigSubfolder { get { return "Mod Configs"; } }
	}




	public partial class JsonConfig<T> : JsonConfig {
		public static string Serialize( T data ) {
			return JsonConvert.SerializeObject( data, Formatting.Indented );
		}
		public static T Deserialize( string data ) {
			return JsonConvert.DeserializeObject<T>( data );
		}


		////////////////
		
		public string FileName { get; private set; }
		public string PathName { get; private set; }
		public T Data { get; private set; }


		////////////////

		public JsonConfig( string file_name, string relative_path ) {
			this.FileName = file_name;
			this.PathName = relative_path;
			this.Data = (T)Activator.CreateInstance( typeof( T ) );

			Directory.CreateDirectory( Main.SavePath );
			Directory.CreateDirectory( this.GetPathOnly() );
		}

		public JsonConfig( string file_name, string relative_path, T defaults_copy_only ) {
			this.FileName = file_name;
			this.PathName = relative_path;
			this.Data = defaults_copy_only;

			Directory.CreateDirectory( Main.SavePath );
			Directory.CreateDirectory( this.GetPathOnly() );
		}


		////////////////

		public string SerializeMe() {
			return JsonConfig<T>.Serialize( this.Data );
		}

		public void DeserializeMe( string str_data, out bool success ) {
			success = false;
			try {
				T data = JsonConfig<T>.Deserialize( str_data );

				this.Data = data;
				success = true;
			} catch( Exception e ) {
				LogHelpers.Log( "JsonConfig.DeserializeMe - "+e.Message );
			}
		}
		[Obsolete( "use JsonConfig.DeserializeMe(string, out bool)", true )]
		public void DeserializeMe( string str_data ) {
			bool _;
			this.DeserializeMe( str_data, out _ );
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
			return File.Exists( this.GetFullPath() );
		}


		public bool LoadFile() {
			string path = this.GetFullPath();
			bool success = true;
			if( !File.Exists( path ) ) {
				success = false;
			}

			if( success ) {
				using( StreamReader r = new StreamReader( path ) ) {
					string json = r.ReadToEnd();
					this.DeserializeMe( json, out success );
				}
			}

			if( this.Data is ConfigurationDataBase ) {
				((ConfigurationDataBase)(object)this.Data).OnLoad( success );
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
