using HamstarHelpers.Components.DataStructures;
using ReLogic.Reflection;
using Terraria;
using Terraria.ID;


namespace HamstarHelpers.Helpers.Projectiles {
	/// <summary>
	/// Assorted static "helper" functions pertaining to players relative to projectile identification
	/// </summary>
	public partial class ProjectileIdentityHelpers {
		private static IdDictionary ProjectileIdSearch = IdDictionary.Create( typeof(ProjectileID), typeof(short) );



		////////////////

		/// <summary>
		/// Gets the identifier of a projectile. For Terraria projectiles, this is `Terraria WoodArrow`, with the portion after
		/// "Terraria" being the NPC's field name in `NPCID`. For modded items, the format is NPCModName ModdedNPCInternalName;
		/// the mod name first, and the modded NPC's internal `Name` after.
		/// </summary>
		/// <param name="projType"></param>
		/// <returns></returns>
		public static string GetUniqueId( int projType ) {
			if( ProjectileIdentityHelpers.ProjectileIdSearch.ContainsId( projType ) ) {
				return "Terraria " + ProjectileIdentityHelpers.ProjectileIdSearch.GetName( projType );
			} else {
				var proj = new Projectile();
				proj.SetDefaults( projType );

				if( proj.modProjectile != null ) {
					return proj.modProjectile.mod.Name + " " + proj.modProjectile.Name;
				}
			}

			return "" + projType;
		}

		/// <summary>
		/// Gets the identifier of a projectile. For Terraria projectiles, this is `Terraria WoodArrow`, with the portion after
		/// "Terraria" being the NPC's field name in `NPCID`. For modded items, the format is NPCModName ModdedNPCInternalName;
		/// the mod name first, and the modded NPC's internal `Name` after.
		/// </summary>
		/// <param name="proj"></param>
		/// <returns></returns>
		public static string GetUniqueId( Projectile proj ) {
			if( proj.modProjectile == null ) {
				return "Terraria " + ProjectileIdentityHelpers.ProjectileIdSearch.GetName( proj.type );
			} else {
				return proj.modProjectile.mod.Name + " " + proj.modProjectile.Name;
			}
		}


		////

		/// <summary>
		/// Gets the "qualified" (human readable) name of a given projectile.
		/// </summary>
		/// <param name="proj"></param>
		/// <returns></returns>
		public static string GetQualifiedName( Projectile proj ) {
			return ProjectileIdentityHelpers.GetQualifiedName( proj.type );
		}

		/// <summary>
		/// Gets the "qualified" (human readable) name of a given projectile.
		/// </summary>
		/// <param name="projType"></param>
		/// <returns></returns>
		public static string GetQualifiedName( int projType ) {
			string name = Lang.GetProjectileName( projType ).Value;
			return name;
		}

		// TODO: GetVanillaSnapshotHash()


		////////////////

		/// <summary>
		/// Provides a map of (qualified) projectile names to their IDs.
		/// </summary>
		public static ReadOnlyDictionaryOfSets<string, int> NamesToIds {
			get { return ModHelpersMod.Instance.ProjectileIdentityHelpers._NamesToIds; }
		}
	}
}
