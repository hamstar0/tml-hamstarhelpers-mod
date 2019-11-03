using HamstarHelpers.Classes.Loadable;
using HamstarHelpers.Helpers.DotNET.Extensions;
using HamstarHelpers.Helpers.DotNET.Reflection;
using HamstarHelpers.Helpers.Players;
using HamstarHelpers.Helpers.TModLoader;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Classes.PlayerData {
	/// <summary>
	/// An alternative to ModPlayer for basic per-player, per-game data storage and Update use.
	/// </summary>
	public partial class CustomPlayerData : ILoadable {
		private static void InitializeAll( int playerWho ) {
			CustomPlayerData singleton = ModContent.GetInstance<CustomPlayerData>();
			IEnumerable<Type> plrDataTypes = ReflectionHelpers.GetAllAvailableSubTypesFromMods( typeof(CustomPlayerData) );

			foreach( Type plrDataType in plrDataTypes ) {
				object data = CustomPlayerData.LoadFileData( plrDataType.Name, PlayerIdentityHelpers.GetUniqueId() );
				var plrData = (CustomPlayerData)Activator.CreateInstance(
					plrDataType,
					ReflectionHelpers.MostAccess,
					null,
					new object[] { },
					null );
				plrData.PlayerWho = playerWho;

				singleton.DataMap.Set2D( playerWho, plrData );

				plrData.OnEnter( data );
			}
		}


		////

		private static void UpdateAll() {
			bool isInGame = !Main.gameMenu && LoadHelpers.IsWorldLoaded();

			CustomPlayerData singleton = ModContent.GetInstance<CustomPlayerData>();
			Player player;

			for( int i=0; i<Main.maxPlayers; i++ ) {
				player = Main.player[i];

				if( player == null || !player.active ) {
					if( singleton.DataMap.ContainsKey(i) ) {
						foreach( CustomPlayerData plrData in singleton.DataMap[i] ) {
							object data = plrData.OnExit();

							CustomPlayerData.SaveFileData( plrData.GetType().Name, PlayerIdentityHelpers.GetUniqueId(), data );
						}
					}
					
					continue;
				}

				if( isInGame ) {
					if( !singleton.DataMap.ContainsKey( i ) ) {
						CustomPlayerData.InitializeAll( i );
					}

					foreach( CustomPlayerData plrData in singleton.DataMap[i] ) {
						plrData.Update();
					}
				} else {
					foreach( CustomPlayerData plrData in singleton.DataMap[i] ) {
						plrData.OnExit();
					}
				}
			}

			if( !isInGame ) {
				singleton.DataMap.Clear();
			}
		}
	}
}
