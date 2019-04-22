using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.TmlHelpers.ModHelpers;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.TmlHelpers {
	public static partial class TmlHelpers {
		[Obsolete( "use `ModIdentityHelpers.GetModUniqueName`", true )]
		public static string GetModUniqueName( Mod mod ) {
			return ModIdentityHelpers.GetModUniqueName( mod );
		}

		[Obsolete( "use `ModIdentityHelpers.FindDependencyModMajorVersionMismatches`", true )]
		public static IDictionary<Mod, Version> FindDependencyModMajorVersionMismatches( Mod mod ) {
			return ModIdentityHelpers.FindDependencyModMajorVersionMismatches( mod );
		}

		[Obsolete( "use `ModIdentityHelpers.FormatBadDependencyModList`", true )]
		public static string FormatBadDependencyModList( Mod mod ) {
			return ModIdentityHelpers.FormatBadDependencyModList( mod );
		}

		[Obsolete("use `ModIdentityHelpers.FormatBadDependencyModList`", true)]
		public static string ReportBadDependencyMods( Mod mod ) {
			return ModIdentityHelpers.FormatBadDependencyModList( mod );
		}


		/*public static string[] AssertCallParams( object[] args, Type[] types, bool[] nullables = null ) {
			if( args.Length != types.Length ) {
				return new string[] { "Mismatched input argument quantity." };
			}

			var errors = new List<string>();

			for( int i = 0; i < types.Length; i++ ) {
				if( args[i] == null ) {
					if( !types[i].IsClass || nullables == null || !nullables[i] ) {
						errors.Add( "Invalid paramater #" + i + ": Expected " + types[i].Name + ", found null" );
					}
				} else if( args[i].GetType() != types[i] ) {
					errors.Add( "Invalid parameter #" + i + ": Expected " + types[i].Name + ", found " + args[i].GetType() );
				}
			}

			return errors.ToArray();
		}*/
	}
}
