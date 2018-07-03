﻿using System.IO;
using Terraria;
using Newtonsoft.Json;
using System;
using HamstarHelpers.DebugHelpers;

namespace HamstarHelpers.Components.Config {
	public class JsonConfig {
		protected static readonly object MyLock = new object();
		protected static readonly object MyFileLock = new object();


		////////////////

		public static string ConfigSubfolder { get { return "Mod Configs"; } }
	}




	public partial class JsonConfig<T> : JsonConfig {
		public static string Serialize( T data ) {
			lock( JsonConfig.MyLock ) {
				return JsonConvert.SerializeObject( data, Formatting.Indented );
			}
		}
		public static T Deserialize( string data ) {
			lock( JsonConfig.MyLock ) {
				return JsonConvert.DeserializeObject<T>( data );
			}
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

			lock( JsonConfig.MyFileLock ) {
				Directory.CreateDirectory( Main.SavePath );
				Directory.CreateDirectory( this.GetPathOnly() );
			}
		}

		public JsonConfig( string file_name, string relative_path, T defaults_copy_only ) {
			this.FileName = file_name;
			this.PathName = relative_path;
			this.Data = defaults_copy_only;

			lock( JsonConfig.MyFileLock ) {
				Directory.CreateDirectory( Main.SavePath );
				Directory.CreateDirectory( this.GetPathOnly() );
			}
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
			lock( JsonConfig.MyFileLock ) {
				return File.Exists( this.GetFullPath() );
			}
		}


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
				var config_data = (ConfigurationDataBase)data;
				config_data.OnLoad( success );
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
				var config_data = (ConfigurationDataBase)data;
				config_data.OnSave();
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
