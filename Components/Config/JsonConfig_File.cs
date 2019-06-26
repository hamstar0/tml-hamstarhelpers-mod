using System.IO;
using Terraria;
using System;
using HamstarHelpers.Helpers.Debug;
using System.Threading;


namespace HamstarHelpers.Components.Config {
	/**
	 * <summary>Implements handling of json file loading and saving.</summary>
	 * <typeparam name="T">Any class. Fields will map to JSON data.</typeparam>
	 */
	public partial class JsonConfig<T> : JsonConfigBase where T : class {
		/**
		 * <summary>Returns the absolute path to the folder of our config.</summary>
		 * <returns>File system path of config folder.</returns>
		 */
		public string GetPathOnly() {
			if( this.PathName != "" ) {
				return Main.SavePath + Path.DirectorySeparatorChar + this.PathName;
			}
			return Main.SavePath;
		}
		/**
		 * <summary>Returns the absolute path to the file of our config.</summary>
		 * <returns>File system path of config file.</returns>
		 */
		public string GetFullPath() {
			return this.GetPathOnly() + Path.DirectorySeparatorChar + this.FileName;
		}

		/**
		 * <summary>Sets the file name and relative path the config file will be saved to or loaded from.</summary>
		 * <param name="filename">File name of JSON file. No extension.</param>
		 * <param name="pathname">Folder of the config file relative to the ModLoader folder.</param>
		 */
		public void SetFilePath( string filename, string pathname ) {
			this.FileName = filename;
			this.PathName = pathname;
		}


		/**
		 * <summary>Indicates if config file exists.</summary>
		 * <returns>Returns `true` if config file exists.</returns>
		 */
		public bool FileExists() {
			lock( JsonConfigBase.MyFileLock ) {
				return File.Exists( this.GetFullPath() );
			}
		}
		

		/**
		 * <summary>Loads the config data from file.</summary>
		 * <returns>Returns `true` if the config file loaded.</returns>
		 */
		public bool LoadFile() {
			string path = this.GetFullPath();
			string json;
			bool success = true;

			lock( JsonConfigBase.MyFileLock ) {
				if( !File.Exists( path ) ) {
					success = false;
				}
			}

			if( success ) {
				using( StreamReader r = new StreamReader( path ) ) {
					lock( JsonConfigBase.MyFileLock ) {
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

		/**
		 * <summary>Saves config data to file.</summary>
		 */
		public void SaveFile() {
			string path = this.GetFullPath();
			string json = this.SerializeMe();

			lock( JsonConfigBase.MyFileLock ) {
				File.WriteAllText( path, json );
			}

			if( this.Data is ConfigurationDataBase ) {
				var data = (object)this.Data;
				var configData = (ConfigurationDataBase)data;
				configData.OnSave();
			}
		}

		/**
		 * <summary>Destroys the config file.</summary>
		 */
		public bool DestroyFile() {
			string path = this.GetFullPath();

			lock( JsonConfigBase.MyFileLock ) {
				if( !File.Exists( path ) ) { return false; }

				File.Delete( path );
			}

			return true;
		}

		

		/**
		 * <summary>Loads the config data from file asynchronously.</summary>
		 * <param name="onCompletion">Runs on completion.</param>
		 */
		public void LoadFileAsync( Action<bool> onCompletion ) {
			ThreadPool.QueueUserWorkItem( _ => {
				onCompletion( this.LoadFile() );
			} );
		}

		/**
		 * <summary>Saves config data to file asynchronously.</summary>
		 * <param name="onCompletion">Runs on completion.</param>
		 */
		public void SaveFileAsync( Action onCompletion ) {
			ThreadPool.QueueUserWorkItem( _ => {
				this.SaveFile();
				onCompletion();
			} );
		}
	}
}
