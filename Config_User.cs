using System;
using System.Reflection;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using HamstarHelpers.Helpers.User;
using HamstarHelpers.Services.Timers;


namespace HamstarHelpers {
	/// <summary>
	/// Defines config settings for a specific "privileged user" entry (used by assorted APIs).
	/// </summary>
	[Label( "Mod Helpers \"Privileged User\" (used by some APIs)" )]
	public class ModHelpersPrivilegedUserConfig : ModConfig {
		/// <summary>
		/// Gets the singleton instance of this config file.
		/// </summary>
		public static ModHelpersPrivilegedUserConfig Instance => ModContent.GetInstance<ModHelpersPrivilegedUserConfig>();



		////////////////

		/// @private
		public override ConfigScope Mode => ConfigScope.ClientSide;
		
		/// <summary>
		/// User ID of a designated privileged (admin) player. Refers to the internal player UID used by Mod Helpers.
		/// </summary>
		[Label( "Privileged User ID (internal UID)" )]
		[Tooltip( "User ID of a designated privileged (admin) player. Refers to the internal player UID used by Mod Helpers." )]
		//[ReloadRequired]
		public string PrivilegedUserId = "";



		////////////////

		/// @private
		public override void OnChanged() {
			if( Main.gameMenu ) { return; }

			base.OnChanged();

			string oldVal = this.PrivilegedUserId;
			this.PrivilegedUserId = "";

			Timers.SetTimer( "ModHelpersConfigSyncPrevention", 1, true, () => {
				this.PrivilegedUserId = oldVal;
				return false;
			} );
		}

		/// @private
		public override bool AcceptClientChanges( ModConfig pendingConfig, int whoAmI, ref string message ) {
			if( !UserHelpers.HasBasicServerPrivilege( Main.player[whoAmI] ) ) {
				message = "Not authorized.";

				var newConfig = pendingConfig as ModHelpersPrivilegedUserConfig;
				IEnumerable<PropertyInfo> properties = this.GetType().GetProperties( BindingFlags.Public );

				foreach( PropertyInfo prop in properties ) {
					prop.SetValue( newConfig, prop.GetValue(this) );
				}

				return false;
			}
			return true;
		}
	}
}
