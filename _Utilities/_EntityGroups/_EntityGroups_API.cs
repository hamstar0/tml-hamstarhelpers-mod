using HamstarHelpers.DebugHelpers;
using HamstarHelpers.DotNetHelpers.DataStructures;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Utilities.EntityGroups {
	[Obsolete( "use Services.EntityGroups.EntityGroups", true )]
	public partial class EntityGroups {
		public static void Enable() {
			Services.EntityGroups.EntityGroups.Enable();
		}


		////////////////

		public static void AddCustomItemGroup( string name, Func<Item, bool> matcher ) {
			Services.EntityGroups.EntityGroups.AddCustomItemGroup( name, matcher );
		}

		public static void AddCustomNPCGroup( string name, Func<NPC, bool> matcher ) {
			Services.EntityGroups.EntityGroups.AddCustomNPCGroup( name, matcher );
		}

		public static void AddCustomProjectileGroup( string name, Func<Projectile, bool> matcher ) {
			Services.EntityGroups.EntityGroups.AddCustomProjectileGroup( name, matcher );
		}


		////////////////

		public static IReadOnlyDictionary<string, ReadOnlySet<int>> ItemGroups {
			get {
				return Services.EntityGroups.EntityGroups.ItemGroups;
			}
		}
		public static IReadOnlyDictionary<string, ReadOnlySet<int>> NPCGroups {
			get {
				return Services.EntityGroups.EntityGroups.NPCGroups;
			}
		}
		public static IReadOnlyDictionary<string, ReadOnlySet<int>> ProjectileGroups {
			get {
				return Services.EntityGroups.EntityGroups.ProjectileGroups;
			}
		}


		public static IReadOnlyDictionary<int, ReadOnlySet<string>> GroupsPerItem {
			get {
				return Services.EntityGroups.EntityGroups.GroupsPerItem;
			}
		}
		public static IReadOnlyDictionary<int, ReadOnlySet<string>> GroupsPerNPC {
			get {
				return Services.EntityGroups.EntityGroups.GroupsPerNPC;
			}
		}
		public static IReadOnlyDictionary<int, ReadOnlySet<string>> GroupsPerProj {
			get {
				return Services.EntityGroups.EntityGroups.GroupsPerProj;
			}
		}
	}
}
