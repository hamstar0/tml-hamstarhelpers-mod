﻿using HamstarHelpers.Components.DataStructures;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.ProjectileHelpers {
	public class ProjectileIdentityHelpers {
		public static string GetProperUniqueId( int projType ) {
			var proj = new Projectile();
			proj.SetDefaults( projType );

			return ProjectileIdentityHelpers.GetProperUniqueId( proj );
		}

		public static string GetProperUniqueId( Projectile proj ) {
			if( proj.modProjectile == null ) {
				return "Terraria." + proj.type;
			}
			return proj.modProjectile.mod.Name + "." + proj.modProjectile.Name;
		}

		////

		public static string GetQualifiedName( Projectile proj ) {
			return ProjectileIdentityHelpers.GetQualifiedName( proj.type );
		}

		public static string GetQualifiedName( int projType ) {
			string name = Lang.GetProjectileName( projType ).Value;
			return name;
		}

		// TODO: GetVanillaSnapshotHash()


		////////////////

		public static ReadOnlyDictionaryOfSets<string, int> NamesToIds {
			get { return ModHelpersMod.Instance.ProjectileIdentityHelpers._NamesToIds; }
		}



		////////////////
		
		private ReadOnlyDictionaryOfSets<string, int> _NamesToIds = null;


		////////////////

		internal void PopulateNames() {
			var dict = new Dictionary<string, ISet<int>>();

			for( int i = 1; i < ProjectileLoader.ProjectileCount; i++ ) {
				string name = ProjectileIdentityHelpers.GetQualifiedName( i );

				if( dict.ContainsKey( name ) ) {
					dict[name].Add( i );
				} else {
					dict[name] = new HashSet<int>() { i };
				}
			}

			this._NamesToIds = new ReadOnlyDictionaryOfSets<string, int>( dict );
		}
	}
}
