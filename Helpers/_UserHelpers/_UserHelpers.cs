using HamstarHelpers.TmlHelpers;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader.IO;


namespace HamstarHelpers.UserHelpers {
	/*public class UserHelpers {
		public static bool IsAdmin( Player player ) {
			var mymod = HamstarHelpersMod.Instance;
			var modworld = mymod.GetModWorld<HamstarHelpersWorld>();
			string plr_id = TmlPlayerHelpers.GetUniqueId( player );

			return mymod.UserHelpers.Admins[ modworld.UID ].Contains( plr_id );
		}

		
		internal static void AddAdmin( Player player ) {    // Unfortunately, this isn't an API method (yet?)
			var mymod = HamstarHelpersMod.Instance;
			var modworld = mymod.GetModWorld<HamstarHelpersWorld>();
			string plr_id = TmlPlayerHelpers.GetUniqueId( player );

			mymod.UserHelpers.Admins[ modworld.UID ].Add( plr_id );
			
			if( Main.netMode == 2 ) {
				ServerPacketHandlers.SendSetAdminFromServer( mymod, player.whoAmI, -1, player.whoAmI, true );
			}
		}

		internal static bool RemoveAdmin( Player player ) {    // Unfortunately, this isn't an API method (yet?)
			var mymod = HamstarHelpersMod.Instance;
			var modworld = mymod.GetModWorld<HamstarHelpersWorld>();
			string plr_id = TmlPlayerHelpers.GetUniqueId( player );
			bool removed = false;

			if( mymod.UserHelpers.Admins[ modworld.UID ].Contains( plr_id ) ) {
				mymod.UserHelpers.Admins[ modworld.UID ].Remove( plr_id );
				removed = true;
			}

			if( Main.netMode == 2 ) {
				ServerPacketHandlers.SendSetAdminFromServer( mymod, player.whoAmI, -1, player.whoAmI, false );
			}

			return removed;
		}


		////////////////

		internal IDictionary<string, ISet<string>> Admins { get; private set; }


		////////////////

		internal UserHelpers() {
			this.Admins = new Dictionary<string, ISet<string>>();
		}

		internal void OnWorldLoad( HamstarHelpersWorld modworld ) {
			this.Admins = new Dictionary<string, ISet<string>> { { modworld.UID, new HashSet<string>() } };
		}

		////////////////

		internal void Load( HamstarHelpersMod mymod, TagCompound tags ) {
			if( tags.ContainsKey( "admin_world_uid_count" ) ) {
				int world_count = tags.GetInt( "admin_world_uid_count" );

				for( int i = 0; i < world_count; i++ ) {
					string world_uid = tags.GetString( "admin_world_uid_" + i );
					int admin_count = tags.GetInt( "admin_world_uid_" + world_uid + "_count" );

					this.Admins[world_uid] = new HashSet<string>();

					for( int j = 0; j < admin_count; j++ ) {
						string admin = tags.GetString( "admin_world_uid_" + world_uid + "_" + j );
						this.Admins[world_uid].Add( admin );
					}
				}
			}
		}

		internal void Save( HamstarHelpersMod mymod, TagCompound tags ) {
			tags.Set( "admin_world_uid_count", this.Admins.Count );

			int i = 0;
			foreach( var kv in this.Admins ) {
				tags.Set( "admin_world_uid_" + i++, kv.Key );
				tags.Set( "admin_world_uid_" + kv.Key + "_count", kv.Value.Count );

				int j = 0;
				foreach( string plr_uid in kv.Value ) {
					tags.Set( "admin_world_uid_" + kv.Key + "_" + j++, plr_uid );
				}
			}
		}
	}*/
}
