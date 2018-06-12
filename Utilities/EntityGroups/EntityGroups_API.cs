using HamstarHelpers.DebugHelpers;
using HamstarHelpers.DotNetHelpers.DataStructures;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Utilities.EntityGroups {
	public partial class EntityGroups {
		public static void Enable() {
			var ent_grps = HamstarHelpersMod.Instance.EntityGroups;
			ent_grps.IsEnabled = true;
		}


		////////////////

		public static void AddCustomItemGroup( string name, Func<Item, bool> matcher ) {
			lock( EntityGroups.MyLock ) {
				var ent_grps = HamstarHelpersMod.Instance.EntityGroups;
				if( !ent_grps.IsEnabled ) { throw new Exception("Entity groups not enabled."); }
				if( ent_grps.CustomItemMatchers == null ) { throw new Exception( "Mods loaded; cannot add new groups." ); }

				var entry = new KeyValuePair<string, Func<Item, bool>>( name, matcher );
				ent_grps.CustomItemMatchers.Add( entry );
			}
		}

		public static void AddCustomNPCGroup( string name, Func<NPC, bool> matcher ) {
			lock( EntityGroups.MyLock ) {
				var ent_grps = HamstarHelpersMod.Instance.EntityGroups;
				if( !ent_grps.IsEnabled ) { throw new Exception( "Entity groups not enabled." ); }
				if( ent_grps.CustomNPCMatchers == null ) { throw new Exception( "Mods loaded; cannot add new groups." ); }

				var entry = new KeyValuePair<string, Func<NPC, bool>>( name, matcher );
				ent_grps.CustomNPCMatchers.Add( entry );
			}
		}

		public static void AddCustomProjectileGroup( string name, Func<Projectile, bool> matcher ) {
			lock( EntityGroups.MyLock ) {
				var ent_grps = HamstarHelpersMod.Instance.EntityGroups;
				if( !ent_grps.IsEnabled ) { throw new Exception( "Entity groups not enabled." ); }
				if( ent_grps.CustomProjMatchers == null ) { throw new Exception( "Mods loaded; cannot add new groups." ); }

				var entry = new KeyValuePair<string, Func<Projectile, bool>>( name, matcher );
				ent_grps.CustomProjMatchers.Add( entry );
			}
		}


		////////////////

		public static IReadOnlyDictionary<string, ReadOnlySet<int>> ItemGroups {
			get {
				lock( EntityGroups.MyLock ) {
					return HamstarHelpersMod.Instance.EntityGroups._ItemGroups;
				}
			}
		}
		public static IReadOnlyDictionary<string, ReadOnlySet<int>> NPCGroups {
			get {
				lock( EntityGroups.MyLock ) {
					return HamstarHelpersMod.Instance.EntityGroups._NPCGroups;
				}
			}
		}
		public static IReadOnlyDictionary<string, ReadOnlySet<int>> ProjectileGroups {
			get {
				lock( EntityGroups.MyLock ) {
					return HamstarHelpersMod.Instance.EntityGroups._ProjGroups;
				}
			}
		}


		public static IReadOnlyDictionary<int, ReadOnlySet<string>> GroupsPerItem {
			get {
				lock( EntityGroups.MyLock ) {
					return HamstarHelpersMod.Instance.EntityGroups._GroupsPerItem;
				}
			}
		}
		public static IReadOnlyDictionary<int, ReadOnlySet<string>> GroupsPerNPC {
			get {
				lock( EntityGroups.MyLock ) {
					return HamstarHelpersMod.Instance.EntityGroups._GroupsPerNPC;
				}
			}
		}
		public static IReadOnlyDictionary<int, ReadOnlySet<string>> GroupsPerProj {
			get {
				lock( EntityGroups.MyLock ) {
					return HamstarHelpersMod.Instance.EntityGroups._GroupsPerProj;
				}
			}
		}
	}
}
