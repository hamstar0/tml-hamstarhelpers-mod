using HamstarHelpers.Components.DataStructures;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Services.EntityGroups {
	public partial class EntityGroups {
		public static void Enable() {
			var ent_grps = ModHelpersMod.Instance.EntityGroups;
			ent_grps.IsEnabled = true;
		}


		////////////////

		public static void AddCustomItemGroup( string name, Func<Item, bool> matcher ) {
			lock( EntityGroups.MyLock ) {
				var ent_grps = ModHelpersMod.Instance.EntityGroups;
				if( !ent_grps.IsEnabled ) { throw new Exception("Entity groups not enabled."); }
				if( ent_grps.CustomItemMatchers == null ) { throw new Exception( "Mods loaded; cannot add new groups." ); }

				var entry = new KeyValuePair<string, Func<Item, bool>>( name, matcher );
				ent_grps.CustomItemMatchers.Add( entry );
			}
		}

		public static void AddCustomNPCGroup( string name, Func<NPC, bool> matcher ) {
			lock( EntityGroups.MyLock ) {
				var ent_grps = ModHelpersMod.Instance.EntityGroups;
				if( !ent_grps.IsEnabled ) { throw new Exception( "Entity groups not enabled." ); }
				if( ent_grps.CustomNPCMatchers == null ) { throw new Exception( "Mods loaded; cannot add new groups." ); }

				var entry = new KeyValuePair<string, Func<NPC, bool>>( name, matcher );
				ent_grps.CustomNPCMatchers.Add( entry );
			}
		}

		public static void AddCustomProjectileGroup( string name, Func<Projectile, bool> matcher ) {
			lock( EntityGroups.MyLock ) {
				var ent_grps = ModHelpersMod.Instance.EntityGroups;
				if( !ent_grps.IsEnabled ) { throw new Exception( "Entity groups not enabled." ); }
				if( ent_grps.CustomProjMatchers == null ) { throw new Exception( "Mods loaded; cannot add new groups." ); }

				var entry = new KeyValuePair<string, Func<Projectile, bool>>( name, matcher );
				ent_grps.CustomProjMatchers.Add( entry );
			}
		}


		////////////////

		public static IReadOnlyDictionary<string, ReadOnlySet<int>> ItemGroups {
			get {
				return ModHelpersMod.Instance.EntityGroups._ItemGroups;
			}
		}
		public static IReadOnlyDictionary<string, ReadOnlySet<int>> NPCGroups {
			get {
				return ModHelpersMod.Instance.EntityGroups._NPCGroups;
			}
		}
		public static IReadOnlyDictionary<string, ReadOnlySet<int>> ProjectileGroups {
			get {
				return ModHelpersMod.Instance.EntityGroups._ProjGroups;
			}
		}


		public static IReadOnlyDictionary<int, ReadOnlySet<string>> GroupsPerItem {
			get {
				return ModHelpersMod.Instance.EntityGroups._GroupsPerItem;
			}
		}
		public static IReadOnlyDictionary<int, ReadOnlySet<string>> GroupsPerNPC {
			get {
				return ModHelpersMod.Instance.EntityGroups._GroupsPerNPC;
			}
		}
		public static IReadOnlyDictionary<int, ReadOnlySet<string>> GroupsPerProj {
			get {
				return ModHelpersMod.Instance.EntityGroups._GroupsPerProj;
			}
		}
	}
}
