﻿using HamstarHelpers.Components.DataStructures;
using ReLogic.Reflection;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.Projectiles {
	/// <summary>
	/// Assorted static "helper" functions pertaining to players relative to projectile identification
	/// </summary>
	public partial class ProjectileIdentityHelpers {
		private static IdDictionary ProjectileIdSearch = IdDictionary.Create( typeof(ProjectileID), typeof(short) );



		////////////////

		/// <summary>
		/// Gets a (human readable) unique key from a given projectile type.
		/// </summary>
		/// <param name="projType"></param>
		/// <returns></returns>
		[Obsolete( "use ProjectileID.GetUniqueKey(int)" )]
		public static string GetUniqueKey( int projType ) {
			if( projType < 0 || projType >= ProjectileLoader.ProjectileCount ) {
				throw new ArgumentOutOfRangeException( "Invalid type: " + projType );
			}
			if( projType < ProjectileID.Count ) {
				return "Terraria " + ProjectileIdentityHelpers.ProjectileIdSearch.GetName( projType );
			}

			var modProjectile = ProjectileLoader.GetProjectile( projType );
			return $"{modProjectile.mod.Name} {modProjectile.Name}";
		}

		/// <summary>
		/// Gets a (human readable) unique key from a given projectile.
		/// </summary>
		/// <param name="projectile"></param>
		/// <returns></returns>
		[Obsolete( "use ProjectileID.GetUniqueKey(Projectile)" )]
		public static string GetUniqueKey( Projectile projectile ) => ProjectileIdentityHelpers.GetUniqueKey( projectile.type );

		/// <summary>
		/// Gets a (human readable) unique key (as segments) from a given projectile type.
		/// </summary>
		/// <param name="projType"></param>
		/// <returns></returns>
		public static Tuple<string, string> GetUniqueKeySegs( int projType ) {
			if( projType < 0 || projType >= ProjectileLoader.ProjectileCount ) {
				throw new ArgumentOutOfRangeException( "Invalid type: " + projType );
			}
			if( projType < ProjectileID.Count ) {
				return Tuple.Create( "Terraria", ProjectileIdentityHelpers.ProjectileIdSearch.GetName( projType ) );
			}

			var modProjectile = ProjectileLoader.GetProjectile( projType );
			return Tuple.Create( modProjectile.mod.Name, modProjectile.Name );
		}

		////

		/// <summary>
		/// Gets a projectile type from a given unique key.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		[Obsolete( "use ProjectileID.TypeFromUniqueKey(string)" )]
		public static int TypeFromUniqueKey( string key ) {
			string[] parts = key.Split( new char[] { ' ' }, 2 );

			if( parts.Length != 2 ) {
				return 0;
			}
			return ProjectileIdentityHelpers.TypeFromUniqueKey( parts[0], parts[1] );
		}

		/// <summary>
		/// Gets a projectile type from a given unique key.
		/// </summary>
		/// <param name="mod"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		[Obsolete( "use ProjectileID.TypeFromUniqueKey(string)" )]
		public static int TypeFromUniqueKey( string mod, string name ) {
			if( mod == "Terraria" ) {
				if( !ProjectileIdentityHelpers.ProjectileIdSearch.ContainsName( name ) ) {
					return 0;
				}
				return ProjectileIdentityHelpers.ProjectileIdSearch.GetId( name );
			}
			return ModLoader.GetMod( mod )?.NPCType( name ) ?? 0;
		}


		////////////////

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
