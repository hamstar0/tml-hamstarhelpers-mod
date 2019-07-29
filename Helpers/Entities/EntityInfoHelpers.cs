using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Helpers.Entities {
	public class EntityInfoHelpers {
		/// <summary>
		/// Generates a table of field and property defaults of a given NPC type.
		/// </summary>
		/// <param name="npcType"></param>
		/// <returns></returns>
		public static IDictionary<string, object> GetNpcInfo( int npcType ) {
			var data = new Dictionary<string, object>();
			var fields = typeof( NPC ).GetFields();
			var props = typeof( NPC ).GetProperties();

			NPC npc = new NPC();
			npc.SetDefaults( npcType );

			foreach( var field in fields ) {
				data[field.Name] = field.GetValue( npc );
			}
			foreach( var prop in props ) {
				data[prop.Name] = prop.GetValue( npc );
			}

			return data;
		}

		/// <summary>
		/// Generates a table of field and property defaults of a given Item type.
		/// </summary>
		/// <param name="itemType"></param>
		/// <returns></returns>
		public static IDictionary<string, object> GetItemInfo( int itemType ) {
			var data = new Dictionary<string, object>();
			var fields = typeof( Item ).GetFields();
			var props = typeof( Item ).GetProperties();

			Item item = new Item();
			item.SetDefaults( itemType );

			foreach( var field in fields ) {
				data[field.Name] = field.GetValue( item );
			}
			foreach( var prop in props ) {
				data[prop.Name] = prop.GetValue( item );
			}

			return data;
		}

		/// <summary>
		/// Generates a table of field and property defaults of a given Projectile type.
		/// </summary>
		/// <param name="projType"></param>
		/// <returns></returns>
		public static IDictionary<string, object> GetProjectileInfo( int projType ) {
			var data = new Dictionary<string, object>();
			var fields = typeof( Projectile ).GetFields();
			var props = typeof( Projectile ).GetProperties();

			Projectile proj = new Projectile();
			proj.SetDefaults( projType );

			foreach( var field in fields ) {
				data[field.Name] = field.GetValue( proj );
			}
			foreach( var prop in props ) {
				data[prop.Name] = prop.GetValue( proj );
			}

			return data;
		}


		////////////////

		/// <summary>
		/// Generates a table of tables of field and property defaults of each given NPC type.
		/// </summary>
		/// <returns></returns>
		public static IDictionary<int, IDictionary<string, object>> GetAllNpcInfo() {
			var data = new Dictionary<int, IDictionary<string, object>>();

			for( int i = 0; i < Main.npcTexture.Length; i++ ) {
				data[i] = EntityInfoHelpers.GetNpcInfo( i );
			}
			return data;
		}

		/// <summary>
		/// Generates a table of tables of field and property defaults of each given Item type.
		/// </summary>
		/// <returns></returns>
		public static IDictionary<int, IDictionary<string, object>> GetAllItemInfo() {
			var data = new Dictionary<int, IDictionary<string, object>>();

			for( int i = 0; i < Main.itemTexture.Length; i++ ) {
				data[i] = EntityInfoHelpers.GetItemInfo( i );
			}
			return data;
		}

		/// <summary>
		/// Generates a table of tables of field and property defaults of each given Projectile type.
		/// </summary>
		/// <returns></returns>
		public static IDictionary<int, IDictionary<string, object>> GetAllProjectileInfo() {
			var data = new Dictionary<int, IDictionary<string, object>>();

			for( int i = 0; i < Main.projectileTexture.Length; i++ ) {
				data[i] = EntityInfoHelpers.GetProjectileInfo( i );
			}
			return data;
		}
	}
}
