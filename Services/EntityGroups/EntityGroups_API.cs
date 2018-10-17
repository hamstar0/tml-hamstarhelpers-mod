using HamstarHelpers.Components.DataStructures;
using System;
using System.Collections.Generic;
using Terraria;

using ItemMatcher = System.Func<Terraria.Item, System.Collections.Generic.IDictionary<string, System.Collections.Generic.ISet<int>>, bool>;
using NPCMatcher = System.Func<Terraria.NPC, System.Collections.Generic.IDictionary<string, System.Collections.Generic.ISet<int>>, bool>;
using ProjMatcher = System.Func<Terraria.Projectile, System.Collections.Generic.IDictionary<string, System.Collections.Generic.ISet<int>>, bool>;


namespace HamstarHelpers.Services.EntityGroups {
	public partial class EntityGroups {
		public static void Enable() {
			var ent_grps = ModHelpersMod.Instance.EntityGroups;
			ent_grps.IsEnabled = true;
		}


		////////////////

		public static void AddCustomItemGroup( string group_name, string[] group_dependencies, Func<Item, IDictionary<string, ISet<int>>, bool> matcher ) {
			lock( EntityGroups.MyLock ) {
				var ent_grps = ModHelpersMod.Instance.EntityGroups;
				if( !ent_grps.IsEnabled ) { throw new Exception("Entity groups not enabled."); }
				if( ent_grps.CustomItemMatchers == null ) { throw new Exception( "Mods loaded; cannot add new groups." ); }

				var entry = new Tuple<string, string[], ItemMatcher>( group_name, group_dependencies, matcher );
				ent_grps.CustomItemMatchers.Add( entry );
			}
		}

		public static void AddCustomNPCGroup( string name, string[] group_dependencies, Func<NPC, IDictionary<string, ISet<int>>, bool> matcher ) {
			lock( EntityGroups.MyLock ) {
				var ent_grps = ModHelpersMod.Instance.EntityGroups;
				if( !ent_grps.IsEnabled ) { throw new Exception( "Entity groups not enabled." ); }
				if( ent_grps.CustomNPCMatchers == null ) { throw new Exception( "Mods loaded; cannot add new groups." ); }

				var entry = new Tuple<string, string[], NPCMatcher>( name, group_dependencies, matcher );
				ent_grps.CustomNPCMatchers.Add( entry );
			}
		}

		public static void AddCustomProjectileGroup( string name, string[] group_dependencies, Func<Projectile, IDictionary<string, ISet<int>>, bool> matcher ) {
			lock( EntityGroups.MyLock ) {
				var ent_grps = ModHelpersMod.Instance.EntityGroups;
				if( !ent_grps.IsEnabled ) { throw new Exception( "Entity groups not enabled." ); }
				if( ent_grps.CustomProjMatchers == null ) { throw new Exception( "Mods loaded; cannot add new groups." ); }

				var entry = new Tuple<string, string[], ProjMatcher>( name, group_dependencies, matcher );
				ent_grps.CustomProjMatchers.Add( entry );
			}
		}


		////////////////

		public static IReadOnlyDictionary<string, ReadOnlySet<int>> ItemGroups => ModHelpersMod.Instance.EntityGroups._ItemGroups;
		public static IReadOnlyDictionary<string, ReadOnlySet<int>> NPCGroups => ModHelpersMod.Instance.EntityGroups._NPCGroups;
		public static IReadOnlyDictionary<string, ReadOnlySet<int>> ProjectileGroups => ModHelpersMod.Instance.EntityGroups._ProjGroups;


		public static IReadOnlyDictionary<int, ReadOnlySet<string>> GroupsPerItem => ModHelpersMod.Instance.EntityGroups._GroupsPerItem;
		public static IReadOnlyDictionary<int, ReadOnlySet<string>> GroupsPerNPC => ModHelpersMod.Instance.EntityGroups._GroupsPerNPC;
		public static IReadOnlyDictionary<int, ReadOnlySet<string>> GroupsPerProj => ModHelpersMod.Instance.EntityGroups._GroupsPerProj;
	}
}
