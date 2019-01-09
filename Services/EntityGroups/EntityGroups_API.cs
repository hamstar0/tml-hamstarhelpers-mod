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
			var entGrps = ModHelpersMod.Instance.EntityGroups;
			entGrps.IsEnabled = true;
		}


		////////////////

		public static void AddCustomItemGroup( string groupName, string[] groupDependencies, Func<Item, IDictionary<string, ISet<int>>, bool> matcher ) {
			lock( EntityGroups.MyLock ) {
				var entGrps = ModHelpersMod.Instance.EntityGroups;
				if( !entGrps.IsEnabled ) { throw new Exception("Entity groups not enabled."); }
				if( entGrps.CustomItemMatchers == null ) { throw new Exception( "Mods loaded; cannot add new groups." ); }

				var entry = new Tuple<string, string[], ItemMatcher>( groupName, groupDependencies, matcher );
				entGrps.CustomItemMatchers.Add( entry );
			}
		}

		public static void AddCustomNPCGroup( string name, string[] groupDependencies, Func<NPC, IDictionary<string, ISet<int>>, bool> matcher ) {
			lock( EntityGroups.MyLock ) {
				var entGrps = ModHelpersMod.Instance.EntityGroups;
				if( !entGrps.IsEnabled ) { throw new Exception( "Entity groups not enabled." ); }
				if( entGrps.CustomNPCMatchers == null ) { throw new Exception( "Mods loaded; cannot add new groups." ); }

				var entry = new Tuple<string, string[], NPCMatcher>( name, groupDependencies, matcher );
				entGrps.CustomNPCMatchers.Add( entry );
			}
		}

		public static void AddCustomProjectileGroup( string name, string[] groupDependencies, Func<Projectile, IDictionary<string, ISet<int>>, bool> matcher ) {
			lock( EntityGroups.MyLock ) {
				var entGrps = ModHelpersMod.Instance.EntityGroups;
				if( !entGrps.IsEnabled ) { throw new Exception( "Entity groups not enabled." ); }
				if( entGrps.CustomProjMatchers == null ) { throw new Exception( "Mods loaded; cannot add new groups." ); }

				var entry = new Tuple<string, string[], ProjMatcher>( name, groupDependencies, matcher );
				entGrps.CustomProjMatchers.Add( entry );
			}
		}


		////////////////

		public static IReadOnlyDictionary<string, ReadOnlySet<int>> ItemGroups { get { return ModHelpersMod.Instance.EntityGroups._ItemGroups; } }
		public static IReadOnlyDictionary<string, ReadOnlySet<int>> NPCGroups { get { return ModHelpersMod.Instance.EntityGroups._NPCGroups; } }
		public static IReadOnlyDictionary<string, ReadOnlySet<int>> ProjectileGroups { get { return ModHelpersMod.Instance.EntityGroups._ProjGroups; } }


		public static IReadOnlyDictionary<int, ReadOnlySet<string>> GroupsPerItem { get { return ModHelpersMod.Instance.EntityGroups._GroupsPerItem; } }
		public static IReadOnlyDictionary<int, ReadOnlySet<string>> GroupsPerNPC { get { return ModHelpersMod.Instance.EntityGroups._GroupsPerNPC; } }
		public static IReadOnlyDictionary<int, ReadOnlySet<string>> GroupsPerProj { get { return ModHelpersMod.Instance.EntityGroups._GroupsPerProj; } }
	}
}
