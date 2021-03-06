﻿using System;
using Terraria;
using HamstarHelpers.Classes.DataStructures;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.TModLoader;


namespace HamstarHelpers.Services.EntityGroups {
	/// <summary>
	/// Supplies collections of named entity groups based on traits shared between entities. Groups are either items, NPCs,
	/// or projectiles. Must be enabled on mod load to be used (note: collections may require memory).
	/// </summary>
	public partial class EntityGroups {
		/// <summary></summary>
		public static bool IsLoaded => ModHelpersMod.Instance.EntityGroups?.IsLoadedPostModLoad ?? false;


		/// <summary></summary>
		public static int ItemCount => ModHelpersMod.Instance.EntityGroups?.ItemGroups.Count ?? -1;

		/// <summary></summary>
		public static int NPCCount => ModHelpersMod.Instance.EntityGroups?.ItemGroups.Count ?? -1;

		/// <summary></summary>
		public static int ProjectileCount => ModHelpersMod.Instance.EntityGroups?.ItemGroups.Count ?? -1;



		////////////////

		/// <summary>
		/// Enables entity groups. Must be called before all mod load and setup functions are called (e.g. `Mod.Load()`).
		/// </summary>
		public static void Enable() {
			var mymod = ModHelpersMod.Instance;
			if( LoadHelpers.IsModLoaded() ) {
				throw new ModHelpersException( "Entity Groups must be enabled before mods finish loading." );
			}

			var entGrps = mymod.EntityGroups;
			entGrps.IsReadyToLoad = true;
		}


		////////////////

		/// <summary>
		/// Defines a custom item group to add to the database.
		/// 
		/// Reminder: Must be called after EntityGroups.Enable(), but before mods finish loading (PostSetupContent,
		/// PostAddRecipes, etc.).
		/// </summary>
		/// <param name="groupName"></param>
		/// <param name="groupDependencies">Other groups the current group references.</param>
		/// <param name="matcher">Function to use to match items for this group.</param>
		public static void AddCustomItemGroup( string groupName, string[] groupDependencies, ItemGroupMatcher matcher ) {
			var entGrps = ModHelpersMod.Instance.EntityGroups;
			if( !entGrps.IsReadyToLoad ) { throw new ModHelpersException( "Entity groups not enabled." ); }
			if( entGrps.CustomItemMatchers == null ) { throw new ModHelpersException( "Mods loaded; cannot add new groups." ); }

			lock( EntityGroups.MyLock ) {
				var entry = new EntityGroupMatcherDefinition<Item>( groupName, groupDependencies, matcher );
				entGrps.CustomItemMatchers.Add( entry );
			}
		}

		/// <summary>
		/// Defines a custom NPC group to add to the database.
		/// 
		/// Reminder: Must be called after EntityGroups.Enable(), but before mods finish loading (PostSetupContent,
		/// PostAddRecipes, etc.).
		/// </summary>
		/// <param name="groupName"></param>
		/// <param name="groupDependencies">Other groups the current group references.</param>
		/// <param name="matcher">Function to use to match NPCs for this group.</param>
		public static void AddCustomNPCGroup( string groupName, string[] groupDependencies, NPCGroupMatcher matcher ) {
			lock( EntityGroups.MyLock ) {
				var entGrps = ModHelpersMod.Instance.EntityGroups;
				if( !entGrps.IsReadyToLoad ) { throw new Exception( "Entity groups not enabled." ); }
				if( entGrps.CustomNPCMatchers == null ) { throw new Exception( "Mods loaded; cannot add new groups." ); }

				var entry = new EntityGroupMatcherDefinition<NPC>( groupName, groupDependencies, matcher );
				entGrps.CustomNPCMatchers.Add( entry );
			}
		}

		/// <summary>
		/// Defines a custom Projectile group to add to the database.
		/// 
		/// Reminder: Must be called after EntityGroups.Enable(), but before mods finish loading (PostSetupContent,
		/// PostAddRecipes, etc.).
		/// </summary>
		/// <param name="groupName"></param>
		/// <param name="groupDependencies">Other groups the current group references.</param>
		/// <param name="matcher">Function to use to match NPCs for this group.</param>
		public static void AddCustomProjectileGroup( string groupName, string[] groupDependencies,
					ProjectileGroupMatcher matcher ) {
			lock( EntityGroups.MyLock ) {
				var entGrps = ModHelpersMod.Instance.EntityGroups;
				if( !entGrps.IsReadyToLoad ) { throw new Exception( "Entity groups not enabled." ); }
				if( entGrps.CustomProjMatchers == null ) { throw new Exception( "Mods loaded; cannot add new groups." ); }

				var entry = new EntityGroupMatcherDefinition<Projectile>( groupName, groupDependencies, matcher );
				entGrps.CustomProjMatchers.Add( entry );
			}
		}



		////////////////

		/// <summary>
		/// Retrieves an item group by its name.
		/// 
		/// Reminder: This must be called after entity groups are initialized (EntityGroups.Enable()), and then after mods are
		/// loaded (use CustomLoadHooks with EntityGroups.LoadedAllValidator).
		/// </summary>
		/// <param name="name"></param>
		/// <param name="group"></param>
		/// <returns></returns>
		public static bool TryGetItemGroup( string name, out IReadOnlySet<int> group ) {
			lock( EntityGroups.MyLock ) {
				return ModHelpersMod.Instance.EntityGroups.ItemGroups.TryGetValue( name, out group );
			}
		}
		/// <summary>
		/// Retrieves an NPC group by its name.
		/// 
		/// Reminder: This must be called after entity groups are initialized (EntityGroups.Enable()), and then after mods are
		/// loaded (use CustomLoadHooks with EntityGroups.LoadedAllValidator).
		/// </summary>
		/// <param name="name"></param>
		/// <param name="group"></param>
		/// <returns></returns>
		public static bool TryGetNpcGroup( string name, out IReadOnlySet<int> group ) {
			lock( EntityGroups.MyLock ) {
				return ModHelpersMod.Instance.EntityGroups.NPCGroups.TryGetValue( name, out group );
			}
		}
		/// <summary>
		/// Retrieves a projectile group by its name.
		/// 
		/// Reminder: This must be called after entity groups are initialized (EntityGroups.Enable()), and then after mods are
		/// loaded (use CustomLoadHooks with EntityGroups.LoadedAllValidator).
		/// </summary>
		/// <param name="name"></param>
		/// <param name="group"></param>
		/// <returns></returns>
		public static bool TryGetProjectileGroup( string name, out IReadOnlySet<int> group ) {
			lock( EntityGroups.MyLock ) {
				return ModHelpersMod.Instance.EntityGroups.ProjGroups.TryGetValue( name, out group );
			}
		}

		/// <summary>
		/// Retrieves all groups (names) of a given item (type).
		/// 
		/// Reminder: This must be called after entity groups are initialized (EntityGroups.Enable()), and then after mods are
		/// loaded (use CustomLoadHooks with EntityGroups.LoadedAllValidator).
		/// </summary>
		/// <param name="itemType"></param>
		/// <param name="groupNames"></param>
		/// <returns></returns>
		public static bool TryGetGroupsPerItem( int itemType, out IReadOnlySet<string> groupNames ) {
			lock( EntityGroups.MyLock ) {
				return ModHelpersMod.Instance.EntityGroups.GroupsPerItem.TryGetValue( itemType, out groupNames );
			}
		}
		/// <summary>
		/// Retrieves all groups (names) of a given NPC (type).
		/// 
		/// Reminder: This must be called after entity groups are initialized (EntityGroups.Enable()), and then after mods are
		/// loaded (use CustomLoadHooks with EntityGroups.LoadedAllValidator).
		/// </summary>
		/// <param name="npcType"></param>
		/// <param name="groupNames"></param>
		/// <returns></returns>
		public static bool TryGetGroupsPerNPC( int npcType, out IReadOnlySet<string> groupNames ) {
			lock( EntityGroups.MyLock ) {
				return ModHelpersMod.Instance.EntityGroups.GroupsPerNPC.TryGetValue( npcType, out groupNames );
			}
		}
		/// <summary>
		/// Retrieves all groups (names) of a given projectile (type).
		/// 
		/// Reminder: This must be called after entity groups are initialized (EntityGroups.Enable()), and then after mods are
		/// loaded (use CustomLoadHooks with EntityGroups.LoadedAllValidator).
		/// </summary>
		/// <param name="projType"></param>
		/// <param name="groupNames"></param>
		/// <returns></returns>
		public static bool TryGetGroupsPerProjectile( int projType, out IReadOnlySet<string> groupNames ) {
			lock( EntityGroups.MyLock ) {
				return ModHelpersMod.Instance.EntityGroups.GroupsPerProj.TryGetValue( projType, out groupNames );
			}
		}
	}
}
