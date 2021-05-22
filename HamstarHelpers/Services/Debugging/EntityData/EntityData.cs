using HamstarHelpers.Libraries.Entities;
using Newtonsoft.Json;
using System;
using System.IO;
using Terraria;


namespace HamstarHelpers.Services.Debug.EntityData {
	/// <summary>
	/// Provides functions for acquiring and dumping entity (NPC, Projectile, Item) data fields to file.
	/// </summary>
	public class EntityData {
		private static readonly object MyFileLock = new object();



		////////////////

		/// <summary>
		/// Dumps all NPC data to a JSON file in the ModLoader folder.
		/// </summary>
		public static void DumpAllNpcDataToJson() {
			string json = JsonConvert.SerializeObject( EntityInfoLibraries.GetAllNpcInfo() );
			
			lock( EntityData.MyFileLock ) {
				File.WriteAllText( Main.SavePath + Path.DirectorySeparatorChar + "All NPCs.json", json );
			}
		}

		/// <summary>
		/// Dumps all Item data to a JSON file in the ModLoader folder.
		/// </summary>
		public static void DumpAllItemDataToJson() {
			string json = JsonConvert.SerializeObject( EntityInfoLibraries.GetAllItemInfo() );

			lock( EntityData.MyFileLock ) {
				File.WriteAllText( Main.SavePath + Path.DirectorySeparatorChar + "All Items.json", json );
			}
		}

		/// <summary>
		/// Dumps all Projectile data to a JSON file in the ModLoader folder.
		/// </summary>
		public static void DumpAllProjectileDataToJson() {
			string json = JsonConvert.SerializeObject( EntityInfoLibraries.GetAllProjectileInfo() );

			lock( EntityData.MyFileLock ) {
				File.WriteAllText( Main.SavePath + Path.DirectorySeparatorChar + "All Projectiles.json", json );
			}
		}
	}
}
