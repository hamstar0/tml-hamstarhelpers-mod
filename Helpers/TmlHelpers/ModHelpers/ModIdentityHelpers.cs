using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace HamstarHelpers.Helpers.TmlHelpers.ModHelpers {
	public class ModIdentityHelpers {
		private static IDictionary<Mod, string> ModIds = new Dictionary<Mod, string>();



		////////////////

		public static string GetModUniqueName( Mod mod ) {
			if( ModIdentityHelpers.ModIds.ContainsKey( mod ) ) {
				return ModIdentityHelpers.ModIds[mod];
			}
			ModIdentityHelpers.ModIds[mod] = mod.Name + ":" + mod.Version;
			return ModIdentityHelpers.ModIds[mod];
		}


		public static IDictionary<Mod, Version> FindDependencyModMajorVersionMismatches( Mod mod ) {
			Services.Tml.BuildPropertiesEditor buildEditor = Services.Tml.BuildPropertiesEditor.GetBuildPropertiesForModFile( mod.File );
			IDictionary<string, Version> modRefs = buildEditor.ModReferences;
			var badModDeps = new Dictionary<Mod, Version>();

			foreach( var kv in modRefs ) {
				Mod depMod = ModLoader.GetMod( kv.Key );
				if( depMod == null ) { continue; }

				if( depMod.Version.Major != kv.Value.Major ) {
					badModDeps[depMod] = kv.Value;
				}
			}

			return badModDeps;
		}

		////////////////

		public static string FormatBadDependencyModList( Mod mod ) {
			IDictionary<Mod, Version> badDepMods = ModIdentityHelpers.FindDependencyModMajorVersionMismatches( mod );

			if( badDepMods.Count != 0 ) {
				IEnumerable<string> badDepModsList = badDepMods.SafeSelect(
					kv => kv.Key.DisplayName + " (needs " + kv.Value.ToString() + ", is " + kv.Key.Version.ToString() + ")"
				);
				return mod.DisplayName + " (" + mod.Name + ") is out of date with its dependency mod(s): " + string.Join( ", \n", badDepModsList );
			}
			return null;
		}
	}
}
